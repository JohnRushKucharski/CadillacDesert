using System;
using System.Collections.Generic;
using Model.Inputs.Functions;
using Model.Inputs.Functions.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests.InputsTests.FunctionsTests
{
    [TestClass]
    public class FunctionFactoryTests
    {
        #region Fields and Properties
        private static double[] testXs = new double[2] { 0, 1 }, testYs = new double[2] { 2, 3 };
        private Statistics.CurveIncreasing testCurveIncreasing = new Statistics.CurveIncreasing(testXs, testYs, true, false);
        private Statistics.LogPearsonIII testLP3 = new Statistics.LogPearsonIII(2, .1, .1, 100);
        #endregion

        #region CreateNew() "Constructor" Tests
        [TestMethod()]
        public void CreateNew_InflowFrequencyLP3ReturnsInflowFrequencyImplementation()
        {
            BaseImplementation testInflowFrequency = FunctionFactory.CreateNew(testLP3, FunctionTypeEnum.InflowFrequency);
            Assert.IsTrue(testInflowFrequency.GetType() == typeof(InflowFrequency));
        }

        [TestMethod()]
        public void CreateNew_InvalidTypeLP3ReurnsUnUsedImplementation()
        {
            BaseImplementation testInflowOutflow = FunctionFactory.CreateNew(testLP3, FunctionTypeEnum.InflowOutflow);
            Assert.IsTrue(testInflowOutflow.GetType() == typeof(UnUsed));
        }

        [TestMethod()]
        public void CreateNew_InflowOutflowCurveIncreasingReturnsInflowOutflowImplementation()
        {
            BaseImplementation testInflowOutflow = FunctionFactory.CreateNew(testCurveIncreasing, FunctionTypeEnum.InflowOutflow);
            Assert.IsTrue(testInflowOutflow.GetType() == typeof(InflowOutflow));
        }

        [TestMethod()]
        public void CreateNew_OutflowFrequencyLP3ReturnsUnUsedImplementation()
        {
            BaseImplementation testOutflowFrequency = FunctionFactory.CreateNew(testLP3, FunctionTypeEnum.OutflowFrequency);
            Assert.IsTrue(testOutflowFrequency.GetType() == typeof(UnUsed));
        }

        [TestMethod()]
        public void CreateNew_OutflowFrequencyCurveIncreasingReturnsOutflowFrequencyImplementation()
        {
            BaseImplementation testOutflowFrequency = FunctionFactory.CreateNew(testCurveIncreasing, FunctionTypeEnum.OutflowFrequency);
            Assert.IsTrue(testOutflowFrequency.GetType() == typeof(OutflowFrequency));
        }

        [TestMethod()]
        public void CreateNew_RatingCurveIncreasingReturnsRatingImplementation()
        {
            BaseImplementation testRating = FunctionFactory.CreateNew(testCurveIncreasing, FunctionTypeEnum.Rating);
            Assert.IsTrue(testRating.GetType() == typeof(Rating));
        }

        [TestMethod()]
        public void CreateNew_InvalidTypeCurveIncreasingReturnsUnUsedImplementation()
        {
            BaseImplementation testInflowFrequency = FunctionFactory.CreateNew(testCurveIncreasing, FunctionTypeEnum.InflowFrequency);
            Assert.IsTrue(testInflowFrequency.GetType() == typeof(UnUsed));
        }

        [TestMethod()]
        public void CreateNew_GoodFunctionReturnsNewFunctionListInstanceWithOneFunction()
        {
            //BaseImplementation testFunction = FunctionFactory.CreateNew(new Statistics.CurveIncreasing(new double[] { 0, 1 }, new double[] { 2, 3 }, true, false), FunctionTypeEnum.InflowOutflow);
            //List<Tuple<string, BaseImplementation, Type>> testList = FunctionList.Instance.Functions;
            //Assert.AreEqual(testList.Count, FunctionList.Instance.Functions.Count);
            //Assert.AreEqual(1, testList.Count);
        }
        #endregion
    }
}
