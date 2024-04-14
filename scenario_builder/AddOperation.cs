using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Voron;


    public class AddOperation : Operation
    {

        /* operation setting default values */
        

        private string path = "Data";
        private string treeName = "Items";

        private int startIndex = 0;
        private int endIndex = 100;



        Dictionary<string, string> synthesizedDB;
        private int charRangeStart = 15;
        private int charRangeEnd = 15;


        public AddOperation(Dictionary<string, string> settings) {

            this.settings = settings;
     
            processOperationSettingsDict();
        }

        private void processOperationSettingsDict() {

            // Shared settings.

            // document name.
            if (!settings.ContainsKey("treeName"))
                settings["treeName"] = treeName;


            // key range.
            if (!settings.ContainsKey("startIndex"))
                settings["startIndex"] = startIndex.ToString();

            if (!settings.ContainsKey("endIndex"))
                settings["endIndex"] = endIndex.ToString();


            // Additional settings for AddOperation.

            // mode.
            if (!settings.ContainsKey("mode"))
                settings["mode"] = "default";
        }


        public void genOP() {


            // Connect.
            using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath(path));


            // Write.
            using (var wtx = env.WriteTransaction()) {

                var tree = wtx.CreateTree(treeName);


                for (int i = startIndex; i < endIndex; i++) {

                    // @Compile - mode:default
                    tree.Add(i.ToString(), (i * 100).ToString());

                    // @Compile - mode:complex
                    string charString = Char.ConvertFromUtf32(i);

                    if (charString.Length != 1)
                        charString = i.ToString();

                    tree.Add(i.ToString(), charString);
                    // @Compile - mode:END
                }


                wtx.Commit();
            }
        }



        public void updateSynthesizedDB(){


            for (int key = startIndex; key < endIndex; key++) {

                // @Compile - mode:default
                synthesizedDB[key.ToString()] = (key * 100).ToString();

                // @Compile - mode:complex
                string charString = Char.ConvertFromUtf32(key);

                if (charString.Length != 1)
                    charString = key.ToString();

                synthesizedDB[key.ToString()] = charString;
                // @Compile - mode:END
            }
        }



        public bool genOPCheck(Dictionary<string, string> synthesizedDB) {


            using var env = new StorageEnvironment(StorageEnvironmentOptions.ForPath(path));
            int counter = 0;

            // read db.
            using (var rtx = env.ReadTransaction())
            {

                var tree = rtx.ReadTree(treeName);
                using var it = tree.Iterate(prefetch: false);

                if (it.Seek(Slices.BeforeAllKeys))
                {

                    do
                    {
                        unsafe
                        {

                            // key exits in db and checker
                            if (synthesizedDB.ContainsKey(it.CurrentKey.ToString()))
                            {

                                // if value of key are the same.
                                if (synthesizedDB[it.CurrentKey.ToString()].Equals(it.CreateReaderForCurrent().ReadString(it.Current->DataSize)))

                                    counter++;


                                else // wrong value. db corrupted.
                                    return false;
                            }

                            else // key does not exits. db corrupted.
                                return false;
                        }

                    } while (it.MoveNext());
                }
            }



            // durability test.
            if ("currentStep".Equals("Step 1"))
            {

                // Check if all the data is the same in the db and in the checker.
                if (synthesizedDB.Count() == counter)
                    return true;

                // Error: some items are missing from the database.
                else
                    return false;

            }

            // atomicity test.
            else
            {

                if (counter == 0 || synthesizedDB.Count() == counter)
                    return true;

                // Error: some items are missing from the database.
                else
                    return false;
            }

        }


    }

