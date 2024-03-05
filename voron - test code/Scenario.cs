
using System.Text.RegularExpressions;


public class Scenario
{


    public Operation[] operations;


    public Scenario(Operation[] operations)
    {


        this.operations = operations;

        validator();
        compileScenario();
    }


    /// <summary>
    /// Validates the sequence of operations in the "operations" array.
    /// </summary>
    /// 
    /// <returns>True if the sequence is valid, false otherwise.</returns>
    /// 
    private bool validator()
    {

        Type[] validOperations = validateOperationSequences();


        // Checking for invalid operation type.
        foreach (var operation in this.operations)
        {

            if (!validOperations.Contains(operation.GetType()))
                return false;
        }

        return true;
    }


    /// <summary>
    /// Determines valid operation types based on the first operation in the "operations" array.
    /// </summary>
    /// 
    /// <returns>An array of valid operation types, or throws an exception if the first operation is not supported.</returns>
    ///
    /// <exception cref="UnsupportedOperationException">Thrown if the first operation type is not recognized.</exception>

    private Type[] validateOperationSequences()
    {

        var operation = this.operations[0];


        if (operation is AddOperation) //  or AddOperation .... 
            return new Type[] { typeof(AddOperation) };


        // Error: Can't validated. (operation not found)
        throw new NotImplementedException("Validation for this operation type is not implemented yet.");
    }


    private void compileScenario()
    {

        compileScenarioTest();
        //compileScenarioChecker();
    }

    private String compileScenarioTest()
    {


        String output = "\r\n            // Connect.\r\n            using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath(path));";
        String currentOperation;

        // Extracting & cleans each operation code.
        for (int i = 0; i < this.operations.Length; i++)
        {

            currentOperation = "";

            // Adding the setting to the operation.
            currentOperation += "\n\n\t    // Settings.\n"
                                + $"\t    {(i == 0 ? "String " : "")}treeName = \"{this.operations[i].settings["treeName"]}\";\n"
                                + $"\t    {(i == 0 ? "int " : "")}startIndex = {this.operations[i].settings["startIndex"]};\n"
                                + $"\t    {(i == 0 ? "int " : "")}endIndex = {this.operations[i].settings["endIndex"]};\n\n";


            // Extracting operation code.
            currentOperation += Utilities.GetFunctionSourceCode($"./{this.operations[i].GetType().Name}.cs", "genOP")
                                + "\n\n\t    }\n\n\t    "
                                + $"Console.WriteLine(\"Stage {i + 1}\");"
                                + "\n";


            // Sanitizing output.
            currentOperation = currentOperation.Replace("\r\n\r\n            // Connect.\r\n            using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath(path));\r\n\r\n\r\n", "");

            String compileType = "mode";
            var matches = Regex.Matches(currentOperation, @"(.*(?<=@Compile - " + compileType + @":)(.*))((?:(?!@Compile - ).|\n)+?)(?= *\/\/ @Compile - mode|$)");

            if (matches != null)

                // Removes un-selected code.
                foreach (Match match in matches.Take(matches.Count - 1))

                    if (!match.Groups[2].Value.Trim().Equals(this.operations[i].settings["mode"]))
                        currentOperation = currentOperation.Replace(match.Value, "");



            // Saving to output.
            output += currentOperation;
        }


        Console.WriteLine($"\n    using Voron;\n\n    public class Program {{\n\n\tpublic static void Main(string[] args) {{\n{output}\n\t}}\n    }}");

        return output;
    }

    private String compileScenarioChecker()
    {

        return "";
    }

}
