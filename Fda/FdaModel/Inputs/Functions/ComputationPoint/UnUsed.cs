namespace Model.Inputs.Functions.ComputationPoint
{
    public sealed class UnUsed : ComputationPointFunctionBase
    {
        #region Fields and Properties
        public override ComputationPointFunctionEnum Type { get; }
        public ComputationPointFunctionEnum TargetType { get; }
        #endregion

        #region Constructors
        internal UnUsed(IFunctionBase function): base(function)
        {
            Type = ComputationPointFunctionEnum.NotSet;
            TargetType = ComputationPointFunctionEnum.NotSet;
            IsValid = Function.IsValid;
        }
        internal UnUsed(IFunctionBase function, ComputationPointFunctionEnum type): base(function)
        {
            TargetType = type;
            Type = ComputationPointFunctionEnum.NotSet;
            IsValid = Function.IsValid;
        }
        #endregion

        #region BaseImplementation Methods
       
        #endregion
    }
}
