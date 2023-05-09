using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AnotherTLHandler.functions
{
    internal class Misc
    {
        // Checks if file exists
        public static bool CheckFileExists(string path)
        {
            if (File.Exists(path)) return true;
            return false;
        }

        // Makes sure a folder exists
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        // Makes sure a folder is deleted
        public static void EnsureDirectoryDel(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        // Make sure a file is deleted
        public static void EnsureFileDel(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        // Making a streamWriter
        public static StreamWriter MakeWriter(string filename, bool append)
        {
            return new StreamWriter(filename, append, Encoding.UTF8) {AutoFlush = true};
        }

        // Calculating MD5
        public static string CalculateMD5(string filename)
        {
            using (var md5Instance = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hashResult = md5Instance.ComputeHash(stream);
                    return BitConverter.ToString(hashResult).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        // Checking if character folders are in the currentfile
        public static bool CheckChar(string CurrentPers, string CurrentFile)
        {
            var advMatch = CurrentFile.Contains($"c{CurrentPers}") && CurrentFile.Contains(@"adv\scenario");
            var comMatch = CurrentFile.Contains($@"communication_{CurrentPers}") ||
                           CurrentFile.Contains($@"optiondisplayitems_{CurrentPers}") ||
                           CurrentFile.Contains($@"communication_off_{CurrentPers}") ||
                           CurrentFile.Contains($@"topiclistena_{CurrentPers}") ||
                           CurrentFile.Contains($@"topiclistenb_{CurrentPers}") ||
                           CurrentFile.Contains($@"topiclistenc_{CurrentPers}") ||
                           CurrentFile.Contains($@"topiclistend_{CurrentPers}") ||
                           CurrentFile.Contains($@"topictalkcommon_{CurrentPers}") ||
                           CurrentFile.Contains($@"topictalkrare_{CurrentPers}") ||
                           CurrentFile.Contains($@"topiclistenrare_{CurrentPers}");
            var hMatch = CurrentFile.Contains($"personality_voice_c{CurrentPers}");
            var estMatch = CurrentFile.Contains($"voicelist_{CurrentPers}");
            if (advMatch || comMatch || hMatch || estMatch) return true;
            return false;
        }

        public static string getCharaTypeName(string charaNumber)
        {
            switch (charaNumber)
            {
                case "00":
                    return "Sexy";
                case "01":
                    return "Ojousama";
                case "02":
                    return "Snobby";
                case "03":
                    return "Kouhai";
                case "04":
                    return "Mysterious";
                case "05":
                    return "Weirdo";
                case "06":
                    return "Yamato Nadeshiko";
                case "07":
                    return "Tomboy";
                case "08":
                    return "Pure";
                case "09":
                    return "Simple";
                case "10":
                    return "Delusional";
                case "11":
                    return "Motherly";
                case "12":
                    return "Big Sisterly";
                case "13":
                    return "Gyaru";
                case "14":
                    return "Delinquent";
                case "15":
                    return "Wild";
                case "16":
                    return "Wannabe";
                case "17":
                    return "Reluctant";
                case "18":
                    return "Jinxed";
                case "19":
                    return "Bookish";
                case "20":
                    return "Timid";
                case "21":
                    return "Typical Schoolgirl";
                case "22":
                    return "Trendy";
                case "23":
                    return "Otaku";
                case "24":
                    return "Yandere";
                case "25":
                    return "Lazy";
                case "26":
                    return "Quiet";
                case "27":
                    return "Stubborn";
                case "28":
                    return "Old-Fashioned";
                case "29":
                    return "Humble";
                case "30":
                    return "Friendly";
                case "31":
                    return "Willful";
                case "32":
                    return "Honest";
                case "33":
                    return "Glamorous";
                case "34":
                    return "Returnee";
                case "35":
                    return "Slangy";
                case "36":
                    return "Sadistic";
                case "37":
                    return "Emotionless";
                case "38":
                    return "Perfectionist";
                case "39":
                    return "Island Girl";
                case "40":
                    return "Noble";
                case "41":
                    return "Bokukko";
                case "42":
                    return "Genuine";
                case "43":
                    return "Hype";
                default:
                    return "";
            }
        }

        // Copying a folder recursively
        public static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
    }
}
