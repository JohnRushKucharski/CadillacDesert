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


        #region Voids
        public void ExportData(string exportedDataFilePath)
        {
            string[] columnTitles = new string[Alternatives.Count + 1];
            object[][] exportData = new object[Alternatives.Count + 1][];

            for (int i = 0; i < Alternatives.Count; i++)
            {
                columnTitles[i + 1] = Alternatives[i].Name;
                exportData[i + 1] = new object[Alternatives[i].Metrics.Count];
                if (i == 0)
                {
                    columnTitles[i] = "Metric:v||Alternative:>";
                    exportData[i] = new object[Alternatives[i].Metrics.Count];
                    for (int j = 0; j < Alternatives[i].Metrics.Count; j++)
                    {
                        exportData[i][j] = Alternatives[i].Metrics[j].Name;
                        exportData[i + 1][j] = Alternatives[i].Metrics[j].Value;
                    }
                }
                else
                {
                    for (int n = 0; n < exportData[i].Length; n++)                              //# of SelectedMetrics
                    {
                        for (int j = 0; j < Alternatives[i].Metrics.Count; j++)                 //# of SelectableMetrics
                        {
                            if ((string)exportData[0][n] == Alternatives[i].Metrics[j].Name)
                            {
                                exportData[i + 1][n] = Alternatives[i].Metrics[j].Value;
                                break;
                            }

                            if (j + 1 == Alternatives[i].Metrics.Count)
                            {
                                exportData[i + 1][n] = "Metric Not Found";
                            }
                        }
                    }

                    for (int k = exportData[0].Length; k < exportData[i + 1].Length; k++)
                    {
                        for (int j = 0; j < Alternatives[i].Metrics.Count; j++)
                        {
                            for (int n = 0; n < exportData[0].Length; n++)
                            {
                                if (Alternatives[i].Metrics[j].Name == (string)exportData[0][n])
                                {
                                    break;
                                }

                                if (n + 1 == exportData.Length)
                                {
                                    exportData[i + 1][k] = Alternatives[i].Metrics[j].Name + "NOT found in all alternatives";
                                }
                            }
                        }
                    }
                }
            }
            Utilities.TextDataExporter.ExportDelimitedColumns(exportedDataFilePath, exportData, columnTitles);
        }

        public void ExportErrors(string errorFilePath)
        {
            List<object> exportData = new List<object>();
            for (int i = 0; i < Alternatives.Count; i++)
            {
                exportData.Add(Alternatives[i].Name);
                for (int j = 0; j < Alternatives[i].Metrics.Count; j++)
                {
                    if (Alternatives[i].Metrics[j].HasError == false)
                    {
                        exportData.Add((j + 1) + ". " + Alternatives[i].Metrics[j].Name + ": NO errors.");
                    }
                    else
                    {
                        exportData.Add((j + 1) + ". " + Alternatives[i].Metrics[j].Name + ": ");
                        exportData.Add(" - " + Alternatives[i].Metrics[j].ErrorMessage);
                    }
                }
                exportData.Add("");
            }
            Utilities.TextDataExporter.ExportSingleColumn(errorFilePath, exportData.ToArray());
        }

        //public void ExportWatershedErrors(string exportWatershedErrorsFilePath)
        //{
        //    List<object> exportData = new List<object>();
        //    for (int i = 0; i < Alternatives.Count; i++)
        //    {
        //        for (int j = 0; j < Alternatives[i].Errors.Count; j++)
        //        {
        //            exportData.Add(Alternatives[i].Errors[j]);
        //        }
        //        exportData.Add("");
        //    }
        //    Utilities.TextDataExporter.ExportSingleColumn(exportWatershedErrorsFilePath, exportData.ToArray());
        //}


        //public void ExportData()
        //{
        //    List<string> metricNames = new List<string>();
        //    List<List<double>> WatershedValues = new List<List<double>>();
        //    for (int i = 0; i < Alternatives.Count; i++)
        //    {
        //        columnNames[i + 1] = Alternatives[i].Name;
        //        List<double> metricValues = new List<double>();
        //        if (i == 0)
        //        {
        //            for (int j = 0; j < Alternatives[i].Metrics.Count; j++)
        //            {
        //                metricNames.Add(Alternatives[i].Metrics[j].Name);
        //                metricValues.Add(Alternatives[i].Metrics[j].Value);
        //            }
        //        }
        //        else
        //        {
        //            metricValues.Clear();
        //            for (int k = 0; k < metricNames.Count; k++)
        //            {
        //                for (int j = 0; j < Alternatives[i].Metrics.Count; j++)
        //                {
        //                    if (metricNames[k] == Alternatives[i].Metrics[j].Item1)
        //                    {
        //                        metricValues.Add(Alternatives[i].Metrics[j].Item2);
        //                        break;
        //                    }

        //                    if (j + 1 == Alternatives[i].Metrics.Count)
        //                    {
        //                        metricValues.Add(double.NaN);
        //                    }
        //                }
        //            }

        //            for (int j = 0; j < Alternatives[i].Metrics.Count; j++)
        //            {
        //                for (int k = 0; k < metricNames.Count; k++)
        //                {
        //                    if (metricNames[k] == Alternatives[i].Metrics[j].Item1)
        //                    {
        //                        break;
        //                    }

        //                    if (k + 1 == metricNames.Count)
        //                    {
        //                        metricNames.Add(Alternatives[i].Metrics[j].Item1);
        //                        metricValues.Add(Alternatives[i].Metrics[j].Item2);

        //                        for (int l = 0; l < WatershedValues.Count; l++)
        //                        {
        //                            WatershedValues[l].Add(double.NaN);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        WatershedValues.Add(metricValues);
        //    }

        //    for (int i = 0; i < WatershedValues.Count; i++)
        //    {
        //        if (i == 0)
        //        {
        //            exportData[i] = metricNames.Cast<object>().ToArray();
        //        }
        //        exportData[i + 1] = WatershedValues[i].Cast<object>().ToArray();
        //    }
        //}
        #endregion

    }
}
