using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase_Reader;

namespace SystemBasedPerformance.Model
{
    public class Event
    {
        #region Fields
        private string _Name;
        private System.IO.DirectoryInfo _FileDirectory;
        private System.IO.DirectoryInfo _AltName;
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
        public System.IO.DirectoryInfo AltName
        {
            get
            {
                return _AltName;
            }
            set
            {
                _AltName = value;
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
        public Event(System.IO.DirectoryInfo directory)
        {
            Name = directory.Name;
            
            FileDirectory = new System.IO.DirectoryInfo(directory.FullName + "\\FIA");
            AltName = (FileDirectory.GetDirectories())[0];
            CompileMetrics();
        }
        #endregion

        #region Functions
        public void CompileMetrics()
        {
            Metrics = new List<Metric>();
            foreach (System.IO.FileInfo dbf in AltName.GetFiles("*.dbf"))
            {
                DBFReader dbfReader = new DBFReader(AltName.FullName + "\\" + dbf.Name);
                foreach (string metric in dbfReader.GetNumericColumns())
                {
                    double sum = 0;
                    foreach (var value in dbfReader.GetColumn(metric))
                    {
                        sum += (double)value;
                    }
                    Metrics.Add(new Metric(dbf.Name + "->" + metric, sum));
                }
            }
        }
        #endregion

    }
}
