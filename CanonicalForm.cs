using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_project
{
    public class CanonicalForm
    {
        public void ConvertToCanonical(bool IsMax, List<double> objectiveFunction, List<List<double>> constraints, List<string> constraintSigns, List<string> signRestrictions, string cont)
        {
            if (!IsMax)
            {
                // Convert minimization problem to maximization by negating the objective function
                for (int i = 0; i < objectiveFunction.Count; i++)
                {
                    objectiveFunction[i] = -objectiveFunction[i];
                }
                IsMax = true; // Mark it as maximization
            }

            for (int i = 0; i < constraints.Count; i++)
            {

                int numVariables = objectiveFunction.Count;
                int numConstraints = constraints.Count;

                // Convert objective function based on problem type
                List<double> canonicalObjectiveFunction = new List<double>(objectiveFunction);
                if (IsMax == false)
                {
                    canonicalObjectiveFunction = canonicalObjectiveFunction.Select(x => -x).ToList();
                }

                // Initialize canonical constraints and sign restrictions
                List<List<double>> canonicalConstraints = new List<List<double>>();
                List<string> canonicalSignRestrictions = new List<string>();

                for (int j = 0; j < numConstraints; j++)
                {
                    List<double> constraint = new List<double>(constraints[j]);
                    string signRestriction = constraintSigns[j];

                    // Add slack or surplus variables
                    if (signRestriction == "<=")
                    {
                        constraint.Insert(numVariables + j, 1.0); // Add slack variable
                        canonicalSignRestrictions.Add("bin");
                    }
                    else if (signRestriction == ">=")
                    {
                        constraint.Insert(numVariables + i, -1.0); // Add surplus variable
                        canonicalSignRestrictions.Add("bin");
                    }
                    else if (signRestriction == "=")
                    {
                        canonicalSignRestrictions.Add("bin"); // Equation constraint
                    }

                    // Append the right-hand side value
                    double rhs = constraint.Last();
                    constraint.RemoveAt(constraint.Count - 1);
                    constraint.Add(rhs);
                    canonicalConstraints.Add(constraint);
                }

                // Adjust the number of variables in the objective function
                int totalVariables = numVariables + numConstraints; // Total variables after adding slack/surplus variables
                while (canonicalObjectiveFunction.Count < totalVariables)
                {
                    canonicalObjectiveFunction.Add(0);
                }

                // Prepare canonical constraints
                for (int j = 0; j < canonicalConstraints.Count; j++)
                {
                    while (canonicalConstraints[j].Count < totalVariables + 1)
                    {
                        canonicalConstraints[j].Insert(0, 0);
                    }
                }
                if (cont =="y")
                {
                    Console.WriteLine("Canonical Objective Function: -" + string.Join(", -", canonicalObjectiveFunction));
                    Console.WriteLine("Canonical Constraints:");
                    foreach (var constraint in canonicalConstraints)
                    {
                        Console.WriteLine(string.Join(", ", constraint));
                    }
                    Console.WriteLine("Canonical Sign Restrictions: " + string.Join(", ", canonicalSignRestrictions));
                }


            }
        }
    }
}
