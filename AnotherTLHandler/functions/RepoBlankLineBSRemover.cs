using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnotherTLHandler.functions
{
    internal class RepoBlankLineBSRemover
    {
        public static void Runner()
        {
            bool KKExists = Directory.Exists("Data/abdataKK");

            Misc.EnsureDirectoryDel("Data/abdataKKS-RAW");
            Misc.CopyFilesRecursively("Data/abdataKKS", "Data/abdataKKS-RAW");
            DoClean("Data/abdataKKS-RAW");

            if (KKExists)
            {
                Misc.EnsureDirectoryDel("Data/abdataKK-RAW");
                Misc.CopyFilesRecursively("Data/abdataKK", "Data/abdataKK-RAW");
                DoClean("Data/abdataKK-RAW");
            }
        }

        private static void DoClean(string folder)
        {
            int linenum = 0;

            Console.WriteLine("Removing machine translation...");
            foreach (var RepoTLFile in Directory.EnumerateFiles(folder, "*.txt", SearchOption.AllDirectories))
            {
                if (RepoTLFile.Contains("zz_machineTranslation"))
                    File.Delete(RepoTLFile);
            }

            Console.WriteLine("Removing comments...");
            foreach (var RepoTLFile in Directory.EnumerateFiles(folder, "*.txt", SearchOption.AllDirectories))
            {
                Console.WriteLine(RepoTLFile);
                var lines = File.ReadAllLines(RepoTLFile);
                linenum = 0;
                List<string> newLines = new List<string>();
                foreach (var line in lines)
                {
                    string exportLine = line;
                    if (exportLine.Contains("Dumped for Koikatsu") || !exportLine.Contains("=") || exportLine.Equals(""))
                        continue;
                    if (line.Substring(0, 2) == "//")
                        exportLine = exportLine.Remove(0,2);
                    newLines.Add($"{exportLine}");
                    linenum++;
                }

                File.WriteAllLines(RepoTLFile, newLines.ToArray());
            }

            Console.WriteLine("Removing blank first lines...");
            foreach (var RepoTLFile in Directory.EnumerateFiles(folder, " *.txt", SearchOption.AllDirectories))
            {
                Console.WriteLine(RepoTLFile);
                var lines = File.ReadAllLines(RepoTLFile);
                if (lines[0] == "")
                {
                    File.WriteAllLines(RepoTLFile, lines.Skip(1).ToArray());
                }
            }

            Console.WriteLine("Removing Existing translations...");
            foreach (var RepoTLFile in Directory.EnumerateFiles(folder, "*.txt", SearchOption.AllDirectories))
            {

                Console.WriteLine(RepoTLFile);
                var lines = File.ReadAllLines(RepoTLFile);
                linenum = 0;
                foreach (var line in lines)
                {
                    if (line.Contains("="))
                    {
                        var splitLine = line.Split('=');
                        lines[linenum] = $"{splitLine[0]}=";
                    }
                    linenum++;
                }

                File.WriteAllLines(RepoTLFile, lines.ToArray());
            }
        }
    }
}
