using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Inventories.OccupancyTypes
{
    internal interface IOccupancyGroup
    {
        IList<IOccupancyType> OccupancyTypes { get; }
        IReadOnlyCollection<string> OccupancyTypesNames { get; }
        void AddToGroup(IOccupancyType occupancyType);
    }
}
