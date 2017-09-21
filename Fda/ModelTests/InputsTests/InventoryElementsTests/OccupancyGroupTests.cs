using System;
using Moq;
using Model.Inputs.Inventories.OccupancyTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests.InputsTests.InventoryElementsTests
{
    [TestClass()]
    public class OccupancyGroupTests
    {
        #region Properties
        //internal IOccupancyGroup _DefaultOccupancyGroup = new OccupancyGroup("defaultGroup");
        #endregion

        #region Constructor Tests
        //[TestMethod()]
        //public void Constructor_NewGroupAddedToRegistry()
        //{
        //    bool existsDefaultGroup = false;
        //    for (int i = 0; i < OccupancyGroupRegistry.Instance.OccupancyGroups.Count; i++)
        //    {
        //        if (OccupancyGroupRegistry.Instance.OccupancyGroups[i].Name == _DefaultOccupancyGroup.Name) { existsDefaultGroup = true; break; } 
        //    }
        //    Assert.IsTrue(existsDefaultGroup);
        //}
        //[TestMethod()]
        //public void Constructor_GroupNameChangedDuplicateRegisteredGroupNameIsPassedIn()
        //{
        //    bool existsReNamedDupicateGroup = false;
        //    IOccupancyGroup duplicateDefaultOccupancyGroup = new OccupancyGroup("defaultGroup");
        //    for (int i = 0; i < OccupancyGroupRegistry.Instance.OccupancyGroups.Count; i++)
        //    {
        //        if(OccupancyGroupRegistry.Instance.OccupancyGroups[i].Name == "defaultGroup1") { existsReNamedDupicateGroup = true; break; }
        //    }
        //    Assert.IsTrue(existsReNamedDupicateGroup);
        //}
        #endregion

        #region Methods
        //[TestMethod()]
        //public void AddToGroup_PlacesNameinNamesProperty()
        //{
        //    bool existsName = false;
        //    //_DefaultOccupancyGroup.AddToGroup(new OccupancyType("testName"));
        //    var mockOccupancyType = new Mock<IOccupancyType>();
        //    //mockOccupancyType.SetupProperty(x => x.Name = "testName");
        //    foreach (var name in _DefaultOccupancyGroup.OccupancyTypesNames)
        //    {
        //        if (name == "testName") { existsName = true; break; }
        //    }
        //    Assert.IsTrue(existsName);
        //}
        //[TestMethod()]
        //public void AddToGroup_PlacesIteminOccupancyTypesProperty()
        //{
        //    bool existsOccupancyType = false;
        //    //IOccupancyType testOccupancyType = new OccupancyType("testName");
        //    //_DefaultOccupancyGroup.AddToGroup(testOccupancyType);
        //    foreach (var type in _DefaultOccupancyGroup.OccupancyTypes)
        //    {
        //        //if (type == testOccupancyType) { existsOccupancyType = true; break; }
        //    }
        //    Assert.IsTrue(existsOccupancyType);
        //}
        //[TestMethod()]
        //public void AddToGroup_SecondItemOfSameNameIsRenamed()
        //{
        //    bool existsOccupancyType = false;
        //    //IOccupancyType testOccupancyType = new OccupancyType("testName");
        //    //IOccupancyType duplicateOccupancyType = new OccupancyType("testName");
        //    //_DefaultOccupancyGroup.AddToGroup(testOccupancyType);
        //    //_DefaultOccupancyGroup.AddToGroup(duplicateOccupancyType);
        //    foreach (var type in _DefaultOccupancyGroup.OccupancyTypes)
        //    {
        //        //if (type == duplicateOccupancyType) { existsOccupancyType = true; break; }
        //    }
        //    Assert.IsTrue(existsOccupancyType);
        //}
        #endregion
    }
}
