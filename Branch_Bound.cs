using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LPR381_project.Branch_and_Bound
{
    public class Branch_Bound
    {
        public double MaxZ = double.MinValue;
        public int[] BestSolution = new int[2];
        static int Iteration = 0;
        public string filePath = "iterations.txt";

        public void BranchAndBound(int[] solution, int variableIndex, double[] constraint1, double constraint1RHS, double[] constraint2, double constraint2RHS)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Branch and Bound");
                writer.WriteLine(" ");
            }
            if (variableIndex == solution.Length)
            {
                // Check if the current solution is feasible
                if (IsFeasible(solution, constraint1, constraint1RHS, constraint2, constraint2RHS))
                {
                    double currentZ = CalculateZ(solution);
                    Iteration++;

                    // Display current iteration, solution, and Z value
                    Console.WriteLine();
                    Console.WriteLine($"Iteration {Iteration}: x1 = {solution[0]}, x2 = {solution[1]}, Z = {currentZ}");

                    // Write to file
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine(" ");
                        writer.WriteLine($"Iteration {Iteration}: x1 = {solution[0]}, x2 = {solution[1]}, Z = {currentZ}");
                    }

                    if (currentZ > MaxZ)
                    {
                        MaxZ = currentZ;
                        Array.Copy(solution, BestSolution, solution.Length);

                        // Display new best solution
                        Console.WriteLine();
                        Console.WriteLine($"New Best Solution Found: x1 = {BestSolution[0]}, x2 = {BestSolution[1]}, Max Z = {MaxZ}");

                        string bestSolutionText = $"New Best Solution Found: x1 = {BestSolution[0]}, x2 = {BestSolution[1]}, Max Z = {MaxZ}";
                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            writer.WriteLine(" ");
                            writer.WriteLine(bestSolutionText);
                        }
                    }
                }
                return;
            }

            // Branching
            for (int i = 0; i <= 6; i++)
            {
                solution[variableIndex] = i;
                BranchAndBound(solution, variableIndex + 1, constraint1, constraint1RHS, constraint2, constraint2RHS);
            }
        }

        static bool IsFeasible(int[] solution, double[] constraint1, double constraint1RHS, double[] constraint2, double constraint2RHS)
        {
            double lhs1 = 0, lhs2 = 0;

            for (int i = 0; i < solution.Length; i++)
            {
                lhs1 += solution[i] * constraint1[i];
                lhs2 += solution[i] * constraint2[i];
            }

            return lhs1 <= constraint1RHS && lhs2 <= constraint2RHS;
        }

        static double CalculateZ(int[] solution)
        {
            return 8 * solution[0] + 5 * solution[1];
        }
    }
}
