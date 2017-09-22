using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Model.Inputs
{
    internal abstract sealed class WaterSurfaceProfilesBase : IWaterSurfaceProfiles
    {
        #region Properties
        public bool IsValid { get; private set; }
        public double[] Probabilities { get; }
        public abstract IDictionary<float, Statistics.CurveIncreasing> Profiles { get; }
        #endregion

        #region Constructor
        public WaterSurfaceProfilesBase(Dictionary<float, Statistics.CurveIncreasing> profiles, double[] probabilities)
        {
            Profiles = profiles;
            Probabilities = probabilities;
            ReportValidationErrors();
        }
        #endregion
        
        #region IValidate Members
        public bool Validate()
        {
            Profiles.OrderBy(k => k.Key);
            if (Probabilities.Length < 8) return false;
            for (int i = 0; i < Profiles.Count - 1; i++)
            {
                if (Profiles[i].Count != Probabilities.Length) return false;
                if (Profiles[i].Count == Profiles[i + 1].Count) 
                {
                    for (int j = 0; j < Profiles[i].Count; j++)
                    {
                        if (Profiles[i + 1].get_X(j) < Profiles[i].get_X(j) ||
                            Profiles[i + 1].get_Y(j) < Profiles[i].get_Y(j)) return false;
                    }
                }
            }
            return true;
        }
        public IEnumerable<string> ReportValidationErrors()
        {
            IsValid = true;
            bool firstLoop = true; 
            StringBuilder message = new StringBuilder();
            List<string> messages = new List<string>();
            Statistics.CurveIncreasing lastOrdinates = new Statistics.CurveIncreasing(new double[] { -1, -2 }, new double[] { -3, -4 }, true, false);
            if (Probabilities.Length < 8) { IsValid = false; messages.Add("The water surface profile does not have the required minimum number of event probabilities. Each water surface profile must contain at least 8 peak flow-peak stage ordinates, assigned to 8 event probabilities."); }

            foreach (var item in Profiles.Reverse())
            {
                if (item.Value.Count != Probabilities.Length) { IsValid = false; messages.Add(message.Append("Cross Section: ").Append(item.Key).Append(" contains ").Append(item.Value.Count).Append(" ordinates. However, the profile contains ").Append(Probabilities.Length).Append(" event probabilities. An equal number of event probabilities and cross section ordinates are required of each cross section.").ToString()); }
                if (firstLoop == true) { lastOrdinates = item.Value; firstLoop = false; }
                if (lastOrdinates.Count == item.Value.Count)
                {
                    for (int j = 0; j < item.Value.Count; j++)
                    {
                        if (lastOrdinates.get_X(j) < item.Value.get_X(j)) { IsValid = false; messages.Add(message.Append("Peak flow values are not monotonically increasing at cross section: ").Append(item.Key).Append(" all water surface profile ordinates must be montonically increasing.").ToString()); }
                        if (lastOrdinates.get_Y(j) < item.Value.get_Y(j)) { IsValid = false; messages.Add(message.Append("Peak stage values are not monotonically increasing at cross section: ").Append(item.Key).Append(" all water surface profile ordinates must be montonically increasing.").ToString()); }
                    }
                }
                lastOrdinates = item.Value;
            }
            return messages;
        }
        #endregion
    }
}
