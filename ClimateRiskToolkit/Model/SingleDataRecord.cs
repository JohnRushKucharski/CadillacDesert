using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateRiskToolkit.Model
{
    public class SingleDataRecord
    {
        #region Fields
        private string _DataLabel;
        private bool _IsSampledData;
        private Tuple<DateTime, double>[] _Record;
        #endregion

        #region Properties
        public string DataLabel
        {
            get
            {
                return _DataLabel;
            }
            private set
            {
                _DataLabel = value;
            }
        }

        public bool IsSampledData
        {
            get
            {
                return _IsSampledData;
            }
            private set
            {
                _IsSampledData = value;
            }
        }
        public Tuple<DateTime, double>[] Record
        {
            get
            {
                return _Record;
            }
            private set
            {
                _Record = value;
            }
        }
        #endregion

        #region Constructor
        public SingleDataRecord(int[] month, int[] year, string dataLabel, double[] data, bool isSampled)
        {
            Tuple<DateTime, double>[] record = new Tuple<DateTime, double>[data.Length];
            if (year.Length == month.Length && month.Length == data.Length)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    record[i] = new Tuple<DateTime, double>(new DateTime(year[i], month[i], 1), data[i]);
                }
            }
            else
            {
                throw new Exception("The date time records and data are not the same length");
            }
            Record = record;
            DataLabel = dataLabel;
            IsSampledData = isSampled;
        }
        private SingleDataRecord(string dataLabel, Tuple<DateTime, double>[] sampledData)
        {
            Tuple<DateTime, double>[] sample = new Tuple<DateTime, double>[sampledData.Length];
            for (int i = 0; i < sampledData.Length; i++)
            {
                sample[i] = sampledData[i];
            }
            Record = sample;
            DataLabel = dataLabel;
            IsSampledData = true;
        }
        #endregion

        #region Functions
        public SingleDataRecord[] Bootstrap(int[] pattern, int nPatterns, int nSamples, int seed)
        {
            //1. Dictionary containing last array value (e.g. count - 1) for each strata
            Dictionary<int, int> patternData = new Dictionary<int, int>();
            for (int i = 0; i < pattern.Length; i++)
            {
                int n = -1;
                foreach (var pair in Record)
                {
                    if (pair.Item1.Month == pattern[i])
                    {
                        n++;
                    }
                }
                patternData.Add(pattern[i], n);
            }

            Random randomNumberGenerator = new Random(seed);
            SingleDataRecord[] samples = new SingleDataRecord[nSamples];
            Tuple<DateTime, double>[] sampledData = new Tuple<DateTime, double>[pattern.Length * nPatterns];

            //2. Number of samples to pull
            for (int n = 0; n < nSamples; n++)
            {
                int c = 0;
                //3. Length of each sample
                for (int t = 0; t < nPatterns; t++)
                {
                    //4. Pattern
                    for (int i = 0; i < pattern.Length; i++)
                    {
                        int selector = 0;
                        //5. Select array value to pull (selector).
                        foreach (var countPair in patternData)
                        {
                            if (countPair.Key == pattern[i])
                            {
                                if (countPair.Value < 0)
                                {
                                    System.Windows.MessageBox.Show("The strata value " + countPair.Key + " does not appear in the data, thus no value can for strata can be drawn.");
                                    return null;
                                }
                                selector = randomNumberGenerator.Next(countPair.Value);
                                break;
                            }
                        }

                        int counter = 0;
                        foreach (var dataPair in Record)
                        {
                            //6. Find ith occurance of strata 
                            if (dataPair.Item1.Month == pattern[i])
                            {
                                if (counter < selector)
                                {
                                    counter++;
                                }
                                else
                                {
                                    sampledData[c] = new Tuple<DateTime, double>(new DateTime(t + 1, dataPair.Item1.Month, 1), dataPair.Item2);
                                    c++;
                                    break;
                                }
                            }
                        }
                    }
                }
                samples[n] = new SingleDataRecord(DataLabel, sampledData);
                Array.Clear(sampledData, 0, sampledData.Length);
            }
            return samples;
        }
        #endregion

    }
}
