using System;
using System.IO;

namespace IO_parser
{
    class Program
    {
        static void Main()
        {
            string inputPath = Directory.GetCurrentDirectory() + @"\files\input";
            string outputPath = Directory.GetCurrentDirectory() + @"\files\output";
            string dictionaryPath = Directory.GetCurrentDirectory() + @"\files\dictionary";

            Reader.ReadFiles(inputPath, outputPath, dictionaryPath);
            Reader.SaveAsCsv(inputPath, outputPath, dictionaryPath);
            Parse.ParseCsv(outputPath, dictionaryPath);

        }
    }
}

