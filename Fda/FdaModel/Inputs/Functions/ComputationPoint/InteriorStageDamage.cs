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
        public IStageDamageTransform Aggregate(IStageDamageTransform toAdd)
        {
            
            IStageDamageTransform lowerFunction, higherFunction;
            if (Ordinates[0].Item1 <= toAdd.Ordinates[0].Item1) { lowerFunction = this; higherFunction = toAdd; }
            else { lowerFunction = toAdd; higherFunction = this; }

            List<double> stages = new List<double>(), damages = new List<double>();
            List<Tuple<double, double>> newOrdinates = new List<Tuple<double, double>>();
            int i = 0, j = 0, I = lowerFunction.Ordinates.Count, J = higherFunction.Ordinates.Count;
            while (lowerFunction.Ordinates[i].Item1 < higherFunction.Ordinates[0].Item1)
            {
                newOrdinates.Add(lowerFunction.Ordinates[i]);
                i++;
            } 

            while (i < I)
            {
                if (lowerFunction.Ordinates[i].Item1 <= higherFunction.Ordinates[i].Item1)
                {
                    newOrdinates.Add(new Tuple<double, double>(lowerFunction.Ordinates[i].Item1, lowerFunction.Ordinates[i].Item2 + higherFunction.GetYFromX(lowerFunction.Ordinates[i].Item1)));
                    if (lowerFunction.Ordinates[i].Item1 == higherFunction.Ordinates[j].Item1)
                    {
                        if (j + 1 < J) j++;
                        else
                        {
                            while (i < I)
                            {
                                newOrdinates.Add(new Tuple<double, double>(lowerFunction.Ordinates[i].Item1, lowerFunction.Ordinates[i].Item2 + higherFunction.Ordinates[j].Item2));
                                i++;
                            }
                            break;
                        }
                            
                    }
                    i++;
                }
                else
                {

                }
            } 
        }
        private List<Tuple<double, double>> AddPointsAbove(List<Tuple<double, double>> ordinatesToAdd, double constantStage)
        public double GetYFromX(double x)
        {
            return Function.GetYfromX(x);
        }
        #endregion
    }
}
