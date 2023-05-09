using AnotherTLHandler.functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnotherTLHandler.functions
{
    internal class CreateDupeDictionaries
    {
        public static void Runner(bool translationClean, string workingDir, string FreshTLFile, string CharaType, bool includeKK)
        {
            CreateDictionary(translationClean, workingDir, FreshTLFile, "H", includeKK, CharaType);
            CreateDictionary(translationClean, workingDir, FreshTLFile, "Communication", includeKK, CharaType);
        }

        private static void CreateDictionary(bool translationClean, string workingDir, string FreshTLFile, string type,
            bool IncludeKK, string CharaType)
        {
            var linesKKS = 0;
            Console.Write($@"Handling lines in {type} for c{CharaType} under KKS. ");
            string splitchar = ";";
            if (FreshTLFile.Contains("tsv")) splitchar = "\t";

            Misc.EnsureDirectoryExists("Data\\DupeLib");
            var MasterWriter = Misc.MakeWriter($@"Data\DupeLib\c{CharaType}_{type}_KKS.txt", false);

            // Adding in stuffs from input translation
            var FreshTLReader = File.ReadAllLines(FreshTLFile);
            string currentSection = string.Empty;

            foreach (var FreshTLLine in FreshTLReader)
            {
                var currentFreshLineSplit = FreshTLLine.Split(splitchar);

                if (currentFreshLineSplit[0].Contains($@"abdata\{type.ToLower()}\"))
                {
                    currentSection = currentFreshLineSplit[0];
                    continue;
                }

                if (currentSection.Contains($@"abdata\{type.ToLower()}\") && currentFreshLineSplit[1] != "")
                {
                    try
                    {
                        if (translationClean)
                        {
                            try
                            {
                                MasterWriter.WriteLine(
                                    $"{currentFreshLineSplit[0]}={currentFreshLineSplit[1]}={currentFreshLineSplit[2]}");
                                linesKKS++;
                            }
                            catch (Exception)
                            {
                                MasterWriter.WriteLine($"{currentFreshLineSplit[0]}={currentFreshLineSplit[1]}="); 
                                linesKKS++;
                            }
                        }
                        else
                        {
                            MasterWriter.WriteLine($"{currentFreshLineSplit[0]}={currentFreshLineSplit[1]}");
                            linesKKS++;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            Console.WriteLine($@"Written {linesKKS} lines.");

            // Adding in KK stuffs
            if (IncludeKK)
            {
                int linesKK = 0;
                Console.Write($@"Handling lines in {type} for c{CharaType} under KK. ");
                var MasterWriterKK = Misc.MakeWriter($@"Data\DupeLib\c{CharaType}_{type}_KK.txt", false);
                Dictionary<string, string> MasterDictionary = new Dictionary<string, string>();
                foreach (string TLFile in Directory.EnumerateFiles($@"Data\abdataKK\{type}", "*.txt",
                             SearchOption.AllDirectories))
                {
                    if (!Misc.CheckChar(CharaType, TLFile)) continue;

                    var file = File.ReadAllLines(TLFile);
                    foreach (var currentLine in file)
                    {
                        var line = currentLine.Split('=');
                        try
                        {
                            if (line[1] == "") continue;
                            MasterWriterKK.WriteLine($@"{line[0]}={line[1]}=");
                            linesKK++;
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
                MasterWriterKK.Close();
                Console.WriteLine($@"Written {linesKK} lines.");
            }
            MasterWriter.Close();
        }
    }
}