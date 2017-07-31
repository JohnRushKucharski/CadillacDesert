using System;
using System.Collections.Generic;

namespace Model.Inputs.Functions.Implementations
{
    internal sealed class InflowOutflow: BaseImplementation, IFunctionTransform
    {
        #region Properties
        //private new IFunctionOrdinates Function;
        public override FunctionTypeEnum Type { get; }
        public IList<Tuple<double,double>> Ordinates { get; private set; }
        #endregion

        #region Constructor
        internal InflowOutflow(IFunctionBase function, IList<Tuple<double, double>> ordinates): base(function)
        {
            Ordinates = ordinates;
            IsValid = Function.IsValid;
            Type = FunctionTypeEnum.InflowOutflow;
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
