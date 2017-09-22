using System;
using System.Collections.Generic;
using Model.Inputs.Functions.ComputationPoint;

namespace Model.Inputs.Functions
{
    public interface IFunctionTransform: IValidateData
    {
        ComputationPointFunctionEnum Type { get; }
        IList<Tuple<double, double>> Ordinates { get; }
        IFunctionTransform Sample(double probability);
        //double GetYFromX(double x);
    }
}
