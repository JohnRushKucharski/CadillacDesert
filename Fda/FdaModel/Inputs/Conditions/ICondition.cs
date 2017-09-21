using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;

namespace Model.Inputs.Conditions
{
    public interface ICondition : IValidateData
    {
        string Id { get; }
        string Name { get; }
        int Year { get; }
        IFunctionCompose EntryPoint { get; }
        IList<IFunctionCompose> FrequencyFunctions { get; }
        IList<IFunctionTransform> TransformFunctions { get; }
        IList<ComputePoint> ComputePoints { get; }
        IList<Tuple<ComputePoint, double>> Metrics { get; }
        void Compute(int seed);

    }
}
