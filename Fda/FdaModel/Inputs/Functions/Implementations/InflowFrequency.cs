using System.Linq;
using System.Collections.Generic;

namespace Model.Inputs.Functions.Implementations
{
    internal sealed class InflowFrequency : BaseImplementation, IFunctionCompose
    {
        #region Properties
        public override FunctionTypeEnum Type { get; }
        public FunctionTypeEnum UseType { get; private set; }
        #endregion

        #region Constructors
        internal InflowFrequency(IFunctionBase function) : base(function)
        {
            IsValid = Validate();
            Type = FunctionTypeEnum.InflowFrequency;
            UseType = FunctionTypeEnum.InflowFrequency;
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
            if (transform.Type == FunctionTypeEnum.Rating) UseType = FunctionTypeEnum.OutflowFrequency;
            if (transform.Type - 1 == UseType) return FunctionFactory.CreateNew(Function.Compose(transform.Ordinates), transform.Type + 1); 
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
