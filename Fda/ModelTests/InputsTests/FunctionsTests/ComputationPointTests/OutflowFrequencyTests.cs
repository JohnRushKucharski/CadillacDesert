using System;
using Model.Inputs.Functions;
using Model.Inputs.Functions.ComputationPoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests.InputsTests.FunctionsTests.ImplementationsTests
{
    [TestClass()]
    public class OutflowFrequencyTests
    {
        #region Fields and Properties
        private OrdinatesFunction defaultTransformOrdinates = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 1, 2 }, new double[] { 2, 3 }, true, false));
        private IFunctionBase defaultFrequencyOrdinates = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 0.01, 0.99 }, new double[] { 2, 3 }, true, false));
        #endregion

        #region Compose() Tests
        [TestMethod()]
        public void Compose_OutflowFrequencyPlusRatingReturnsExteriorStageFrequency()
        {
            IFunctionCompose testFrequencyFunction = (IFunctionCompose)ComputationPointFunctionFactory.CreateNew(defaultFrequencyOrdinates, ComputationPointFunctionEnum.OutflowFrequency);
            IFunctionTransform testTransformFunction = ComputationPointFunctionFactory.CreateNew(defaultTransformOrdinates, defaultTransformOrdinates.Ordinates, ComputationPointFunctionEnum.Rating);
            Assert.AreEqual(typeof(ExteriorStageFrequency), testFrequencyFunction.Compose(testTransformFunction).GetType());
        }
        [TestMethod()]
        public void Compose_OutflowFrequencyPlusRatingReturnsIncrementedExteriorStageFrequencyFunctionTypeEnum()
        {
            IFunctionCompose testFrequencyFunction = (IFunctionCompose)ComputationPointFunctionFactory.CreateNew(defaultFrequencyOrdinates, ComputationPointFunctionEnum.OutflowFrequency);
            IFunctionTransform testTransformFunction = ComputationPointFunctionFactory.CreateNew(defaultTransformOrdinates, defaultTransformOrdinates.Ordinates, ComputationPointFunctionEnum.Rating);
            ComputationPointFunctionBase testComposedFunction = (ComputationPointFunctionBase)testFrequencyFunction.Compose(testTransformFunction);
            Assert.AreEqual(ComputationPointFunctionEnum.ExteriorStageFrequency, testComposedFunction.Type);
        }
        #endregion

        #region Validate()
        [TestMethod()]
        public void Validate_GoodOrdinatesReturnTrue()
        {
            OutflowFrequency testOutflowFrequency = new OutflowFrequency(defaultFrequencyOrdinates);
            Assert.IsTrue(testOutflowFrequency.IsValid);
        }

        [TestMethod()]
        public void Validate_BadProbabilitiesReturnsFalse()
        {
            OutflowFrequency testOutflowFrequency = new OutflowFrequency(defaultTransformOrdinates);
            Assert.IsFalse(testOutflowFrequency.IsValid);
        }
        #endregion
    }
}
