using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions.PercentDamageFunctions;
using Model.Inputs.Functions;


namespace Model.Inputs.Inventories.Assets
{
    internal sealed class Asset: IAsset
    {
        #region Properties
        public bool IsValid { get; }
        public double ValueError
        {
            get
            {
                return ValueDistribution.GetCentralTendency;
            }
        }
        private Statistics.ContinuousDistribution ValueDistribution { get; }
        public IPercentDamageFunction PercentDamageFunction { get; }
        #endregion

        #region Constructor
        public Asset(Statistics.ContinuousDistribution valueDistribution, IPercentDamageFunction percentDamageFunction)
        {
            ValueDistribution = valueDistribution;
            PercentDamageFunction = percentDamageFunction;
        }
        #endregion

        #region Methods
        public IAsset Sample(double valueProbability, double percentDamageProbability)
        {
          return new AssetSampled(ValueDistribution.getDistributedVariable(valueProbability), PercentDamageFunction.Sample(percentDamageProbability));
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
