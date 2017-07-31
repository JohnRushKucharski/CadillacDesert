using System;

namespace Model.Inputs.Functions
{
    public sealed class Threshold : IThreshold
    {
        #region Properties
        public double Value { get; }
        public bool IsValid { get; private set; }
        public ThresholdTypes ThresholdType { get; }
        public FunctionTypeEnum ThresholdFunction
        {
            get
            {
                return AssignThresholdFunction();
            }
        }
        #endregion

        #region Constructors
        public Threshold(ThresholdTypes type, double value)
        {
            Value = value;
            ThresholdType = type;
        }
        #endregion

        #region Methods
        public FunctionTypeEnum AssignThresholdFunction()
        {
            switch (ThresholdType)
            {
                case ThresholdTypes.ExteriorStage:
                    return FunctionTypeEnum.ExteriorStageFrequency;
                case ThresholdTypes.InteriorStage:
                    return FunctionTypeEnum.InteriorStageFrequency;
                case ThresholdTypes.Damages:
                    return FunctionTypeEnum.DamageFrequency;
                default:
                    throw new NotImplementedException(); 
            }
        }
        #endregion

        public enum ThresholdTypes
        {
            ExteriorStage = 0,
            InteriorStage = 1,
            Damages = 2,
        }
    }
}
