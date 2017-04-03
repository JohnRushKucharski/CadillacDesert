using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SystemBasedPerformance.ViewModel
{
    public class WatershedViewModel: BaseViewModel
    {
        #region Fields
        private Model.Watershed _Watershed;
        private string _WatershedFolderPath;

        private List<string> _SelectableMetrics;
        private List<string> _SelectedMetrics = new List<string>();
        private List<string> _WriteDataFilesPaths;

        private NamedAction _CleanDirectory;
        private NamedAction _ReadData;
        private NamedAction _WriteData;

        private bool _IsProgressing;
        private int _Progress;
        #endregion


        #region Properties
        public Model.Watershed Watershed
        {
            get
            {
                return _Watershed;
            }
            set
            {
                _Watershed = value;
            }
        }

        public string WatershedFolderPath
        {
            get
            {
                return _WatershedFolderPath;
            }
            set
            {
                _WatershedFolderPath = value;
                if (System.IO.Directory.Exists(WatershedFolderPath) == true)
                {
                    CleanDirectory.IsEnabled = true;
                    ReadData.IsEnabled = true;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<string> SelectableMetrics
        {
            get
            {
                return _SelectableMetrics;
            }
            set
            {
                _SelectableMetrics = value;
                NotifyPropertyChanged();
            }
        }

        public List<string> SelectedMetrics
        {
            get
            {
                return _SelectedMetrics;
            }
            set
            {
                _SelectedMetrics = value;
                NotifyPropertyChanged();
            }
        }

        public List<string> WriteDataFilePaths
        {
            get
            {
                return _WriteDataFilesPaths;
            }
            set
            {
                _WriteDataFilesPaths = value;
                NotifyPropertyChanged();
            }
        }

        public NamedAction CleanDirectory
        {
            get
            {
                return _CleanDirectory;
            }
            private set
            {
                _CleanDirectory = value;
                NotifyPropertyChanged();
            }
        }
        
        public NamedAction ReadData
        {
            get
            {
                return _ReadData;
            }
            private set
            {
                _ReadData = value;
                NotifyPropertyChanged();
            }
        }

        //public NamedAction SelectMetrics
        //{
        //    get
        //    {
        //        return _SelectMetrics;
        //    }
        //    set
        //    {
        //        _SelectMetrics = value;
        //    }
        //}

        public NamedAction WriteData
        {
            get
            {
                return _WriteData;
            }
            set
            {
                _WriteData = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsProgressing
        {
            get
            {
                return _IsProgressing;
            }
            set
            {
                _IsProgressing = value;
            }
        }

        public int Progress
        {
            get
            {
                return _Progress;
            }
            set
            {
                _Progress = value;
            }
        }
        #endregion


        #region Constructor
        public WatershedViewModel()
        {
            WatershedFolderPath ="";

            CleanDirectory = new NamedAction("Clean Watershed Directory");
            CleanDirectory.IsEnabled = false;
            CleanDirectory.Action = CleanWatershedAlternativesEventsDirectories;

            ReadData = new NamedAction("Read Watershed Data");
            ReadData.IsEnabled = false;
            ReadData.Action = ReadWatershedData;

            //SelectMetrics = new NamedAction("SelectMetrics");
            //SelectMetrics.IsEnabled = true;
            //SelectMetrics.Action = BuildSelectedMetrics;

            WriteData = new NamedAction("Write Watershed Data To Text Files");
            WriteData.IsEnabled = false;
            WriteData.Action = WriteWatershedData;

            
        }
        #endregion


        #region NamedActions
        public void CleanWatershedAlternativesEventsDirectories(object sender, EventArgs e)
        {
            CleanDirectory.IsEnabled = false;
            CleanDirectory.Name = "Please be patient I'm working...";

            System.IO.DirectoryInfo watershedDirectory = new System.IO.DirectoryInfo(WatershedFolderPath);
            foreach (System.IO.DirectoryInfo alternativeDirectory in watershedDirectory.GetDirectories())
            {
                foreach (System.IO.DirectoryInfo eventDirectory in alternativeDirectory.GetDirectories())
                {
                    foreach (System.IO.DirectoryInfo modelDirectory in eventDirectory.GetDirectories())
                    {
                        if (modelDirectory.Name != "FIA")
                        {
                            modelDirectory.Delete(true);
                        }
                    }
                }
            }
            CleanDirectory.Name = "The Watershed Alternatives Directory has been Cleaned.";
        }

        public void ReadWatershedData(object sender, EventArgs e)
        {
            
            //double iteration = 0;
            //IsProgressing = true;
            //object thisLock = new object();
            //double iterations = new System.IO.DirectoryInfo(WatershedFolderPath).GetDirectories().Count();

            ReadData.IsEnabled = false;
            ConcurrentBag<string> AlternativesDirectories = new ConcurrentBag<string>();
            ConcurrentBag<string> ExportAlternativeDataFilePaths = new ConcurrentBag<string>();
            System.IO.DirectoryInfo watershedDirectory = new System.IO.DirectoryInfo(WatershedFolderPath);

            //Task.Factory.StartNew(() =>
            //Parallel.ForEach(watershedDirectory.GetDirectories(), alternativePath =>
            //{
            //    AlternativesDirectories.Add(alternativePath.FullName);
            //    ExportAlternativeDataFilePaths.Add(watershedDirectory.FullName + "-" + alternativePath.Name + "-" + DateTime.Now.ToString("yyMMdd") + ".txt");

            //    lock (thisLock)
            //    {
            //        Progress = (int)((iteration + 1) / iterations * 100);
            //    }

            //}));
            Parallel.ForEach(watershedDirectory.GetDirectories(), alternativePath =>
            {
                AlternativesDirectories.Add(alternativePath.FullName);
                ExportAlternativeDataFilePaths.Add(watershedDirectory.FullName + "-" + alternativePath.Name + "-" + DateTime.Now.ToString("yyMMdd") + ".txt");
            });
            WriteDataFilePaths = ExportAlternativeDataFilePaths.ToList();
            Watershed = new Model.Watershed(AlternativesDirectories.ToList());
            BuildSelectableList();

            WriteData.IsEnabled = true;
            ReadData.Name = "The Watershed Data is being Held in Memory";
        }

        //public void BuildSelectedMetrics(object sender, EventArgs e)
        //{
        //    SelectionChangedEventArgs d = (SelectionChangedEventArgs)e;
        //    foreach (string metric in d.RemovedItems)
        //    {
        //        SelectedMetrics.Remove(metric);
        //    }
        //    foreach (string metric in d.AddedItems)
        //    {
        //        SelectedMetrics.Add(metric);
        //    }
        //}

        public void WriteWatershedData(object sender, EventArgs e)
        {
            WriteData.IsEnabled = false;
            for (int i = 0; i < Watershed.Alternatives.Count; i++)
            {
                Watershed.Alternatives[i].CalculateAlternativeMetrics(SelectedMetrics);
                Watershed.Alternatives[i].ExportData(WriteDataFilePaths[i]);
            }


            //Parallel.For(0, Watershed.Alternatives.Count, i =>
            //{
            //    Watershed.Alternatives[i].CalculateAlternativeMetrics(SelectedMetrics);
            //    Watershed.Alternatives[i].ExportData(WriteDataFilePaths[i]);
            //});
            Watershed.ExportData(WatershedFolderPath + "-" + DateTime.Now.ToString("yyMMdd") + ".txt");
            Watershed.ExportErrors(WatershedFolderPath + "Errors-" + DateTime.Now.ToString("yyMMdd") + ".txt");
            WriteData.Name = "The Results have been Complied and Placed in the Watershed Alterantives Directory";
        }
        #endregion

        #region Voids
        private void BuildSelectableList()
        {
            SelectableMetrics = new List<string>();
            for (int i = 0; i < Watershed.Alternatives.Count; i++)
            {
                for (int j = 0; j < Watershed.Alternatives[i].Events.Length; j++)
                {
                    for (int n = 0; n < Watershed.Alternatives[i].Events[j].Metrics.Count; n++)
                    {
                        if (SelectableMetrics.Contains(Watershed.Alternatives[i].Events[j].Metrics[n].Name) == false)
                        {
                            SelectableMetrics.Add(Watershed.Alternatives[i].Events[j].Metrics[n].Name);
                        }
                    }
                }
            }
        }

        //public void CompileResults(object sender, EventArgs e)
        //{
        //    CompileWatershedResults.IsEnabled = false;
        //    CompileWatershedResults.Name = "Please be patient I'm working...";

        //    List<string> AlternativesDirectories = new List<string>();
        //    List<string> ExportAlternativeDataFilePath = new List<string>();
        //    System.IO.DirectoryInfo watershedDirectory = new System.IO.DirectoryInfo(WatershedFolderPath);
        //    foreach (System.IO.DirectoryInfo alternativePath in watershedDirectory.GetDirectories())
        //    {
        //        AlternativesDirectories.Add(alternativePath.FullName);
        //        ExportAlternativeDataFilePath.Add(alternativePath.Name + "-" + watershedDirectory.FullName + "-" +DateTime.Now.ToString("yyMMdd") + ".txt");
        //    }

        //    Model.Watershed WatershedCompute = new Model.Watershed(AlternativesDirectories);
        //    for (int i = 0; i < WatershedCompute.Alternatives.Count; i++)
        //    {
        //        WatershedCompute.Alternatives[i].ExportData(ExportAlternativeDataFilePath[i]);
        //    }
        //    WatershedCompute.ExportWatershedData(watershedDirectory.FullName + "-" + DateTime.Now.ToString("yyMMdd") + ".txt");
        //    CompileWatershedResults.Name = "The Results have been Complied and Placed in the Watershed Alterantives Directory";
        //}
        #endregion

    }
}
