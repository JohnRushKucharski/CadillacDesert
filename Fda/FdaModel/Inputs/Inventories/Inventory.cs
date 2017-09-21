using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Functions;
using Model.Inputs.Inventories.InventoryElements;

namespace Model.Inputs.Inventories
{
    public sealed class Inventory
    {
        #region Properties
        //private int TimeStampSeed { get; set; }
        public IDictionary<string, IInventoryElement> InventoryElements { get; }
        #endregion

        #region Methods
        public void AddEntry(string name, IInventoryElement structure)
        {
            if (InventoryElements.ContainsKey(name)) ReportValidationErrors();
            else InventoryElements.Add(name, structure);
        }
        
        public StageDamageInventory Compute(IWaterSurfaceProfiles wsps)
        {
            int timeStampSeed = (int)new DateTime().Ticks;
            Random numberGenerator = new Random(timeStampSeed);
            IDictionary<string, IDictionary<AssetTypeEnum, IFunctionTransform>> stageDamageInventory = new Dictionary<string, IDictionary<AssetTypeEnum, IFunctionTransform>>();

            foreach (var item in InventoryElements)
            {
                stageDamageInventory.Add(item.Key, item.Value.ComputeStageDamageFunctions(wsps, numberGenerator.Next()));
            }
            return new StageDamageInventory(stageDamageInventory, timeStampSeed);
        }
        #endregion

        public IEnumerable<string> ReportValidationErrors()
        {
            throw new NotImplementedException();
        }
    }
}
