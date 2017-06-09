using System.Collections.Generic;

namespace Model.Inputs.Functions
{
    public sealed class FrequencyFunction : IFunction
    {
        #region Notes
        /* Entity Notes
        1. If Entity cannot be trained to deal with IFunction (or I choose not use it because of this) this would inherit BaseFunction: public sealed FrequencyFunction: BaseFunction
        */
        #endregion

        #region Fields (without Auto Implemented Properties)
        private bool _IsValid;
        private Statistics.LogPearsonIII _Function;
        #endregion

        #region Properties   
        public bool IsValid
        {
            get
            {
                return Validate();
            }
            private set
            {
                _IsValid = Validate();
            }
        }
        public Statistics.LogPearsonIII Function
        {
            get
            {
                return _Function;
            }
            private set
            {
                _Function = value;
                _IsValid = Validate();
            }
        }
        public FunctionType Type { get; }
        #endregion

        #region Constructors
        internal FrequencyFunction(Statistics.LogPearsonIII function)
        {
            Function = function;
            IsValid = Validate();
            ReportValidationErrors();
        }
        #endregion

        #region Method
        public bool Validate()
        {
            return true;
        }

        public IEnumerable<string> ReportValidationErrors()
        {
            return new List<string>();
        }

        public double GetXfromY(double y)
        {
            return Function.GetCDF(y);
        }
        #endregion
    }
}
