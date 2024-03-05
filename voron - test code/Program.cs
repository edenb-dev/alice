using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class Program {

    private static void Main(string[] args) {


        createTest1();
    }


    private static void createTest1(){

        // Build Test 1
        Scenario test1 = new Scenario(new Operation[] { new AddOperation( new Dictionary<string, string>() { ["mode"] = "complex", ["startIndex"] = "20", ["endIndex"] = "100" }),
                                                        new AddOperation( new Dictionary<string, string>() { ["mode"] = "default", ["startIndex"] = "0", ["endIndex"] = "30" })});


        //Scenario test1 = new Scenario(new Operation() { new AddOperation("JSON{ input:Complex, inputSize:30MB }"), DeleteOperation("JSON{ startIndex: 0, endIndex: end, iteration: %2 == 0 }") });
    }
}

