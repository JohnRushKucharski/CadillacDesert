using System;
using Model.Inputs.Functions;
using Model.Inputs.Functions.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests.InputsTests.FunctionsTests.ImplementationsTests
{
    [TestClass()]
    public class InflowFrequencyTests
    {
        #region Fields and Properties
        private BaseImplementation testInflowFrequency = FunctionFactory.CreateNew(new Statistics.LogPearsonIII(1, 0.1, 0.1, 100), FunctionTypeEnum.InflowFrequency);
        #endregion

        #region Constructor Tests
        [TestMethod()]
        public void InflowFrequency_FunctionFactoryCreatesInflowFrequencyFunction()
        {
            Assert.AreEqual(typeof(InflowFrequency), testInflowFrequency.GetType());
        }
        #endregion

        #region Compose() Tests
        [TestMethod()]
        public void Compose_InflowOutflowInputReturnsOutflowFrequency()
        {
            InflowFrequency testCompose = (InflowFrequency)testInflowFrequency;
            IFunctionTransform testInflowOutflow = (IFunctionTransform)FunctionFactory.CreateNew(new Statistics.CurveIncreasing(new double[] { 0, 1 }, new double[] { 2, 3 }, true, false), FunctionTypeEnum.InflowOutflow);
            IFunctionCompose returnedFunction = testCompose.Compose(testInflowOutflow);
            Assert.AreEqual(returnedFunction.GetType(), typeof(OutflowFrequency));
        }

        [TestMethod()]
        public void Compose_RatingInputDoesNotReturnNull()
        {
            InflowFrequency testCompose = (InflowFrequency)testInflowFrequency;
            IFunctionTransform testRating = (IFunctionTransform)FunctionFactory.CreateNew(new Statistics.CurveIncreasing(new double[] { 0, 1 }, new double[] { 2, 3 }, true, false), FunctionTypeEnum.Rating);
            IFunctionCompose returnedFunction = testCompose.Compose(testRating);
            Assert.IsFalse(returnedFunction == null);
        }

        [TestMethod()]
        public void Compose_RatingInputConvertsUseTypeToOutflowFrequency()
        {
            InflowFrequency testCompose = (InflowFrequency)testInflowFrequency;
            IFunctionTransform testRating = (IFunctionTransform)FunctionFactory.CreateNew(new Statistics.CurveIncreasing(new double[] { 0, 1 }, new double[] { 2, 3 }, true, false), FunctionTypeEnum.Rating);
            testCompose.Compose(testRating);
            Assert.AreEqual(FunctionTypeEnum.OutflowFrequency, testCompose.UseType);
        }
        
        [TestMethod()]
        public void Compose_RatingInputReturnsExteriorStageFrequency()
        {
            InflowFrequency testCompose = (InflowFrequency)testInflowFrequency;
            IFunctionTransform testRating = (IFunctionTransform)FunctionFactory.CreateNew(new Statistics.CurveIncreasing(new double[] { 0, 1 }, new double[] { 2, 3 }, true, false), FunctionTypeEnum.Rating);
            IFunctionCompose returnedFunction = testCompose.Compose(testRating);
            Assert.AreEqual(returnedFunction.GetType(), typeof(ExteriorStageFrequency));
        }

        [TestMethod()]
        public void Compose_InvalidTransformTypeInputReturnsNull()
        {

        }
        #endregion

        #region Validate() Tests
        [TestMethod()]
        public void Validate_GoodFrequencyFunctionReturnsTrue()
        {
            Assert.AreEqual(testInflowFrequency.IsValid, true);
        }

        [TestMethod()]
        public void Validate_NoFrequencyFunctionReturnsFalse()
        {
            OrdinatesFunction testOrdinatesFunction = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 0, 1 }, new double[] { 2, 3 }, true, false));
            InflowFrequency testInflowFrequency = new InflowFrequency(testOrdinatesFunction); //shouldn't construct this way.
            Assert.AreEqual(testInflowFrequency.IsValid, false);
        }
        #endregion
    }
}
