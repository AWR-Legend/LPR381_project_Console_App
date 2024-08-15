using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using LPR381_project;


namespace LPR381_project
{
    internal class Program
    {
        enum Menu
        {
            Primal = 1, BranchBound, Knapsack, CuttinPlane, Close
        }
        static void Main(string[] args)
        {
            ReadWriteTextFile reader = new ReadWriteTextFile();

            Console.WriteLine("Press any key to enter a text file containing the problem you want to solve:");
            Console.ReadLine();
            reader.Reader();
            var objectiveFunction = reader.ObjectiveFunction;
            var constraints = reader.Constraints;
            var constraintSigns = reader.ConstraintSigns;
            var signRestrictions = reader.SignRestrictions;
            bool IsMax = reader.IsMax;
            int varNum = reader.varNum;
            PrintLP printLP = new PrintLP();
            bool cont = true;
            while (cont)
            {
            Console.WriteLine("1: Primal Simplex");
            Console.WriteLine("2: Branch and Bound");
            Console.WriteLine("3: Knapsack");
            Console.WriteLine("4: Cutting Plane");
            Console.WriteLine("5: Close Program");
            int option = Convert.ToInt32(Console.ReadLine());
                switch ((Menu)option)
                {
                case Menu.Primal:
                    {
                        printLP.Print(IsMax,objectiveFunction, constraints,constraintSigns,signRestrictions);
                        Console.ReadLine();
                            PrimalSimplex primal = new PrimalSimplex();
                            var canonicalForm = new CanonicalForm();
                            Console.WriteLine("would you like to see the Canonical form?");
                            string printCan = Console.ReadLine();
                            canonicalForm.ConvertToCanonical(IsMax, objectiveFunction, constraints, constraintSigns, signRestrictions,printCan);
                            // Access the canonical form values
                            var objectiveFunctionCanonical = canonicalForm.CanonicalObjectiveFunction;
                            var constraintsCanonical = canonicalForm.CanonicalConstraints;
                            var signRestrictionsCanonical = canonicalForm.CanonicalSignRestrictions;
                            primal.primalAlgorithm(IsMax, objectiveFunctionCanonical, constraintsCanonical, constraintSigns, signRestrictionsCanonical, varNum);
                        Console.WriteLine();
                        Console.WriteLine("Press enter to go to restart application.");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    }
                case Menu.BranchBound:
                    {

                        Console.Clear();
                        printLP.Print(IsMax, objectiveFunction, constraints, constraintSigns, signRestrictions);
                        Console.ReadLine();

                        Branch_Bound branchBound = new Branch_Bound();
                        File.WriteAllText(branchBound.filePath, string.Empty);
                        // This defines the objective function coefficients

                        double[] objectiveFunctionp1 = { 2, 3, 3, 5, 2, 4 };

                        // Defines the constraints as a list of tuples (coefficients and RHS)
                        List<(double[] Coefficients, double RHS, string Inequality)> constraintsp1 = new List<(double[], double, string)>
                                    {
                                    (new double[] { 11, 8,6,14,10,10}, 40, "<="),


                                    };
                        // Initializes the best solution array
                        branchBound.BestSolution = new int[objectiveFunctionp1.Length];

                        // Performs sensitivity analysis on the objective function
                        branchBound.PerformSensitivityAnalysis(objectiveFunctionp1, constraintsp1);

                        Console.WriteLine("All iterations have been written to " + branchBound.filePath);

                        // This calls the function that does branching and bounding
                        branchBound.BranchAndBound(new int[objectiveFunctionp1.Length], 0, objectiveFunctionp1, constraintsp1);

                        // The following code writes the results from the branch and bound to a text file.
                        using (StreamWriter writer = new StreamWriter(branchBound.filePath, true))
                        {
                            writer.WriteLine(" ");
                            writer.WriteLine("Final Maximum Z: " + branchBound.MaxZ);
                            writer.WriteLine("Best Final Solution: " + string.Join(", ", branchBound.BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")));
                            writer.WriteLine(" ");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Final Maximum Z: " + branchBound.MaxZ);
                        Console.WriteLine("Best Final Solution: " + string.Join(", ", branchBound.BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")));
                        Console.WriteLine("All iterations have been written to " + branchBound.filePath);
                        Console.WriteLine();

                        Console.WriteLine();
                        Console.WriteLine("Press enter to go to restart application.");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    }



                case Menu.Knapsack:
                    {
                        printLP.Print(IsMax, objectiveFunction, constraints, constraintSigns, signRestrictions);
                        Console.ReadLine();
                        Console.Clear();

                        Console.WriteLine();
                        Console.WriteLine("Press enter to go to restart application.");
                        Console.Clear();
                        break;

                    }
                case Menu.CuttinPlane:
                    {
                        printLP.Print(IsMax, objectiveFunction, constraints, constraintSigns, signRestrictions);
                        Console.ReadLine();
                        Console.Clear();

                        Console.WriteLine();
                        Console.WriteLine("Press enter to go to restart application.");
                        Console.Clear();
                        break;
                    }
                case Menu.Close:
                    {
                        Environment.Exit(0);
                        break;
                    }
                }
            }
            Console.Clear();
            Console.ReadKey();
        }
    }  
}
