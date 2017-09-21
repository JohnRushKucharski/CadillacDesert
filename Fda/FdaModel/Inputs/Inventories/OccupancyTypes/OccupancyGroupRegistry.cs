using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Model.Inputs.InventoryElements
{
    internal sealed class OccupancyGroupRegistry
    {
        #region Properties
        private int NameCounter { get; set; }
        public static OccupancyGroupRegistry Instance { get; private set; }
        public IList<IOccupancyGroup> OccupancyGroups { get; }
        #endregion

        #region Constructor
        private OccupancyGroupRegistry()
        {
            NameCounter = 0;
            OccupancyGroups = new List<IOccupancyGroup>();
        }
        #endregion

        #region Methods
        private static OccupancyGroupRegistry CreateNew()
        {
            if (Instance == null) Instance = new OccupancyGroupRegistry();
            return Instance;
        }
        public static bool IsRegisteredGroup(string groupName)
        {
            for (int i = 0; i < Instance.OccupancyGroups.Count; i++)
            {
                if (Instance.OccupancyGroups[i].Name == groupName)
                {
                    return true;
                }
            }
            return false;
        }
        public static void AddToRegistry(IOccupancyGroup group)
        {
            if (Instance == null) CreateNew();
            Instance.ValidateEntry(group);
            Instance.OccupancyGroups.Add(group);
        }
        public void ValidateEntry(IOccupancyGroup group)
        {
            for (int i = 0; i < OccupancyGroups.Count; i++)
            {
                if (OccupancyGroups[i].Name == group.Name)
                {
                    NameCounter++;
                    ReportGroupNameConflict(group.Name);
                    group.ChangeGroupName(new StringBuilder(group.Name).Append(NameCounter).ToString());
                }
            }
        }
        private string ReportGroupNameConflict(string name)
        {
            return new StringBuilder("The occupancy type group name was changed to ").Append(name).Append(Instance.NameCounter).Append(" because another occupancy type group was already registered with the same name.").ToString();
        }
        #endregion
    }
}
