using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class KnapsackItem
{
    public int Weight { get; set; }
    public int Value { get; set; }
    public double ValuePerWeight => Math.Round((double)Value / Weight, 3);
}

class KnapsackSolution
{
    public int TotalValue { get; set; }
    public int TotalWeight { get; set; }
    public List<int> SelectedItems { get; set; }

    public KnapsackSolution()
    {
        SelectedItems = new List<int>();
    }
}

class BranchAndBoundKnapsack
{
    private int Capacity;
    private List<KnapsackItem> Items;
    private KnapsackSolution BestSolution;
    private StreamWriter Writer;
    private string ObjectiveType;
    private int iterationCounter = 0;

    public BranchAndBoundKnapsack(int capacity, List<KnapsackItem> items, StreamWriter writer, string objectiveType)
    {
        Capacity = capacity;
        Items = items.OrderByDescending(i => i.ValuePerWeight).ToList();
        BestSolution = new KnapsackSolution();
        Writer = writer;
        ObjectiveType = objectiveType.ToLower();
    }

    public KnapsackSolution Solve()
    {
        BranchAndBound(0, 0, 0, new List<int>(new int[Items.Count]));
        return BestSolution;
    }

    public void BranchAndBound(int index, int currentWeight, int currentValue, List<int> selectedItems)
    {
        iterationCounter++;
        Writer.WriteLine($"Iteration: {iterationCounter}, Index: {index}, Current Weight: {currentWeight}, Current Value: {currentValue}, Selected Items: {string.Join(", ", selectedItems)}");

        if (index == Items.Count)
        {
            // Check if we have a better solution
            if ((ObjectiveType == "max" && currentWeight <= Capacity && currentValue > BestSolution.TotalValue) ||
                (ObjectiveType == "min" && currentWeight <= Capacity && (BestSolution.TotalValue == 0 || currentValue < BestSolution.TotalValue)))
            {
                BestSolution.TotalValue = currentValue;
                BestSolution.TotalWeight = currentWeight;
                BestSolution.SelectedItems = new List<int>(selectedItems);
            }
            return;
        }

        double bound = CalculateBound(index, currentWeight, currentValue);
        Writer.WriteLine($"Bound: {Math.Round(bound, 3)}, Current Best Value: {BestSolution.TotalValue}");

        // Fathoming: Check if bound is worse than the best solution found so far
        if ((ObjectiveType == "max" && bound <= BestSolution.TotalValue) ||
            (ObjectiveType == "min" && bound >= BestSolution.TotalValue && BestSolution.TotalValue != 0))
        {
            return;
        }

        // Branch on including the current item
        if (currentWeight + Items[index].Weight <= Capacity)
        {
            selectedItems[index] = 1;
            BranchAndBound(index + 1, currentWeight + Items[index].Weight, currentValue + Items[index].Value, selectedItems);
        }

        // Branch on excluding the current item
        selectedItems[index] = 0;
        BranchAndBound(index + 1, currentWeight, currentValue, selectedItems);
    }

    public double CalculateBound(int index, int currentWeight, int currentValue)
    {
        double bound = currentValue;
        int totalWeight = currentWeight;

        for (int i = index; i < Items.Count; i++)
        {
            if (totalWeight + Items[i].Weight <= Capacity)
            {
                totalWeight += Items[i].Weight;
                bound += Items[i].Value;
            }
            else
            {
                int remainingWeight = Capacity - totalWeight;
                bound += Items[i].ValuePerWeight * remainingWeight;
                break;
            }
        }
        return bound;
    }
}

class SensitivityAnalysis
{
    public static List<string> PerformSensitivityAnalysis(BranchAndBoundKnapsack solver, List<KnapsackItem> items, KnapsackSolution solution, int capacity)
    {
        var results = new List<string>();

        results.Add("Sensitivity Study for Non-Basic Variables:");
        for (int i = 0; i < items.Count; i++)
        {
            if (solution.SelectedItems[i] == 0)
            {
                results.Add($"Variable x{i + 1} is non-basic.");
                DisplayRangeAndApplyChange(results, solver, items, i, solution, capacity);
            }
        }

        results.Add("Sensitivity Study for Basic Variables:");
        for (int i = 0; i < items.Count; i++)
        {
            if (solution.SelectedItems[i] == 1)
            {
                results.Add($"Variable x{i + 1} is basic.");
                DisplayRangeAndApplyChange(results, solver, items, i, solution, capacity);
            }
        }

        results.Add("Sensitivity Study for Constraint RHS Values:");
        for (int j = 0; j < items.Count; j++)
        {
            int originalWeight = items[j].Weight;
            items[j].Weight += 1;
            var newSolution = solver.Solve();
            results.Add($"After increasing weight of item {j + 1} by 1, new Objective Value = {newSolution.TotalValue}");
            items[j].Weight = originalWeight;
        }

        results.Add("Adding a new activity to the model:");
        items.Add(new KnapsackItem { Weight = 1, Value = 1 });
        var solutionWithNewActivity = solver.Solve();
        results.Add($"Objective Value with new activity = {solutionWithNewActivity.TotalValue}");

        results.Add("Adding a new constraint to the model:");
        items.Last().Weight += 1;  // Example constraint adjustment
        var solutionWithNewConstraint = solver.Solve();
        results.Add($"Objective Value with new constraint = {solutionWithNewConstraint.TotalValue}");

        results.Add("Shadow Prices:");
        foreach (var item in items)
        {
            results.Add($"Shadow price for item {items.IndexOf(item) + 1}: {CalculateShadowPrice(item, solution.TotalValue)}");
        }

        results.Add("Implementing Duality:");
        var dualSolution = ApplyDuality(items, out int dualValue);
        results.Add($"Dual Objective Value: {dualValue}");
        results.Add(dualValue == solution.TotalValue ? "Strong Duality" : "Weak Duality");

        return results;
    }

    public static void DisplayRangeAndApplyChange(List<string> results, BranchAndBoundKnapsack solver, List<KnapsackItem> items, int variableIndex, KnapsackSolution solution, int capacity)
    {
        int originalValue = items[variableIndex].Value;
        items[variableIndex].Value += 1;
        var newSolution = solver.Solve();
        results.Add($"After increasing value of item {variableIndex + 1} by 1, new Objective Value = {newSolution.TotalValue}");
        items[variableIndex].Value = originalValue;
    }

    public static int CalculateShadowPrice(KnapsackItem item, int bestValue)
    {
        return item.Weight * bestValue;
    }

    public static List<int> ApplyDuality(List<KnapsackItem> items, out int dualValue)
    {
        dualValue = 0; // Implement dual problem logic here.
        return new List<int>(new int[items.Count]);
    }

    public static List<int> SolveDualProgrammingModel(List<KnapsackItem> items, out int dualProgValue)
    {
        // Placeholder implementation
        dualProgValue = 0;

        // Since this is a placeholder, we'll return a list of zeroes
        return new List<int>(new int[items.Count]);
    }
}

class MenuRelated
{
    private List<double> _objectiveFunction;
    private List<List<double>> _constraints;
    private bool _isMax;

    public void RunKnapsackMenu(List<double> objectiveFunction, List<List<double>> constraints, bool isMax)
    {
        _objectiveFunction = objectiveFunction;
        _constraints = constraints;
        _isMax = isMax;

        bool continueLoop = true;

        while (continueLoop)
        {
            Console.WriteLine("Please choose an operation:");
            Console.WriteLine("1. Execute Knapsack Problem");
            Console.WriteLine("2. Conduct Sensitivity Study");
            Console.WriteLine("3. Terminate Program");
            Console.Write("Your selection: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ExecuteKnapsackProblem();
                    break;

                case "2":
                    ConductSensitivityStudy();
                    break;

                case "3":
                    continueLoop = false;
                    Console.WriteLine("Shutting down the program...");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please select again.");
                    break;
            }
        }
    }

    public void ExecuteKnapsackProblem()
    {
        Console.WriteLine("Specify the output file path:");
        string outputFilePath = Console.ReadLine();

        if (_objectiveFunction == null || _constraints == null || _constraints.Count == 0)
        {
            Console.WriteLine("Input data is missing or incorrect.");
            return;
        }

        try
        {
            int capacity = (int)_constraints[0].Last();  // Assuming capacity is the last value in the first constraint row
            List<KnapsackItem> items = new List<KnapsackItem>();

            for (int i = 0; i < _objectiveFunction.Count; i++)
            {
                items.Add(new KnapsackItem
                {
                    Value = (int)_objectiveFunction[i],
                    Weight = (int)_constraints[0][i]
                });
            }

            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                BranchAndBoundKnapsack solver = new BranchAndBoundKnapsack(capacity, items, writer, _isMax ? "max" : "min");
                KnapsackSolution solution = solver.Solve();

                writer.WriteLine();
                writer.WriteLine("Best Solution:");
                writer.WriteLine($"Total Value: {solution.TotalValue}");
                writer.WriteLine($"Total Weight: {solution.TotalWeight}");
                writer.WriteLine($"Selected Items: {string.Join(", ", solution.SelectedItems.Select((s, i) => s == 1 ? $"Item {i + 1}" : string.Empty).Where(x => !string.IsNullOrEmpty(x)))}");

                Console.WriteLine("Solution has been written to the output file.");
                Console.WriteLine($"Total Value: {solution.TotalValue}");
                Console.WriteLine($"Total Weight: {solution.TotalWeight}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    public void ConductSensitivityStudy()
    {
        Console.WriteLine("Specify the output file path:");
        string outputFilePath = Console.ReadLine();

        if (_objectiveFunction == null || _constraints == null || _constraints.Count == 0)
        {
            Console.WriteLine("Input data is missing or incorrect.");
            return;
        }

        try
        {
            int capacity = (int)_constraints[0].Last();  // Assuming capacity is the last value in the first constraint row
            List<KnapsackItem> items = new List<KnapsackItem>();

            for (int i = 0; i < _objectiveFunction.Count; i++)
            {
                items.Add(new KnapsackItem
                {
                    Value = (int)_objectiveFunction[i],
                    Weight = (int)_constraints[0][i]
                });
            }

            using (StreamWriter writer = new StreamWriter(outputFilePath, true))
            {
                BranchAndBoundKnapsack solver = new BranchAndBoundKnapsack(capacity, items, writer, _isMax ? "max" : "min");
                KnapsackSolution solution = solver.Solve();

                writer.WriteLine();
                writer.WriteLine("Sensitivity Study Results:");
                var sensitivityResults = new List<string>();

                while (true)
                {
                    Console.WriteLine("Select an option for Sensitivity Study:");
                    Console.WriteLine("1. Show the range for a chosen Non-Basic Variable");
                    Console.WriteLine("2. Apply and show a modification for a selected Non-Basic Variable");
                    Console.WriteLine("3. Show the range for a selected Basic Variable");
                    Console.WriteLine("4. Apply and show a modification for a selected Basic Variable");
                    Console.WriteLine("5. Show the range for a chosen constraint RHS value");
                    Console.WriteLine("6. Apply and show a modification for a chosen constraint RHS value");
                    Console.WriteLine("7. Introduce a new element into the model");
                    Console.WriteLine("8. Impose a new limitation on the model");
                    Console.WriteLine("9. Show shadow prices");
                    Console.WriteLine("10. Implement Duality in the programming model");
                    Console.WriteLine("11. Resolve the Dual Programming Model");
                    Console.WriteLine("12. End Sensitivity Study");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("Provide the index of the Non-Basic Variable:");
                            int nonBasicIndex = int.Parse(Console.ReadLine()) - 1;
                            sensitivityResults.Add($"Showing range for Non-Basic Variable x{nonBasicIndex + 1}");
                            SensitivityAnalysis.DisplayRangeAndApplyChange(sensitivityResults, solver, items, nonBasicIndex, solution, capacity);
                            break;
                        case "2":
                            Console.WriteLine("Provide the index of the Non-Basic Variable:");
                            int nonBasicChangeIndex = int.Parse(Console.ReadLine()) - 1;
                            sensitivityResults.Add($"Applying and displaying modification for Non-Basic Variable x{nonBasicChangeIndex + 1}");
                            SensitivityAnalysis.DisplayRangeAndApplyChange(sensitivityResults, solver, items, nonBasicChangeIndex, solution, capacity);
                            break;
                        case "3":
                            Console.WriteLine("Provide the index of the Basic Variable:");
                            int basicIndex = int.Parse(Console.ReadLine()) - 1;
                            sensitivityResults.Add($"Showing range for Basic Variable x{basicIndex + 1}");
                            SensitivityAnalysis.DisplayRangeAndApplyChange(sensitivityResults, solver, items, basicIndex, solution, capacity);
                            break;
                        case "4":
                            Console.WriteLine("Provide the index of the Basic Variable:");
                            int basicChangeIndex = int.Parse(Console.ReadLine()) - 1;
                            sensitivityResults.Add($"Applying and displaying modification for Basic Variable x{basicChangeIndex + 1}");
                            SensitivityAnalysis.DisplayRangeAndApplyChange(sensitivityResults, solver, items, basicChangeIndex, solution, capacity);
                            break;
                        case "5":
                            sensitivityResults.Add("Showing range for constraint RHS values");
                            foreach (var item in items)
                            {
                                sensitivityResults.Add($"Item {items.IndexOf(item) + 1} weight: {item.Weight}");
                            }
                            break;
                        case "6":
                            Console.WriteLine("Provide the index of the constraint RHS value:");
                            int constraintIndex = int.Parse(Console.ReadLine()) - 1;
                            sensitivityResults.Add($"Applying and displaying modification for constraint RHS value {constraintIndex + 1}");
                            int originalWeight = items[constraintIndex].Weight;
                            items[constraintIndex].Weight += 1;
                            var newSolution = solver.Solve();
                            sensitivityResults.Add($"After increasing weight of item {constraintIndex + 1} by 1, new Objective Value = {newSolution.TotalValue}");
                            items[constraintIndex].Weight = originalWeight;
                            break;
                        case "7":
                            sensitivityResults.Add("Introducing a new element into the model");
                            items.Add(new KnapsackItem { Weight = 1, Value = 1 });
                            var solutionWithNewElement = solver.Solve();
                            sensitivityResults.Add($"Objective Value with new element = {solutionWithNewElement.TotalValue}");
                            break;
                        case "8":
                            sensitivityResults.Add("Imposing a new limitation on the model");
                            items.Last().Weight += 1;  // Example constraint adjustment
                            var solutionWithNewLimitation = solver.Solve();
                            sensitivityResults.Add($"Objective Value with new limitation = {solutionWithNewLimitation.TotalValue}");
                            break;
                        case "9":
                            sensitivityResults.Add("Showing shadow prices");
                            foreach (var item in items)
                            {
                                sensitivityResults.Add($"Shadow price for item {items.IndexOf(item) + 1}: {SensitivityAnalysis.CalculateShadowPrice(item, solution.TotalValue)}");
                            }
                            break;
                        case "10":
                            sensitivityResults.Add("Implementing Duality in the programming model");
                            var dualSolution = SensitivityAnalysis.ApplyDuality(items, out int dualValue);
                            sensitivityResults.Add($"Dual Objective Value: {dualValue}");
                            sensitivityResults.Add(dualValue == solution.TotalValue ? "Strong Duality" : "Weak Duality");
                            break;
                        case "11":
                            sensitivityResults.Add("Resolving the Dual Programming Model");
                            var dualProgSolution = SensitivityAnalysis.SolveDualProgrammingModel(items, out int dualProgValue);
                            sensitivityResults.Add($"Dual Programming Model Objective Value: {dualProgValue}");
                            break;

                        case "12":
                            WriteSensitivityAnalysisResults(outputFilePath, sensitivityResults);
                            return;
                        default:
                            Console.WriteLine("Invalid selection. Please choose again.");
                            break;
                    }

                    Console.WriteLine("Sensitivity Study outcomes:");
                    foreach (var result in sensitivityResults)
                    {
                        Console.WriteLine(result);
                    }

                    Console.WriteLine("\nWould you like to continue with the Sensitivity Study? (yes/no)");
                    string continueAnalysis = Console.ReadLine().ToLower();
                    if (continueAnalysis != "yes")
                    {
                        WriteSensitivityAnalysisResults(outputFilePath, sensitivityResults);
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    public void WriteSensitivityAnalysisResults(string outputFilePath, List<string> sensitivityAnalysisResults)
    {
        using (var file = new StreamWriter(outputFilePath, true))
        {
            file.WriteLine("\nSensitivity Analysis Results:");
            foreach (var result in sensitivityAnalysisResults)
            {
                file.WriteLine(result);
            }
        }
    }
}
