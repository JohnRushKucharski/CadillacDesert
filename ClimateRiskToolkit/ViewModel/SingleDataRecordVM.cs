using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateRiskToolkit.ViewModel
{
    public class SingleDataRecordVM: BaseViewModel 
    {
        #region Fields
        private string _FilePath;
        private string _DataLabel;
        private List<string> _ColumnDataHeaders;
        private List<string> _SelectedColumnDataHeaders;
        private int _DataColumn;
        private int _YearColumn;
        private int _MonthColumn;
        private int _DayColumn;
        private NamedAction _ReadColumnHeaders;
        private NamedAction _ImportDataCommand;
        #endregion


        #region Properties
        public string FilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                _FilePath = value;
                NotifyPropertyChanged(nameof(FilePath));

                ColumnDataHeaders = Utilities.TextDataImporter.ReadColumnNames(FilePath).ToList();
                SelectedColumnHeaders = new List<string> { ColumnDataHeaders.FirstOrDefault() };
            }
        }
        public string DataLabel
        {
            get
            {
                return _DataLabel;
            }
            set
            {
                _DataLabel = value;
                NotifyPropertyChanged(nameof(DataLabel));
            }
        }
        public List<string> ColumnDataHeaders
        {
            get
            {
                return _ColumnDataHeaders;
            }
            private set
            {
                _ColumnDataHeaders = value;
                NotifyPropertyChanged(nameof(ColumnDataHeaders));
            }
        }
        public List<string> SelectedColumnHeaders
        {
            get
            {
                return _SelectedColumnDataHeaders;
            }
            set
            {
                _SelectedColumnDataHeaders = value;
                NotifyPropertyChanged(nameof(SelectedColumnHeaders));
            }
        }
        public int DataColumn
        {
            get
            {
                return _DataColumn;
            }
            set
            {
                _DataColumn = value;
                NotifyPropertyChanged(nameof(DataColumn));
            }
        }
        public int YearColumn
        {
            get
            {
                return _YearColumn;
            }
            set
            {
                _YearColumn = value;
                NotifyPropertyChanged(nameof(YearColumn));
            }
        }
        public int MonthColumn
        {
            get
            {
                return _MonthColumn;
            }
            set
            {
                _MonthColumn = value;
                NotifyPropertyChanged(nameof(MonthColumn));
            }
        }
        public int DayColumn
        {
            get
            {
                return _DayColumn;
            }
            set
            {
                _DayColumn = value;
                NotifyPropertyChanged(nameof(DayColumn));
            }
        }
        public NamedAction ReadColumnHeaders
        {
            get
            {
                return _ReadColumnHeaders;
            }
            set
            {
                _ReadColumnHeaders = value;
                NotifyPropertyChanged(nameof(GetColumnHeaders));
            }
        }
        public NamedAction ImportDataCommand
        {
            get
            {
                return _ImportDataCommand;
            }
            set
            {
                _ImportDataCommand = value;
                NotifyPropertyChanged(nameof(ImportDataCommand));
            }
        }
        #endregion


        #region Constructors
        public SingleDataRecordVM()
        {
            
            DataLabel = "describe the data here, e.g. Kafue NOAA Precipitation and Temperature Record 1960-1996";
            ImportDataCommand = new NamedAction("Import Data");
            ImportDataCommand.Action = ImportData;
            ReadColumnHeaders = new NamedAction("Read Column Headers");
            ReadColumnHeaders.Action = GetColumnHeaders;
        }
        #endregion

        #region NamedActionMembers
        public void ImportData(object o, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("This worked in the VM.");
            ImportDataCommand.Action = GetColumnHeaders;
            ImportDataCommand.Name = ("Read Column Headers");
        }
        public void GetColumnHeaders(object o, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("This got column headers.");
            ImportDataCommand.Action = ImportData;
            ImportDataCommand.Name = ("Import Data");
        }

        #endregion
    }
}
