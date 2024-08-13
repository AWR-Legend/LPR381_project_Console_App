using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_project
{
    internal class PrintLP
    {

        public void Print(bool IsMax,List<double> objectiveFunction,List<List<double>> constraints,List<string> constraintSigns,List<string> signRestrictions)
        {
                Console.WriteLine("\nMax or Min problem?");
                if (IsMax == true)
                {
                    Console.WriteLine("max");
                }
                else
                {
                    Console.WriteLine("min");
                }
                // Print Objective Function
                Console.WriteLine("Objective Function:");

                foreach (var value in objectiveFunction)
                {
                    Console.Write($"{value} ");
                }
                Console.WriteLine();

                // Print Constraints
                Console.WriteLine("\nConstraints:");
                foreach (var constraint in constraints)
                {
                    foreach (var value in constraint)
                    {
                        Console.Write($"{value} ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\nConstraint Sign Restrictions:");

                foreach (var value in constraintSigns)
                {
                    Console.Write($"{value}");
                    Console.WriteLine();
                }
                // Print Sign Restrictions
                Console.WriteLine("\nSign Restrictions:");
                foreach (var restriction in signRestrictions)
                {
                    Console.Write($"{restriction} ");

                }
                Console.WriteLine("");
                Console.WriteLine("would you like to see the Canonical form?( 'y' for yes | 'n' for no )");
                var cont=Console.ReadLine();
                if (cont == "y")
                {
                    CanonicalForm canonicalForm = new CanonicalForm();
                    canonicalForm.ConvertToCanonical(IsMax, objectiveFunction, constraints, constraintSigns, signRestrictions, cont);
                }
                else if (cont =="n")
                {
                    CanonicalForm canonicalForm = new CanonicalForm();
                    canonicalForm.ConvertToCanonical(IsMax, objectiveFunction, constraints, constraintSigns, signRestrictions, cont);

                }
                Console.ReadLine();
            
        }
    }
}
