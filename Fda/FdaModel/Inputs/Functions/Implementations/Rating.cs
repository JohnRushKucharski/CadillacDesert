using System;
using System.Collections.Generic;

namespace Model.Inputs.Functions.Implementations
{
    internal sealed class Rating : BaseImplementation, IFunctionTransform
    {
        #region Properties
        public override FunctionTypeEnum Type { get; }
        public IList<Tuple<double,double>> Ordinates { get; private set; }
        #endregion

        #region Constructor
        internal Rating(IFunctionBase function, IList<Tuple<double, double>> ordinates) : base(function)
        {
            Ordinates = ordinates;
            IsValid = Function.IsValid;
            Type = FunctionTypeEnum.Rating;
        }
        #endregion

        #region BaseImplementation Methods
        public override double GetXfromY(double y)
        {
            return Function.GetXfromY(y);
        } 
        #endregion
    }
}
