
using Voron;

using System.Text.RegularExpressions;



/// <summary>
/// Contains utility methods for Voron database operations and code extraction.
/// </summary>

internal static class Utilities {

    /// <summary>
    /// Deletes a Voron database instance at the specified location.
    /// </summary>
    /// <param name="path">The file path of the database to be deleted.</param>
    /// <returns>True if deletion is successful or no instance of a database is found at the provided path, otherwise false.</returns>

    public static bool deleteDatabaseInstance(string path) {

        // No instance of a database could be found at the provided path.
        if (!Directory.Exists(path))
            return true;
            
        try {

            Directory.Delete(path, true);
            return true;
        }
        catch (IOException e) { Console.WriteLine(e.Message); return false; }
    }

    /// <summary>
    /// Initializes a new Voron database instance at the specified location.
    /// </summary>
    /// <param name="path">The file path to initialize the database.</param>
    /// <returns>True if initialization is successful, otherwise false.</returns>

    public static bool initializeDatabaseInstance(string path) {

        // A directory with the identical database name already exists.
        if (Directory.Exists(path))
            return false;

        try {

            using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath(path));
            return true;
        }
        catch (IOException e) { Console.WriteLine(e.Message); return false; }
    }

    /// <summary>
    /// Retrieves the source code of a specific function from a file.
    /// </summary>
    /// <param name="filePath">The path to the file containing the function.</param>
    /// <param name="functionName">The name of the function to extract.</param>
    /// <returns>The source code of the specified function.</returns>

    public static string GetFunctionSourceCode(string filePath, string functionName) {

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found at path '{filePath}'.");


        string fileContent = File.ReadAllText(filePath);
        /*  //$@"{functionName}\s*\(.*?\)\s*(?<content>{{(?:[^{{}}]+|(?<content>{{)|(?<-content>}}))*?;)";  -> old
            //({functionName}\s*\(.*?\)\s)({((?>[^{}]+|(?2))*)})  -> best - need external libs.*/
        string pattern = $@"(?i){functionName}(.*?\n)(.*?)(?=public|private\b)";
        Regex regex = new Regex(pattern, RegexOptions.Singleline);

        Match match = regex.Match(fileContent);

        if (match.Success)
            return match.Groups[2].Value.TrimEnd().TrimEnd('}'); // Function body.

        else
            throw new InvalidOperationException($"Function '{functionName}' not found in the file.");
    }
}

