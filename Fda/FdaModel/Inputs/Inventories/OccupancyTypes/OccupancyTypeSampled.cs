using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Inventories.Assets;

namespace Model.Inputs.Inventories.OccupancyTypes
{
    internal class OccupancyTypeSampled : IOccupancyType
    {
        #region Properties
        public bool IsValid { get; }
        public string Name { get; set; }
        public string DamageCategory { get; }
        public double ElevationError { get; }
        public IDictionary<AssetTypeEnum, IAsset> Assets { get; }
        #endregion

        #region Constructor
        internal OccupancyTypeSampled(string name, 
                                      string damageCategory, 
                                      double sampledElevationError, 
                                      IDictionary<AssetTypeEnum, IAsset> sampledAssets)
        {
            Name = name;
            DamageCategory = damageCategory;
            ElevationError = sampledElevationError;
            Assets = sampledAssets;
        }
        #endregion

        #region Methods
        public IOccupancyType Sample(Random numberGenerator)
        {
            return this;
        }
        #endregion

        #region IValidateData
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
