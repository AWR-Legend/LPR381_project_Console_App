using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LPR381_project.Branch_and_Bound
{
    internal class Branch_Bound
    {
        public double MaxZ = double.MinValue;
        public int[] BestSolution = new int[2];
        static int Iteration = 0;
        public string filePath = "iterations.txt";

        public void PerformSensitivityAnalysis(double[] objectiveFunction, List<(double[] Coefficients, double RHS, string Inequality)> constraints)
        {
            double[] sensitivityIncrements = { -10, 0, 10 }; // For example: Decrease by 10, No change, Increase by 10

            foreach (double increment in sensitivityIncrements)
            {
                double[] adjustedObjectiveFunction = objectiveFunction.Select(coef => coef + increment).ToArray();

                MaxZ = double.MinValue; // Reset MaxZ for each sensitivity analysis
                BranchAndBound(new int[objectiveFunction.Length], 0, adjustedObjectiveFunction, constraints);


                Console.WriteLine();
                Console.WriteLine("This is the increment: " + increment);
                Console.WriteLine("Objective Function: " + string.Join(", ", adjustedObjectiveFunction.Select((val, idx) => $"x{idx + 1} = {val}")));
                Console.WriteLine("Final Maximum Z: " + MaxZ);
                Console.WriteLine("Best Final Solution: " + string.Join(", ", BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")));
                Console.WriteLine();


                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("This is the increment: "+ increment);
                    writer.WriteLine("Objective Function: " + string.Join(", ", adjustedObjectiveFunction.Select((val, idx) => $"x{idx + 1} = {val}")));
                    writer.WriteLine("Final Maximum Z: " + MaxZ);
                    writer.WriteLine("Best Final Solution: " + string.Join(", ", BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")));
                    writer.WriteLine();
                }
            }
        }

        public void BranchAndBound(int[] solution, int variableIndex, double[] objectiveFunction, List<(double[] Coefficients, double RHS, string Inequality)> constraints)
        {
            if (variableIndex == solution.Length)
            {
                // Check if the current solution is feasible
                if (IsFeasible(solution, constraints))
                {
                    double currentZ = CalculateZ(solution, objectiveFunction);
                    Iteration++;

                    // Display current iteration, solution, and Z value
                    Console.WriteLine($"Iteration {Iteration}: " + string.Join(", ", solution.Select((val, idx) => $"x{idx + 1} = {val}")) + $", Z = {currentZ}");

                    // Write to file
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine($"Iteration {Iteration}: " + string.Join(", ", solution.Select((val, idx) => $"x{idx + 1} = {val}")) + $", Z = {currentZ}");
                    }

                    if (currentZ > MaxZ)
                    {
                        MaxZ = currentZ;
                        Array.Copy(solution, BestSolution, solution.Length);

                        // Display and write new best solution
                        string bestSolutionText = $"New Best Solution Found: " + string.Join(", ", BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")) + $", Max Z = {MaxZ}";
                        Console.WriteLine(bestSolutionText);

                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            writer.WriteLine(bestSolutionText);
                        }
                    }
                }
                return;
            }

            // Branching
            for (int i = 0; i <= 10; i++) // You can adjust the upper bound for branching
            {
                solution[variableIndex] = i;
                BranchAndBound(solution, variableIndex + 1, objectiveFunction, constraints);
            }
        }

        static bool IsFeasible(int[] solution, List<(double[] Coefficients, double RHS, string Inequality)> constraints)
        {
            foreach (var (coefficients, rhs, inequality) in constraints)
            {
                double lhs = 0;

                for (int i = 0; i < solution.Length; i++)
                {
                    lhs += solution[i] * coefficients[i];
                }

                if (inequality == "<=" && lhs > rhs)
                    return false;
                if (inequality == ">=" && lhs < rhs)
                    return false;
                if (inequality == "==" && lhs != rhs)
                    return false;
            }

            return true;
        }

        static double CalculateZ(int[] solution, double[] objectiveFunction)
        {
            double z = 0;

            for (int i = 0; i < solution.Length; i++)
            {
                z += solution[i] * objectiveFunction[i];
            }

            return z;
        }








        //    public void BranchAndBound(int[] solution, int variableIndex, double[] constraint1, double constraint1RHS, double[] constraint2, double constraint2RHS)
        //    {
        //        using (StreamWriter writer = new StreamWriter(filePath, true))
        //        {
        //            writer.WriteLine("Branch and Bound");
        //            writer.WriteLine(" ");
        //        }
        //        if (variableIndex == solution.Length)
        //        {
        //            // Check if the current solution is feasible
        //            if (IsFeasible(solution, constraint1, constraint1RHS, constraint2, constraint2RHS))
        //            {
        //                double currentZ = CalculateZ(solution);
        //                Iteration++;

        //                // Display current iteration, solution, and Z value
        //                Console.WriteLine();
        //                Console.WriteLine($"Iteration {Iteration}: x1 = {solution[0]}, x2 = {solution[1]}, Z = {currentZ}");

        //                // Write to file
        //                using (StreamWriter writer = new StreamWriter(filePath, true))
        //                {
        //                    writer.WriteLine(" ");
        //                    writer.WriteLine($"Iteration {Iteration}: x1 = {solution[0]}, x2 = {solution[1]}, Z = {currentZ}");
        //                }

        //                if (currentZ > MaxZ)
        //                {
        //                    MaxZ = currentZ;
        //                    Array.Copy(solution, BestSolution, solution.Length);

        //                    // Display new best solution
        //                    Console.WriteLine();
        //                    Console.WriteLine($"New Best Solution Found: x1 = {BestSolution[0]}, x2 = {BestSolution[1]}, Max Z = {MaxZ}");

        //                    string bestSolutionText = $"New Best Solution Found: x1 = {BestSolution[0]}, x2 = {BestSolution[1]}, Max Z = {MaxZ}";
        //                    using (StreamWriter writer = new StreamWriter(filePath, true))
        //                    {
        //                        writer.WriteLine(" ");
        //                        writer.WriteLine(bestSolutionText);
        //                    }
        //                }
        //            }
        //            return;
        //        }

        //        // Branching
        //        for (int i = 0; i <= 6; i++)
        //        {
        //            solution[variableIndex] = i;
        //            BranchAndBound(solution, variableIndex + 1, constraint1, constraint1RHS, constraint2, constraint2RHS);
        //        }
        //    }

        //    static bool IsFeasible(int[] solution, double[] constraint1, double constraint1RHS, double[] constraint2, double constraint2RHS)
        //    {
        //        double lhs1 = 0, lhs2 = 0;

        //        for (int i = 0; i < solution.Length; i++)
        //        {
        //            lhs1 += solution[i] * constraint1[i];
        //            lhs2 += solution[i] * constraint2[i];
        //        }

        //        return lhs1 <= constraint1RHS && lhs2 <= constraint2RHS;
        //    }

        //    static double CalculateZ(int[] solution)
        //    {
        //        return 8 * solution[0] + 5 * solution[1];
        //    }
    }
}
