using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ExcelDataReader;


namespace IO_parser
{
    class Reader
    {
        public static string inputPath = Directory.GetCurrentDirectory() + @"\files\input";
        public static string outputPath = Directory.GetCurrentDirectory() + @"\files\output";

        public static bool CheckFiles()
        {
            bool emptyInput = false;

            Console.WriteLine("Текущая директория: {0}", Directory.GetCurrentDirectory());

            if (!Directory.Exists(inputPath) || !Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(inputPath);
                Directory.CreateDirectory(outputPath);
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
        
        public static bool SaveAsCsv()
        {
            DirectoryInfo directory = new DirectoryInfo(inputPath);
            FileInfo[] files = directory.GetFiles();

            foreach (FileInfo currentFile in files)
            {
                FileStream stream = new FileStream(currentFile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                {
                   
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    IExcelDataReader reader = null;
                    if (currentFile.FullName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (currentFile.FullName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    if (reader == null)
                        return false;

                    var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = false
                        }
                    });
                    
                    var csvContent = string.Empty;
                    int row_no = 0;
                    while (row_no < ds.Tables[0].Rows.Count)
                    {
                        var arr = new List<string>();
                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            arr.Add(ds.Tables[0].Rows[row_no][i].ToString());
                        }
                        row_no++;
                        csvContent += string.Join(";", arr) + "\n";
                    }

                    StreamWriter csv = new StreamWriter(outputPath + "\\" + currentFile.Name.Remove(currentFile.Name.LastIndexOf(".xls")) + ".csv", false);
                    csv.Write(csvContent);
                    csv.Close();
                }
                Console.WriteLine("Файл {0} успешно переконвертирован", currentFile.Name);
            }

            Console.WriteLine("Все {0} файлов переконвертированы в формат CSV", files.Length);
            return true;
        }
    }
}

