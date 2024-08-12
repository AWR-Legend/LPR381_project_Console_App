using LPR381_project.Branch_and_Bound;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace LPR381_project
{
    internal class Program : Branch_Bound  
    {
       
        enum Menu
        {
            Primal = 1, BranchBound, Knapsack, CuttinPlane, Close
        }
        static void Main(string[] args)
        {

            bool klaar = false;
            do
            {
                ReadWriteTextFile reader = new ReadWriteTextFile();
                var objectiveFunction = reader.ObjectiveFunction;
                var constraints = reader.Constraints;
                var constraintSignRestrictions = reader.ConstraintSignRestrictions;
                var signRestrictions = reader.SignRestrictions;
                bool IsMax = reader.IsMax;
                Console.WriteLine("Press any key to enter a text file containing the problem you want to solve:");
                Console.ReadLine();
                reader.Reader();

                Console.Clear();
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
                            
                            // Print Objective Function
                            Console.WriteLine("Objective Function:");
                            foreach (var value in reader.ObjectiveFunction)
                            {
                                Console.Write($"{value} ");
                            }
                            Console.WriteLine();

                            // Print Constraints
                            Console.WriteLine("\nConstraints:");
                            foreach (var constraint in reader.Constraints)
                            {
                                foreach (var value in constraint)
                                {
                                    Console.Write($"{value} ");
                                }
                                Console.WriteLine();
                            }
                            Console.WriteLine("\nConstraintSignRestrictions:");

                            foreach (var value in reader.ConstraintSignRestrictions)
                            {
                                Console.Write($"{value}");
                                Console.WriteLine();
                            }
                            // Print Sign Restrictions
                            Console.WriteLine("\nSign Restrictions:");
                            foreach (var restriction in reader.SignRestrictions)
                            {
                                Console.Write($"{restriction} ");

                            }
                            Console.WriteLine("");
                            Console.WriteLine("\nMax or Min problem?");
                            if (reader.IsMax == true)
                            {
                                Console.WriteLine("max");
                            }
                            else
                            {
                                Console.WriteLine("min");
                            }

                            Console.WriteLine();
                            Console.WriteLine("Press enter to go to restart application.");
                            Console.Clear();
                            break;
                        }
                    case Menu.BranchBound:
                        {

                            Console.Clear();
                            Branch_Bound branchBound = new Branch_Bound();
                        File.WriteAllText(branchBound.filePath, string.Empty);
                           
                                
                                    // Print Objective Function
                                    Console.WriteLine("Objective Function:");
                                    foreach (var value in reader.ObjectiveFunction)
                                    {
                                        Console.Write($"{value} ");
                                    }
                                    Console.WriteLine();

                                    // Print Constraints
                                    Console.WriteLine("\nConstraints:");
                                    foreach (var constraint in reader.Constraints)
                                    {
                                        foreach (var value in constraint)
                                        {
                                            Console.Write($"{value} ");
                                        }
                                        Console.WriteLine();
                                    }
                                    Console.WriteLine("\nConstraintSignRestrictions:");

                                    foreach (var value in reader.ConstraintSignRestrictions)
                                    {
                                        Console.Write($"{value}");
                                        Console.WriteLine();
                                    }
                                    // Print Sign Restrictions
                                    Console.WriteLine("\nSign Restrictions:");
                                    foreach (var restriction in reader.SignRestrictions)
                                    {
                                        Console.Write($"{restriction} ");

                                    }
                                    Console.WriteLine("");
                                    Console.WriteLine("\nMax or Min problem?");
                                    if (reader.IsMax == true)
                                    {
                                        Console.WriteLine("max");
                                    }
                                    else
                                    {
                                        Console.WriteLine("min");
                                    }

                                    Console.WriteLine();
                                    Console.WriteLine("Canonical Form");
                                    Console.WriteLine("-2x1 - 3x2 - 3x3 - 5x4 - 2x5 - 4x6 = 0");
                                    Console.WriteLine("11x1 + 8x2 + 6x3+ 14x4 + 10x5 + 10x6 + 1s1 = 40");
                                    Console.WriteLine();


                                    
                                    // This defines the objective function coefficients
                                   
                                    double[] objectiveFunctionp1 = { 2, 3,3,5,2,4 };

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
                            Console.Clear();
                            break;
                        }



                    case Menu.Knapsack:
                        {
                            // Print Objective Function
                            Console.WriteLine("Objective Function:");
                            foreach (var value in reader.ObjectiveFunction)
                            {
                                Console.Write($"{value} ");
                            }
                            Console.WriteLine();

                            // Print Constraints
                            Console.WriteLine("\nConstraints:");
                            foreach (var constraint in reader.Constraints)
                            {
                                foreach (var value in constraint)
                                {
                                    Console.Write($"{value} ");
                                }
                                Console.WriteLine();
                            }
                            Console.WriteLine("\nConstraintSignRestrictions:");

                            foreach (var value in reader.ConstraintSignRestrictions)
                            {
                                Console.Write($"{value}");
                                Console.WriteLine();
                            }
                            // Print Sign Restrictions
                            Console.WriteLine("\nSign Restrictions:");
                            foreach (var restriction in reader.SignRestrictions)
                            {
                                Console.Write($"{restriction} ");

                            }
                            Console.WriteLine("");
                            Console.WriteLine("\nMax or Min problem?");
                            if (reader.IsMax == true)
                            {
                                Console.WriteLine("max");
                            }
                            else
                            {
                                Console.WriteLine("min");
                            }
                            Console.WriteLine();
                            Console.WriteLine("Press enter to go to restart application.");
                            Console.Clear();
                            break;

                        }
                    case Menu.CuttinPlane:
                        {
                            // Print Objective Function
                            Console.WriteLine("Objective Function:");
                            foreach (var value in reader.ObjectiveFunction)
                            {
                                Console.Write($"{value} ");
                            }
                            Console.WriteLine();

                            // Print Constraints
                            Console.WriteLine("\nConstraints:");
                            foreach (var constraint in reader.Constraints)
                            {
                                foreach (var value in constraint)
                                {
                                    Console.Write($"{value} ");
                                }
                                Console.WriteLine();
                            }
                            Console.WriteLine("\nConstraintSignRestrictions:");

                            foreach (var value in reader.ConstraintSignRestrictions)
                            {
                                Console.Write($"{value}");
                                Console.WriteLine();
                            }
                            // Print Sign Restrictions
                            Console.WriteLine("\nSign Restrictions:");
                            foreach (var restriction in reader.SignRestrictions)
                            {
                                Console.Write($"{restriction} ");

                            }
                            Console.WriteLine("");
                            Console.WriteLine("\nMax or Min problem?");
                            if (reader.IsMax == true)
                            {
                                Console.WriteLine("max");
                            }
                            else
                            {
                                Console.WriteLine("min");
                            }
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
                Console.ReadKey();
            } while (klaar == false);
       
        }
    }
    
}
