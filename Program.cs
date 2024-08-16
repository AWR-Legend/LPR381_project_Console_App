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
            ReadWriteTextFile reader = new ReadWriteTextFile();

            Console.WriteLine("Press any key to enter a text file containing the problem you want to solve:");
            Console.ReadLine();
            Console.Clear();
            reader.Reader();
            var objectiveFunction = reader.ObjectiveFunction;
            var constraints = reader.Constraints;
            var constraintSigns = reader.ConstraintSigns;
            var signRestrictions = reader.SignRestrictions;
            bool IsMax = reader.IsMax;
            

            PrintLP printLP = new PrintLP();
            bool cont = true;
            while (cont)
            {
                Console.WriteLine();
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
                            printLP.Print(IsMax, objectiveFunction, constraints, constraintSigns, signRestrictions);
                            Console.ReadLine();

                            Console.WriteLine();
                            Console.WriteLine("Press enter to go to restart application.");
                            Console.Clear();
                            break;
                        }
                    case Menu.BranchBound:
                        {
                            Console.Clear();
                            printLP.Print(IsMax, objectiveFunction, constraints, constraintSigns, signRestrictions);
                            Console.ReadLine();
                            Console.Clear();

                            Console.Clear();
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
                            List<double> objective_Function = reader.ObjectiveFunction;
                            List<List<double>> con_straints = reader.Constraints;
                            bool isMax = reader.IsMax;

                            printLP.Print(IsMax, objective_Function, con_straints, constraintSigns, signRestrictions);
                            Console.ReadLine();
                            Console.Clear();


                            MenuRelated knapsackMenu = new MenuRelated();
                            knapsackMenu.RunKnapsackMenu(objectiveFunction, constraints, isMax);

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

                            CuttingPlane plane = new CuttingPlane();

                            // Example coefficients of constraints
                            double[,] A = 
                                {
                                      { 11, 8, 6, 14, 10, 10 }
                                };

                            // Right-hand side of constraints
                            double[] b = { 40 };

                            // Coefficients of the objective function
                            double[] c = { 2, 3, 3, 5, 2, 4 };

                            double[] solution = null;

                            int iteration = 0;
                            HashSet<string> visitedSolutions = new HashSet<string>();

                            while (true)
                            {
                                iteration++;
                                Console.WriteLine($"Iteration {iteration}");

                                // Solve the LP problem using the Simplex method
                                solution = plane.SimplexMethod(A, b, c);

                                // Display current solution
                                Console.WriteLine("Current solution:");
                                for (int i = 0; i < solution.Length; i++)
                                {
                                    Console.WriteLine($"x{i + 1} = {solution[i]}");
                                }

                                // Check if the solution is integer
                                if (plane.IsInteger(solution))
                                {
                                    Console.WriteLine("Integer solution found.");
                                    break;
                                }

                                // Detect cycling: check if this solution has been encountered before
                                string solutionKey = string.Join(",", solution);
                                if (visitedSolutions.Contains(solutionKey))
                                {
                                    Console.WriteLine("Cycling detected. Exiting.");
                                    break;
                                }
                                visitedSolutions.Add(solutionKey);

                                // Generate a cutting plane
                                double[] cut = plane.GenerateCuttingPlane(solution);

                                // Display generated cut
                                Console.WriteLine("Generated cutting plane:");
                                for (int i = 0; i < cut.Length - 1; i++)
                                {
                                    Console.Write($"{cut[i]} ");
                                }
                                Console.WriteLine($"<= {cut[cut.Length - 1]}");

                                // Add the cutting plane to the constraints
                                plane.AddCuttingPlane(ref A, ref b, cut);
                            }

                            Console.WriteLine("Optimal integer solution:");
                            for (int i = 0; i < solution.Length; i++)
                            {
                                Console.WriteLine($"x{i + 1} = {solution[i]}");
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
            }
            Console.Clear();
            Console.ReadKey();
            //    bool klaar = false;
            //    do
            //    {
            //        ReadWriteTextFile reader = new ReadWriteTextFile();
            //        var objectiveFunction = reader.ObjectiveFunction;
            //        var constraints = reader.Constraints;
            //        var constraintSignRestrictions = reader.ConstraintSignRestrictions;
            //        var signRestrictions = reader.SignRestrictions;
            //        bool IsMax = reader.IsMax;
            //        Console.WriteLine("Press any key to enter a text file containing the problem you want to solve:");
            //        Console.ReadLine();
            //        reader.Reader();

            //        Console.Clear();
            //        Console.WriteLine("1: Primal Simplex");
            //        Console.WriteLine("2: Branch and Bound");
            //        Console.WriteLine("3: Knapsack"); 
            //        Console.WriteLine("4: Cutting Plane");
            //        Console.WriteLine("5: Close Program");
            //        int option = Convert.ToInt32(Console.ReadLine());
            //        switch ((Menu)option)
            //        {
            //            case Menu.Primal:
            //                {

            //                    // Print Objective Function
            //                    Console.WriteLine("Objective Function:");
            //                    foreach (var value in reader.ObjectiveFunction)
            //                    {
            //                        Console.Write($"{value} ");
            //                    }
            //                    Console.WriteLine();

            //                    // Print Constraints
            //                    Console.WriteLine("\nConstraints:");
            //                    foreach (var constraint in reader.Constraints)
            //                    {
            //                        foreach (var value in constraint)
            //                        {
            //                            Console.Write($"{value} ");
            //                        }
            //                        Console.WriteLine();
            //                    }
            //                    Console.WriteLine("\nConstraintSignRestrictions:");

            //                    foreach (var value in reader.ConstraintSignRestrictions)
            //                    {
            //                        Console.Write($"{value}");
            //                        Console.WriteLine();
            //                    }
            //                    // Print Sign Restrictions
            //                    Console.WriteLine("\nSign Restrictions:");
            //                    foreach (var restriction in reader.SignRestrictions)
            //                    {
            //                        Console.Write($"{restriction} ");

            //                    }
            //                    Console.WriteLine("");
            //                    Console.WriteLine("\nMax or Min problem?");
            //                    if (reader.IsMax == true)
            //                    {
            //                        Console.WriteLine("max");
            //                    }
            //                    else
            //                    {
            //                        Console.WriteLine("min");
            //                    }

            //                    Console.WriteLine();
            //                    Console.WriteLine("Press enter to go to restart application.");
            //                    Console.Clear();
            //                    break;
            //                }
            //            case Menu.BranchBound:
            //                {

            //                    Console.Clear();
            //                    Branch_Bound branchBound = new Branch_Bound();
            //                File.WriteAllText(branchBound.filePath, string.Empty);


            //                            // Print Objective Function
            //                            Console.WriteLine("Objective Function:");
            //                            foreach (var value in reader.ObjectiveFunction)
            //                            {
            //                                Console.Write($"{value} ");
            //                            }
            //                            Console.WriteLine();

            //                            // Print Constraints
            //                            Console.WriteLine("\nConstraints:");
            //                            foreach (var constraint in reader.Constraints)
            //                            {
            //                                foreach (var value in constraint)
            //                                {
            //                                    Console.Write($"{value} ");
            //                                }
            //                                Console.WriteLine();
            //                            }
            //                            Console.WriteLine("\nConstraintSignRestrictions:");

            //                            foreach (var value in reader.ConstraintSignRestrictions)
            //                            {
            //                                Console.Write($"{value}");
            //                                Console.WriteLine();
            //                            }
            //                            // Print Sign Restrictions
            //                            Console.WriteLine("\nSign Restrictions:");
            //                            foreach (var restriction in reader.SignRestrictions)
            //                            {
            //                                Console.Write($"{restriction} ");

            //                            }
            //                            Console.WriteLine("");
            //                            Console.WriteLine("\nMax or Min problem?");
            //                            if (reader.IsMax == true)
            //                            {
            //                                Console.WriteLine("max");
            //                            }
            //                            else
            //                            {
            //                                Console.WriteLine("min");
            //                            }

            //                            Console.WriteLine();
            //                            Console.WriteLine("Canonical Form");
            //                            Console.WriteLine("-2x1 - 3x2 - 3x3 - 5x4 - 2x5 - 4x6 = 0");
            //                            Console.WriteLine("11x1 + 8x2 + 6x3+ 14x4 + 10x5 + 10x6 + 1s1 = 40");
            //                            Console.WriteLine();



            //                            // This defines the objective function coefficients

            //                            double[] objectiveFunctionp1 = { 2, 3,3,5,2,4 };

            //                            // Defines the constraints as a list of tuples (coefficients and RHS)
            //                            List<(double[] Coefficients, double RHS, string Inequality)> constraintsp1 = new List<(double[], double, string)>
            //                                {
            //                                (new double[] { 11, 8,6,14,10,10}, 40, "<="),


            //                                };
            //                            // Initializes the best solution array
            //                            branchBound.BestSolution = new int[objectiveFunctionp1.Length];

            //                            // Performs sensitivity analysis on the objective function
            //                            branchBound.PerformSensitivityAnalysis(objectiveFunctionp1, constraintsp1);

            //                            Console.WriteLine("All iterations have been written to " + branchBound.filePath);

            //                            // This calls the function that does branching and bounding
            //                            branchBound.BranchAndBound(new int[objectiveFunctionp1.Length], 0, objectiveFunctionp1, constraintsp1);

            //                            // The following code writes the results from the branch and bound to a text file.
            //                            using (StreamWriter writer = new StreamWriter(branchBound.filePath, true))
            //                            {
            //                                writer.WriteLine(" ");
            //                                writer.WriteLine("Final Maximum Z: " + branchBound.MaxZ);
            //                                writer.WriteLine("Best Final Solution: " + string.Join(", ", branchBound.BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")));
            //                                writer.WriteLine(" ");
            //                            }

            //                            Console.WriteLine();
            //                            Console.WriteLine("Final Maximum Z: " + branchBound.MaxZ);
            //                            Console.WriteLine("Best Final Solution: " + string.Join(", ", branchBound.BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")));
            //                            Console.WriteLine("All iterations have been written to " + branchBound.filePath);
            //                            Console.WriteLine();

            //                    Console.WriteLine();
            //                    Console.WriteLine("Press enter to go to restart application.");
            //                    Console.Clear();
            //                    break;
            //                }



            //            case Menu.Knapsack:
            //                {
            //                    // Print Objective Function
            //                    Console.WriteLine("Objective Function:");
            //                    foreach (var value in reader.ObjectiveFunction)
            //                    {
            //                        Console.Write($"{value} ");
            //                    }
            //                    Console.WriteLine();

            //                    // Print Constraints
            //                    Console.WriteLine("\nConstraints:");
            //                    foreach (var constraint in reader.Constraints)
            //                    {
            //                        foreach (var value in constraint)
            //                        {
            //                            Console.Write($"{value} ");
            //                        }
            //                        Console.WriteLine();
            //                    }
            //                    Console.WriteLine("\nConstraintSignRestrictions:");

            //                    foreach (var value in reader.ConstraintSignRestrictions)
            //                    {
            //                        Console.Write($"{value}");
            //                        Console.WriteLine();
            //                    }
            //                    // Print Sign Restrictions
            //                    Console.WriteLine("\nSign Restrictions:");
            //                    foreach (var restriction in reader.SignRestrictions)
            //                    {
            //                        Console.Write($"{restriction} ");

            //                    }
            //                    Console.WriteLine("");
            //                    Console.WriteLine("\nMax or Min problem?");
            //                    if (reader.IsMax == true)
            //                    {
            //                        Console.WriteLine("max");
            //                    }
            //                    else
            //                    {
            //                        Console.WriteLine("min");
            //                    }
            //                    Console.WriteLine();
            //                    Console.WriteLine("Press enter to go to restart application.");
            //                    Console.Clear();
            //                    break;

            //                }
            //            case Menu.CuttinPlane:
            //                {
            //                    // Print Objective Function
            //                    Console.WriteLine("Objective Function:");
            //                    foreach (var value in reader.ObjectiveFunction)
            //                    {
            //                        Console.Write($"{value} ");
            //                    }
            //                    Console.WriteLine();

            //                    // Print Constraints
            //                    Console.WriteLine("\nConstraints:");
            //                    foreach (var constraint in reader.Constraints)
            //                    {
            //                        foreach (var value in constraint)
            //                        {
            //                            Console.Write($"{value} ");
            //                        }
            //                        Console.WriteLine();
            //                    }
            //                    Console.WriteLine("\nConstraintSignRestrictions:");

            //                    foreach (var value in reader.ConstraintSignRestrictions)
            //                    {
            //                        Console.Write($"{value}");
            //                        Console.WriteLine();
            //                    }
            //                    // Print Sign Restrictions
            //                    Console.WriteLine("\nSign Restrictions:");
            //                    foreach (var restriction in reader.SignRestrictions)
            //                    {
            //                        Console.Write($"{restriction} ");

            //                    }
            //                    Console.WriteLine("");
            //                    Console.WriteLine("\nMax or Min problem?");
            //                    if (reader.IsMax == true)
            //                    {
            //                        Console.WriteLine("max");
            //                    }
            //                    else
            //                    {
            //                        Console.WriteLine("min");
            //                    }
            //                    Console.WriteLine();
            //                    Console.WriteLine("Press enter to go to restart application.");
            //                    Console.Clear();
            //                    break;
            //                }
            //            case Menu.Close:
            //                {

            //                    Environment.Exit(0);
            //                    break;
            //                }

            //        }
            //        Console.ReadKey();
            //    } while (klaar == false);

        }
        }
    
}
