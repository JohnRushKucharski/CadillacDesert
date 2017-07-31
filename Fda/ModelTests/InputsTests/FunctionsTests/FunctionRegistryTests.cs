using System;
using System.Collections.Generic;
using Model.Inputs.Functions;
using Model.Inputs.Functions.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests.InputsTests.FunctionsTests
{
    [TestClass()]
    public class FunctionRegistryTests
    {
        #region CreateNew() Tests
        [TestMethod()]
        public void CreateNew_ReturnsSingleInstance()
        {
            FunctionRegistry one = FunctionRegistry.CreateNew();
            FunctionRegistry two = FunctionRegistry.CreateNew();
            Assert.AreEqual(one, two);
        }

        [TestMethod()]
        public void Add_FunctionReturnsNamesAndAddsToExistingInstance()
        {
            //BaseImplementation testFunction = FunctionFactory.CreateNew(new Statistics.CurveIncreasing(new double[] { 0, 1 }, new double[] { 2, 3 }, true, false), FunctionTypeEnum.InflowOutflow);
            //FunctionList.Add(testFunction);
            //Assert.AreEqual(2, FunctionList.Instance.Functions.Count);
        }

        public void Add_DuplicateNameIsAppendedWithCounterValue()
        {

        }


        #endregion
    }
}
