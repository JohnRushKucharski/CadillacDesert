using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions
{
    public sealed class OutflowFrequencyDecorator : Decorator
    {
        #region Fields (without Auto Implemented Properties)
        #endregion

        #region Properties
        public override FunctionType Type { get; }
        public FunctionType OriginType { get; } = FunctionType.OutflowFrequency;
        #endregion

        #region Constructor
        internal OutflowFrequencyDecorator(IFunction function) : base(function)
        {
            IsValid = function.IsValid;
            if (function.Type == FunctionType.NotSet) OriginType = FunctionType.OutflowFrequency;
            else OriginType = function.Type;
            DecoratorList.Add(this);
        }
        #endregion

        #region Methods
        public IComputableFunction Compute(IFunction transformFunction)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
