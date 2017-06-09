using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions
{
    internal sealed class InflowOutflowDecorator: Decorator
    {
        #region Properties
        public override FunctionType Type { get; }
        #endregion

        #region Constructor
        internal InflowOutflowDecorator(IFunction function): base(function)
        {
            IsValid = function.IsValid;
            Type = FunctionType.InflowOutflow;
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
