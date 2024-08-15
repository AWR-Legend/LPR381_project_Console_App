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
        public List<double> CanonicalObjectiveFunction { get; private set; }
        public List<List<double>> CanonicalConstraints { get; private set; }
        public List<string> CanonicalSignRestrictions { get; private set; }

        public void ConvertToCanonical(bool IsMax, List<double> objectiveFunction, List<List<double>> constraints, List<string> constraintSigns, List<string> signRestrictions, string cont)
        {
            // Initialize canonical constraints and sign restrictions
            CanonicalConstraints = new List<List<double>>(constraints);
            CanonicalSignRestrictions = new List<string>(constraintSigns);
            CanonicalObjectiveFunction = new List<double>(objectiveFunction);

            if (!IsMax)
            {
                // Convert minimization problem to maximization by negating the objective function
                for (int i = 0; i < objectiveFunction.Count; i++)
                {
                    CanonicalObjectiveFunction[i] = -objectiveFunction[i];
                }
                IsMax = true;
            }

            int numVariables = objectiveFunction.Count;
            int numConstraints = constraints.Count;

            // Adjust constraints and objective function for sign restrictions
            for (int j = 0; j < numConstraints; j++)
            {
                if (CanonicalSignRestrictions[j] == "<=")
                {
                    // Add slack variable
                    CanonicalConstraints[j].Add(1.0);
                }
                else if (CanonicalSignRestrictions[j] == ">=")
                {
                    // Add surplus variable and possibly artificial variable
                    CanonicalConstraints[j].Add(-1.0);
                }
                else if (CanonicalSignRestrictions[j] == "=")
                {
                    // Equality constraint, treat as such in canonical form
                }
            }

            // Adjust the number of variables in the objective function
            int totalVariables = numVariables + CanonicalConstraints[0].Count - numVariables;
            while (CanonicalObjectiveFunction.Count < totalVariables)
            {
                CanonicalObjectiveFunction.Add(0);
            }

            // Prepare canonical constraints
            for (int j = 0; j < CanonicalConstraints.Count; j++)
            {
                while (CanonicalConstraints[j].Count < totalVariables)
                {
                    CanonicalConstraints[j].Add(0);
                }
            }

            // Optionally print the canonical form
            if (cont == "y")
            {
                Console.WriteLine("Canonical Objective Function: " + string.Join(", ", CanonicalObjectiveFunction));
                Console.WriteLine("Canonical Constraints:");
                foreach (var constraint in CanonicalConstraints)
                {
                    Console.WriteLine(string.Join(", ", constraint));
                }
                Console.WriteLine("Canonical Sign Restrictions: " + string.Join(", ", CanonicalSignRestrictions));
            }
        }
    }
}