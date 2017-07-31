using System;
using System.Collections.Generic;
using Model.Inputs.Functions.Implementations;

namespace Model.Inputs.Functions
{
    public interface IFunctionTransform: IValidateData
    {
        FunctionTypeEnum Type { get; }
        IList<Tuple<double, double>> Ordinates { get; }
        BaseImplementation Sample(Random randomNumberGenerator);
    }
}
