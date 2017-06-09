namespace Model.Inputs
{
    public sealed class Threshold : IThreshold
    {
        #region Fields
        private ThresholdTypes _Type;
        #endregion

        #region Properties
        public Functions.FunctionType ThresholdFunction { get; private set; }
        public double Value { get; }
        public bool IsValid { get; private set; }
        #endregion

        #region Constructors
        public Threshold(ThresholdTypes type, double value)
        {
            _Type = type;
            Value = value;
            AssignThresholdFunction();
        }
        #endregion

        #region Methods
        public void AssignThresholdFunction()
        {
            switch (_Type)
            {
                case ThresholdTypes.ExteriorStage:
                    ThresholdFunction = Functions.FunctionType.ExteriorStageFrequency;
                    break;
                case ThresholdTypes.InteriorStage:
                    ThresholdFunction = Functions.FunctionType.InteriorStageFrequency;
                    break;
                case ThresholdTypes.Damages:
                    ThresholdFunction = Functions.FunctionType.DamageFrequency;
                    break;
            }
        }
        #endregion
    }

    public enum ThresholdTypes
    {
        ExteriorStage = 0,
        InteriorStage = 1,
        Damages = 2,
    }
}
