using AnotherTLHandler.functions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace AnotherTLHandler.functions
{
    internal class ADVMatcher
    {
        public static void Runner(bool translationClean, string workFolder, string charaType)
        {
            // Generating dictionary of the whole ADV for KK as relates to the character type in question
            Dictionary<string, string> fileDictionaryKK = createKKDict(charaType, "KK");
            Dictionary<string, string> fileDictionaryKKS = createKKDict(charaType, "KKS");

            Console.Write($@"Attempting to match already translated KK files with KKS files. ");
            int dupeMatches = 0;
            
            bool newfile = !File.Exists("Output\\DupeList.csv");

            var DupWriter = Misc.MakeWriter($@"DupeList.csv", true);
            if (newfile) DupWriter.WriteLine("Koikatsu File;Koikatsu Sunshine File");

            // Trying to match
            foreach (string advFile in Directory.EnumerateFiles($@"Data\abdataKKS-RAW\adv", "*.txt", SearchOption.AllDirectories))
            {
                if (!advFile.Contains("c" + charaType)) continue;

                string currentMD5 = Misc.CalculateMD5(advFile);

                string directoryIn = Path.GetDirectoryName($@"{workFolder}\c{charaType}\{advFile.Replace("Data\\abdataKKS-RAW", "abdata")}");
                string directoryOut = Path.GetDirectoryName($@"{workFolder}\c{charaType}\{advFile.Replace("Data\\abdataKKS-RAW", "abdata")}");

                try
                {
                    fileDictionaryKK.Add(currentMD5, directoryIn);
                    //Console.WriteLine($"New: {directoryIn}\\translation.txt");
                }
                catch (Exception)
                {
                    if (!fileDictionaryKK[currentMD5].Contains("Output"))
                    {
                        directoryIn = fileDictionaryKK[currentMD5].Replace("abdata", "abdataKK").Replace("translation.txt", "");
                        DupWriter.WriteLine($@"{fileDictionaryKK[currentMD5]};{fileDictionaryKKS[currentMD5]}");
                    }

                    dupeMatches++;
                    //Console.WriteLine($"Dupe: {directoryIn}\\translation.txt");
                }

                if (directoryIn == directoryOut) continue;

                Misc.EnsureDirectoryExists(directoryOut);
                Misc.EnsureFileDel($@"{directoryOut}\translation.txt");
                File.Copy($@"{directoryIn}\translation.txt", $@"{directoryOut}\translation.txt");
            }

            Console.WriteLine($@"Matched {dupeMatches} files.");
        }

        private static Dictionary<string, string> createKKDict(string charaType, string game)
        {
            Dictionary<string, string> fileDictionary = new Dictionary<string, string>();

            string OutFolder = $@"Data\abdata{game}-RAW-hashes\";
            int HashNumber = 0;

            Console.Write($@"Looking for unique files in ADV for c{charaType}. ");

            if (!File.Exists($@"{OutFolder}\c{charaType}.txt"))
            {
                Misc.EnsureDirectoryExists($@"{OutFolder}");

                // Capturing MD5 and writing it to a file for later capture
                var file = Misc.MakeWriter($@"{OutFolder}c{charaType}.txt", true);
                foreach (var currentFile in Directory.EnumerateFiles($@"Data\abdata{game}-RAW\adv", $@"*.txt", SearchOption.AllDirectories))
                {
                    if (!Misc.CheckChar(charaType, currentFile)) continue;

                    string currentMD5 = Misc.CalculateMD5(currentFile);
                    try
                    {
                        string currentFileFix = currentFile.Replace($@"abdata{game}-RAW", $@"abdata");
                        fileDictionary.Add(currentMD5, currentFileFix);
                        file.WriteLine($@"{currentMD5};{currentFileFix}");
                        HashNumber++;
                    }
                    catch
                    {

                    }
                }
                file.Close();
            }
            else
            {
                // Reading the MD5s from file
                var file = File.ReadAllLines($@"{OutFolder}\c{charaType}.txt");
                foreach (var currentLine in file)
                {
                    var line = currentLine.Split(';');
                    fileDictionary.Add(line[0], line[1]);
                    HashNumber++;
                }
            }
            Console.WriteLine($@"Found and hashed {HashNumber} unique files.");
            return fileDictionary;
        }
    }
}
