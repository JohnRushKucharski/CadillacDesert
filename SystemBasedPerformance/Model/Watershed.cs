using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase_Reader;

namespace SystemBasedPerformance.Model
{
    public class Watershed
    {
        #region Fields
        List<Alternative> _Alternatives;
        #endregion


        #region Properties
        public List<Alternative> Alternatives
        {
            get
            {
                return _Alternatives;
            }
            set
            {
                _Alternatives = value;
            }
        }
        #endregion


        #region Constructor
        public Watershed(List<string> fileDirectories)
        {
            Alternatives = new List<Alternative>();
            foreach (string directory in fileDirectories)
            {
                Alternatives.Add(new Alternative(directory));
            }
        }
        #endregion


        #region Functions
        public void ExportWatershedData(string exportWatershedDataFilePath)
        {
            string[] columnNames = new string[Alternatives.Count + 1];
            object[][] exportData = new object[Alternatives.Count + 1][];

            columnNames[0] = "Metric Name";
            List<string> metricNames = new List<string>();     
            List<List<double>> WatershedValues = new List<List<double>>();
            for (int i = 0; i < Alternatives.Count; i++)
            {
                columnNames[i + 1] = Alternatives[i].Name;
                List<double> metricValues = new List<double>();
                

                if (i == 0)
                {
                    for (int j = 0; j < Alternatives[i].Metrics.Count; j++)
                    {
                        metricNames.Add(Alternatives[i].Metrics[j].Item1);
                        metricValues.Add(Alternatives[i].Metrics[j].Item2);
                    }
                }
                else
                {
                    metricValues.Clear();
                    for (int k = 0; k < metricNames.Count; k++)
                    {
                        for (int j = 0; j < Alternatives[i].Metrics.Count; j++)
                        {
                            if (metricNames[k] == Alternatives[i].Metrics[j].Item1)
                            {
                                metricValues.Add(Alternatives[i].Metrics[j].Item2);
                                break;
                            }

                            if (j + 1 == Alternatives[i].Metrics.Count)
                            {
                                metricValues.Add(double.NaN);
                            }
                        }
                    }

                    for (int j = 0; j < Alternatives[i].Metrics.Count; j++)
                    {
                        for (int k = 0; k < metricNames.Count; k++)
                        {
                            if (metricNames[k] == Alternatives[i].Metrics[j].Item1)
                            {
                                break;
                            }

                            if (k + 1 == metricNames.Count)
                            {
                                metricNames.Add(Alternatives[i].Metrics[j].Item1);
                                metricValues.Add(Alternatives[i].Metrics[j].Item2);

                                for (int l = 0; l < WatershedValues.Count; l++)
                                {
                                    WatershedValues[l].Add(double.NaN);
                                }
                            }
                        }
                    }
                }
                WatershedValues.Add(metricValues);
            }

            for (int i = 0; i < WatershedValues.Count; i++)
            {
                if (i == 0)
                {
                    exportData[i] = metricNames.Cast<object>().ToArray();
                }
                exportData[i + 1] = WatershedValues[i].Cast<object>().ToArray();
            }
            Utilities.TextDataExporter.ExportDelimitedColumns(exportWatershedDataFilePath, exportData, columnNames);
        }

        public void ExportWatershedErrors(string exportWatershedErrorsFilePath)
        {
            List<object> exportData = new List<object>();
            for (int i = 0; i < Alternatives.Count; i++)
            {
                for (int j = 0; j < Alternatives[i].Errors.Count; j++)
                {
                    exportData.Add(Alternatives[i].Errors[j]);
                }
                exportData.Add("");
            }
            Utilities.TextDataExporter.ExportSingleColumn(exportWatershedErrorsFilePath, exportData.ToArray());
        }


        public void ExportDelimitedColumns(string fullFilePath, object[][] exportData, string[] exportColumnNames, char delimiter = '\t')
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
        #endregion
    }
}
