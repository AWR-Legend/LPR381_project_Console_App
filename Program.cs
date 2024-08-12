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


                            break;
                        }
                    case Menu.BranchBound:
                        {
                            bool Selected = false;

                            Branch_Bound branchBound = new Branch_Bound();
                        File.WriteAllText(branchBound.filePath, string.Empty);
                            while (Selected == false)
                            {
                                Console.Clear();
                                Console.WriteLine("Original LP Model (Problem 1)");
                                Console.WriteLine("max Z = 100x1 + 30x2");
                                Console.WriteLine("x2 >= 3");
                                Console.WriteLine("x1 + x2 <= 7");
                                Console.WriteLine("10x1 + 4x2 <= 40");
                                Console.WriteLine("x1, x2 >= 0");
                                Console.WriteLine();
                                Console.WriteLine("Original LP Model (Problem 2)");
                                Console.WriteLine("max Z = 60x1 + 30x2 + 20x3");
                                Console.WriteLine("8x1 + 6x2 + 1x3 <= 48");
                                Console.WriteLine("4x1 + 2x2 + 1.5x3 <= 20");
                                Console.WriteLine("2x1 + 1.5x2 + 0.5x3 <= 8");
                                Console.WriteLine("x1, x2, x3 >= 0");
                                Console.WriteLine();
                                Console.WriteLine("Would you like to solve problem 1 or problem 2?");
                                Console.WriteLine("Please answer by entering 1 for problem 1 or 2 for problem 2");
                                int option2 = Convert.ToInt32(Console.ReadLine());
                                if (option2 == 1)
                                {
                                    string filePath = "Problem1.txt"; // Path to your text file
                                    double[] objectiveFunction = null;
                                    List<(double[] Coefficients, double RHS, string Inequality)> constraints = new List<(double[], double, string)>();

                                    using (StreamReader sr = new StreamReader(filePath))
                                    {
                                        string line;
                                        while ((line = sr.ReadLine()) != null)
                                        {
                                            string[] parts = line.Split(new[] { ' ', ':', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                                            if (parts[0] == "objective")
                                            {
                                                objectiveFunction = new double[parts.Length - 1];
                                                for (int i = 1; i < parts.Length; i++)
                                                {
                                                    objectiveFunction[i - 1] = double.Parse(parts[i]);
                                                }
                                            }
                                            else if (parts[0] == "constraint")
                                            {
                                                double[] coefficients = new double[parts.Length - 3];
                                                for (int i = 1; i < parts.Length - 3; i++)
                                                {
                                                    coefficients[i - 1] = double.Parse(parts[i]);
                                                }
                                                string inequality = parts[parts.Length - 2];
                                                double rhs = double.Parse(parts[parts.Length - 1]);
                                                constraints.Add((coefficients, rhs, inequality));
                                            }
                                        }
                                    }

                                    // Output to check if the input was correctly read
                                    Console.WriteLine("Objective Function: ");
                                    Console.WriteLine(string.Join(" ", objectiveFunction));

                                    Console.WriteLine("\nConstraints: ");
                                    foreach (var constraint in constraints)
                                    {
                                        Console.WriteLine($"{string.Join(" ", constraint.Coefficients)} {constraint.Inequality} {constraint.RHS}");
                                    }

















                                    //Console.WriteLine("Original LP Model");
                                    //Console.WriteLine("max Z = 100x1 + 30x2");
                                    //Console.WriteLine("x2 >= 3");
                                    //Console.WriteLine("x1 + x2 <= 7");
                                    //Console.WriteLine("10x1 + 4x2 <= 40");
                                    //Console.WriteLine("x1, x2 >= 0");
                                    //Console.WriteLine();
                                    //Console.WriteLine("Canonical Form");
                                    //Console.WriteLine("-100x1 -30x2 = 0");
                                    //Console.WriteLine("-x1 + e1 = -3");
                                    //Console.WriteLine("x1 +x2 +s2 = 7");
                                    //Console.WriteLine("10x1 + 4x2 + s3 = 40");
                                    //Console.WriteLine();

                                    //Selected = true;
                                    //// This defines the objective function coefficients
                                    //double[] objectiveFunction = { 100, 30 }; // For example: 100x1 + 30x2 + 10x3


                                    //// Defines the constraints as a list of tuples (coefficients and RHS)
                                    //List<(double[] Coefficients, double RHS, string Inequality)> constraints = new List<(double[], double, string)>
                                    //    {
                                    //        (new double[] { 0, 1 }, 3, ">="),  
                                    //        (new double[] { 1, 1}, 7, "<="),  
                                    //        (new double[] { 10, 4 }, 40, "<=") 

                                    //    };
                                    //// Initializes the best solution array
                                    //branchBound.BestSolution = new int[objectiveFunction.Length];

                                    //// Performs sensitivity analysis on the objective function
                                    //branchBound.PerformSensitivityAnalysis(objectiveFunction, constraints);

                                    //Console.WriteLine("All iterations have been written to " + branchBound.filePath);

                                    //// This calls the function that does branching and bounding
                                    //branchBound.BranchAndBound(new int[objectiveFunction.Length], 0, objectiveFunction, constraints);

                                    //// The following code writes the results from the branch and bound to a text file.
                                    //using (StreamWriter writer = new StreamWriter(branchBound.filePath, true))
                                    //{
                                    //    writer.WriteLine(" ");
                                    //    writer.WriteLine("Final Maximum Z: " + branchBound.MaxZ);
                                    //    writer.WriteLine("Best Final Solution: " + string.Join(", ", branchBound.BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")));
                                    //    writer.WriteLine(" ");
                                    //}

                                    //Console.WriteLine();
                                    //Console.WriteLine("Final Maximum Z: " + branchBound.MaxZ);
                                    //Console.WriteLine("Best Final Solution: " + string.Join(", ", branchBound.BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")));
                                    //Console.WriteLine("All iterations have been written to " + branchBound.filePath);
                                    //Console.WriteLine();
                                }
                                else if (option2 == 2)
                                {
                                    Selected = true;
                                    
                                    Console.WriteLine("Original LP Model");
                                    Console.WriteLine("max Z = 60x1 + 30x2 + 20x3");
                                    Console.WriteLine("8x1 + 6x2 + 1x3 <= 48");
                                    Console.WriteLine("4x1 + 2x2 + 1.5x3 <= 20");
                                    Console.WriteLine("2x1 + 1.5x2 + 0.5x3 <= 8");
                                    Console.WriteLine("x1, x2, x3 >= 0");
                                    Console.WriteLine();
                                    Console.WriteLine("Canonical Form");
                                    Console.WriteLine("-60x1 -30x2 -20x3 = 0");
                                    Console.WriteLine("8x1 + 6x2 + 1x3 + s1 = 48");
                                    Console.WriteLine("4x1 + 2x2 + 1.5x3 + s2 = 20");
                                    Console.WriteLine("2x1 + 1.5x2 + 0.5x3 + s3 = 8");
                                    Console.WriteLine();

                                    // This defines the objective function coefficients
                                    double[] objectiveFunction = { 60, 30, 20 };


                                    // Defines the constraints as a list of tuples (coefficients and RHS)
                                    List<(double[] Coefficients, double RHS, string Inequality)> constraints = new List<(double[], double, string)>
                                        {
                                            (new double[] { 8, 6, 1 }, 48, "<="),
                                            (new double[] { 4, 2, 1.5}, 20, "<="),
                                            (new double[] { 2, 1.5, 0.5 }, 8, "<=")

                                        };
                                    // Initializes the best solution array
                                    branchBound.BestSolution = new int[objectiveFunction.Length];

                                    // Performs sensitivity analysis on the objective function
                                    branchBound.PerformSensitivityAnalysis(objectiveFunction, constraints);

                                    Console.WriteLine("All iterations have been written to " + branchBound.filePath);

                                    // This calls the function that does branching and bounding
                                    branchBound.BranchAndBound(new int[objectiveFunction.Length], 0, objectiveFunction, constraints);


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
                                }
                                else
                                {
                                    Selected = false;
                                    Console.Clear();
                                    Console.WriteLine("You have inputed the the wrong number. ");
                                    Console.WriteLine("Please answer by entering 1 for problem 1 or 2 for problem 2"); 
                                    Thread.Sleep(3500);
                                    
                                }
                            }
                           
                           break;
                        }



                    case Menu.Knapsack:
                        {
                            break;

                        }
                    case Menu.CuttinPlane:
                        {
                            
                            break;
                        }
                    case Menu.Close:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    
                }
                Console.ReadKey();
            } while (klaar == true);
       
        }
    }
    
}
