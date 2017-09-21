using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Inventories.Assets;

namespace Model.Inputs.Inventories.OccupancyTypes
{
    internal sealed class OccupancyType : IOccupancyType
    {
        #region Fields
        private bool _IsValid;
        #endregion

        #region Properties
        public bool IsValid
        {
            get
            {
                return _IsValid;
            }
            private set
            {
                _IsValid = Validate();
            }
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DamageCategory { get; set; }
        public double ElevationError
        {
            get
            {
                return ElevationUncertainty.GetCentralTendency;
            }
        }

        private Statistics.ContinuousDistribution ElevationUncertainty { get; }
        public IDictionary<AssetTypeEnum, IAsset> Assets { get; }
        #endregion

        #region Constructor
        internal OccupancyType(string name, 
                               IDictionary<AssetTypeEnum, IAsset> damagableAssets, 
                               Statistics.ContinuousDistribution elevationUncertainty, 
                               string damageCategory, 
                               string description = "")
        {
            Name = name;
            Description = description;
            DamageCategory = damageCategory;
            ElevationUncertainty = elevationUncertainty;
            Assets = damagableAssets;
            IsValid = Validate();
        }
        internal OccupancyType(OccupancyGroup registryGroup,
                               string name,
                               IDictionary<AssetTypeEnum, IAsset> damagableAssets,
                               Statistics.ContinuousDistribution elevationUncertainty,
                               string damageCategory,
                               string description = "")
        {
            Name = name;
            Description = description;
            DamageCategory = damageCategory;
            ElevationUncertainty = elevationUncertainty;
            Assets = damagableAssets;
            IsValid = Validate();
            registryGroup.AddToGroup(this);   
        }
        #endregion

        #region IOccupancyType Methods
        public void AddAsset(IAsset damageableAsset, AssetTypeEnum assetType)
        {
            Assets.Add(assetType, damageableAsset);
            IsValid = Validate();
        }
        public IOccupancyType Sample(Random numberGenerator)
        {
            int i = 0;
            IDictionary<AssetTypeEnum, IAsset> sampledAssets = new Dictionary<AssetTypeEnum, IAsset>();
            foreach (var item in Assets)
            {
                sampledAssets.Add(item.Key, item.Value.Sample(numberGenerator.NextDouble(), numberGenerator.NextDouble()));
                i = i + 2;
            }
            return new OccupancyTypeSampled(Name, DamageCategory, ElevationUncertainty.getDistributedVariable(numberGenerator.NextDouble()), sampledAssets);
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
