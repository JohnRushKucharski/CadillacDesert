using System;
using Model.Inputs.Functions.Implementations;

namespace Model.Inputs.Functions
{
    public interface IFunctionCompose: IValidateData
    {
        BaseImplementation Sample(Random randomNumberGenerator);
        IFunctionCompose Compose(IFunctionTransform transform);
    }
}
