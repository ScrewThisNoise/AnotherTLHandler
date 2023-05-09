using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnotherTLHandler.functions
{
    internal class TransProgressClean
    {
        public static void Runner(bool translationClean, string inputFile, string workFolder, bool includeKK)
        {
            Misc.EnsureDirectoryExists("Data");
            Misc.EnsureDirectoryExists("Input");
            Misc.EnsureDirectoryExists("Output");

            string[] fileexts = { "csv", "tsv"};

            var fileList = fileLister(fileexts);

            if (fileList.Count == 0)
            {
                Console.WriteLine("Nothing to handle!");
            }

            foreach (var workingFile in fileList)
            {
                Handler(translationClean, inputFile, workFolder, includeKK, workingFile);
            }
        }

        private static List<string> fileLister(string[] fileexts)
        {
            var fileList = new List<string>();

            foreach (var file in fileexts)
            {
                foreach (var FreshTLFile in Directory.EnumerateFiles(@"Input", @$"*.{file}", SearchOption.TopDirectoryOnly))
                {
                    fileList.Add(FreshTLFile);
                }
            }
            return fileList;
        }

        private static void Handler(bool translationClean, string inputFile, string workFolder, bool includeKK, string FreshTLFile)
        {
            // Capturing character code
            string currentPersType = FreshTLFile.Remove(FreshTLFile.Length - 4, 4).Remove(0, 7);

            // TODO: check that folders are cleaned and remove testing things
            //Misc.EnsureDirectoryDel("Data\\abdataKK-RAW-hashes");
            //Misc.EnsureDirectoryDel("Data\\abdataKKS-RAW-hashes");
            Misc.EnsureDirectoryDel($@"Output\c{currentPersType}");
            string personalityName = Misc.getCharaTypeName(currentPersType);

            Console.WriteLine($@"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($@"Currently working on c{currentPersType} ({personalityName}).");

            MergeWithRepo.Runner(translationClean, workFolder, FreshTLFile, currentPersType);

            CreateDupeDictionaries.Runner(translationClean, workFolder, FreshTLFile, currentPersType, true);
            ADVMatcher.Runner(translationClean, workFolder, currentPersType);
            MergeDupes.Runner(translationClean, currentPersType, includeKK);
            Matcher.Runner(currentPersType);
            StyleEnforcement.Runner(translationClean, currentPersType);
            CreateCSV.Runner(translationClean, workFolder, currentPersType, @$"Output\c{currentPersType}\abdata");
            ZipHandler.Runner(workFolder, currentPersType, personalityName);
        }
    }
}
