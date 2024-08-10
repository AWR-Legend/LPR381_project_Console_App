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
