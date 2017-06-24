using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zebra
{
    public class Zebra
    {
        private string[] args = null;

        public Zebra(string[] args)
        {
            this.args = args;
        }

        public void Run()
        {
            try
            {
                string workDir = "";
                if (args.Length > 0)
                {
                    workDir = args[0];
                }
                else
                {
                    Console.WriteLine("No directory specified, using current...");
                    workDir = Directory.GetCurrentDirectory();
                }
                if (!Directory.Exists(workDir))
                {
                    Message("Error, specified source dir does not exist or not accessible : " + workDir);
                    Environment.Exit(3);
                }
                Console.WriteLine("Using base directory : " + workDir);

                // get all folders
                Regex reg = new Regex(@"^group\d+$", RegexOptions.IgnoreCase);
                Console.WriteLine("Searching folders matching regex pattern : " + reg.ToString());
                string[] dirs = Directory.GetDirectories(workDir).Where(path => reg.IsMatch(Path.GetFileName(path))).ToArray();

                if (dirs.Length == 0)
                {
                    Message("Error, did not find any matching folder names");
                    Environment.Exit(3);
                }
                Console.WriteLine("Found " + dirs.Length + " matching folders");
                List<Dictionary<string, List<Dictionary<string, string[]>>>> data = new List<Dictionary<string, List<Dictionary<string, string[]>>>>();

                // get all files in folders
                foreach (string dir in dirs)
                {
                    Console.WriteLine("\nScanning dir : " + dir);
                    string[] fileNames = Directory.GetFiles(dir, "*.txt", SearchOption.TopDirectoryOnly);

                    if (fileNames.Length == 0)
                    {
                        Console.WriteLine("No files found to work with, skipping dir : " + dir);
                        continue;
                    }

                    Console.WriteLine("Found " + fileNames.Length + " files :");

                    List<string[]> fileContents = new List<string[]>();
                    Console.Write("\t");
                    List<Dictionary<string, string[]>> files = new List<Dictionary<string, string[]>>();
                    foreach (string fileName in fileNames)
                    {
                        Console.Write(Path.GetFileNameWithoutExtension(fileName) + " \t");
                        string[] readText = File.ReadAllLines(fileName);
                        if (readText.Length == 0)
                        {
                            Console.WriteLine("\nEmpty file, skipping : " + Path.GetFileNameWithoutExtension(fileName));
                            continue;
                        }
                        fileContents.Add(readText);
                        files.Add(new Dictionary<string, string[]>() { { fileName, readText } });
                    }
                    data.Add(new Dictionary<string, List<Dictionary<string, string[]>>>() { { dir, files } });
                }
                Console.WriteLine("\nFiles loaded successfully");
                Console.WriteLine("\nProcessing ...");

                Message("\n\nOperation completed successfully !");

            }
            catch (Exception e)
            {
                Message(e.ToString());
            }


        }
        static void Message(string msg)
        {
            Console.WriteLine("\n" + msg);
            Console.WriteLine("Press any key ...");
            Console.ReadKey();
        }
    }
}
