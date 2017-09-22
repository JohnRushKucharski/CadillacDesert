using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;
using Model.Inputs.Functions.ComputationPoint;

namespace Model.Inputs.Inventories
{
    public class StageDamageInventory
    {
        #region Properties
        private int TimeStampSeed { get; } 
        public IDictionary<string, IDictionary<AssetTypeEnum, IFunctionTransform>> StageDamageFunctions { get; }
        public IFunctionTransform InventoryStageDamge { get; private set; }
        #endregion

        #region Constructor
        public StageDamageInventory(IDictionary<string, IDictionary<AssetTypeEnum, IFunctionTransform>> stageDamageFunctions, int seed)
        {
            TimeStampSeed = seed;
            StageDamageFunctions = stageDamageFunctions;
        }
        public StageDamageInventory(IWaterSurfaceProfiles wsp, )
        #endregion

        #region Methods
        public void AggregateInventoryStageDamage()
        {
            List<double> stages = new List<double>(), damages = new List<double>();
            foreach (var item in StageDamageFunctions)
            {
                foreach (var asset in item.Value)
                {
                    
                }
            }
        }
        #endregion
    }
}
