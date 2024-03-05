
                using Voron;

                public class add_min_simple {

                    public static void Main(string[] args) {

                        

            using var instance = new StorageEnvironment(StorageEnvironmentOptions.ForPath("./FinalDatabaseState"));
            string treeName = "Items";


            using (var wtx = instance.WriteTransaction()) {

                var tree = wtx.CreateTree(treeName);

                for (int i = 0; i < 1; i++) {

                    tree.Add(i.ToString(), (i * 100).ToString());
                }

                wtx.Commit();
            }

            Console.WriteLine("Changes commited successfully.");
                    }
                }