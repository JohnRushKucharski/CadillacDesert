using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBasedPerformance.Model.Utilities
{
    public static class TextDataExporter
    {
        public static void ExportDelimitedColumns(string fullFilePath, object[][] exportData, string[] exportColumnNames, char delimiter = '\t')
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.FileStream(fullFilePath, System.IO.FileMode.Create)))
            {
                //1. Find longest sublist
                int n = 0;
                for (int k = 0; k < exportData.Length; k++)
                {
                    if (exportData[k].Length > n)
                    {
                        n = exportData[k].Length;
                    }
                }
                //2. Column Names
                for (int s = 0; s < exportColumnNames.Length; s++)
                {
                    writer.Write(exportColumnNames[s]);
                    writer.Write(delimiter);
                }
                writer.WriteLine();

                //3. Loop across sublist elements
                for (int j = 0; j < n; j++)
                {
                    //4. Loop across list number
                    for (int i = 0; i < exportData.Length; i++)
                    {
                        if (j < exportData[i].Length)
                        {
                            writer.Write(exportData[i][j]);
                            writer.Write(delimiter);
                        }
                        else
                        {
                            writer.Write(delimiter);
                        }
                    }
                    writer.WriteLine();
                }
            }
        }

        public static void ExportSingleColumn(string fullFilePath, object[] exportData)
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(new System.IO.FileStream(fullFilePath, System.IO.FileMode.Create)))
            {
                //1. Find longest sublist
                for (int i = 0; i < exportData.Length; i++)
                {
                    writer.WriteLine(exportData[i]);
                }
            }
        }
    }
}
