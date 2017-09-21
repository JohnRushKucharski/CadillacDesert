using System;
using System.Collections.Generic;

namespace Model.Inputs.Functions.ComputationPoint
{
    public sealed class Rating : ComputationPointFunctionBase, IFunctionTransform
    {
        #region Properties
        public override ComputationPointFunctionEnum Type { get; }
        public IList<Tuple<double,double>> Ordinates { get; private set; }
        #endregion

        #region Constructor
        internal Rating(IFunctionBase function, IList<Tuple<double, double>> ordinates) : base(function)
        {
            Ordinates = ordinates;
            IsValid = Function.IsValid;
            Type = ComputationPointFunctionEnum.Rating;
        }
        #endregion

        #region IFunctionTransform Methods
        public IFunctionTransform Sample(double probability)
        {
            return ComputationPointFunctionFactory.CreateNew(Function.Sample(probability), Ordinates, Type);
        }
        #endregion
    }
}
