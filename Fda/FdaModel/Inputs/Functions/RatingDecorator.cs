using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions
{
    internal sealed class RatingDecorator : Decorator
    {
        #region Properties
        public override FunctionType Type { get; }
        #endregion

        #region Constructor
        internal RatingDecorator(IFunction function) : base(function)
        {
            IsValid = function.IsValid;
            Type = FunctionType.Rating;
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
