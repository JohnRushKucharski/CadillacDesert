using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Inputs
{
    public sealed class ImpactArea: WaterSurfaceProfiles, IImpactArea
    {
        #region Properties
        public string Name { get; }
        public float ComputationPoint { get; }
        public bool IsValid { get; private set; }
        #endregion

        #region Constructors
        public ImpactArea(string name, KeyValuePair<float, Statistics.CurveIncreasing> wsp, IWaterSurfaceProfiles wsps): base()
        {
            Name = name;
            ComputationPoint = wsp.Value;
            IsValid = Validate();
        }
        #endregion

        #region Methods
        private void SelectIAProfiles(IWaterSurfaceProfiles wsps, float minXS, float maxXS)
        {
            foreach (var wsp in iAProfile)
            {
                if (wsp.Key < minXS || wsp.Key > maxXS) continue;
                else IAProfiles.Add(wsp.Key, wsp.Value);
            } 
        }
        private void SetComputePoint(float computePointXS)
        {
            foreach (var wsp in IAProfiles)
            {
                if (wsp.Key == computePointXS) { computePointXS = wsp; break; }
            }
        }
        #endregion

        #region IValidateData Methods
        public bool Validate()
        {
            return true;
        }
        public IEnumerable<string> ReportValidationErrors()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
