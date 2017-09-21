using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;
using Model.Inputs.Inventories.OccupancyTypes;
using Model.Inputs.Functions.ComputationPoint;

namespace Model.Inputs.Inventories.InventoryElements
{
    public class ElementBase : IInventoryElement
    {
        #region Properties
        public bool IsValid { get; private set; }
        public float Location { get; }
        public double FoundationHeight { get; }
        public IOccupancyType OccupancyType { get; }        
        public IDictionary<AssetTypeEnum, double> AssetValues { get; }
        #endregion

        #region Constructors
        public ElementBase(float location, 
                         double estimatedFoundationHeight, 
                         IOccupancyType occupancyType, 
                         IDictionary<AssetTypeEnum, double> assetValues)
        {
            Location = location;
            FoundationHeight = estimatedFoundationHeight;
            OccupancyType = occupancyType;
            AssetValues = assetValues;
            IsValid = Validate();
        }
        #endregion

        #region Methods
        public IDictionary<AssetTypeEnum, IFunctionTransform> ComputeStageDamageFunctions(IWaterSurfaceProfiles wsps, int seed)
        {
            Random numberGenerator = new Random(seed);
            IOccupancyType sampledOccupancyType = OccupancyType.Sample(numberGenerator);
            double sampledFoundationHeight = ComputeFoundationHeight(sampledOccupancyType);
            IDictionary<AssetTypeEnum, IFunctionTransform> stageDamageFunctions = new Dictionary<AssetTypeEnum, IFunctionTransform>();

            Statistics.CurveIncreasing wsp; double value;
            if (wsps.Profiles.TryGetValue(Location, out wsp))
            {
                foreach (var item in sampledOccupancyType.Assets)
                {
                    if (AssetValues.TryGetValue(item.Key, out value)) value += value * item.Value.ValueError; else value = 0;
                    List<double> stages = new List<double>(), damages = new List<double>();
                    List<Tuple<double, double>> ordinates = new List<Tuple<double, double>>();
                    for (int i = 0; i < wsp.Count; i++)
                    {
                        stages.Add(wsp.get_Y(i));
                        damages.Add(value * (1 - item.Value.PercentDamageFunction.ComputePercentDamage(wsp.get_Y(i) - sampledFoundationHeight)));
                    }
                    stageDamageFunctions.Add(item.Key, ComputationPointFunctionFactory.CreateNew(new Statistics.CurveIncreasing(stages, damages, true, false), ComputationPointFunctionEnum.InteriorStageDamage));
                }
            }
            return stageDamageFunctions;
        }                     
        private double ComputeFoundationHeight(IOccupancyType sampledOccupancyType)
        {
            return FoundationHeight + sampledOccupancyType.ElevationError;
        }
        #endregion

        #region IValidateData Methods
        private bool Validate()
        {
            foreach (var item in AssetValues) if (!OccupancyType.Assets.ContainsKey(item.Key)) ReportValidationErrors();
            foreach (var item in OccupancyType.Assets) if (!AssetValues.ContainsKey(item.Key)) ReportValidationErrors();
            throw new NotImplementedException();
        }
        public IEnumerable<string> ReportValidationErrors()
        {
            List<string> messages = new List<string>();
            foreach (var item in AssetValues)
                if (!OccupancyType.Assets.ContainsKey(item.Key))
                    messages.Add(new StringBuilder("The structure contains an estimated value for ").Append(item.Key).Append(" assets. This asset type is missing in the structure occupancy type. No damages to this asset will be computed, since no depth-percent damage function for this asset exists.").ToString());
            foreach (var item in OccupancyType.Assets)
                if (!OccupancyType.Assets.ContainsKey(item.Key))
                    messages.Add(new StringBuilder("The structure occupancy type contains a depth-percent damage relationship for ").Append(item.Key).Append(" assets. No damages for this asset type will be computed, since no estimate value for this asset is found at the structure.").ToString());
            throw new NotImplementedException();
        }
        #endregion
    }
}
