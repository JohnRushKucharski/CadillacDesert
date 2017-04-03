using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemBasedPerformance.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.btnCleanAlternativeDirectories.Click += (sender, e) => ((ViewModel.WatershedViewModel)this.Resources["WatershedVM"]).CleanDirectory.Action(sender, e);
            this.btnReadData.Click += (sender, e) => ((ViewModel.WatershedViewModel)this.Resources["WatershedVM"]).ReadData.Action(sender, e);
            this.btnWriteData.Click += (sender, e) => ((ViewModel.WatershedViewModel)this.Resources["WatershedVM"]).WriteData.Action(sender, e);
            //this.lstBoxSelectableMetrics.SelectionChanged += (sender, e) => ((ViewModel.WatershedViewModel)this.Resources["WatershedVM"]).ReadData.Action(sender, e);
        }

        private void lstBoxSelectableMetrics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            foreach (string metric in e.RemovedItems)
            {
                ((ViewModel.WatershedViewModel)this.Resources["WatershedVM"]).SelectedMetrics.Remove(metric);
            }
            foreach (string metric in e.AddedItems)
            {
                ((ViewModel.WatershedViewModel)this.Resources["WatershedVM"]).SelectedMetrics.Add(metric);
            }
        }



        //private void btnGenerateResultsFiles_Click(object sender, RoutedEventArgs e)
        //{
        //    List<string>  AlternativesDirectories = new List<string>();
        //    List<string> ExportAlternativeDataFilePath = new List<string>();
        //    System.IO.DirectoryInfo oldGeometryAlternatives = new System.IO.DirectoryInfo(@"X:\kucharski\SystemBasedPerformance\NorthBranchResults\24hour\NewGeometry");
        //    foreach (System.IO.DirectoryInfo alternativePath in oldGeometryAlternatives.GetDirectories())
        //    {
        //        AlternativesDirectories.Add(alternativePath.FullName);
        //        ExportAlternativeDataFilePath.Add(alternativePath.FullName + "-oldGeometry170315.txt");
        //    }

        //    Model.Watershed WatershedCompute = new Model.Watershed(AlternativesDirectories);
        //    for (int i = 0; i < WatershedCompute.Alternatives.Count; i++)
        //    {
        //        WatershedCompute.Alternatives[i].ExportData(ExportAlternativeDataFilePath[i]);
        //    }
        //    WatershedCompute.ExportData(oldGeometryAlternatives.FullName + "\\Watershed-oldGeometry170315.txt");
        //}
    }
}
