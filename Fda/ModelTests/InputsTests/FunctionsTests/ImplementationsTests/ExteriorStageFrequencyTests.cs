using System;
using Model.Inputs.Functions;
using Model.Inputs.Functions.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests.InputsTests.FunctionsTests.ImplementationsTests
{
    [TestClass()]
    public class ExteriorStageFrequencyTests
    {
        #region Fields and Properties
        private Statistics.CurveIncreasing defaultCurveIncreasing = new Statistics.CurveIncreasing(new double[] { 0, 1 }, new double[] { 2, 3 }, true, false);
        private Statistics.CurveIncreasing defaultFrequencyOrdinates = new Statistics.CurveIncreasing(new double[] { 0.01, 0.99 }, new double[] { 2, 3 }, true, false);
        private Statistics.CurveIncreasing defaultTransformOrdinates = new Statistics.CurveIncreasing(new double[] { 2, 3 }, new double[] { 4, 5 }, true, false);
        #endregion 

        #region Validate()
        [TestMethod()]
        public void Validate_GoodOrdinatesReturnTrue()
        {
            ExteriorStageFrequency testExteriorStageFrequency = (ExteriorStageFrequency)FunctionFactory.CreateNew(defaultFrequencyOrdinates, FunctionTypeEnum.ExteriorStageFrequency);
            Assert.IsTrue(testExteriorStageFrequency.IsValid);
        }

        [TestMethod()]
        public void Validate_BadProbabilitiesReturnsFalse()
        {
            ExteriorStageFrequency testExteriorStageFrequency = (ExteriorStageFrequency)FunctionFactory.CreateNew(defaultCurveIncreasing, FunctionTypeEnum.ExteriorStageFrequency);
            Assert.IsFalse(testExteriorStageFrequency.IsValid);
        }
        #endregion
    }
}
