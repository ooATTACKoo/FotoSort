using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FotoSorting
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> folders = new List<string>();

            folders.Add(@"\\ACKERMANNNAS\photo\Oti iPhone Upload\");
            folders.Add(@"\\ACKERMANNNAS\photo\Chris iPhone Upload\");
   
            string targetfolderrootjpg, targetfolderrootmov;
            SetTargets(out targetfolderrootjpg, out targetfolderrootmov);

            foreach (string readfolder in folders)
            {
                Console.WriteLine(String.Format($"Investigating folder {readfolder}."));
                SortFotosInFolder(readfolder, targetfolderrootjpg);
                SortMoviesInFolder(readfolder, targetfolderrootmov);
            }

            Console.ReadLine();
        }

        private static void SetTargets(out string targetfolderrootjpg, out string targetfolderrootmov)
        {
            // DO NOT CHANGE:
            targetfolderrootjpg = @"\\ACKERMANNNAS\photo\Chronologische Fotos\";
            targetfolderrootmov = @"\\ACKERMANNNAS\video\";
        }

                private static List<string> FindFolders()
        {
            string searchinfolder = @"\\ACKERMANNNAS\home\Alte Festplatte\PICTURE";
            string targetfolderrootmov = @"\\ACKERMANNNAS\video\";
            List<string> folderlist= new List<string>();
            return folderlist;
        }

        private static void SortFotosInFolder(string readfolder, string targetfolderroot)
        {
            string[] files = GetFilesFromFolder(readfolder, "*.jpg");
            Console.WriteLine(String.Format($"Images..."));
            if (files.Length == 0) return;

            int count = 0;
            Console.Write(count);
            foreach (string image in files)
            {
                count++;
                FileInfo info = new FileInfo(image);
                string filename = info.Name;
                DateTime createTS = info.LastWriteTimeUtc;

                int year = GetYearFromDateTime(createTS);
                int month = GetMonthFromDateTime(createTS);

                string qx = QxCalculation(month);

                string targetFolder = String.Format(@"{0}{1}\{2}", targetfolderroot, year, qx);
                Directory.CreateDirectory(targetFolder);
                string targetFileName = String.Format(@"{0}\{1}", targetFolder, filename);
                MoveFile(image, targetFileName);
                float prozent = 100 * (float)count / files.Length;
                OverwriteConsole(prozent);
            }
            Console.WriteLine();
            Console.WriteLine(String.Format($"{count} Images found in {readfolder} and moved."));
        }

        public static void ClearCurrentConsoleLine()
        {
             int currentLineCursor = Console.CursorTop;
             Console.SetCursorPosition(0, Console.CursorTop);
             for (int i = 0; i < Console.WindowWidth; i++)
                 Console.Write(" ");
             Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void OverwriteConsole(float prozent)
        {
             ClearCurrentConsoleLine();
             Console.Write($"{prozent}% moved.");
        }

        private static void SortMoviesInFolder(string readfolder, string targetfolderroot)
        {
            string[] files = GetFilesFromFolder(readfolder, "*.mov");
            Console.WriteLine(String.Format($"Movies..."));
            if (files.Length == 0) return;

            int count = 0;
            foreach (string movie in files)
            {
                count++;
                FileInfo info = new FileInfo(movie);
                string filename = info.Name;
                DateTime createTS = info.LastWriteTimeUtc;

                int year = GetYearFromDateTime(createTS);         

                string targetFolder = String.Format(@"{0}{1}", targetfolderroot, year);
                Directory.CreateDirectory(targetFolder);
                string targetFileName = String.Format(@"{0}\{1}", targetFolder, filename);
                MoveFile(movie, targetFileName);
                float prozent = 100 * (float)count / files.Length;
                OverwriteConsole(prozent);
            }
            Console.WriteLine();
            Console.WriteLine(String.Format($"{count} Movies found in {readfolder} and moved."));
        }

        private static string[] GetFilesFromFolder(string folder, string fileend)
        {
            return Directory.GetFiles(folder, fileend);
        }

        private static int GetYearFromDateTime(DateTime timestamp)
        {
            return timestamp.Year;
        }

        private static int GetMonthFromDateTime(DateTime timestamp)
        {
            return timestamp.Month;
        }

        private static string QxCalculation(int month)
        {
            decimal quartal = Math.Ceiling((decimal)month / 3);
            return $"Q{quartal}";
        }

        private static void MoveFile(string filename, string targetFileName)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Console.WriteLine("File {0} not found in the source folder.", filename);
                    return;
                }

                if (filename == targetFileName) { return; }

                // Don't move again when file already moved
                if (File.Exists(targetFileName))
                {
                    File.Delete(filename);
                    return;
                }

                // Move the file.
                File.Move(filename, targetFileName);
                //Console.WriteLine("{0} was moved to {1}.", filename, targetFolder);

                // See if the original exists now.
                if (File.Exists(filename))
                {
                    Console.WriteLine("The original file still exists, which is unexpected.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
    }
}
