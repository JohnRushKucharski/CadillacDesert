namespace Model.Inputs.Functions.Implementations
{
    internal sealed class UnUsed : BaseImplementation
    {
        #region Fields and Properties
        public override FunctionTypeEnum Type { get; }
        public FunctionTypeEnum TargetType { get; }
        #endregion

        #region Constructors
        internal UnUsed(IFunctionBase function): base(function)
        {
            Type = FunctionTypeEnum.NotSet;
            TargetType = FunctionTypeEnum.NotSet;
            IsValid = Function.IsValid;
        }
        internal UnUsed(IFunctionBase function, FunctionTypeEnum type): base(function)
        {
            TargetType = type;
            Type = FunctionTypeEnum.NotSet;
            IsValid = Function.IsValid;
        }
        #endregion

        #region BaseImplementation Methods
        public override double GetXfromY(double y)
        {
            return Function.GetXfromY(y);
        }
        #endregion
    }
}
