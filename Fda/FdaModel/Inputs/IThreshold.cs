namespace Model.Inputs
{
    public interface IThreshold
    {
        Functions.FunctionTypeEnum ThresholdFunction { get; }
        double Value { get; }
    }
}
