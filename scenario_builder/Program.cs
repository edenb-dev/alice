using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voron;


using System.Runtime.InteropServices;


public class Program
{

    private static void Main(string[] args)
    {

        // test_basicAdd();
        check_basicAdd(args);

        // test_complexCharAdd();
        // check_complexCharAdd(args);

        // using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath("Data"));
        // Console.WriteLine("Hello World!");

        // example_toy();
        // Console.WriteLine("Hello World!");
    }



    [DllImport("libc", EntryPoint = "sync", SetLastError = false)]
    public static extern void sync();

    public static void example_toy()
    {

        // using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath("Data"));

        // if (!Directory.Exists("test"))
        // {

        //     DirectoryInfo di = Directory.CreateDirectory("test");
        //     // return;
        // }

        // Try to create the directory.


        // if (!File.Exists("tmp")) {
        // using (var file = File.Create("tmp"))
        //     file.Write("world"u8);

        // using (var file = File.Create("tmp"))
        //     file.Write("world"u8);
        // }

        // using (var fs = new FileStream("tmp", FileMode.Create, FileAccess.Write))
        //     using (var sw = new StreamWriter(fs))
        //     {
        //         sw.Write("world");
        //     }

        // FileStream("tmp", FileMode.OpenOrCreate, FileAccess.Write)
        // using (FileStream fs = new FileStream("tmp", FileMode.CreateNew, FileAccess.Write))
        // {
        //     byte[] info = new System.Text.UTF8Encoding(true).GetBytes("world");
        //     fs.Write(info, 0, info.Length);
        // }

        // File.Delete("tmp");

        // sync();


        // File.Move("tmp", "file1", true);
        // sync();

        using (var fs = new FileStream("tmp", FileMode.CreateNew, FileAccess.Write))
            File.Delete("tmp");

        Console.WriteLine("Updated");
    }


    public static void toy_cs()
    {


        using (var file = File.Create("tmp"))
            file.Write("world"u8);

        sync();


        File.Move("tmp", "file1", true);
        sync();

        Console.WriteLine("Updated");
    }

    public static void test_basicAdd()
    {

        var options = StorageEnvironmentOptions.ForPath(Directory.GetCurrentDirectory() + "/Data");
        options.ManualFlushing = true;
        options.ManualSyncing = true;

        using var env = new StorageEnvironment(options);

        using (var wtx = env.WriteTransaction())
        {
            var tree = wtx.CreateTree("Items");

            for (int i = 0; i < 100; i++)
            {
                tree.Add(i.ToString(), (i * 100).ToString());
            }

            wtx.Commit();
        }

        Console.WriteLine("Commit Finished.");

        env.FlushLogToDataFile();

        Console.WriteLine("FlushLogToDataFile Finished.");

        env.ForceSyncDataFile();

        Console.WriteLine("ForceSyncDataFile Finished.");
    }

    public static void check_basicAdd(string[] args)
    {

        // Loading crash step data.

        string[] steps = extract_stdout(args);


        // Updating synthesized DB state.

        Dictionary<string, string> synthesizedDB = new Dictionary<string, string>();

        for (int i = 0; i < 100; i++)
        {
            synthesizedDB.Add(i.ToString(), (i * 100).ToString());
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

