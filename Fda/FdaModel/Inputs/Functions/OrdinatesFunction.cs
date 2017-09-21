using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Model.Inputs.Functions
{
    internal sealed class OrdinatesFunction: IFunctionBase
    {
        #region Fields
        private Statistics.CurveIncreasing _Function;
        #endregion

        #region Properties     
        public bool IsValid { get; private set; }
        public Statistics.CurveIncreasing Function
        {
            get
            {
                return _Function;
            }
            private set
            {
                _Function = value;
                IsValid = Validate();
                Ordinates = SetOrdinates();
            }
        }
        public IList<Tuple<double, double>> Ordinates { get; private set; }
        #endregion

        #region Constructors
        internal OrdinatesFunction(Statistics.CurveIncreasing function)
        {
            Function = function;
            ReportValidationErrors();
        }
        #endregion

        #region Methods
        private IList<Tuple<double, double>> SetOrdinates()
        {
            return Function.XValues.Zip(Function.YValues, (x, y) => new Tuple<double, double>(x, y)).ToList();
        }
        #endregion

        #region IFunctionBase Methods
        public bool ValidateFrequencyValues()
        {
           if (Ordinates[0].Item1 <= 0 ||
               Ordinates[0].Item1 >= 1 ||
               Ordinates[Function.Count - 1].Item1 <= 0 ||
               Ordinates[Function.Count - 1].Item1 >= 1) return false;
           else return true;
        }
        /// <summary>
        /// Since the Y ordinate values are expressed as point estimates, instead of distribution functions, this method returns the intance of ordinates function to which the method refers.
        /// </summary>
        /// <param name="probability"> All input values return the same result. A default value of double.NaN is provided. </param>
        /// <returns> The current instance of the ordinates function. </returns>
        public IFunctionBase Sample(double probability = double.NaN)
        {
            return this;
        }
        public IList<Tuple<double, double>> GetOrdinates()
        {
            return Ordinates;
        }
        public IList<Tuple<double, double>> Compose(IList<Tuple<double, double>> transformOrdinates)
        {
            List<Tuple<double, double>> newOrdinates;
            int i = FirstFrequencyIndex(transformOrdinates, out newOrdinates), I = Ordinates.Count - 1;
            if (i == I) return newOrdinates = Ordinates.Select((x, y) => new Tuple<double, double>(x.Item1, double.NaN)).ToList();

            int j = FirstTransformIndex(transformOrdinates), J = transformOrdinates.Count - 1;
            if (j == J) return newOrdinates = Ordinates.Select((x, y) => new Tuple<double, double>(x.Item1, double.NaN)).ToList();
            
            while (i < I + 1)
            {
                newOrdinates.Add(new Tuple<double, double>(Ordinates[i].Item1, transformOrdinates[j].Item2 + (Ordinates[i].Item2 - transformOrdinates[j].Item1) / (transformOrdinates[j + 1].Item1 - transformOrdinates[j].Item1) * (transformOrdinates[j + 1].Item2 - transformOrdinates[j].Item2)));
                while (IsTransformPointInBetweenFrequencyOrdinates(transformOrdinates, i, j) == true)
                {
                    if (j + 1 > J)
                    {
                        AddPointsAboveTransform(newOrdinates, transformOrdinates[J].Item2, ref i);
                        break;
                    }
                    else
                    {
                        j++;
                        newOrdinates.Add(new Tuple<double, double>(Ordinates[i].Item1 + (transformOrdinates[j].Item1 - Ordinates[i].Item2) / (Ordinates[i + 1].Item2 - Ordinates[i].Item2) * (Ordinates[i + 1].Item1 - Ordinates[i].Item1), transformOrdinates[j].Item2));
                    }
                }
                if (IncrementTransformOrdinates(transformOrdinates, i, j) == true) j++;
                if (j == J) AddPointsAboveTransform(newOrdinates, transformOrdinates[J].Item2, ref i);
                i++;  
            }
            return newOrdinates;
        }
        private int FirstFrequencyIndex(IList<Tuple<double, double>> transformOrdinates, out List<Tuple<double, double>> newOrdinates)
        {
            int i = 0, I = Ordinates.Count - 1;
            newOrdinates = new List<Tuple<double, double>>();
            while (Ordinates[i].Item2 < transformOrdinates[0].Item1)
            {
                newOrdinates.Add(new Tuple<double, double>(Ordinates[i].Item1, transformOrdinates[0].Item2));
                if (i == I) break;
                else i++;
            }
            return i;
        }
        private int FirstTransformIndex(IList<Tuple<double, double>> transformOrdinates)
        {
            int j = 0, J = transformOrdinates.Count - 1;
            while (transformOrdinates[j].Item1 < Ordinates[0].Item2)
            {
                if (j == J) break;
                else j++;
            }
            if (!(j == 0 || j == J)) j = j - 1;
            return j;
        }
        private bool IsTransformPointInBetweenFrequencyOrdinates(IList<Tuple<double, double>> transformOrdinates, int i, int j)
        {
            if (i < Ordinates.Count - 1 && j < transformOrdinates.Count - 1 &&
                transformOrdinates[j + 1].Item1 > Ordinates[i].Item2 &&
                transformOrdinates[j + 1].Item1 < Ordinates[i + 1].Item2) return true;
            else return false;
        }
        private bool IncrementTransformOrdinates(IList<Tuple<double,double>> transformOrdinates, int i, int j)
        {
            if (i < Ordinates.Count - 1 && j < transformOrdinates.Count - 1 &&  //changed from j + 1 => j and from Ordinates => transformOrdinates
                transformOrdinates[j + 1].Item1 < Ordinates[i + 1].Item2) return true;
            else return false;
        }
        private IList<Tuple<double, double>> AddPointsAboveTransform(IList<Tuple<double, double>> newOrdinates, double maxTransformValue, ref int i)
        {
            while (i < Ordinates.Count - 1)
            {
                i++;
                newOrdinates.Add(new Tuple<double, double>(Ordinates[i].Item1, maxTransformValue));
            }
            return newOrdinates;
        }
        public string ReportCompositionMessages(IFunctionTransform transform)
        {
            StringBuilder compositionMessages = new StringBuilder();
            return compositionMessages.ToString();
        }
        public double GetXfromY(double y)
        {
            if (Ordinates[0].Item2 >= y) return Ordinates[0].Item1;
            if (Ordinates[Ordinates.Count - 1].Item2 <= y) return Ordinates[Ordinates.Count - 1].Item1;

            int i = 0;
            while (Ordinates[i + 1].Item2 < y)
            {
                i++;
            }
            return Ordinates[i].Item1 + (y - Ordinates[i].Item2) / (Ordinates[i + 1].Item2 - Ordinates[i].Item2) * (Ordinates[i + 1].Item1 - Ordinates[i].Item1);
        }
        public double GetYfromX(double x)
        {
            if (Ordinates[0].Item1 >= x) return Ordinates[0].Item2;
            if (Ordinates[Ordinates.Count - 1].Item1 <= x) return Ordinates[Ordinates.Count - 1].Item2;

            int i = 0;
            while (Ordinates[i + 1].Item1 < x)
            {
                i++;
            }
            return Ordinates[i].Item2 + (x - Ordinates[i].Item1) / (Ordinates[i + 1].Item1 - Ordinates[i].Item1) * (Ordinates[i + 1].Item2 - Ordinates[i].Item2);
        }
        public double TrapezoidalRiemannSum()
        {
            double riemannSum = 0;
            for (int i = 0; i < Function.Count - 1; i++)
            {
                riemannSum += (Function.get_Y(i + 1) + Function.get_Y(i)) * (Function.get_X(i + 1) - Function.get_X(i)) / 2;
            }
            return riemannSum;
        }
        
        private IList<Tuple<double, double>> Aggregate(IList<Tuple<double, double>> addOrdinates)
        {
            int i = 0, I = Ordinates.Count - 1, j = 0, J = addOrdinates.Count - 1;
            IList<Tuple<double, double>> aggregatedOrdinates = new List<Tuple<double, double>>();
            if (Ordinates[0].Item1 < addOrdinates[0].Item1) i = IndexBelowOverlap(Ordinates, addOrdinates[0].Item1, out aggregatedOrdinates);     
            if (Ordinates[0].Item1 > addOrdinates[0].Item1) j = IndexBelowOverlap(addOrdinates, Ordinates[0].Item1, out aggregatedOrdinates);
            //if (i == I || j == J) return aggregatedOrdinates;

            int n = 0, N = (I - i) + (J - j);
            while (n < N + 1)
            {
                if (Ordinates[i].Item1 < addOrdinates[j].Item1) aggregatedOrdinates.Add(Ordinates[i].Item1, Ordinates[i].Item2 + ())

            }
            while (i < I + 1)
            {
                
            }

        }
        private IFunctionBase Aggregate(IFunctionBase functionToAggregate)
        {
            IList<Tuple<double, double> addOrdinates
            IFunctionBase lowerOrdinates, higherOrdinates;
            if (Ordinates[0].Item1 < addOrdinates.GetOrdinates().Ordinates[0].Item1) { lowerOrdinates = Ordinates; higherOrdinates = addOrdinates; }
            else { lowerOrdinates = addOrdinates; higherOrdinates = Ordinates; }

            int i = 0, j = 0, I = lowerOrdinates.Count, J = higherOrdinates.Count;
            while (lowerOrdinates[i].Item1 < higherOrdinates[j].Item1)
            {
                double newStage = lowerOrdinates[i].Item1;
                
            }
        }


        private int IndexBelowOverlap(IList<Tuple<double, double>> lowerOrdinates, double firstSharedX, out IList<Tuple<double, double>> newOrdinates)
        {
            int n = 0, N = lowerOrdinates.Count - 1;
            newOrdinates = new List<Tuple<double, double>>();
            while (lowerOrdinates[n].Item1 < firstSharedX)
            {
                newOrdinates.Add(lowerOrdinates[n]);
                if (n == N) break;
                else n++;
            }
            return n;
        }
        
        #endregion

        #region IValidateData Methods
        public bool Validate()
        {
            if (Function.Count < 2) return false;
            for (int i = 0; i < Function.Count - 1; i++)
            {
                if (Function.get_X(i + 1) == Function.get_X(i))
                {
                    Function.RemoveAt(i);
                    if (Function.Count < 2) return false;
                    else i--;
                }
            }
            if (Function.IsValid == false) return false;
            else return true;
        }
        public IEnumerable<string> ReportValidationErrors()
        {
            List<string> errors = new List<string>();
            if (Function.Count < 2)
            {
                IsValid = false;
                errors.Add("The domain and range (e.g. X and Y ordinates) must contain two or more equal length data pairs. The provided data does not; it will be stored in an unusable state and must be edited before it can be used in a compute.");
            }


            else
            {
                for (int i = 0; i < Function.Count - 1; i++)
                {
                    if (Function.get_X(i + 1) == Function.get_X(i))
                    {
                        errors.Add("A duplicate ordinate (" + Function.get_X(i) + "," + Function.get_Y(i) + ") was found. This ordinate has been removed.");
                        Function.RemoveAt(i);
                        if (Function.Count < 2)
                        {
                            IsValid = false;
                            errors.Add("One or more duplicate ordinates was removed and the provided data no longer contains two or more equal length pairs of data (as is required by the program); the function will be stored in an unusable state and must be edited before it can be used in a compute.");
                        }
                        else i--;
                    }
                }
            }
            if (Function.IsValid == false)
            {
                IsValid = false;
                errors.AddRange(Function.GetErrors());
            }
            if (errors.Count == 0)
            {
                errors.Add("No errors were found.");
            }
            return errors;
        }
        #endregion
    }
}
