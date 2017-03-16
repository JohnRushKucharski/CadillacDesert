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
using System.Windows.Shapes;

namespace ClimateRiskToolkit.View
{
    /// <summary>
    /// Interaction logic for ImportDataWindow.xaml
    /// </summary>
    public partial class ImportDataWindow : Window
    {
        public ImportDataWindow()
        {
            InitializeComponent();
            this.btnLoadDataRecord.Click += (o, e) => ((ViewModel.SingleDataRecordVM)this.Resources["SingleDataRecordVM"]).ImportDataCommand.Action(o, e);

            //ViewModel.SingleDataRecordVM record = (ViewModel.SingleDataRecordVM)this.Resources["SingleDataRecordVM"];
            //ViewModel.NamedAction action = record.ImportDataCommand;


            //ViewModel.NamedAction action = ((ViewModel.SingleDataRecordVM)this.Resources["SingleDataRecordVM"]).ImportDataCommand;
            //System.Windows.Controls.Button button = this.btnLoadDataRecord;
            //button.Click += (o, e) => action.Action(o, e);
        }
    }
}
