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
        private int _DataColumn;
        private int _YearColumn;
        private int _MonthColumn;
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
        #endregion


        #region Constructors
        public SingleDataRecordVM()
        {
            FilePath = "Select a File Path...";
            DataLabel = "Enter a data description, e.g. Precipitation, Temperature, Flow, etc...";
        }
        #endregion
    }
}
