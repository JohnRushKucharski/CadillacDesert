using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs.Inventories.OccupancyTypes
{
    internal sealed class OccupancyGroup : IOccupancyGroup
    {
        #region Properties
        private int NameCounter { get; set; } 
        public IReadOnlyCollection<string> OccupancyTypesNames
        {
            get
            {
                return GetOccupancyTypeNames();
            }
        }
        public IList<IOccupancyType> OccupancyTypes { get; }
        #endregion

        #region Constructor
        internal OccupancyGroup(IList<IOccupancyType> occupancyTypes)
        {
            OccupancyTypes = occupancyTypes;
        }
        #endregion

        #region Methods  
        public void AddToGroup(IOccupancyType occupancyType)
        {
            foreach (var item in OccupancyTypes)
            {
                if (item.Name == occupancyType.Name)
                {
                    NameCounter++;
                    ReportOccupancyTypeNameConflict(occupancyType.Name);
                    occupancyType.Name = occupancyType.Name + NameCounter; break;
                }
            }
            OccupancyTypes.Add(occupancyType);
        }
        private string ReportOccupancyTypeNameConflict(string name)
        {
            return new StringBuilder("The occupancy type name was changed to ").Append(name).Append(NameCounter).Append(" because another occupancy type was already registered in the group with the same name.").ToString();
        }
        private IReadOnlyCollection<string> GetOccupancyTypeNames()
        {
            List<string> names = new List<string>();
            foreach (var item in OccupancyTypes)
            {
                names.Add(item.Name);
            }
            return names;
        }
        #endregion
    }
}
