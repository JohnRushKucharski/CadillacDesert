using System;
using System.Linq;
using System.Collections.Generic;

namespace Model.Inputs.Functions
{
    internal sealed class UncertainOrdinatesFunction : IFunctionBase, IFunctionOrdinates
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
            ReportValidationErrors();
        }
        #endregion

        #region Methods
        private IList<Tuple<double, double>> SetOrdinates()
        {
            return UncertainFunction.XValues.Zip(UncertainFunction.YValues, (x, y) => new Tuple<double, double>(x, y.GetCentralTendency)).ToList();
        }
        #endregion

        #region IFunctionBase Methods
        public bool ValidateFrequencyValues(FunctionTypeEnum type)
        {
            return CentralTendencyFunction.ValidateFrequencyValues(type);
        }
        public IFunctionBase Sample(Random randomNumberGenerator)
        {
            return new OrdinatesFunction((Statistics.CurveIncreasing)UncertainFunction.CurveSample(randomNumberGenerator.NextDouble()));
        }
        public IList<Tuple<double, double>> Compose(IList<Tuple<double, double>> transformOrdinates)
        {
            return CentralTendencyFunction.Compose(transformOrdinates);
        }
        public double GetXfromY(double y)
        {
            return CentralTendencyFunction.GetXfromY(y);
        }
        #endregion

        #region IValidateData Methods
        public bool Validate()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<string> ReportValidationErrors()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
