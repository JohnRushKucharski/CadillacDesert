using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SystemBasedPerformance.ViewModel
{
    public class WatershedViewModel: BaseViewModel
    {
        #region Fields
        private string _WatershedFolderPath;
        private NamedAction _CleanWatershedDirectory;
        private NamedAction _CompileWatershedResults;
        #endregion


        #region Properties
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
                    CleanWatershedDirectory.IsEnabled = true;
                    CompileWatershedResults.IsEnabled = true;
                    NotifyPropertyChanged();
                }
            }
        }

        public NamedAction CleanWatershedDirectory
        {
            get
            {
                return _CleanWatershedDirectory;
            }
            private set
            {
                _CleanWatershedDirectory = value;
                NotifyPropertyChanged();
            }
        }
        
        public NamedAction CompileWatershedResults
        {
            get
            {
                return _CompileWatershedResults;
            }
            private set
            {
                _CompileWatershedResults = value;
                NotifyPropertyChanged();
            }
        }
        #endregion


        #region Constructor
        public WatershedViewModel()
        {
            WatershedFolderPath ="";
            CleanWatershedDirectory = new NamedAction("Clean Watershed Directory");
            CompileWatershedResults = new NamedAction("Compile Watershed Results");
            CleanWatershedDirectory.IsEnabled = false;
            CompileWatershedResults.IsEnabled = false;
            CleanWatershedDirectory.Action = CleanWatershedAlternativesEventsDirectories;
            CompileWatershedResults.Action = CompileResults;
        }
        #endregion


        #region NamedActions
        public void CleanWatershedAlternativesEventsDirectories(object sender, EventArgs e)
        {
            CleanWatershedDirectory.IsEnabled = false;
            CleanWatershedDirectory.Name = "Please be patient I'm working...";

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
            CleanWatershedDirectory.Name = "The Watershed Alternatives Directory has been Cleaned.";
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

        public void CompileResults(object sender, EventArgs e)
        {
            CompileWatershedResults.IsEnabled = false;
            CompileWatershedResults.Name = "Please be patient I'm working...";

            ConcurrentBag<string> AlternativesDirectories = new ConcurrentBag<string>();
            ConcurrentBag<string> ExportAlternativeDataFilePaths = new ConcurrentBag<string>();
            System.IO.DirectoryInfo watershedDirectory = new System.IO.DirectoryInfo(WatershedFolderPath);
            Parallel.ForEach(watershedDirectory.GetDirectories(), alternativePath =>
            {
                AlternativesDirectories.Add(alternativePath.FullName);
                ExportAlternativeDataFilePaths.Add(watershedDirectory.FullName + "-" + alternativePath.Name  + "-" + DateTime.Now.ToString("yyMMdd") + ".txt");
            });

            List<string> ExportFilePaths = ExportAlternativeDataFilePaths.ToList();
            Model.Watershed WatershedCompute = new Model.Watershed(AlternativesDirectories.ToList());
            Parallel.For(0, WatershedCompute.Alternatives.Count, i =>
            {
                WatershedCompute.Alternatives[i].ExportData(ExportFilePaths[i]);
            });

            WatershedCompute.ExportWatershedData(watershedDirectory.FullName + "-" + DateTime.Now.ToString("yyMMdd") + ".txt");
            WatershedCompute.ExportWatershedErrors(watershedDirectory.FullName + "Errors-" + DateTime.Now.ToString("yyMMdd") + ".txt");
            CompileWatershedResults.Name = "The Results have been Complied and Placed in the Watershed Alterantives Directory";
        }
       
        #endregion

    }
}
