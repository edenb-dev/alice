
                using Voron;

                public class add_min_simple {

                    public static void Main(string[] args) {

                        

            using var instance = new StorageEnvironment(StorageEnvironmentOptions.ForPath("./FinalDatabaseState"));

            Console.WriteLine("Changes commited successfully.");
                    }
                }