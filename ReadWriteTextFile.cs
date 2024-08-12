using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

public class ReadWriteTextFile
{
    public List<int> ObjectiveFunction { get; private set; }
    public List<List<int>> Constraints { get; private set; }
    public List<string> SignRestrictions { get; private set; }
    public List<string> ConstraintSignRestrictions { get; private set; }
    public bool IsMax { get; set; }

    public string ReadTextFile()
    {
        string selectedFilePath = "";
        Thread t = new Thread(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Title = "Select a File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = openFileDialog.FileName;
                Console.WriteLine("Selected file: " + selectedFilePath);
            }
            else
            {
                Console.WriteLine("No file selected.");
            }
        });

        t.SetApartmentState(ApartmentState.STA); // Set the thread to STA
        t.Start();
        t.Join();
        return selectedFilePath;
    }
    public void Reader()
    {
        string filePath = ReadTextFile();
        ObjectiveFunction = new List<int>();
        Constraints = new List<List<int>>();
        ConstraintSignRestrictions = new List<string>();
        SignRestrictions = new List<string>();
        ParseFile(filePath);
    }

    private void ParseFile(string filePath)
    {

        var lines = File.ReadAllLines(filePath);
        // Determine if the objective function is maximization or minimization

        if (lines[0].Trim().ToLower().StartsWith("max"))
        {
            IsMax = true;
        }
        else if (lines[0].Trim().ToLower().StartsWith("min"))
        {
            IsMax = false;
        }
        // Parse Objective Function
        var objFuncParts = lines[0].Split(' ').Skip(1).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        foreach (var part in objFuncParts)
        {
            if (int.TryParse(part.Trim('+'), out int value))
            {
                ObjectiveFunction.Add(value);
            }
        }
        // Parse Constraints
        for (int i = 1; i < lines.Length - 1; i++)
        {
            var constraintParts = lines[i].Split(new[] { " ", "<=", ">=", "=" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<int> constraint = new List<int>();

            // Extract coefficients for the constraint
            for (int j = 0; j < constraintParts.Count; j++)
            {
                //constraint.Add(constraintParts[j]);
                if (int.TryParse(constraintParts[j], out int number))
                {
                    constraint.Add(int.Parse(constraintParts[j]));
                }
            }
            if (lines[i].Contains(">="))
            {
                ConstraintSignRestrictions.Add(">=");
            }
            else if (lines[i].Contains("<="))
            {
                ConstraintSignRestrictions.Add("<=");

            }
            else if (lines[i].Contains("="))
            {
                ConstraintSignRestrictions.Add("=");
            }

            Constraints.Add(constraint);

            // Add the sign restriction
        }

        // Parse Sign Restrictions (usually last line in the text file)
        var signRestrictionsParts = lines.Last().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        SignRestrictions.AddRange(signRestrictionsParts);

    }
}
