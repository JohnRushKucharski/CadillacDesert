using System;
using Model.Inputs.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests
{
    [TestClass]
    public class FunctionFactoryTests
    {
        [TestMethod()]
        public void CreateNew_CurveIncreasingOutflowFrequencyYieldsOutflowFrequencyDecorator()
        {
            //Arrange
            double[] xs = new double[2] { 0, 1 }, ys = new double[2] { 3, 4 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);

            //Act
            IFunction testOutflowFrequencyDecorator = FunctionFactory.CreateNew(testFunction, FunctionType.OutflowFrequency);
            Type expectedType = typeof(Model.Inputs.Functions.OutflowFrequencyDecorator);
            Type actualType = testOutflowFrequencyDecorator.GetType();

            //Assert
            Assert.AreEqual(expectedType, actualType);
        }

        [TestMethod()]
        public void CreateNew_LPIIIOutflowFrequencyYieldsOutflowFrequencyDecorator()
        {
            //Arrange
            Statistics.LogPearsonIII testFunction = new Statistics.LogPearsonIII(1, 0.2, 0.1, 100);

            //Act
            IFunction testOutflowFrequencyDecorator = FunctionFactory.CreateNew(testFunction, FunctionType.OutflowFrequency);
            Type expectedType = typeof(Model.Inputs.Functions.OutflowFrequencyDecorator);
            Type actualType = testOutflowFrequencyDecorator.GetType();

            //Assert
            Assert.AreEqual(expectedType, actualType);
        }
    }
}
