using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IO_parser
{
    class Checker
    {
        public static bool CheckFiles(string inputPath, string outputPath, string dictionaryPath)
        {

            bool emptyInput = false;

            Console.WriteLine("Текущая директория: {0}", Directory.GetCurrentDirectory());

            if (!Directory.Exists(inputPath) || !Directory.Exists(outputPath) || !Directory.Exists(dictionaryPath))
            {
                Directory.CreateDirectory(inputPath);
                Directory.CreateDirectory(outputPath);
                Directory.CreateDirectory(dictionaryPath);
            }

            try
            {
                string[] directoryFiles = Directory.GetFiles(inputPath);

                if (directoryFiles.Length == 0)
                {
                    Console.WriteLine("В папке input нет файлов для чтения");
                    emptyInput = false;
                }
                else
                {
                    Console.WriteLine("Входные файлы: ");
                    foreach (string s in directoryFiles)
                    {
                        Console.WriteLine(s);
                    }
                    emptyInput = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

            return emptyInput;
        }
    }
}

