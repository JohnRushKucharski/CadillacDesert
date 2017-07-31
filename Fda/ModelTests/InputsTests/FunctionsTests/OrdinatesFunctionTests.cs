using System;
using Model.Inputs.Functions;
using Model.Inputs.Functions.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ModelTests.InputsTests.FunctionsTests
{
    [TestClass()]
    public class OrdinatesFunctionTests
    {
        #region Fields and Properties
        private Statistics.CurveIncreasing defaultCurveIncreasing = new Statistics.CurveIncreasing(new double[] { 0, 1 }, new double[] { 2, 3 }, true, false);
        private Statistics.CurveIncreasing defaultFrequencyOrdinates = new Statistics.CurveIncreasing(new double[] { 0.01, 0.99 }, new double[] { 2, 3 }, true, false);
        private Statistics.CurveIncreasing defaultTransformOrdinates = new Statistics.CurveIncreasing(new double[] { 2, 3 }, new double[] { 4, 5 }, true, false);
        #endregion

        #region Validate() Tests
        [TestMethod()]
        public void Validate_GoodDataReturnsTrue()
        {

            BaseImplementation testOrdinatesFunction = FunctionFactory.CreateNew(defaultCurveIncreasing, FunctionTypeEnum.NotSet);
            Assert.IsTrue(testOrdinatesFunction.IsValid);
        }
        [TestMethod()]
        public void Validate_SingleOrdinateReturnsFalse()
        {
            //Arrange
            double[] testXs = new double[1] { 1 };
            double[] testYs = new double[1] { 2 };
            Statistics.CurveIncreasing testCurveIncreasing = new Statistics.CurveIncreasing(testXs, testYs, true, false);
            BaseImplementation testOrdinatesFunction = FunctionFactory.CreateNew(testCurveIncreasing, FunctionTypeEnum.NotSet);
            //Assert
            Assert.IsFalse(testOrdinatesFunction.IsValid);
        }
        [TestMethod()]
        public void Validate_RepeatingOrdinatesRemoved()
        {
            //Arrange
            double[] testXs = new double[3] { 0, 1, 1 };
            double[] testYs = new double[3] { 2, 3, 3 };
            Statistics.CurveIncreasing testCurveIncreasing = new Statistics.CurveIncreasing(testXs, testYs, true, false);
            BaseImplementation testOrdinatesFunction = FunctionFactory.CreateNew(testCurveIncreasing, FunctionTypeEnum.NotSet);
            //Act
            List<double> expectedXs = new List<double>() { 0, 1 }, expectedYs = new List<double>() { 2, 3 };
            List<double> actualXs = new List<double>(), actualYs = new List<double>();
            for (int i = 0; i < testCurveIncreasing.Count; i++)
            {
                actualXs.Add(testCurveIncreasing.get_X(i));
                actualYs.Add(testCurveIncreasing.get_Y(i));
            }
            //Assert
            Assert.AreEqual(expectedXs[0], actualXs[0]);
            Assert.AreEqual(expectedXs[1], actualXs[1]);
            Assert.AreEqual(expectedYs[0], actualYs[0]);
            Assert.AreEqual(expectedYs[1], actualYs[1]);
            Assert.AreEqual(testXs.Length - 1, testCurveIncreasing.Count);
        }

        [TestMethod()]
        public void Validate_RepeatingOrdinatesRemovedReturnsTrue()
        {
            //Arrange
            double[] testXs = new double[3] { 0, 1, 1 };
            double[] testYs = new double[3] { 2, 3, 3 };
            Statistics.CurveIncreasing testCurveIncreasing = new Statistics.CurveIncreasing(testXs, testYs, true, false);
            BaseImplementation testOrdinatesFunction = FunctionFactory.CreateNew(testCurveIncreasing, FunctionTypeEnum.NotSet);
            //Assert
            Assert.IsTrue(testOrdinatesFunction.IsValid);
        }

        [TestMethod()]
        public void Validate_TooManyRepeatingOrdinatesRemovedReturnsFalse()
        {
            //Arrange
            double[] testXs = new double[3] { 0, 0, 0 };
            double[] testYs = new double[3] { 2, 2, 2 };
            Statistics.CurveIncreasing testCurveIncreasing = new Statistics.CurveIncreasing(testXs, testYs, true, false);
            BaseImplementation testOrdinatesFunction = FunctionFactory.CreateNew(testCurveIncreasing, FunctionTypeEnum.NotSet);
            //Assert
            Assert.IsFalse(testOrdinatesFunction.IsValid);
            Assert.AreEqual(1, testCurveIncreasing.Count);
        }
        #endregion

        #region ValidateFrequencyValues() Tests
        [TestMethod()]
        public void ValidateFrequencyValues_FrequencyTypePlusGoodValuesReturnTrue()
        {
            FunctionTypeEnum testFrequencyType = FunctionTypeEnum.OutflowFrequency;
            OrdinatesFunction testOrdinatesFunction = new OrdinatesFunction(defaultFrequencyOrdinates);
            Assert.IsTrue(testOrdinatesFunction.ValidateFrequencyValues(testFrequencyType));
        }

        [TestMethod()]
        public void ValidateFrequencyValues_FrequencyTypePlusBadValuesReturnsFalse()
        {
            FunctionTypeEnum testFrequencyType = FunctionTypeEnum.OutflowFrequency;
            OrdinatesFunction testOrdinatesFunction = new OrdinatesFunction(defaultCurveIncreasing);
            Assert.IsFalse(testOrdinatesFunction.ValidateFrequencyValues(testFrequencyType));
        }

        [TestMethod()]
        public void ValidateFrequencyValues_NotFrequencyTypePlusGoodValuesReturnsFalse()
        {
            FunctionTypeEnum testFrequencyType = FunctionTypeEnum.InflowOutflow;
            OrdinatesFunction testOrdinatesFunction = new OrdinatesFunction(defaultFrequencyOrdinates);
            Assert.IsFalse(testOrdinatesFunction.ValidateFrequencyValues(testFrequencyType));
        }
        #endregion

        #region GetXfromY() Tests
        [TestMethod()]
        public void GetXfromY_TinyYReturnsSmallestX()
        {
            double y = -1;
            IFunctionBase testOrdinatesFunction = new OrdinatesFunction(defaultCurveIncreasing);
            Assert.AreEqual(0, testOrdinatesFunction.GetXfromY(y));
        }

        public void GetXfromY_SmallestYReturnsSmallestX()
        {
            double y = 2;
            IFunctionBase testOrdinatesFunction = new OrdinatesFunction(defaultCurveIncreasing);
            Assert.AreEqual(0, testOrdinatesFunction.GetXfromY(y));
        }

        [TestMethod()]
        public void GetXfromY_GiantYReturnsLargestX()
        {
            double y = 100;
            IFunctionBase testOrdinatesFunction = new OrdinatesFunction(defaultCurveIncreasing);
            Assert.AreEqual(1, testOrdinatesFunction.GetXfromY(y));
        }

        [TestMethod()]
        public void GetXfromY_LargestYReturnsLargestX()
        {
            double y = 3;
            IFunctionBase testOrdinatesFunction = new OrdinatesFunction(defaultCurveIncreasing);
            Assert.AreEqual(1, testOrdinatesFunction.GetXfromY(y));
        }

        [TestMethod()]
        public void GetXfromY_ReturnsInterpolatedXForInbetweenY()
        {
            double y = 2.5;
            IFunctionBase testOrdinatesFunction = new OrdinatesFunction(defaultCurveIncreasing);
            Assert.AreEqual(0.5, testOrdinatesFunction.GetXfromY(y));
        }

        [TestMethod()]
        public void GetXfromY_FindsRightInterpolationOrdinatesForInbetweenY()
        {
            //Arrange
            double y = 7.3;
            double[] testXs = new double[5] { 0, 1, 2, 3, 4 }, testYs = new double[5] { 5, 6, 7, 8, 9 };
            Statistics.CurveIncreasing testCurveIncreasing = new Statistics.CurveIncreasing(testXs, testYs, true, false);
            IFunctionBase testOrdinatesFunction = new OrdinatesFunction(testCurveIncreasing);
            Assert.AreEqual(2.3, testOrdinatesFunction.GetXfromY(y));
        }

        [TestMethod()]
        public void GetXfromY_FindsRightInterpolationOrdinatesForMatchingYs()
        {
            //Arrange
            double y = 8;
            double[] testXs = new double[5] { 0, 1, 2, 3, 4 }, testYs = new double[5] { 5, 6, 7, 8, 9 };
            Statistics.CurveIncreasing testCurveIncreasing = new Statistics.CurveIncreasing(testXs, testYs, true, false);
            IFunctionBase testOrdinatesFunction = new OrdinatesFunction(testCurveIncreasing);
            Assert.AreEqual(3, testOrdinatesFunction.GetXfromY(y));
        }
        #endregion

        #region Compose() Tests
        
        [TestMethod()]
        public void Compose_LowerTransformNoOverlapReturns_DoubleNaNYValueOrdinates()
        {
            // 0, 1 => 2, 3 || 0, 1 => 4, 5
            OrdinatesFunction testOrdinates = new OrdinatesFunction(defaultCurveIncreasing);
            OrdinatesFunction testTransform = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 0, 1 }, new double[] { 4, 5 }, true, false));
            List<Tuple<double, double>> actualResult = (List<Tuple<double, double>>)testOrdinates.Compose(testTransform.Ordinates);
            List<Tuple<double, double>> expectedResult = new List<Tuple<double, double>>() { new Tuple<double, double>(0, double.NaN),
                                                                                             new Tuple<double, double>(1, double.NaN) };
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Compose_LowerTransformWithPerfectMatchReturns_MatchedOverlappingOrdinates()
        {
            // 0, 1 => 2, 3 || 1, 2, 3 => 3, 4, 5
            OrdinatesFunction testOrdinates = new OrdinatesFunction(defaultCurveIncreasing);
            OrdinatesFunction testTransform = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 1, 2, 3 }, new double[] { 3, 4, 5 }, true, false));
            List<Tuple<double, double>> actualResult = (List<Tuple<double, double>>)testOrdinates.Compose(testTransform.Ordinates);
            List<Tuple<double, double>> expectedResult = new List<Tuple<double, double>>() { new Tuple<double, double>(0, 4),
                                                                                             new Tuple<double, double>(1, 5) };
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Compose_HigherTransformNoOverlapReturns_DoubleNaNYValueOrdinates()
        {
            // 0, 1 => 2, 3 || 4, 5 => 6, 7
            OrdinatesFunction testOrdinates = new OrdinatesFunction(defaultCurveIncreasing);
            OrdinatesFunction testTransform = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 4, 5 }, new double[] { 4, 5 }, true, false));
            List<Tuple<double, double>> actualResult = (List<Tuple<double, double>>)testOrdinates.Compose(testTransform.Ordinates);
            List<Tuple<double, double>> expectedResult = new List<Tuple<double, double>>() { new Tuple<double, double>(0, double.NaN),
                                                                                             new Tuple<double, double>(1, double.NaN) };
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Compose_HigherTransformWithPerfectMatchReturns_MatchedOverlappingOrdinates()
        {
            //0, 1 => 2, 3 || 2, 3, 4 => 4, 5, 6
            OrdinatesFunction testOrdinates = new OrdinatesFunction(defaultCurveIncreasing);
            OrdinatesFunction testTransform = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 2, 3, 4 }, new double[] { 4, 5, 6 }, true, false));
            List<Tuple<double, double>> actualResult = (List<Tuple<double, double>>)testOrdinates.Compose(testTransform.Ordinates);
            List<Tuple<double, double>> expectedResult = new List<Tuple<double, double>>() { new Tuple<double, double>(0, 4),
                                                                                             new Tuple<double, double>(1, 5) };
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Compose_PerfectMatchReturns_MatchedTransformedOrdinates()
        {
            // 0, 1 => 2, 3 || 2, 3 => 4, 5
            OrdinatesFunction testOrdinates = new OrdinatesFunction(defaultCurveIncreasing);
            OrdinatesFunction testTransform = new OrdinatesFunction(defaultTransformOrdinates);
            List<Tuple<double, double>> actualResult = (List<Tuple<double, double>>)testOrdinates.Compose(testTransform.Ordinates);
            List<Tuple<double, double>> expectedResult = new List<Tuple<double, double>>() { new Tuple<double, double>(0, 4),
                                                                                             new Tuple<double, double>(1, 5) };
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Compose_PerfectMatchPlusExtraTranformPointReturns_AllOrdinates()
        {
            // 0, 1 => 2, 3 || 2, 2.25, 2.50, 2.75, 3 => 4, 4.25, 4.50, 4.75, 5
            OrdinatesFunction testOrdinates = new OrdinatesFunction(defaultCurveIncreasing);
            OrdinatesFunction testTransform = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 2, 2.25, 2.5, 2.75, 3 }, new double[] { 4, 4.25, 4.5, 4.75, 5 }, true, false));
            List<Tuple<double, double>> actualResult = (List<Tuple<double, double>>)testOrdinates.Compose(testTransform.Ordinates);
            List<Tuple<double, double>> expectedResult = new List<Tuple<double, double>>() { new Tuple<double, double>(0, 4),
                                                                                             new Tuple<double, double>(0.25, 4.25),
                                                                                             new Tuple<double, double>(0.5, 4.5),
                                                                                             new Tuple<double, double>(0.75, 4.75),
                                                                                             new Tuple<double, double>(1, 5) };
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Compose_InBetweenPerfectMatchReturns_MatchedTransformedOrdinatesPlusConstants()
        {
            OrdinatesFunction testOrdinates = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 0, 0.25, 0.5, 0.75, 1 }, new double[] { 2, 3, 4, 5, 6 }, true, false));
            OrdinatesFunction testTransform = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 3, 4, 5 }, new double[] { 7, 8, 9 }, true, false));
            List<Tuple<double, double>> actualResult = (List<Tuple<double, double>>)testOrdinates.Compose(testTransform.Ordinates);
            List<Tuple<double, double>> expectedResult = new List<Tuple<double, double>>() { new Tuple<double, double>(0, 7),
                                                                                             new Tuple<double, double>(0.25, 7),
                                                                                             new Tuple<double, double>(0.50, 8),
                                                                                             new Tuple<double, double>(0.75, 9),
                                                                                             new Tuple<double, double>(1, 9) };
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void Compose_HigherandLowerTransformWithPerfectMatchReturns_MatchedTransformOrdinates()
        {

        }

        [TestMethod()]
        public void Compose_LowerTransformWithOverlapandExtraPointsReturns_MatchedPoints()
        {
            OrdinatesFunction testOrdinates = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 0, 0.25, 0.5, 0.75, 1 }, new double[] { 25, 40, 50, 75, 100 }, true, false));
            OrdinatesFunction testTransform = new OrdinatesFunction(new Statistics.CurveIncreasing(new double[] { 0, 10, 40, 70 }, new double[] { 10, 20, 50, 80 }, true, false));
            List<Tuple<double, double>> actualResult = (List<Tuple<double, double>>)testOrdinates.Compose(testTransform.Ordinates);
            List<Tuple<double, double>> expectedResult = new List<Tuple<double, double>>() { new Tuple<double, double>(0, 35),
                                                                                             new Tuple<double, double>(0.25, 50),
                                                                                             new Tuple<double, double>(0.50, 60),
                                                                                             new Tuple<double, double>(0.70, 80),
                                                                                             new Tuple<double, double>(0.75, 80),
                                                                                             new Tuple<double, double>(1, 80) };
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }
        #endregion
    }
}
