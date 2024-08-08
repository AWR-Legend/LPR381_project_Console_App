using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_project
{
    internal class Program
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
                Console.WriteLine("3: Knapsack");
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
