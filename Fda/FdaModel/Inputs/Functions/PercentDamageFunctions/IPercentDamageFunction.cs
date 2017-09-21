using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Functions.PercentDamageFunctions
{
    public interface IPercentDamageFunction: IValidateData
    {
        //IFunctionBase Function { get; }
        IPercentDamageFunction Sample(double probability);
        double ComputePercentDamage(double x);
    }
}
