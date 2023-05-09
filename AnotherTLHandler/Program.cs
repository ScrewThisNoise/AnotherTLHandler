using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AnotherTLHandler.functions;

namespace TranslationConverter
{
    internal static class Program
    {


        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Checking that the input file is a csv 
            if (args.Length == 0)
            {

            }
            else if (args[0].Remove(0, args[0].Length - 4) == ".csv")
            {
                Console.WriteLine("You've passed a csv file!");
            }
            else if (Directory.Exists(args[0]))
            {
                Console.WriteLine("You've passed a folder!");
            }
            else
            {
                Console.WriteLine("You haven't passed any arguments!");
                //Environment.Exit(0);
            }

        // Offering Menu
        Restart:
            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~BetterRepack Translation Manipulation~~~~~~~~~~~~~~~~~~\n");
            Console.WriteLine(
                "Welcome to the translation manipulation protocol, soldier! Choose something and get on with it! =P\n");
            Console.WriteLine("1. Convert csv to XUA compatible txts TODO");
            Console.WriteLine("2. CheckFile and populate duplicate h lines TODO");
            Console.WriteLine("3. Cleanup translation for release TODO");
            Console.WriteLine("4. Convert XUA folder to CSV TODO");
            Console.WriteLine("5. Turn h duplicate checked folder back into CSV TODO");
            Console.WriteLine("6. Turn cleanuped folder back into CSV TODO");
            Console.WriteLine("7. Remove Steam comparison from CSV TODO");
            Console.WriteLine("66. Create dupechecked and cleaned CSVs with KK");
            Console.WriteLine("67. Create dupechecked and cleaned CSVs without KK");
            Console.WriteLine("t. Cleanup repo (Remove translations, comments, etc)");
            Console.WriteLine("Q. Exit");
            Console.WriteLine("");
            Console.Write("Please enter your choice: ");

            var menuChoice = Console.ReadLine().ToLower();

            Console.WriteLine(menuChoice.ToUpper());
            Console.Clear();

            // Launching part based on user input
            Runner(menuChoice, args);
            goto Restart;
        }

        private static void Runner(string menuChoice, IReadOnlyList<string> args)
        {
            string arg;
            try
            {
                arg = args[0];
            }
            catch (Exception)
            {
                arg = "csvfile.csv";
            }
            switch (menuChoice)
            {
                case "1":
                    //CsvToXua.Run(false, arg, "Work");
                    break;
                case "2":
                    //FillHDupes.RunTask(false, "Work");
                    break;
                case "4":
                    //XUAtoCSV.StartTask();
                    break;
                case "66":
                    TransProgressClean.Runner(true, arg, "Output", true);
                    break;
                case "67":
                    TransProgressClean.Runner(true, arg, "Output", false);
                    break;
                case "t":
                    RepoBlankLineBSRemover.Runner();
                    break;
                default:
                    break;
            }
        }
    }
}
