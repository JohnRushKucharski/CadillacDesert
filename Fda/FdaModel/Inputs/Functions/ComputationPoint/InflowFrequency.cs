using System;
using System.Linq;
using System.Collections.Generic;

namespace Model.Inputs.Functions.ComputationPoint
{
    public sealed class InflowFrequency : ComputationPointFunctionBase, IFunctionCompose
    {
        #region Properties
        public override ComputationPointFunctionEnum Type { get; }
        public ComputationPointFunctionEnum UseType { get; private set; }
        #endregion

        #region Constructors
        internal InflowFrequency(IFunctionBase function) : base(function)
        {
            IsValid = Validate();
            Type = ComputationPointFunctionEnum.InflowFrequency;
            UseType = ComputationPointFunctionEnum.InflowFrequency;
        }
        #endregion

        #region IFunctionCompose Methods
        /// <summary> Generates either (a) an outflow frequency function by composing the inflow frequency function with a inflow outflow transform function, or (b) an exterior stage frequency function by composing the inflow frequency function (being used as an outflow frequency function) with a rating function. If transform function that is passed in is not an inflow outflow or rating function an exception is thrown, because the functions do not share a common set of ordinates. </summary>
        /// <param name="transform"> The transform function to be used in the composition equation, often written as f(g(x)). An exception will be thrown if the computation function type does not share a common set of ordinates with the frequency function used in the composition. </param>
        /// <param name="frequencyFunctionProbability"> An optional sampling parameter between 0 and 1 that is set to 0.50 as a default. The parameter value represents the chance that the frequency function values are less than or equal to sampled frequency function values. The central tendency of the statistically distrubuted frequency function is computed as a default. If the input frequency function is not statistically distributed the input frequency function values are computed. </param>
        /// <param name="transformFunctionProbability"> An optional sampling parameter between 0 and 1 that is set to 0.50 as a default. The parameter value represents the chance that the transform function values are less than or equal to sampled transform function values. The central tendency of the statistically distrubuted transform function is computed as a default. If the input transform function is not statistically distributed the input transform function values are computed. </param>
        /// <returns> A new frequency function if the transform functions range can be mapped to the frequency function domain. </returns>
        public IFunctionCompose Compose(IFunctionTransform transform, double frequencyFunctionProbability = 0.5, double transformFunctionProbability = 0.5)
        {
            if (IsValidComposition(transform) == true)
                return ComputationPointFunctionFactory.CreateNew(Function.Sample(frequencyFunctionProbability).Compose(transform.Sample(transformFunctionProbability).Ordinates), transform.Type + 1);
            else ReportCompositionError(); throw new NotImplementedException();
        }
        private bool IsValidComposition(IFunctionTransform transform)
        {
            if (transform.Type == ComputationPointFunctionEnum.Rating) UseType = ComputationPointFunctionEnum.OutflowFrequency;
            if (transform.Type - 1 == UseType) return true;
            else return false;
        }
        private string ReportCompositionError()
        {
            return "Composition could not be initialized because the two functions do not share a common set of ordinates.";
        }
        public double GetXFromY(double y)
        {
            return Function.GetXfromY(y);
        }
        public double Integrate()
        {
            return Function.TrapezoidalRiemannSum();
        }
        #endregion

        #region IValidateData Methods
        public override bool Validate()
        {
            if (!(Function.GetType() == typeof(FrequencyFunction))) return false;
            else return Function.IsValid;
        }
        public override IEnumerable<string> ReportValidationErrors()
        {
            List<string> errors = Function.ReportValidationErrors().ToList();
            if (Function.GetType() != typeof(FrequencyFunction)) errors.Add("A valid statistical relationship must define inflow frequency functions. The input inflow frequency function is not defined by a valid statistical relationship, it will be held in an uncomputable state until this error has been corrected.");
            return errors;
        }
        #endregion
    }
}
