using System;

public class ReadTextFile
{
	public string ReadTextFile()
	{
        // Create an instance of OpenFileDialog
        OpenFileDialog openFileDialog = new OpenFileDialog();
        string selectedFilePath = "";
        // Set filter options and filter index.
        openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;

        // Set the title of the dialog
        openFileDialog.Title = "Select a File";

        // Display the dialog and check if the user selected a file
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            // Get the file path selected by the user
            selectedFilePath = openFileDialog.FileName;

            // Display the selected file path in the console
            Console.WriteLine("Selected file: " + selectedFilePath);
        }
        else
        {
            Console.WriteLine("No file selected.");
        }
        return selectedFilePath;
    }
    public DataExtractor()
    {
        string filePath = ReadTextFile();
        ObjectiveFunction = new List<int>();
        Constraints = new List<List<int>>();
        SignRestrictions = new List<string>();

        ParseFile(filePath);
    }

    private void ParseFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);

        // Parse the Objective Function
        var objFuncParts = lines[0].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        foreach (var part in objFuncParts)
        {
            if (int.TryParse(part, out int value))
            {
                ObjectiveFunction.Add(value);
            }
        }

        // Parse Constraints
        for (int i = 1; i < lines.Length - 1; i++)
        {
            var constraintParts = lines[i].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            List<int> constraint = new List<int>();

            foreach (var part in constraintParts)
            {
                if (int.TryParse(part, out int value))
                {
                    constraint.Add(value);
                }
            }

            Constraints.Add(constraint);
        }

        // Parse Sign Restrictions
        var signRestrictionsParts = lines.Last().Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        SignRestrictions.AddRange(signRestrictionsParts);
    }
}
