using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions.ComputationPoint
{
    public sealed class DamageFrequency : ComputationPointFunctionBase, IFunctionCompose
    {
        #region Properties
        public override ComputationPointFunctionEnum Type { get; }
        #endregion

        #region Constructor
        internal DamageFrequency(IFunctionBase function) : base(function)
        {
            Type = ComputationPointFunctionEnum.DamageFrequency;
            IsValid = Validate();
        }
        #endregion

        #region IFunctionCompose Methods
        public IFunctionCompose Compose(IFunctionTransform transform, double frequencyFunctionProbability, double transformFunctionProbability)
        {
            if (transform.Type - 1 == Type)
                return ComputationPointFunctionFactory.CreateNew(Function.Sample(frequencyFunctionProbability).Compose(transform.Sample(transformFunctionProbability).Ordinates), transform.Type + 1);
            else ReportCompositionError(); return null;
        }
        private string ReportCompositionError()
        {
            return "Composition could not be initialized because no transform function was provided or the two functions do not share a common set of ordinates.";
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
            if (Function.ValidateFrequencyValues() == false) return false;
            else return Function.IsValid;
        }
        public override IEnumerable<string> ReportValidationErrors()
        {
            List<string> messages = Function.ReportValidationErrors().ToList();
            if (Function.ValidateFrequencyValues() == false) { IsValid = false; messages.Add("The frequency function is invalid because it contain ordinates outside of the valid domain of [0, 1]."); }
            return messages;
        }
        #endregion
    }
}
