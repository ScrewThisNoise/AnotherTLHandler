using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnotherTLHandler.functions
{
    internal class Matcher
    {
        public static void Runner(string currentPersType)
        {
            string workFolder = $@"Output\c{currentPersType}\";
            string inFile = $@"Input\c{currentPersType}.tsv";
            string splitter = "\t";

            Console.Write("Writing translations from Input. ");

            if (Misc.CheckFileExists($@"Input\c{currentPersType}.csv"))
            {
                splitter = ";";
                inFile = $@"Input\c{currentPersType}.csv";
            }

            var workLines = File.ReadAllLines(inFile);

            var outFiles = Directory.EnumerateFiles(workFolder, "*.txt", SearchOption.AllDirectories);
            var filesList = new List<string>();

            foreach (string s in outFiles) filesList.Add(s);
            filesList.Sort();

            foreach (var currentFile in filesList)
            {
                string outFileName = currentFile.Remove(0, workFolder.Length);
                var outFileLines = File.ReadAllLines(currentFile);
                List<string> newLines = new List<string>();

                foreach (string Outline in outFileLines)
                {
                    string result = Outline;
                    string currentInPart = string.Empty;
                    bool hit = false;
                    var splitOutfile = Outline.Split('=');
                    foreach (string InLine in workLines)
                    {
                        var splitInLine = InLine.Split(splitter);
                        if (splitInLine[0].Contains("abdata") && currentInPart.Contains("abdata")) break;
                        if (splitInLine[0].Contains("abdata") && splitInLine[0] == outFileName) currentInPart = splitInLine[0];
                        if (currentInPart.Contains("abdata"))
                        {
                            if (splitInLine[0] == splitOutfile[0] && splitInLine[1] != "")
                            {
                                try
                                {
                                    result = $@"{splitInLine[0]}={splitInLine[1]}={splitInLine[2]}";
                                    hit = true;
                                }
                                catch (Exception)
                                {
                                    result = $@"{splitInLine[0]}={splitInLine[1]}=";
                                    hit = true;
                                }
                            }
                        }
                        if ( hit ) break;
                    }
                    newLines.Add(result);
                }
                File.WriteAllLines(currentFile, newLines.ToArray());
            }

            Console.WriteLine("OK!");
        }
    }
}
