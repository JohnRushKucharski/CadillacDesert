using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions.ComputationPoint
{
    public interface IStageDamageTransform: IFunctionTransform
    {
        IStageDamageTransform Aggregate(IStageDamageTransform toAdd);
    }
}
