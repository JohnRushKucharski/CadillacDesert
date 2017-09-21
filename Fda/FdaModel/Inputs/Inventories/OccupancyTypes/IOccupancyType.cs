using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Inputs.Inventories.Assets;


namespace Model.Inputs.Inventories.OccupancyTypes
{
    public interface IOccupancyType: IValidateData
    {
        string Name { get; set; }
        string DamageCategory { get; }
        double ElevationError { get; }
        IDictionary<AssetTypeEnum, IAsset> Assets { get; }
        IOccupancyType Sample(Random numberGenerator);


        //string Name { get; set; }
        //string Description { get; set; }
        //string DamageCategory { get; set; }
        //Statistics.ContinuousDistribution ElevationUncertainty { get; }
        //IDictionary<AssetTypeEnum, IAsset> Assets { get; }
        //void AddAsset(IAsset damageableAsset, AssetTypeEnum assetType);
        //IOccupancyType Sample(Random numberGenerator);
    }
}
