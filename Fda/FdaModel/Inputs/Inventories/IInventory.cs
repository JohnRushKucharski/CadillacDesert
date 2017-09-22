using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Inputs.Inventories.InventoryElements;

namespace Model.Inputs.Inventories
{
    public interface IInventory
    {
        public IDictionary<string, IInventoryElement> Elements { get; }
    }
}
