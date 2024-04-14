using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voron;


public class Program {

    private static void Main(string[] args) {

        check_basicAdd(args);
    }

    public static void test_basicAdd() {

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

    public static void check_basicAdd(string[] args) {


        // Loading crash step data.

        string[] Steps;

        using (StreamWriter sw = File.AppendText("./print.txt")) {


            String stdout = File.ReadAllText(args[1]);
            Steps = stdout.Split(new string[] { "\n" }, StringSplitOptions.None); // Should ignore last.

            // DEBUG
            //sw.WriteLine(stdout + "\n\n length:" + Steps.Length);     
        }



        // Checking.


        Dictionary<string, string> finalDB = new Dictionary<string, string>();

        for (int i = 0; i < 100; i++)
        {
            finalDB.Add(i.ToString(), (i * 100).ToString());
        }

        int counter = 0;

        using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath("Data"));
        using (var rtx = env.ReadTransaction())
        {
            var tree = rtx.ReadTree("Items");
            using var it = tree.Iterate(prefetch: false);
            if (it.Seek(Slices.BeforeAllKeys))
            {
                
                do
                {
                    unsafe
                    {
                        String key = it.CurrentKey.ToString();
                        String value = it.CreateReaderForCurrent().ReadString(it.Current->DataSize);
                        if (finalDB[key].Equals(value))
                        {
                       
                            counter++;
                        }
                    }
                } while (it.MoveNext());
            }
        }

        // need to add durability
        

        if (counter == finalDB.Count || counter == 0)
        {
            System.Console.WriteLine("The transaction is atomicity");
        }
        else
        {
            System.Console.WriteLine("The transaction isn't atomicity");

        }

    }


    private static void createTest1()
    {

        // Build Test 1
        Scenario test1 = new Scenario(new Operation[] { new AddOperation( new Dictionary<string, string>() { ["mode"] = "complex", ["startIndex"] = "20", ["endIndex"] = "100" }),
                                                        new AddOperation( new Dictionary<string, string>() { ["mode"] = "default", ["startIndex"] = "0", ["endIndex"] = "30" })});


        //Scenario test1 = new Scenario(new Operation() { new AddOperation("JSON{ input:Complex, inputSize:30MB }"), DeleteOperation("JSON{ startIndex: 0, endIndex: end, iteration: %2 == 0 }") });
    }
}

