using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_project
{
    internal class PrimalSimplex
    {
        public void primalAlgorithm(bool IsMax, List<double> objectiveFunction, List<List<double>> constraints, List<string> constraintSigns, List<string> signRestrictions, int varNum)
        {
            int numConstraints = constraints.Count;
            double[,] tableau = new double[numConstraints + 1, varNum + numConstraints + 1];
            int numColumns = tableau.GetLength(1);
            int columnWidth = 13;
            string separator = new string('-', columnWidth * (numColumns + 1));

            // Fill the tableau with the constraints
            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < varNum; j++)
                {
                    tableau[i, j] = constraints[i][j];
                }
                tableau[i, varNum + i] = constraintSigns[i] == "<=" ? 1 : -1;
                tableau[i, numColumns - 1] = constraints[i][varNum];
            }

            // Fill the Z row (objective function row)
            for (int j = 0; j < varNum; j++)
            {
                tableau[numConstraints, j] = IsMax ? -objectiveFunction[j] : objectiveFunction[j];
            }

            // Iterative process
            while (true)
            {
                // Print the current tableau
                PrintTableau(tableau, varNum, numConstraints, columnWidth, separator);

                // Step 1: Identify the pivot column (most negative value in the Z row)
                int pivotColumn = FindPivotColumn(tableau, numConstraints, numColumns);
                if (pivotColumn == -1)
                {
                    // Optimal solution found (no negative values in Z row)
                    Console.WriteLine("Optimal solution reached.");
                    break;
                }

                // Step 2: Identify the pivot row (minimum positive ratio of RHS / pivot column)
                int pivotRow = FindPivotRow(tableau, pivotColumn, numConstraints, numColumns);
                if (pivotRow == -1)
                {
                    Console.WriteLine("Unbounded solution detected.");
                    break;
                }

                // Step 3: Perform the pivot operation
                PerformPivotOperation(tableau, pivotRow, pivotColumn, numConstraints, numColumns);
            }

            // Print the optimal results
            PrintOptimalResults(tableau, varNum, numConstraints, columnWidth, IsMax);
        }

        private int FindPivotColumn(double[,] tableau, int numConstraints, int numColumns)
        {
            int pivotColumn = -1;
            double minValue = 0;

            for (int j = 0; j < numColumns - 1; j++)
            {
                if (tableau[numConstraints, j] < minValue)
                {
                    minValue = tableau[numConstraints, j];
                    pivotColumn = j;
                }
            }

            return pivotColumn;
        }

        private int FindPivotRow(double[,] tableau, int pivotColumn, int numConstraints, int numColumns)
        {
            int pivotRow = -1;
            double minRatio = double.PositiveInfinity;

            for (int i = 0; i < numConstraints; i++)
            {
                double element = tableau[i, pivotColumn];
                if (element > 0)
                {
                    double ratio = tableau[i, numColumns - 1] / element;
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotRow = i;
                    }
                }
            }

            return pivotRow;
        }

        private void PerformPivotOperation(double[,] tableau, int pivotRow, int pivotColumn, int numConstraints, int numColumns)
        {
            double pivotElement = tableau[pivotRow, pivotColumn];

            // Normalize the pivot row
            for (int j = 0; j < numColumns; j++)
            {
                tableau[pivotRow, j] /= pivotElement;
            }

            // Update other rows
            for (int i = 0; i <= numConstraints; i++)
            {
                if (i != pivotRow)
                {
                    double multiplier = tableau[i, pivotColumn];
                    for (int j = 0; j < numColumns; j++)
                    {
                        tableau[i, j] -= multiplier * tableau[pivotRow, j];
                    }
                }
            }
        }

        private void PrintTableau(double[,] tableau, int varNum, int numConstraints, int columnWidth, string separator)
        {
            int numColumns = tableau.GetLength(1);

            Console.WriteLine(separator);

            // Print headers
            Console.Write("   ");
            for (int j = 0; j < varNum; j++)
            {
                Console.Write($"x{j + 1}".PadLeft(columnWidth));
            }
            for (int k = 0; k < numConstraints; k++)
            {
                Console.Write($"s{k + 1}".PadLeft(columnWidth));
            }
            Console.Write(" RHS".PadLeft(columnWidth));
            Console.WriteLine();
            Console.WriteLine(separator);

            // Print the Z row
            Console.Write("Z |");
            for (int j = 0; j < numColumns; j++)
            {
                Console.Write(tableau[numConstraints, j].ToString("F2").PadLeft(columnWidth) + " ");
            }
            Console.WriteLine();
            Console.WriteLine(separator);

            // Print the rest of the tableau rows
            for (int i = 0; i < numConstraints; i++)
            {
                Console.Write($"B{i + 1}|"); // Basis row
                for (int j = 0; j < numColumns; j++)
                {
                    Console.Write(tableau[i, j].ToString("F2").PadLeft(columnWidth) + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine(separator);
            Console.ReadLine();
        }

        private void PrintOptimalResults(double[,] tableau, int varNum, int numConstraints, int columnWidth, bool IsMax)
        {
            double[] solution = new double[varNum];
            double zValue = IsMax ? -tableau[numConstraints, tableau.GetLength(1) - 1] : tableau[numConstraints, tableau.GetLength(1) - 1];

            // Determine the solution values for the decision variables
            for (int i = 0; i < varNum; i++)
            {
                bool isBasic = false;
                double value = 0;

                for (int j = 0; j < numConstraints; j++)
                {
                    if (tableau[j, i] == 1)
                    {
                        isBasic = true;
                        value = tableau[j, tableau.GetLength(1) - 1];
                        break;
                    }
                }

                if (isBasic)
                {
                    solution[i] = value;
                }
                else
                {
                    solution[i] = 0;
                }
            }

            // Print the solution
            Console.WriteLine("Optimal solution:");
            for (int i = 0; i < solution.Length; i++)
            {
                Console.WriteLine($"x{i + 1} = {solution[i]:F2}");
            }

            // Print the optimal Z value
            Console.WriteLine($"Optimal Z value = {zValue:F2}");
        }
    }
}