using System;
using Model.Inputs.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ModelTests
{
    [TestClass]
    public class OrdinatesFunctionTests
    {
        #region Validate() Tests
        [TestMethod()] 
        public void Validate_GoodDataReturnsTrue()
        {
            //Arrange
            double[] xs = new double[2] { 0, 1 }, ys = new double[2] { 2, 3 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            //Act
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            //Assert
            Assert.IsTrue(testOrdinatesFunction.IsValid);
        }

        [TestMethod()]
        public void Validate_SingleOrdinateReturnsFalse()
        {
            //Arrange
            double[] xs = new double[1] { 1 }, ys = new double[1] { 2 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            //Act
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            //Assert
            Assert.IsFalse(testOrdinatesFunction.IsValid);
        }

        [TestMethod()]
        public void Validate_RepeatingOrdinatesRemoved()
        {
            double[] xs = new double[3] { 0, 1, 1 }, ys = new double[3] { 2, 3, 3 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            //Act
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            List<double> expectedXs = new List<double>() { 0, 1 }, expectedYs = new List<double>() { 2, 3 };
            List<double> actualXs = new List<double>(), actualYs = new List<double>();
            for(int i = 0; i < testFunction.Count; i++)
            {
                actualXs.Add(testFunction.get_X(i));
                actualYs.Add(testFunction.get_Y(i));
            }
            //Assert
            Assert.AreEqual(expectedXs[0], actualXs[0]);
            Assert.AreEqual(expectedXs[1], actualXs[1]);
            Assert.AreEqual(expectedYs[0], actualYs[0]);
            Assert.AreEqual(expectedYs[1], actualYs[1]);
            Assert.AreEqual(xs.Length - 1, testFunction.Count);
        }

        [TestMethod()]
        public void Validate_RepeatingOrdinatesRemovedReturnsTrue()
        {
            double[] xs = new double[3] { 0, 1, 1 }, ys = new double[3] { 2, 3, 3 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            //Act
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            //Assert
            Assert.IsTrue(testOrdinatesFunction.IsValid);
        }

        [TestMethod()]
        public void Validate_TooManyRepeatingOrdinatesRemovedReturnsFalse()
        {
            //Arrange
            double[] xs = new double[3] { 0, 0, 0 }, ys = new double[3] { 2, 2, 2 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            //Act
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            //Assert
            Assert.IsFalse(testOrdinatesFunction.IsValid);
            Assert.AreEqual(1, testFunction.Count);
        }
        #endregion

        #region GetXfromY() Tests
        [TestMethod()]
        public void GetXfromY_TinyYReturnsSmallestX()
        {
            //Arrange
            double y = -1;
            double[] xs = new double[2] { 0, 1 }, ys = new double[2] { 2, 3 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            //Act

            //Assert
            Assert.AreEqual(0, testOrdinatesFunction.GetXfromY(y));
        }

        public void GetXfromY_SmallestYReturnsSmallestX()
        {
            //Arrange
            double y = 2;
            double[] xs = new double[2] { 0, 1 }, ys = new double[2] { 2, 3 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            //Act

            //Assert
            Assert.AreEqual(0, testOrdinatesFunction.GetXfromY(y));
        }

        [TestMethod()]
        public void GetXfromY_GiantYReturnsLargestX()
        {
            //Arrange
            double y = 100;
            double[] xs = new double[2] { 0, 1 }, ys = new double[2] { 2, 3 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            //Act

            //Assert
            Assert.AreEqual(1, testOrdinatesFunction.GetXfromY(y));
        }

        [TestMethod()]
        public void GetXfromY_LargestYReturnsLargestX()
        {
            //Arrange
            double y = 3;
            double[] xs = new double[2] { 0, 1 }, ys = new double[2] { 2, 3 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            //Act

            //Assert
            Assert.AreEqual(1, testOrdinatesFunction.GetXfromY(y));
        }

        [TestMethod()]
        public void GetXfromY_ReturnsInterpolatedXForInbetweenY()
        {
            //Arrange
            double y = 2.5;
            double[] xs = new double[2] { 0, 1 }, ys = new double[2] { 2, 3 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            //Act

            //Assert
            Assert.AreEqual(0.5, testOrdinatesFunction.GetXfromY(y));
        }

        [TestMethod()]
        public void GetXfromY_FindsRightInterpolationOrdinatesForInbetweenY()
        {
            //Arrange
            double y = 7.3;
            double[] xs = new double[5] { 0, 1, 2, 3, 4 }, ys = new double[5] { 5, 6, 7, 8, 9 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            //Act

            //Assert
            Assert.AreEqual(2.3, testOrdinatesFunction.GetXfromY(y));
        }

        [TestMethod()]
        public void GetXfromY_FindsRightInterpolationOrdinatesForMatchingYs()
        {
            //Arrange
            double y = 8;
            double[] xs = new double[5] { 0, 1, 2, 3, 4 }, ys = new double[5] { 5, 6, 7, 8, 9 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            IFunction testOrdinatesFunction = FunctionFactory.CreateNew(testFunction, FunctionType.NotSet);
            //Act

            //Assert
            Assert.AreEqual(3, testOrdinatesFunction.GetXfromY(y));
        }
        #endregion
    }
}
