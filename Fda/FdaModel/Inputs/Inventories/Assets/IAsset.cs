using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions.PercentDamageFunctions;

namespace Model.Inputs.Inventories.Assets
{
    public interface IAsset: IValidateData
    {
        double ValueError { get; }
        IPercentDamageFunction PercentDamageFunction { get; }
        IAsset Sample(double valueProbability, double percentDamageProbability);


    }
}
