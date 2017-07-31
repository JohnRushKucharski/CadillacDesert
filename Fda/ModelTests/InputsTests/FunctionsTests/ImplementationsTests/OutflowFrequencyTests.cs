using System;
using Model.Inputs.Functions;
using Model.Inputs.Functions.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests.InputsTests.FunctionsTests.ImplementationsTests
{
    [TestClass()]
    public class OutflowFrequencyTests
    {
        #region Fields and Properties
        private Statistics.CurveIncreasing defaultCurveIncreasing = new Statistics.CurveIncreasing(new double[] { 0, 1 }, new double[] { 2, 3 }, true, false);
        private Statistics.CurveIncreasing defaultFrequencyOrdinates = new Statistics.CurveIncreasing(new double[] { 0.01, 0.99 }, new double[] { 2, 3 }, true, false);
        private Statistics.CurveIncreasing defaultTransformOrdinates = new Statistics.CurveIncreasing(new double[] { 2, 3 }, new double[] { 4, 5 }, true, false);
        #endregion

        #region Compose() Tests
        [TestMethod()]
        public void Compose_OutflowFrequencyPlusRatingReturnsExteriorStageFrequency()
        {
            IFunctionCompose testFrequencyFunction = (IFunctionCompose)FunctionFactory.CreateNew(defaultFrequencyOrdinates, FunctionTypeEnum.OutflowFrequency);
            IFunctionTransform testTransformFunction = (IFunctionTransform)FunctionFactory.CreateNew(defaultTransformOrdinates, FunctionTypeEnum.Rating);
            Assert.AreEqual(typeof(Model.Inputs.Functions.Implementations.ExteriorStageFrequency), testFrequencyFunction.Compose(testTransformFunction).GetType());
        }
        [TestMethod()]
        public void Compose_OutflowFrequencyPlusRatingReturnsIncrementedExteriorStageFrequencyFunctionTypeEnum()
        {
            IFunctionCompose testFrequencyFunction = (IFunctionCompose)FunctionFactory.CreateNew(defaultFrequencyOrdinates, FunctionTypeEnum.OutflowFrequency);
            IFunctionTransform testTransformFunction = (IFunctionTransform)FunctionFactory.CreateNew(defaultTransformOrdinates, FunctionTypeEnum.Rating);
            Model.Inputs.Functions.Implementations.BaseImplementation testComposedFunction = (Model.Inputs.Functions.Implementations.BaseImplementation)testFrequencyFunction.Compose(testTransformFunction);
            Assert.AreEqual(FunctionTypeEnum.ExteriorStageFrequency, testComposedFunction.Type);
        }

        #endregion

        #region Validate()
        [TestMethod()]
        public void Validate_GoodOrdinatesReturnTrue()
        {
            OutflowFrequency testOutflowFrequency = (OutflowFrequency)FunctionFactory.CreateNew(defaultFrequencyOrdinates, FunctionTypeEnum.OutflowFrequency);
            Assert.IsTrue(testOutflowFrequency.IsValid);
        }

        [TestMethod()]
        public void Validate_BadProbabilitiesReturnsFalse()
        {
            OutflowFrequency testOutflowFrequency = (OutflowFrequency)FunctionFactory.CreateNew(defaultCurveIncreasing, FunctionTypeEnum.OutflowFrequency);
            Assert.IsFalse(testOutflowFrequency.IsValid);
        }
        #endregion
    }
}
