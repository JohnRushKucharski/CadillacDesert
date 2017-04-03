using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBasedPerformance.Model
{
    public class Metric
    {
        private string _Name;
        private double _Value;
        private bool _HasError;
        private string _ErrorMessage;

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
        public bool HasError
        {
            get
            {
                return _HasError;
            }
            set
            {
                _HasError = value;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
            set
            {
                _ErrorMessage = value;
            }
        }

        public Metric(string metricName, double metricValue)
        {
            Name = metricName;
            Value = metricValue;
            if (double.IsNaN(metricValue) == true)
            {
                HasError = true;
                ErrorMessage = "The metric value is not a number.";
            }
            else
            {
                HasError = false;
            }
        }
        public Metric(string metricName, string errorMessage)
        {
            Name = metricName;
            Value = double.NaN;
            HasError = true;
            ErrorMessage = errorMessage;
        }


        public void ReportMessage(string errorMessage)
        {
            Value = double.NaN;
            HasError = true;
            ErrorMessage = errorMessage;
        }
    }
}
