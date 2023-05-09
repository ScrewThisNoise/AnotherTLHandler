using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using Ionic.Zlib;
using CompressionLevel = Ionic.Zlib.CompressionLevel;
using ZipFile = Ionic.Zip.ZipFile;

namespace AnotherTLHandler.functions
{
    internal class ZipHandler
    {
        public static void Runner(string workFolder, string currentPersType, string personalityName)
        {
            string folder = $@"Output\c{currentPersType}";
            string zipName = $@"BR_c{currentPersType}_{personalityName}.zip";
            zipName = zipName.Replace(" ", "_");

            Console.Write("Removing TL Notes from Output. ");
            cheanupCrew(workFolder, currentPersType);
            Console.WriteLine("OK!");

            Console.Write("Creating zip " + zipName + "." );
            Seal(folder, $@"{workFolder}\ZIP", zipName);
            Console.WriteLine("OK!");
        }

        private static void cheanupCrew(string workFolder, string currentPersType)
        {

            foreach (string currentFile in Directory.EnumerateFiles(workFolder, "*.txt", SearchOption.AllDirectories))
            {
                List<string> newLines = new List<string>();
                var currentFileLines = File.ReadAllLines(currentFile);

                foreach (var line in currentFileLines)
                {
                    var splitOutfile = line.Split('=');
                    newLines.Add($@"{splitOutfile[0]}={splitOutfile[1]}");
                }
                File.WriteAllLines(currentFile, newLines.ToArray());
            }

        }

        public static void Seal(string tempFolder, string outputFolder, string filename)
        {
            Misc.EnsureDirectoryExists(outputFolder);
            Misc.EnsureFileDel($"{outputFolder}\\{filename}");

            try
            {
                using (var zip = new ZipFile())
                {
                    zip.CompressionLevel = CompressionLevel.Level0;
                    zip.CompressionMethod = CompressionMethod.None;
                    zip.AddDirectory(tempFolder);
                    zip.Save($"{outputFolder}\\{filename}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}
