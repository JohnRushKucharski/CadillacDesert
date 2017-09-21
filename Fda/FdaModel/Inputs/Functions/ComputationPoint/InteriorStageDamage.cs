using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions.ComputationPoint
{
    public sealed class InteriorStageDamage : ComputationPointFunctionBase, IStageDamageTransform
    {
        #region Properties
        public override ComputationPointFunctionEnum Type { get; }
        public IList<Tuple<double, double>> Ordinates { get; private set; }
        #endregion

        #region Constructor
        internal InteriorStageDamage(IFunctionBase function, IList<Tuple<double, double>> ordinates) : base(function)
        {
            Ordinates = ordinates;
            Type = ComputationPointFunctionEnum.InteriorStageDamage;
        }
        #endregion

        #region IFunctionTransform Methods
        public IFunctionTransform Sample(double probability)
        {
            return ComputationPointFunctionFactory.CreateNew(Function.Sample(probability), Ordinates, Type);
        }
        #endregion

        #region IStageDamageTransform
        public IStageDamageTransform Aggregate (IStageDamageTransform toAdd)
        {
            IFunctionTransform lowerFunction, higherFunction;
            if (Ordinates[0].Item1 <= toAdd.Ordinates[0].Item1) { lowerFunction = this; higherFunction = toAdd; }
            else { lowerFunction = toAdd; higherFunction = this; }

            if (lowerFunction.Ordinates[0].Item1 < higherFunction.Ordinates[0].Item1) 
        }

        #endregion
    }
}
