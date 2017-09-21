using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions.PercentDamageFunctions;

namespace Model.Inputs.Inventories.Assets
{
    internal class AssetSampled: IAsset
    {
        #region Properties
        public bool IsValid { get; }
        public double ValueError { get; }
        public IPercentDamageFunction PercentDamageFunction { get; }
        #endregion

        public AssetSampled(double value, IPercentDamageFunction percentDamageFunction)
        {
            ValueError = value;
            PercentDamageFunction = percentDamageFunction;
        }

        #region IAsset Methods
        public IAsset Sample(double valueProbability, double percentDamageProbability)
        {
            return this;
        }
        #endregion

        #region IValidateData Methods
        public bool Validate()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<string> ReportValidationErrors()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
