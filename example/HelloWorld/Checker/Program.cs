
public class Program {

    private static void Main(string[] args) {


        // Getting the input from alice. (crash dir & stdout file)

        using (var stream = File.AppendText("./alice_checker_input.txt")) {

            stream.WriteLine("======== START ========");

            foreach (string arg in args) { 

                stream.WriteLine(arg);
            }

            stream.WriteLine("========= END =========" + "\n\n");
        }


        // Saving the stdout to a file.

        using (var stream = File.AppendText("./alice_checker_stdout_received.txt")) {

            string alice_output = File.ReadAllText(args[1]);

            stream.WriteLine("======== START ========");

            stream.WriteLine(alice_output);

            stream.WriteLine("========= END =========" + "\n\n");
        }
    }
}