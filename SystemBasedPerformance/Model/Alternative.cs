﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBasedPerformance.Model
{
    public class Alternative
    {
        #region Fields
        private string _Name;
        private System.IO.DirectoryInfo _FileDirectory;
        private Event[] _Events;
        private List<Metric> _Metrics;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        public System.IO.DirectoryInfo FileDirectory
        {
            get
            {
                return _FileDirectory;
            }
            set
            {
                _FileDirectory = value;
            }
        }
        public Event[] Events
        {
            get
            {
                return _Events;
            }
            set
            {
                _Events = value;
            }
        }
        public List<Metric> Metrics
        {
            get
            {
                return _Metrics;
            }
            set
            {
                _Metrics = value;
            }
        }
        #endregion

        #region Constructors
        public Alternative(string directory)
        {
            Name = directory;
            FileDirectory = new System.IO.DirectoryInfo(directory);

            int nEvents = FileDirectory.GetDirectories().Count();
            Events = new Event[nEvents];
            foreach (System.IO.DirectoryInfo eventDirectory in FileDirectory.GetDirectories())
            {
                int eventNumber = -1;
                double eventProbability = -1;
                double[] probabilities = new double[10] { 0.99, 0.50, 0.20, 0.10, 0.04, 0.02, 0.01, 0.005, 0.002, 0.001 };
                string directoryNameNoDashes = eventDirectory.Name.Replace("-", string.Empty);
                for (int i = 1; i < directoryNameNoDashes.Length; i++)
                {
                    bool numeric = int.TryParse(directoryNameNoDashes.Substring(directoryNameNoDashes.Length - i), out eventNumber);
                    if (numeric == false)
                    {
                        eventNumber = int.Parse(directoryNameNoDashes.Substring(directoryNameNoDashes.Length - (i - 1)));
                        eventProbability = probabilities[eventNumber - 1];
                        break;
                    }
                }
                Events[eventNumber - 1] = new Event(eventDirectory, eventProbability);
            }
                  
        }
        #endregion

        #region Functions

        #endregion

        #region Voids
        public void CalculateAlternativeMetrics(List<string> selectedMetrics = null)
        {
            if (selectedMetrics == null)
            {
                CalculateAlternativeMetrics();
            }
            else
            {
                Metrics = new List<Metric>();
                for (int i = 0; i < Events.Length; i++)                                 //1. Loop Over Events
                {
                    if (i == 0)                                                         //a. Create inital values
                    {
                        for (int n = 0; n < selectedMetrics.Count; n++)
                        {
                            for (int j = 0; j < Events[i].Metrics.Count; j++)
                            {
                                if (selectedMetrics[n] == Events[i].Metrics[j].Name)
                                {
                                    if (double.IsNaN(Events[i].Metrics[j].Value))
                                    {
                                        Metrics.Add(new Metric(Events[i].Metrics[j].Name, "The metric value is not a number. No value existed for event 1."));
                                    }
                                    else
                                    {
                                        Metrics.Add(new Metric(Events[i].Metrics[j].Name, 0));
                                    }
                                    break;
                                }

                                if (j + 1 == Events[i].Metrics.Count)
                                {
                                    Metrics.Add(new Metric(selectedMetrics[n], "This metric was not found for event 1."));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int n = 0; n < Metrics.Count; n++)                         //2. Loop Over Captured Metrics
                        {
                            if (Metrics[n].HasError == false)                           //a. if the metric is in error state exit
                            {
                                for (int j = 0; j < Events[i].Metrics.Count; j++)
                                {
                                    if (Metrics[n].Name == Events[i].Metrics[j].Name)
                                    {
                                        if (double.IsNaN(Events[i].Metrics[j].Value) == true)
                                        {
                                            Metrics[n].ReportMessage("The metric value is not a number. The value was invalid for event " + (i + 1) + ".");
                                        }
                                        else
                                        {
                                            for (int k = 0; k < Events[i-1].Metrics.Count; k++)
                                            {
                                                if (Events[i].Metrics[j].Name == Events[i - 1].Metrics[k].Name)
                                                {
                                                    if (Events[i].Metrics[j].Value < Events[i - 1].Metrics[k].Value)
                                                    {
                                                        Metrics[n].ReportMessage("The metric value is not a number. The value was not increasing between events " + i + " and " + (i + 1) + ".");
                                                    }
                                                    else
                                                    {
                                                        Metrics[n].Value += (Events[i - 1].Metrics[k].Value + Events[i].Metrics[j].Value) / 2 * (Events[i - 1].Probability - Events[i].Probability);
                                                    }
                                                }
                                            }
                                            
                                        }
                                        break;
                                    }

                                    if (j + 1 == Events[i].Metrics.Count)
                                    {
                                        Metrics[n].ReportMessage("The metric value is not a number. It was not computed for event " + (i + 1) + ".");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CalculateAlternativeMetrics()
        {
            Metrics = new List<Metric>();
            for (int i = 0; i < Events.Length; i++)                                 //1. Loop Over Events
            {
                if (i == 0)                                                         //a. Create inital values
                {
                    for (int j = 0; j < Events[i].Metrics.Count; j++)
                    {
                        if (double.IsNaN(Events[i].Metrics[j].Value))
                        {
                            Metrics.Add(new Metric(Events[i].Metrics[j].Name, "The metric value is not a number. No value existed for event 1."));
                        }
                        else
                        {
                            Metrics.Add(new Metric(Events[i].Metrics[j].Name, 0));
                        }
                    }
                }
                else
                {
                    for (int n = 0; n < Metrics.Count; n++)                         //2. Loop Over Captured Metrics
                    {
                        if (Metrics[n].HasError == false)                           //a. if the metric is in error state exit
                        {
                            for (int j = 0; j < Events[i].Metrics.Count; j++)
                            {
                                if (Metrics[n].Name == Events[i].Metrics[j].Name)
                                {
                                    if (double.IsNaN(Events[i].Metrics[j].Value) == true)
                                    {
                                        Metrics[n].ReportMessage("The metric value is not a number. The value was invalid for event " + (i + 1) + ".");
                                    }
                                    else
                                    {
                                        if (Events[i].Metrics[j].Value < Events[i - 1].Metrics[j].Value)
                                        {
                                            Metrics[n].ReportMessage("The metric value is not a number. The value was not increasing between events " + i + " and " + (i + 1) + ".");
                                        }
                                        else
                                        {
                                            Metrics[n].Value += (Events[i - 1].Metrics[j].Value + Events[i].Metrics[j].Value) / 2 * (Events[i - 1].Probability - Events[i].Probability);
                                        }
                                    }
                                    break;
                                }

                                if (j + 1 == Events[i].Metrics.Count)
                                {
                                    Metrics[n].ReportMessage("The metric value is not a number. It was not computed for event " + (i + 1) + ".");
                                }
                            }
                        }
                    }

                    for (int j = 0; j < Events[i].Metrics.Count; j++)
                    {
                        for (int n = 0; n < Metrics.Count; n++)
                        {
                            if (Events[i].Metrics[j].Name == Metrics[n].Name)
                            {
                                break;
                            }

                            if (n + 1 == Metrics.Count)
                            {
                                Metrics.Add(new Metric(Events[i].Metrics[j].Name, "The metric value is not a number. Its first occurence was event " + (i + 1) + "."));
                            }
                        }
                    }
                }
            }
        }

        public void ExportData(string exportedDataFile)
        {
            object[] metricsNames = new object[Metrics.Count];
            object[] metricsValue = new object[Metrics.Count];
            string[] columnTitles = new string[Events.Length + 2]; columnTitles[0] = "Metric:v|Event:>";
            object[][] exportData = new object[Events.Length + 2][];  
            
            for (int n = 0; n < Events.Length; n++)
            {
                columnTitles[n + 1] = Events[n].Name + ": " + Events[n].Probability + " ACE event";
                object[] eventColumn = new object[Metrics.Count]; 
                for (int i = 0; i < Metrics.Count; i++)
                {
                    if (n == 0)
                    {   
                        metricsNames[i] = Metrics[i].Name;
                    }

                    for (int j = 0; j < Events[n].Metrics.Count; j++)
                    {
                        if (Events[n].Metrics[j].Name == Metrics[i].Name)
                        {
                            eventColumn[i] = Events[n].Metrics[j].Value;
                            break;
                        }

                        if (j == Events[n].Metrics.Count - 1)
                        {
                            eventColumn[i] = "Not Found";
                        }
                    }

                    if (n == Events.Length - 1)
                    {
                        metricsValue[i] = Metrics[i].Value;
                    }
                }

                if (n == 0)
                {
                    exportData[n] = metricsNames;
                }
                exportData[n + 1] = eventColumn;
                if (n == Events.Length - 1)
                {
                    exportData[n + 2] = metricsValue;
                }
            }
            Utilities.TextDataExporter.ExportDelimitedColumns(exportedDataFile, exportData, columnTitles);
        }

        //public void ExportData(string exportedDataFile)
        //{
        //    string[] columnTitles = new string[Events.Length + 2];
        //    object[][] data = new object[columnTitles.Length][]; data[0] = new object[Metrics.Count]; data[Events.Length + 1] = new object[Metrics.Count];
        //    for (int n = 0; n < Metrics.Count; n++)
        //    {
        //        data[0][n] = Metrics[n].Name;
        //        columnTitles[0] = "Metric:v|Event:>";

        //        data[i] = new object[Events.Count];
        //        for (int j = 0; j < Events.Length; j++)
        //        {
        //            for (int i = 0; i < Events[j].Metrics.Count; i++)
        //            {
        //                if (Metrics[n].Name == Events[j].Metrics[i].Name)
        //                {
        //                    data[i][j] = Events[j].Metrics[i].Value;
        //                }
        //            }
        //        }

        //        for (int i = 0; i < Events.Length; i++)
        //        {
        //            data[i + 1] = new object[Metrics.Count];
        //            for (int j = 0; j < Events[i].Metrics.Count; j++)
        //            {
        //                if (Events[i].Metrics[j].Name == Metrics[n].Name)
        //                {
        //                    columnTitles[i + 1] = Events[i].Name + ": " + Events[i].Probability;
        //                    data[i + 1][n] = Events[i].Metrics[j].Value;
        //                    break;
        //                }

        //                if (j + 1 == Events[i].Metrics.Count)
        //                {
        //                    columnTitles[i + 1] = Events[i].Name;
        //                    data[i + 1][n] = "Not Found";
        //                }
        //            }
        //        }
        //        data[Events.Length + 1][n] = Metrics[n].Value;
        //        columnTitles[columnTitles.Length - 1] = "Expected Annual Value";
        //    }
        //    Utilities.TextDataExporter.ExportDelimitedColumns(exportedDataFile, data, columnTitles);
        //}

        //public void ExportData(string exportAlternativeDataFilePath)
        //{
        //    List<string> eventColumns = new List<string>() { Name };
        //    List<string> metricsNames = new List<string>();
        //    List<List<double>> metricValues = new List<List<double>>();
        //    double[] probabilities = new double[10] { 0.99, 0.50, 0.20, 0.10, 0.04, 0.02, 0.01, 0.005, 0.002, 0.001 };
        //    for (int i = 0; i < Events.Length; i++)
        //    {
        //        List<double> eventValues = new List<double>();
        //        eventColumns.Add(Events[i].Name + "->" + probabilities[i].ToString());
        //        if (i == 0)
        //        {
        //            for (int j = 0; j < Events[i].Metrics.Count; j++)
        //            {
        //                metricsNames.Add(Events[i].Metrics[j].Name);
        //                eventValues.Add(Events[i].Metrics[j].Value);
        //            }
        //            metricValues.Add(eventValues);
        //        }
        //        else
        //        {
        //            for (int j = 0; j < metricsNames.Count; j++)
        //            {
        //                for (int k = 0; k < Events[i].Metrics.Count; k++)
        //                {
        //                    if (metricsNames[j] == Events[i].Metrics[k].Name)
        //                    {
        //                        eventValues.Add(Events[i].Metrics[k].Value);
        //                        break;
        //                    }

        //                    if (k + 1 == Events[i].Metrics.Count)
        //                    {
        //                        eventValues.Add(double.NaN);
        //                    }
        //                }
        //            }

        //            for (int k = 0; k < Events[i].Metrics.Count; k++)
        //            {
        //                for (int j = 0; j < metricsNames.Count; j++)
        //                {
        //                    if (metricsNames[j] == Events[i].Metrics[k].Name)
        //                    {
        //                        break;
        //                    }

        //                    if (j + 1 == metricsNames.Count)
        //                    {
        //                        metricsNames.Add(Events[i].Metrics[k].Name);
        //                        eventValues.Add(Events[i].Metrics[k].Value);

        //                        for (int l = 0; l < metricValues.Count; l++)
        //                        {
        //                            metricValues[l].Add(double.NaN);
        //                        }
        //                    }
        //                }
        //            }
        //            metricValues.Add(eventValues);
        //        }
        //    }
        //    ReportErrors(metricsNames, metricValues);

        //    eventColumns.Add("ExpectedValue");
        //    metricValues.Add(CalculatedExpectedValue(metricsNames, metricValues));
        //    object[][] dataToExport = new object[Events.Length + 2][];
        //    for (int i = 0; i < metricValues.Count; i++)
        //    {
        //        if (i == 0)
        //        {
        //            dataToExport[i] = metricsNames.Cast<object>().ToArray();
        //        }
        //        dataToExport[i + 1] = metricValues[i].Cast<object>().ToArray();
        //    }
        //    Utilities.TextDataExporter.ExportDelimitedColumns(exportAlternativeDataFilePath, dataToExport, eventColumns.ToArray());
        //}

        //public void ReportErrors(List<string> metricNames, List<List<double>> metricValues)
        //{
        //    Errors = new List<string> { "ALTERNATIVE => " + Name.ToUpper()};
        //    for (int i = 0; i < metricValues[0].Count; i++)
        //    {
        //        Errors.Add((i + 1) + ". " + metricNames[i].ToUpper() + ":");
        //        if (double.IsNaN(metricValues[0][i]) == true)
        //        {
        //            Errors.Add("- A double.NaN value was recorded for event 1.");
        //        }

        //        double previousValue;
        //        for (int j = 1; j < metricValues.Count; j++)
        //        {
        //            if (double.IsNaN(metricValues[j][i]) == true)
        //            {
        //                Errors.Add("- A double.NaN value was recorded for event " + (j + 1) + ".");
        //            }
        //            else
        //            {
        //                if (double.IsNaN(metricValues[j - 1][i]) == true)
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    previousValue = metricValues[j - 1][i];
        //                    if (previousValue > metricValues[j][i])
        //                    {
        //                        Errors.Add("- The " + metricNames[i] + " metric value is not monotonically increasing around event " + (j + 1) + ". " +
        //                                   "The value for event " + (j) + " is " + metricValues[j - 1][i] + ". " +
        //                                   "The value for event " + (j + 1) + "is " + metricValues[j][i] + ".");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion

    }
}
