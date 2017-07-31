using System;
using System.Collections.Generic;

namespace Model.Inputs.Functions
{
    public interface IFunctionRegistry
    {
        IEnumerable<string> NamedFunctions { get; }
        IEnumerable<Tuple<string, IFunctionTransform>> TransformFunctions { get; }

    }
}
