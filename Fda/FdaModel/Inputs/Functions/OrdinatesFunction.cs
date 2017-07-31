using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Model.Inputs.Functions
{
    internal sealed class OrdinatesFunction: IFunctionBase, IFunctionOrdinates //FunctionBase //Database.IEntity
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
        public bool ValidateFrequencyValues(FunctionTypeEnum type)
        {
            if (!((int)type % 2 == 0))
            {
                if (Ordinates[0].Item1 <= 0 ||
                    Ordinates[0].Item1 >= 1 ||
                    Ordinates[Function.Count - 1].Item1 <= 0 ||
                    Ordinates[Function.Count - 1].Item1 >= 1) return false;
                else return true;
            }
            else return false;
        }
        public IFunctionBase Sample(Random randomNumberGenerator)
        {
            return this;
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
            if (Function.get_Y(0) >= y) return Function.get_X(0);
            if (Function.get_Y(Function.Count - 1) <= y) return Function.get_X(Function.Count - 1);

            int i = 0;
            while (Function.get_Y(i + 1) < y)
            {
                i++;
            }
            return Function.get_X(i) + (y - Function.get_Y(i)) / (Function.get_Y(i + 1) - Function.get_Y(i)) * (Function.get_X(i + 1) - Function.get_X(i));
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
