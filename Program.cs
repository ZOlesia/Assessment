using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nintedo_Assessment
{
    class Program
    {
        public static void ReadingFile(string oldBackupFile, string newBackupFile){

                //creating output files --> please edit the path if needed
            var oldNotInNew = "C:/oldNotInNew.txt";
            TextWriter tw2 = new StreamWriter(oldNotInNew);
            tw2.Close();

            var newNotInOld = "C:/newNotInOld.txt";
            TextWriter tw1 = new StreamWriter(newNotInOld);
            tw1.Close();


                //creating HashSet where I will push all the sha1's of oldBackupFile 
                //and then it will help me to check with the newBackupFile for the duplicates
            var set = new HashSet<string>();

                //creating HashMap where files that is not in the newBackupFile will be stored 
            var map = new Dictionary<string, string>();


                // Read and display lines from oldBackupFile file until the end of the file is reached
            using (StreamReader sr = new StreamReader(oldBackupFile)) {
               string line;

               while ((line = sr.ReadLine()) != null) 
               {
                        // as sha1 is the first 40 characters of the line I am grabbing sha1 using the substring
                    string sha1 = line.Substring(0, 40);  
                    string file = line.Substring(41);

                        //adding sha1 as a key and file as a value to the HashMap
                    map[sha1] = file;

                        //adding sha1 to the HashSet
                    set.Add(sha1);
               }
            } 

                // Read and display lines from newBackupFile file until the end of the file is reached
            using (StreamReader sr = new StreamReader(newBackupFile)) {
               string line;

               while ((line = sr.ReadLine()) != null) 
               {
                    string sha1 = line.Substring(0, 40);
                    string file = line.Substring(41);

                        //checking through the HashSet if the current sha1 we are on exists in oldBackupFile 
                    if(set.Contains(sha1)){
                            //if true, removing the record from the HashMap which will eventally hold the records of the files that are not in newBackupFile
                        map.Remove(sha1); 
                    } else {
                        //if false that means that oldBackupFile doesn't have that file and I am appending records to the new line in the ouput file newNotInOld
                        File.AppendAllText(newNotInOld, line + Environment.NewLine); 
                    }
               }
                
                //everything that left in the HashMap are unique files that newBackupFile doesn't contain so I am adding the records to the output file oldNotInNew
               foreach(var item in map){
                   string fullLine = item.Key + " " + item.Value;
                   File.AppendAllText(oldNotInNew, fullLine + Environment.NewLine);
               }
            }
        }
        static void Main(string[] args)
        {
            var oldBackupFile = "C:/Users/files/Old.sha1.txt";   // please put the right path to the Old.sha1.txt 
            var newBackupFile = "C:/Users/files/New.sha1.txt";   // please put the right path to the New.sha1.txt 
            ReadingFile(oldBackupFile, newBackupFile);
        }
    }
}
