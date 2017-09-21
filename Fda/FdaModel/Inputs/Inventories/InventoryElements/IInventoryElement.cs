using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;
using Model.Inputs.Inventories.OccupancyTypes;

namespace Model.Inputs.Inventories.InventoryElements
{
    public interface IInventoryElement: IValidateData
    {
        float Location { get; }
        double FoundationHeight { get; }
        IOccupancyType OccupancyType { get; }
        IDictionary<AssetTypeEnum, double> AssetValues { get; }
        IDictionary<AssetTypeEnum, IFunctionTransform> ComputeStageDamageFunctions(IWaterSurfaceProfiles wsps, int seed);
    }
}
