using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnotherTLHandler.functions
{
    internal class MergeWithRepo
    {
        public static void Runner(bool translationClean, string workingDir, string FreshTLFile, string CurrentPers)
        {
            string currentFileSection = String.Empty;
            string abdataFolder = @"Data\abdataKKS";
            string output = $@"Output\c{CurrentPers}\abdata";

            Console.Write("Attempting to merge Input with updated Repo. ");

            foreach (var RepoTLFile in Directory.EnumerateFiles(abdataFolder, "*.txt", SearchOption.AllDirectories))
            {
                if (!Misc.CheckChar(CurrentPers, RepoTLFile)) continue;
                Misc.EnsureDirectoryExists(output);

                var FreshTLReader = File.ReadAllLines(FreshTLFile);
                var RepoTLReader = File.ReadAllLines(RepoTLFile);

                var outDir = output + RepoTLFile.Remove(0, 14).Replace("translation.txt", "");
                Misc.EnsureDirectoryExists(outDir);
                var fileWriter = Misc.MakeWriter($@"{output}{RepoTLFile.Remove(0,14)}", false);

                foreach (string currentRepoLine in RepoTLReader)
                {
                    var currentRepoLineSplit = currentRepoLine.Replace("//", "").Split('=');
                    var hit = false;

                    if (currentRepoLineSplit[0] == "")
                    {
                        fileWriter.WriteLine("");
                        continue;
                    }

                    foreach (var FreshTLLine in FreshTLReader)
                    {
                        var currentFreshLineSplit = FreshTLLine.Split(';');

                        if (currentFreshLineSplit[0].Contains(@"abdata\adv\scenario") ||
                            currentFreshLineSplit[0].Contains(@"abdata\communication") ||
                            currentFreshLineSplit[0].Contains(@"abdata\esthetic") ||
                            currentFreshLineSplit[0].Contains(@"abdata\h"))
                            currentFileSection = currentFreshLineSplit[0];

                        if (currentFileSection != RepoTLFile || hit || currentFreshLineSplit[0].Replace(" ", "").Replace("　", "") !=
                            currentRepoLineSplit[0].Replace(" ", "").Replace("　", "")) continue;

                        var result = $"{currentRepoLineSplit[0]}={currentFreshLineSplit[1]}";
                        if (translationClean)
                            try
                            {
                                result += $"={currentFreshLineSplit[2]}";
                            }
                            catch (Exception)
                            {
                                result += "=";
                            }

                        fileWriter.WriteLine(result);
                        hit = true;
                    }

                    if (!hit)
                    {
                        var result = $"{currentRepoLineSplit[0]}=";
                        if (translationClean)
                            result += "=";

                        fileWriter.WriteLine(result);
                    }
                }
                fileWriter.Close();
            }
            Console.WriteLine("OK!");
        }
    }
}
