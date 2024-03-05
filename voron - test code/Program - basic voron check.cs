
using Voron;

class Program_Basic {

    private static void Main_(string[] args) {


        
        writeToDB();
        readFromDB();

    }

    public static void writeToDB() {

        Console.WriteLine("Loading DB");


        using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath("Data"));
        string treeName = "items";


        Console.WriteLine("DB Loaded");


        using (var wtx = env.WriteTransaction())
        {

            var tree = wtx.CreateTree(treeName);


            for (int i = 0; i < 100; i++)
            {

                tree.Add(i.ToString(), (i * 100).ToString());
            }

            wtx.Commit();
            Console.WriteLine("Commited Changes.");
        }

        Console.WriteLine("End.");
    }

    public static void readFromDB() {


        Console.WriteLine("Loading DB");

        using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath("Data"));
        string treeName = "items";

        Console.WriteLine("DB Loaded");


        using (var rtx = env.ReadTransaction()) {

            var tree = rtx.ReadTree(treeName);
            using var it = tree.Iterate(prefetch: false);

            if (it.Seek(Slices.BeforeAllKeys)) {

                do {

                    unsafe {

                        Console.WriteLine(it.CurrentKey.ToString() + " " + it.CreateReaderForCurrent().ReadString(it.Current->DataSize));
                    }

                } while (it.MoveNext());
            }
        }

        Console.WriteLine("End.");
    }
}