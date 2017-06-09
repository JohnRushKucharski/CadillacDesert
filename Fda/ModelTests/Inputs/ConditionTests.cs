using System;
using Model;
using Model.Inputs;
using Model.Inputs.Functions;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests.Inputs
{
    [TestClass]
    public class ConditionTests
    {
        #region Validate() Tests
        [TestMethod()]
        public void Validate_OrderedTransformsReturnValidCondition()
        {
            //Arrange
            IComputableFunction EntryFunction = (IComputableFunction)FunctionFactory.CreateNew(new Statistics.LogPearsonIII(1, .5, 0, 100), FunctionType.InflowFrequency);
            double[] Xs = { 0, 1 }, Ys = { 2, 3 };
            IFunction IOFunction = FunctionFactory.CreateNew(new Statistics.CurveIncreasing(Xs, Ys, true, false), FunctionType.InflowOutflow);
            IFunction RatingFunction = FunctionFactory.CreateNew(new Statistics.CurveIncreasing(Xs, Ys, true, false), FunctionType.Rating);
            List<IFunction> transformFunctions = new List<IFunction>() { IOFunction, RatingFunction };
            IThreshold threshold = new Threshold(ThresholdTypes.ExteriorStage, 10);
            ICondition testCondition = new Condition(EntryFunction, transformFunctions, threshold);
            //Act 
            

            //Assert
            Assert.IsTrue(testCondition.Validate());
        }

        [TestMethod()]
        public void Validate_UnOrderedTransformsReturnValidCondition()
        {
            //Arrange
            IComputableFunction EntryFunction = (IComputableFunction)FunctionFactory.CreateNew(new Statistics.LogPearsonIII(1, .5, 0, 100), FunctionType.InflowFrequency);
            double[] Xs = { 0, 1 }, Ys = { 2, 3 };
            IFunction IOFunction = FunctionFactory.CreateNew(new Statistics.CurveIncreasing(Xs, Ys, true, false), FunctionType.InflowOutflow);
            IFunction RatingFunction = FunctionFactory.CreateNew(new Statistics.CurveIncreasing(Xs, Ys, true, false), FunctionType.Rating);
            List<IFunction> transformFunctions = new List<IFunction>() { RatingFunction, IOFunction };
            IThreshold threshold = new Threshold(ThresholdTypes.ExteriorStage, 10);
            ICondition testCondition = new Condition(EntryFunction, transformFunctions, threshold);
            //Act 


            //Assert
            Assert.IsTrue(testCondition.Validate());
        }

        #endregion

        #region ReportValidationErrors() Tests
        [TestMethod()]
        public void ReportValidationErrors_OrderedTransformsReturnValidCondition()
        {
            //Arrange
            IComputableFunction EntryFunction = (IComputableFunction)FunctionFactory.CreateNew(new Statistics.LogPearsonIII(1, .5, 0, 100), FunctionType.InflowFrequency);
            double[] Xs = { 0, 1 }, Ys = { 2, 3 };
            IFunction IOFunction = FunctionFactory.CreateNew(new Statistics.CurveIncreasing(Xs, Ys, true, false), FunctionType.InflowOutflow);
            IFunction RatingFunction = FunctionFactory.CreateNew(new Statistics.CurveIncreasing(Xs, Ys, true, false), FunctionType.Rating);
            List<IFunction> transformFunctions = new List<IFunction>() { IOFunction, RatingFunction };
            IThreshold threshold = new Threshold(ThresholdTypes.ExteriorStage, 10);
            ICondition testCondition = new Condition(EntryFunction, transformFunctions, threshold);
            //Act 
            testCondition.ReportValidationErrors();

            //Assert
            Assert.IsTrue(testCondition.IsValid);
        }

        #endregion

       
    }
}
