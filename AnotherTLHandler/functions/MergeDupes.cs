using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnotherTLHandler.functions
{
    internal class MergeDupes
    {
        public static void Runner(bool TranslationClean, string CharaType, bool IncludeKK)
        {
            ReadDictionaries(CharaType, "H", TranslationClean, "KKS");
            if (IncludeKK) ReadDictionaries(CharaType, "H", TranslationClean, "KK");
            ReadDictionaries(CharaType, "communication", TranslationClean, "KKS");
            if (IncludeKK) ReadDictionaries(CharaType, "communication", TranslationClean, "KK");
        }

        private static void ReadDictionaries(string CharaType, string type, bool translationClean, string game)
        {
            Console.Write($@"Writing dupes into output for c{CharaType} {type} lines from {game}. ");
            var DupeMaster = File.ReadAllLines(@$"Data\DupeLib\c{CharaType}_{type}_{game}.txt");
            int counter = 0;
            int skipped = 0;
            int total = 0;
            string output = $@"Output\c{CharaType}\abdata\{type}";

            foreach (string currentFile in Directory.EnumerateFiles(output, "*.txt", SearchOption.AllDirectories))
            {
                var FileList = File.ReadAllLines(currentFile);
                total = total + FileList.Length;
                var lines = File.ReadAllLines(currentFile);
                int linenum = 0;
                List<string> newLines = new List<string>();

                foreach (string oldLine in lines)
                {
                    var splitOldLine = oldLine.Split('=');
                    if (splitOldLine[1] != "" || DupeMaster.Length == 0)
                    {
                        newLines.Add(oldLine);
                        if (splitOldLine[1] != "") skipped++;
                        continue;
                    }

                    bool hit = false;
                    foreach (string MasterLine in DupeMaster)
                    {
                        var splitMasterLine = MasterLine.Split("=");

                        if (splitOldLine[0].Replace(" ", "").Replace("　", "") != splitMasterLine[0].Replace(" ", "").Replace("　", "") || hit) continue;
                        if (translationClean)
                        {
                            try
                            {
                                newLines.Add(splitOldLine[0] + "=" + splitMasterLine[1] + "=" + splitOldLine[2]);
                                linenum++;
                                counter++;
                            }
                            catch (Exception)
                            {
                                newLines.Add(splitOldLine[0] + "=" + splitMasterLine[1]);
                                linenum++;
                                counter++;
                            }
                        }
                        else
                        {
                            newLines.Add(splitOldLine[0] + "=" + splitMasterLine[1]);
                            linenum++;
                            counter++;
                        }
                        hit = true;
                    }
                }
                //fileWriter.Close();
                File.WriteAllLines(currentFile, newLines.ToArray());
            }
            Console.WriteLine($@"{counter} lines written. {skipped} already populated");
        }
    }
}
