using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LPR381_project
{
    internal class CuttingPlane
    {
        
        public  double[] SimplexMethod(double[,] A, double[] b, double[] c)
        {
            int m = A.GetLength(0); // Number of constraints
            int n = A.GetLength(1); // Number of variables

            // Create tableau
            double[,] tableau = new double[m + 1, n + m + 1];

            // Fill the tableau with the coefficients of constraints and objective function
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    tableau[i, j] = A[i, j];
                }
                tableau[i, n + i] = 1; // Slack variable
                tableau[i, n + m] = b[i];
            }

            for (int j = 0; j < n; j++)
            {
                tableau[m, j] = -c[j];
            }

            // Simplex algorithm
            while (true)
            {
                // Find the entering variable (most negative coefficient in the objective row)
                int pivotCol = -1;
                double minValue = 0;
                for (int j = 0; j < n + m; j++)
                {
                    if (tableau[m, j] < minValue)
                    {
                        minValue = tableau[m, j];
                        pivotCol = j;
                    }
                }

                // If all coefficients are non-negative, the current solution is optimal
                if (pivotCol == -1) break;

                // Find the leaving variable (minimum positive ratio of RHS to pivot column)
                int pivotRow = -1;
                double minRatio = double.PositiveInfinity;
                for (int i = 0; i < m; i++)
                {
                    if (tableau[i, pivotCol] > 0)
                    {
                        double ratio = tableau[i, n + m] / tableau[i, pivotCol];
                        if (ratio < minRatio)
                        {
                            minRatio = ratio;
                            pivotRow = i;
                        }
                    }
                }

                // If no valid pivot row, the problem is unbounded
                if (pivotRow == -1)
                {
                    throw new Exception("The problem is unbounded.");
                }

                // Pivot operation
                double pivotValue = tableau[pivotRow, pivotCol];
                for (int j = 0; j <= n + m; j++)
                {
                    tableau[pivotRow, j] /= pivotValue;
                }

                for (int i = 0; i <= m; i++)
                {
                    if (i != pivotRow)
                    {
                        double factor = tableau[i, pivotCol];
                        for (int j = 0; j <= n + m; j++)
                        {
                            tableau[i, j] -= factor * tableau[pivotRow, j];
                        }
                    }
                }
            }

            // Extract the solution
            double[] solution = new double[n];
            for (int i = 0; i < n; i++)
            {
                bool isBasic = false;
                for (int j = 0; j < m; j++)
                {
                    if (tableau[j, i] == 1)
                    {
                        solution[i] = tableau[j, n + m];
                        isBasic = true;
                        break;
                    }
                }
                if (!isBasic) solution[i] = 0;
            }

            return solution;
        }

        public  bool IsInteger(double[] solution)
        {
            foreach (double x in solution)
            {
                if (Math.Abs(x - Math.Round(x)) > 1e-6)
                    return false;
            }
            return true;
        }

        public  double[] GenerateCuttingPlane(double[] solution)
        {
            double[] cut = new double[solution.Length + 1];
            for (int i = 0; i < solution.Length; i++)
            {
                cut[i] = solution[i] - Math.Floor(solution[i]);
            }
            cut[solution.Length] = 0.5; // Adjust the RHS of the cut to ensure it's a half-integer
            return cut;
        }

        public  void AddCuttingPlane(ref double[,] A, ref double[] b, double[] cut)
        {
            int m = A.GetLength(0);
            int n = A.GetLength(1);

            // Expand the constraint matrix A
            double[,] newA = new double[m + 1, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    newA[i, j] = A[i, j];
                }
            }
            for (int j = 0; j < n; j++)
            {
                newA[m, j] = cut[j];
            }

            A = newA;

            // Expand the right-hand side vector b
            double[] newB = new double[m + 1];
            for (int i = 0; i < m; i++)
            {
                newB[i] = b[i];
            }
            newB[m] = cut[cut.Length - 1];

            b = newB;
        }
    }
}
