using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

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
            bool IsValue1 = false;
            bool IsValue2 = false;
            bool IsValue3 = false;
            double val1 = 0;
            double val2 = 0;
            double val3 = 0;

            Console.WriteLine("Please enter 3 values that you would like to use for the sensitivity analysis.");
            while (!IsValue1)
            {
                Console.WriteLine("Value 1: ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out val1))
                {
                    // Input is a valid number
                    IsValue1 = true;
                }
                else
                {
                    // Input is either empty or not a valid number
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                }
            }
            while (!IsValue2)
            {
                Console.WriteLine("Value 2: ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out val2))
                {
                    // Input is a valid number
                    IsValue2 = true;
                }
                else
                {
                    // Input is either empty or not a valid number
                    Console.WriteLine("Invalid input. Please enter a numeric value for value 2.");
                }
            }
            while (!IsValue3)
            {
                Console.WriteLine("Value 3: ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out val3))
                {
                    // Input is a valid number
                    IsValue3 = true;
                }
                else
                {
                    // Input is either empty or not a valid number
                    Console.WriteLine("Invalid input. Please enter a numeric value for value 3.");
                }
            }
            // For example: Decrease by 10, No change, Increase by 10
            double[] sensitivityIncrements = { val1, val2, val3 };
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
                        Console.WriteLine();
                        Console.WriteLine(bestSolutionText);
                        Console.WriteLine();

                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            writer.WriteLine();
                            writer.WriteLine(bestSolutionText);
                            writer.WriteLine();
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
    }
}
