using LPR381_project.Branch_and_Bound;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LPR381_project
{
    internal class Program : Branch_Bound  // //BB3
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
                Console.WriteLine("3: Knapsack"); //hi my names dylan
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
                            // LP model for values below
                            // Maximize Z=2x1 +5x2 +10x3 +5x4
                            //40x1 +30x2 +50x3 +10x4 ≤16
                            /* int n = 4;
                             int W = 16;
                             int[] w = { 2, 5, 10, 5 };
                             int[] p = { 40, 30, 50, 10 };  */

                            /* BB3 branchBound = new BB3 ();
                             int n = 5;
                             int W = 15;
                             int[] w = { 4, 2, 2, 1,10 };
                             int[] p = { 12, 2, 1, 1, 4 };

                             int maxProfit = branchBound.Knapsack(n, p, w, W);

                             Console.WriteLine(maxProfit);
                             Console.ReadLine();*/
                            /////////////////////////////////////////////
                           
                            Branch_Bound branchBound = new Branch_Bound();
                            File.WriteAllText(branchBound.filePath, string.Empty);
                            // Constraints
                            double[] constraint1 = { 1, 1 }; // x1 + x2 <= 6
                            double constraint1RHS = 6;

                            double[] constraint2 = { 9, 5 }; // 9x1 + 5x2 <= 45
                            double constraint2RHS = 45;

                            // Start branching and bounding
                            branchBound.BranchAndBound(new int[2], 0, constraint1, constraint1RHS, constraint2, constraint2RHS);

                            using (StreamWriter writer = new StreamWriter(branchBound.filePath, true))
                            {
                                writer.WriteLine(" ");
                                writer.WriteLine("Final answer");
                                writer.WriteLine("Final Maximum Z: " + branchBound.MaxZ);
                                writer.WriteLine("Best Final Solution: x1 = " + branchBound.BestSolution[0] + ", x2 = " + branchBound.BestSolution[1]);
                            }

                            Console.WriteLine();
                            Console.WriteLine("Final answer");
                            Console.WriteLine("Maximum Z: " + branchBound.MaxZ);
                            Console.WriteLine("Best Solution: x1 = " + branchBound.BestSolution[0] + ", x2 = " + branchBound.BestSolution[1]);
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
