using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

public class Branch_Bound
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
            bool IsValue1 = false;
            bool IsValue2 = false;
            bool IsValue3 = false;
            double val1 = 0;
            double val2 = 0;
            double val3 = 0;
            // The following While loop are used to test the values that are entered by the user to prevent the 
            // users from providing the wrong input.
            Console.WriteLine("Please enter 3 values that you would like to use for the sensitivity analysis.");
            while (!IsValue1)
            {
                Console.WriteLine("Value 1: ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out val1))
                {
                   
                    IsValue1 = true;
                }
                else
                {
                    // // This code outputs when input is either empty or not a valid number
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                }
            }
            else
            {
                Console.WriteLine("Value 2: ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out val2))
                {
                    
                    IsValue2 = true;
                }
                else
                {
                    // This code outputs when input is either empty or not a valid number
                    Console.WriteLine("Invalid input. Please enter a numeric value for value 2.");
                }
            }
        }
        while (!IsValue2)
        {
            Console.WriteLine("Value 2: ");
            string input = Console.ReadLine();

                if (double.TryParse(input, out val3))
                {
                    
                    IsValue3 = true;
                }
                else
                {
                    // // This code outputs when input is either empty or not a valid number
                    Console.WriteLine("Invalid input. Please enter a numeric value for value 3.");
                }
            }
            // For example: Decrease by 10, No change, Increase by 10
            double[] Increments_for_sensitivity = { val1, val2, val3 };
            foreach (double increment in Increments_for_sensitivity)
            {
                double[] adjusted_Objective_Function = objectiveFunction.Select(coef => coef + increment).ToArray();

                MaxZ = double.MinValue; // Resets MaxZ for each sensitivity analysis
                BranchAndBound(new int[objectiveFunction.Length], 0, adjusted_Objective_Function, constraints);

                // This prints the output given by the calculations from the diffrent increments
                Console.WriteLine();
                Console.WriteLine("This is the increment: " + increment);
                Console.WriteLine("Objective Function: " + string.Join(", ", adjusted_Objective_Function.Select((val, idx) => $"x{idx + 1} = {val}")));
                Console.WriteLine("Final Maximum Z: " + MaxZ);
                Console.WriteLine("Best Final Solution: " + string.Join(", ", BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")));
                Console.WriteLine();

                // The following code writes the output to a text file
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine();
                    writer.WriteLine("This is the increment: "+ increment);
                    writer.WriteLine("Objective Function: " + string.Join(", ", adjusted_Objective_Function.Select((val, idx) => $"x{idx + 1} = {val}")));
                    writer.WriteLine("Final Maximum Z: " + MaxZ);
                    writer.WriteLine("Best Final Solution: " + string.Join(", ", BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")));
                    writer.WriteLine();
                }

        public void BranchAndBound(int[] solution, int variableIndex, double[] objectiveFunction, List<(double[] Coefficients, double RHS, string Inequality)> constraints)
        {
            if (variableIndex == solution.Length)
            {
                // This checks if the current solution is feasible
                if (IsFeasible(solution, constraints))
                {
                    MaxZ = currentZ;
                    Array.Copy(solution, BestSolution, solution.Length);

                    // This displays the current iteration, solution, and Z value
                    Console.WriteLine($"Iteration {Iteration}: " + string.Join(", ", solution.Select((val, idx) => $"x{idx + 1} = {val}")) + $", Z = {currentZ}");

                    // This writes to the text file
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine($"Iteration {Iteration}: " + string.Join(", ", solution.Select((val, idx) => $"x{idx + 1} = {val}")) + $", Z = {currentZ}");
                    }

                    if (currentZ > MaxZ)
                    {
                        MaxZ = currentZ;
                        Array.Copy(solution, BestSolution, solution.Length);

                        // Displays and writes new best solution to the text file
                        string bestSolutionText = $"New Best Solution Found: " + string.Join(", ", BestSolution.Select((val, idx) => $"x{idx + 1} = {val}")) + $", Max Z = {MaxZ}";
                        Console.WriteLine();
                        Console.WriteLine(bestSolutionText);
                        Console.WriteLine();


                        // This writes to the text file
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
            for (int i = 0; i <= 10; i++) // You can use this to adjust the upper bound for branching
            {
                solution[variableIndex] = i;
                BranchAndBound(solution, variableIndex + 1, objectiveFunction, constraints);
            }
            return;
        }
        //This checks whether it is feasible or not. 
        static bool IsFeasible(int[] solution, List<(double[] Coefficients, double RHS, string Inequality)> constraints)
        {
            solution[variableIndex] = i;
            BranchAndBound(solution, variableIndex + 1, objectiveFunction, constraints);
        }
        // This calculates the Z value
        static double CalculateZ(int[] solution, double[] objectiveFunction)
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

