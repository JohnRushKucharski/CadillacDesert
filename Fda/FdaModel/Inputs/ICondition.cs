using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions
{
    public interface ICondition: IValidateData
    { 
        IComputableFunction GetCurrentFrequencyFunction { get; }
        IFunction GetCurrentTransformFunction { get; }

    }
}
