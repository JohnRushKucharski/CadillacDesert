using System;
using System.Collections.Generic;

namespace Model.Inputs.Functions
{
    public interface IFunctionBase: IValidateData
    {
        bool ValidateFrequencyValues(FunctionTypeEnum type);
        IFunctionBase Sample(Random randomNumberGenerator);
        IList<Tuple<double, double>> Compose(IList<Tuple<double, double>> transformOrdinates);
        double GetXfromY(double y);
    }
}
