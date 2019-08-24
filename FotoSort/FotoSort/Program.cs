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
          //  string readfolder= @"\\ACKERMANNNAS\photo\Oti iPhone Upload\";
          string readfolder= @"\\ACKERMANNNAS\photo\Chris iPhone Upload\";
            string targetfolderroot= @"\\ACKERMANNNAS\photo\Chronologische Fotos\";
            string[] files = GetFilesFromFolder(readfolder);
            int count =0;
            foreach (string image in files)
            {
                count++;
                FileInfo info = new System.IO.FileInfo(image) ;
                string filename = info.Name;
                DateTime createTS = info.LastWriteTimeUtc;

                int year = GetYearFromDateTime(createTS);
                int month = GetMonthFromDateTime(createTS);

                string qx = QxCalculation(month);

                string targetFolder = String.Format(@"{0}{1}\{2}",targetfolderroot,year,qx);
                Directory.CreateDirectory(targetFolder);
                string targetFileName = String.Format(@"{0}\{1}",targetFolder,filename);
                MoveFile(image,targetFileName)  ; 
                float prozent = 100*(float)count/files.Length;
                Console.WriteLine(prozent);
            }
            Console.WriteLine(count);
            Console.ReadLine();

          

          string[] GetFilesFromFolder(string folder){
            
             return Directory.GetFiles(folder, "*.jpg");


            }

            int GetYearFromDateTime(DateTime timestamp){
                return timestamp.Year;
            }

            int GetMonthFromDateTime(DateTime timestamp){
                return timestamp.Month;     
            }

            string QxCalculation(int month){
                decimal quartal = Math.Ceiling((decimal)month/3);
                return $"Q{quartal}";
            }

                void MoveFile(string filename, string targetFolder) 
            {
                try 
                {
                    if (!File.Exists(filename)) 
                    {
                        Console.WriteLine("File {0} not found in the source folder.", filename);
                        return;
                    }

                    // Don't move again when file already moved
                    if (File.Exists(targetFolder))	
                    return;

                    // Move the file.
                    File.Move(filename, targetFolder);
                    //Console.WriteLine("{0} was moved to {1}.", filename, targetFolder);

                    // See if the original exists now.
                    if (File.Exists(filename)) 
                    {
                       // Console.WriteLine("The original file still exists, which is unexpected.");
                    } 
                    else 
                    {
                      //  Console.WriteLine("The original file no longer exists, which is expected.");
                    }			

                } 
                catch (Exception e) 
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }
            }
        }

    }
}
