using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnotherTLHandler.functions
{
    internal class CreateCSV
    {
        public static void Runner(bool translationClean, string workingDir, string charaType, string folderName)
        {
            Console.Write("Writing CSV. ");
            string errorOutput = $@"Output\CSV_Errors_c{charaType}.txt";
            Misc.EnsureFileDel(errorOutput);
            string output = $@"{workingDir}\CSVExport\";
            string csvFilename = $@"{output}\c{charaType}.csv";
            bool firstLine = true; 
            
            Misc.EnsureDirectoryExists(output);
            Misc.EnsureFileDel(csvFilename);

            if (Misc.CheckFileExists(csvFilename)) firstLine = false;
            var outputfile = Misc.MakeWriter(csvFilename, false);
            var filesList = new List<string>();

            foreach (var currentFile in Directory.EnumerateFiles(folderName, "*.txt", SearchOption.AllDirectories)) filesList.Add(currentFile);
            filesList.Sort();

            outputfile.WriteLine("Original;Current Fantrans;TL Note\n;;\n;;");

            foreach (var currentFile in filesList)
            {
                if (!Misc.CheckChar(charaType, currentFile)) continue;

                var newDoc = true;
                var translationLines = File.ReadAllLines(currentFile);

                string filenameforex = currentFile.Remove(0, workingDir.Length + 5);
                outputfile.WriteLine(filenameforex + ";;");

                foreach (string line in translationLines)
                {
                    var splitLine = line.Split('=');
                    try
                    {
                        outputfile.WriteLine($@"{splitLine[0]};{splitLine[1]};{splitLine[2]}");
                    }
                    catch (Exception)
                    {
                        outputfile.WriteLine($@"{splitLine[0]};{splitLine[1]};");
                    }
                }
                outputfile.WriteLine(";;");
                outputfile.WriteLine(";;");
            }
            outputfile.Close();

            Console.WriteLine("OK!");
        }
    }
}
