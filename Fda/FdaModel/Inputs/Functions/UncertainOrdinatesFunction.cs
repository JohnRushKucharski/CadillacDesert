using System;
using System.Linq;
using System.Collections.Generic;

namespace Model.Inputs.Functions
{
    internal sealed class UncertainOrdinatesFunction : IFunctionBase
    {
        #region Fields
        private Statistics.UncertainCurveIncreasing _UncertainFunction;
        #endregion

        #region Properties
        public bool IsValid { get; private set; }
        public Statistics.UncertainCurveIncreasing UncertainFunction
        {
            get
            {
                return _UncertainFunction;
            }
            private set
            {
                _UncertainFunction = value;
                IsValid = Validate();
                Ordinates = SetOrdinates();
                CentralTendencyFunction = new OrdinatesFunction(new Statistics.CurveIncreasing(UncertainFunction.XValues.ToArray(), Ordinates.Select(o => o.Item2).ToArray(), true, false));
            }
        }
        private IFunctionBase CentralTendencyFunction { get; set; }
        public IList<Tuple<double, double>> Ordinates { get; private set; }
        #endregion

        #region Constructors
        internal UncertainOrdinatesFunction(Statistics.UncertainCurveIncreasing function)
        {
            UncertainFunction = function;
            Validate();
        }
        #endregion

        #region Methods
        private IList<Tuple<double, double>> SetOrdinates()
        {
            return UncertainFunction.XValues.Zip(UncertainFunction.YValues, (x, y) => new Tuple<double, double>(x, y.GetCentralTendency)).ToList();
        }
        #endregion

        #region IFunctionBase Methods
        public bool ValidateFrequencyValues()
        {
            return CentralTendencyFunction.ValidateFrequencyValues();
        }
        /// <summary>
        /// Shifts the entire function by getting Y values from the Y values cummulative density function, based on a single input probability.
        /// </summary>
        /// <param name="probability"> The probability of the Y values to be sampled from the Y values cummulative density functions. The same probability value is applied to all ordinates. </param>
        /// <returns> A new, shifted ordinates function with Y ordinates expressed as point values (rather than distribution functions). </returns>
        public IFunctionBase Sample(double probability)
        {
            return new OrdinatesFunction((Statistics.CurveIncreasing)UncertainFunction.CurveSample(probability));
        }
        public IList<Tuple<double, double>> GetOrdinates()
        {
            return Ordinates;
        }
        public IList<Tuple<double, double>> Compose(IList<Tuple<double, double>> transformOrdinates)
        {
            return CentralTendencyFunction.Compose(transformOrdinates);
        }
        public double GetXfromY(double y)
        {
            return CentralTendencyFunction.GetXfromY(y);
        }
        public double GetYfromX(double x)
        {
            return CentralTendencyFunction.GetYfromX(x);
        }
        public double TrapezoidalRiemannSum()
        {
            double riemannSum = 0;
            for (int i = 0; i < Ordinates.Count - 1; i++)
            {
                riemannSum += (Ordinates[i + 1].Item2 + Ordinates[i].Item2) * (Ordinates[i + 1].Item1 - Ordinates[i].Item1) / 2;
            }
            return riemannSum;
        }
        #endregion

        #region IValidateData Methods
        public bool Validate()
        {
            if (UncertainFunction.IsValid == true) return true;
            else return false;
        }
        public IEnumerable<string> ReportValidationErrors()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
