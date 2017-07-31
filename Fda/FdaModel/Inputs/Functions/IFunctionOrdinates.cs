using System;
using System.Collections.Generic;

namespace Model.Inputs.Functions
{
    public interface IFunctionOrdinates: IFunctionBase, IValidateData
    {
        IList<Tuple<double, double>> Ordinates { get; }
    }
}
