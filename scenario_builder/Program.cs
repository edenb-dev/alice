using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voron;


public class Program
{

    private static void Main(string[] args)
    {

        // check_basicAdd(args);
        //check_complexCharAdd(args);
        {
            using var f = File.Create("yellow");
            f.Write("world"u8);
        }
        File.Move("yellow", "file1");
        Console.WriteLine("DoneDoneDoneDoneDoneDoneDoneDoneDoneDoneDoneDoneDoneDone");




    }

    public static void test_basicAdd()
    {

        using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath("Data"));
        using (var wtx = env.WriteTransaction())
        {
            var tree = wtx.CreateTree("Items");

            for (int i = 0; i < 100; i++)
            {
                tree.Add(i.ToString(), (i * 100).ToString());
            }

            wtx.Commit();
        }

        Console.WriteLine("Step 1");
    }

    public static void check_basicAdd(string[] args)
    {

        // Loading crash step data.

        string[] steps = extract_stdout(args);


        // Updating synthesized DB state.

        Dictionary<string, string> synthesizedDB = new Dictionary<string, string>();

        for (int i = 0; i < 100; i++)
        {
            synthesizedDB.Add(i.ToString(), (i * 1001).ToString());
        }

        // Checking.

        int counter = 0;

        using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath("Data"));
        using (var rtx = env.ReadTransaction())
        {
            var tree = rtx.ReadTree("Items");
            using var it = tree.Iterate(prefetch: false);

            String key;
            String value;

            if (it.Seek(Slices.BeforeAllKeys))
            {

                do
                {
                    unsafe
                    {
                        key = it.CurrentKey.ToString();
                        value = it.CreateReaderForCurrent().ReadString(it.Current->DataSize);

                        if (synthesizedDB[key].Equals(value))
                            counter++;

                        else
                        {
                            Debug.Assert(false, $"ERROR: key or value not correct.\t(key: {key}  |  value: {value})");
                        }
                    }
                } while (it.MoveNext());
            }
        }


        // durability check.       
        if (Array.IndexOf(steps, "Step 1") > -1)
        {

            Debug.Assert(counter == synthesizedDB.Count, "Durability check failed. at step 1");
        }

        // atomicity check.
        else
        {

            Debug.Assert(counter == synthesizedDB.Count || counter == 0, "Atomicity check failed. at step 0");
        }
    }


    public static void test_complexCharAdd()
    {

        using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath("Data"));
        using (var wtx = env.WriteTransaction())
        {
            var tree = wtx.CreateTree("Items");

            for (int i = 0; i < 2048; i++)
            {
                string charString = Char.ConvertFromUtf32(i);

                if (charString.Length != 1)
                    charString = i.ToString();

                tree.Add(i.ToString(), charString.ToString());
            }

            wtx.Commit();
        }

        Console.WriteLine("Step 1");
    }


    public static void check_complexCharAdd(string[] args)
    {

        // Loading crash step data.

        string[] steps = extract_stdout(args);


        // Updating synthesized DB state.

        Dictionary<string, string> synthesizedDB = new Dictionary<string, string>();

        for (int i = 0; i < 2048; i++)
        {
            string charString = Char.ConvertFromUtf32(i);

            if (charString.Length != 1)
                charString = i.ToString();

            synthesizedDB.Add(i.ToString(), charString.ToString());
        }

        // Checking.

        int counter = 0;

        using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath("Data"));
        using (var rtx = env.ReadTransaction())
        {
            var tree = rtx.ReadTree("Items");
            using var it = tree.Iterate(prefetch: false);

            String key;
            String value;

            if (it.Seek(Slices.BeforeAllKeys))
            {

                do
                {
                    unsafe
                    {
                        key = it.CurrentKey.ToString();
                        value = it.CreateReaderForCurrent().ReadString(it.Current->DataSize);

                        if (synthesizedDB[key].Equals(value))
                            counter++;

                        else
                        {
                            Debug.Assert(false, $"ERROR: key or value not correct.\t(key: {key}  |  value: {value})");
                        }
                    }
                } while (it.MoveNext());
            }
        }


        // durability check.       
        if (Array.IndexOf(steps, "Step 1") > -1)
        {

            Debug.Assert(counter == synthesizedDB.Count, "Durability check failed. at step 1");
        }

        // atomicity check.
        else
        {

            Debug.Assert(counter == synthesizedDB.Count || counter == 0, "Atomicity check failed. at step 0");
        }
    }




    public static String[] extract_stdout(string[] args)
    {

        // Loading crash step data.

        string[] Steps;

        String stdout = File.ReadAllText(args[1]);
        Steps = stdout.Split(new string[] { "\n" }, StringSplitOptions.None);

        return Steps.Take(Steps.Count() - 1).ToArray();

        /*
        // DEBUG
        using (StreamWriter sw = File.AppendText("./print.txt")) {


            String stdout = File.ReadAllText(args[1]);
            Steps = stdout.Split(new string[] { "\n" }, StringSplitOptions.None); // Should ignore last.

            // DEBUG
            //sw.WriteLine(stdout + "\n\n length:" + Steps.Length);     
        }
        */
    }
    private static void createTest1()
    {

        // Build Test 1
        Scenario test1 = new Scenario(new Operation[] { new AddOperation( new Dictionary<string, string>() { ["mode"] = "complex", ["startIndex"] = "20", ["endIndex"] = "100" }),
                                                        new AddOperation( new Dictionary<string, string>() { ["mode"] = "default", ["startIndex"] = "0", ["endIndex"] = "30" })});


        //Scenario test1 = new Scenario(new Operation() { new AddOperation("JSON{ input:Complex, inputSize:30MB }"), DeleteOperation("JSON{ startIndex: 0, endIndex: end, iteration: %2 == 0 }") });
    }
}

