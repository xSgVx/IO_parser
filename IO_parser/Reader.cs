using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ExcelDataReader;


namespace IO_parser
{

    class Reader
    {

        public static bool SaveAsCsv(string inputPath, string outputPath)
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
                        FilterSheet = (tableReader, sheetIndex) => true,
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = false,
                        }
                    });

                    var csvContent = string.Empty;
                    int row_no = 0;
                    for (int j = 0; j < ds.Tables.Count; j++)
                    {
                        while (row_no < ds.Tables[j].Rows.Count)
                        {
                            var arr = new List<string>();
                            for (int i = 0; i < ds.Tables[j].Columns.Count; i++)
                            {
                                arr.Add(ds.Tables[j].Rows[row_no][i].ToString());
                            }

                            row_no++;
                            csvContent += string.Join(";", arr) + "\n";
                        }
                        row_no = 0;
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


