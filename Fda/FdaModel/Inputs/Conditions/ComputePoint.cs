using System;
using Model.Inputs.Functions.ComputationPoint;

namespace Model.Inputs.Conditions
{
    public class ComputePoint
    {
        #region Fields
        private ComputationPointFunctionEnum _ThresholdFuncton;
        #endregion

        #region Properties
        public double Value { get; }
        public ComputePointUnitTypeEnum Unit { get; }
        public ComputationPointFunctionEnum ComputePointFunction { get; }
        #endregion

        #region Constructors
        public ComputePoint(ComputePointUnitTypeEnum type, double value)
        {
            Value = value;
            Unit = type;
            ComputePointFunction = AssignThresholdFunction();
        }
        #endregion

        #region Methods
        private ComputationPointFunctionEnum AssignThresholdFunction()
        {
            switch (Unit)
            {
                case ComputePointUnitTypeEnum.ExteriorStage:
                    return ComputationPointFunctionEnum.ExteriorStageFrequency;
                case ComputePointUnitTypeEnum.InteriorStage:
                    return ComputationPointFunctionEnum.InteriorStageFrequency;
                case ComputePointUnitTypeEnum.Damages:
                    return ComputationPointFunctionEnum.DamageFrequency;
                case ComputePointUnitTypeEnum.ExpectedAnnualDamage:
                    return ComputationPointFunctionEnum.DamageFrequency;
                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        
    }
}
