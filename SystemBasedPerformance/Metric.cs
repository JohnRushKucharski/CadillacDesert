using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBasedPerformance
{
    public class Metric
    {
        private string _Name;
        private double _Value;

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
        public double Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }

        public Metric(string metricName, double metricValue)
        {
            Name = metricName;
            Value = metricValue;
        }
    }
}
