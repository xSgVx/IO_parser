using System;
using System.IO;

namespace IO_parser
{
    class Parse
    {
        public static bool ParseCsv(string inputPath, string outputPath)
        {
            
            DirectoryInfo directory = new DirectoryInfo(inputPath);
            FileInfo[] files = directory.GetFiles();

            foreach (FileInfo currentFile in files)
            {
                Console.WriteLine("Обработка файла {0} начата", currentFile.Name);
                try
                {
                    StreamWriter sw = new StreamWriter(outputPath + "\\dictionary.csv", false);
                    using (StreamReader sr = new StreamReader(currentFile.FullName, System.Text.Encoding.Default))
                    {

                        string line;

                        while (((line = sr.ReadLine()) != null) || ((line = sr.ReadLine()) != "") || (line = sr.ReadLine()) != "\n")
                        {
                            if (line == null) break;
                            string[] splittedLine = line.Split(";");
                            if (splittedLine.Length <= 3) continue;
                            if ((splittedLine[1] == "") || (splittedLine[1] == " ") || (splittedLine[3] == "") || (splittedLine[3] == " ") || (splittedLine[1].Contains("Резерв"))) continue;
                            else sw.WriteLine(splittedLine[1] + ";" + splittedLine[3]);

                        }
                    }

                    sw.Close();
                    Console.WriteLine("Обработка файла {0} закончена", currentFile.Name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("Запись словаря завершена");
            return true;
        }
    }
}
