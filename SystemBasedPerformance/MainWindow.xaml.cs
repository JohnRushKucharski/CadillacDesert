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

namespace SystemBasedPerformance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGenerateResultsFiles_Click(object sender, RoutedEventArgs e)
        {
            List<string> AlternativesDirectories = new List<string>();
            List<string> ExportAlternativeDataFilePath = new List<string>();
            System.IO.DirectoryInfo oldGeometryAlternatives = new System.IO.DirectoryInfo(@"X:\kucharski\SystemBasedPerformance\NorthBranchResults\24hour\NewGeometry");
            foreach (System.IO.DirectoryInfo alternativePath in oldGeometryAlternatives.GetDirectories())
            {
                AlternativesDirectories.Add(alternativePath.FullName);
                ExportAlternativeDataFilePath.Add(alternativePath.FullName + "-oldGeometry170315.txt");
            }

            Watershed WatershedCompute = new Watershed(AlternativesDirectories);
            for (int i = 0; i < WatershedCompute.Alternatives.Count; i++)
            {
                WatershedCompute.Alternatives[i].ExportData(ExportAlternativeDataFilePath[i]);
            }
            WatershedCompute.ExportWatershedData(oldGeometryAlternatives.FullName + "\\Watershed-oldGeometry170315.txt");
        }

        private void btnCleanAlternativeDirectories_Click(object sender, RoutedEventArgs e)
        {
            System.IO.DirectoryInfo deleteTestAlternatives = new System.IO.DirectoryInfo(@"X:\kucharski\SystemBasedPerformance\NorthBranchResults\24hour\NewGeometry");
            foreach (System.IO.DirectoryInfo alternativeDirectory in deleteTestAlternatives.GetDirectories())
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
        }
    }
}
