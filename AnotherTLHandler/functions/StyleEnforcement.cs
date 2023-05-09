using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace AnotherTLHandler.functions
{
    internal class StyleEnforcement
    {
        public static void Runner(bool translationClean, string CharaType)
        {
            Console.Write($@"Applying Repo Style rules to c{CharaType}. ");
            string workingDir = @$"Output\c{CharaType}\abdata\";
            int fileNum = 0;

            foreach (var currentFile in Directory.EnumerateFiles(workingDir, "*.txt", SearchOption.AllDirectories))
            {
                var lines = File.ReadAllLines(currentFile);
                int linenum = 0;
                List<string> newLines = new List<string>();

                foreach (var line in lines)
                {
                    string[] LineSplit = line.Split('=');
                    string result = String.Empty;

                    try
                    {
                        string temp = RunFix(LineSplit[1]);
                        result = $"{LineSplit[0]}={temp}";
                        if (translationClean)
                        {
                            try
                            {
                                result += $@"={LineSplit[2]}";
                            }
                            catch (Exception)
                            {
                                result += $@"=";
                            }
                        }
                    }
                    catch (Exception)
                    {
                        result = line;
                    }
                    newLines.Add(result);
                    fileNum++;
                }
                File.WriteAllLines(currentFile, newLines.ToArray());
            }
            Console.WriteLine($@"Checked {fileNum} files.");
        }

        private static string RunFix(string translation)
        {
            var replacer = translation;

            if (replacer == "")
            {
                return replacer.TrimEnd();
            }

            List<string> killstringList = new List<string>
            {
                "(Anal Virgin)",
                "(Limited to lovers)",
                "(Beginning Dialogue)",
                "(Rejection and Removal)",
                "(Too Intense, Rejected)",
                "(Rejection)",
                "CHOICE:",
                "(Rejection and Removal)",
                "(First Kiss)",
                "(Rejection and Removal)",
                "(Virgin)",
                "(First Kiss)",
                "(Limited to lovers)",
                "(Limited to the first touch during an H-scene)",
                "(First Insertion)"
            };
            
            replacer = replacer.Replace("…", "...");


            replacer = Regex.Replace(replacer, @"(^|[^\.])(\.{3})+(\.{1,2})([^\.]|$)", "$1$2$4");
            replacer = Regex.Replace(replacer, @"(^|[^\.])(\.{2})([^\.]|$)", "$1$2.$3");
            replacer = Regex.Replace(replacer, "[ ]{2,}", " ");

            replacer = replacer.Replace("\"\"", "\"");

            foreach (string entry in killstringList)
            {
                replacer = replacer.Replace(entry, "");
            }

            return replacer.TrimEnd();
        }
    }
}
