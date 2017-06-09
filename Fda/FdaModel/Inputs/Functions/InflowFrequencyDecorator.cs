using System;
using System.Linq;
using System.Collections.Generic;

namespace Model.Inputs.Functions
{
    internal sealed class InflowFrequencyDecorator : Decorator, IComputableFunction
    {
        #region Properties
        public override FunctionType Type { get; }
        #endregion

        #region Constructors
        internal InflowFrequencyDecorator(IFunction function) : base(function)
        {
            IsValid = Validate();
            Type = FunctionType.InflowFrequency;
        }
        #endregion

        #region Methods
        public override bool Validate()
        {
            if (Function.GetType() == typeof(FrequencyFunction)) return Function.IsValid;
            else return false;
        }

        public override IEnumerable<string> ReportValidationErrors()
        {
            List<string> errors = Function.ReportValidationErrors().ToList();
            if (Function.GetType() != typeof(FrequencyFunction)) errors.Add("A valid statistical relationship must define inflow frequency functions. The input inflow frequency function is not defined by a valid statistical relationship, it will be held in an uncomputable state until this error has been corrected.");
            return errors;
        }

        public IComputableFunction Compose(IFunction transformFunction)
        {
            if (transformFunction.Type == FunctionType.InflowOutflow) throw new NotImplementedException(); //Function.Compose
            if (transformFunction.Type == FunctionType.Rating) FunctionFactory.CreateNew(Function, FunctionType.OutflowFrequency);
            //Compose();

            
            throw new NotImplementedException();
        }

        #endregion
    }
}
