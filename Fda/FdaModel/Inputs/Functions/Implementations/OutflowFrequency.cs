using System.Linq;
using System.Collections.Generic;

namespace Model.Inputs.Functions.Implementations
{
    internal sealed class OutflowFrequency : BaseImplementation, IFunctionCompose
    {
        #region Properties
        public override FunctionTypeEnum Type { get; }
        #endregion

        #region Constructor
        internal OutflowFrequency(IFunctionBase function) : base(function)
        {
            Type = FunctionTypeEnum.OutflowFrequency;
            IsValid = Validate();
        }
        #endregion

        #region BaseImplementation Methods
        public override double GetXfromY(double y)
        {
            return Function.GetXfromY(y);
        }
        #endregion

        #region IFunctionCompose Methods
        public IFunctionCompose Compose(IFunctionTransform transform)
        {
            if (transform.Type - 1 == Type) return FunctionFactory.CreateNew(Function.Compose(transform.Ordinates), transform.Type + 1);
            else ReportCompositionError(); return null;
        }
        private string ReportCompositionError()
        {
            return "Composition could not be initialized because no transform function was provided or the two functions do not share a common set of ordinates.";
        }
        #endregion

        #region IValidateData Methods
        public override bool Validate()
        {
            if (Function.ValidateFrequencyValues(Type) == false) return false;
            else return Function.IsValid;
        }
        public override IEnumerable<string> ReportValidationErrors()
        {
            List<string> messages = Function.ReportValidationErrors().ToList();
            if (Function.ValidateFrequencyValues(Type) == false) { IsValid = false; messages.Add("The frequency function is invalid because it contains ordinates outside of the valid domain of [0, 1]."); }
            return messages;
        }
        #endregion
    }
}
