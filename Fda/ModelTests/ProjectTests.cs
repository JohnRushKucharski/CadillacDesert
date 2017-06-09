using System;
using Model;
using Model.Inputs.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelTests
{
    [TestClass()]
    public class ProjectTests
    {
        [TestMethod()]
        public void CreateNewProject_NullInstanceNoPathReturnsCurrentDirectory()
        {
            //Arrange
            string testName = "testProject";
            string testDirectory = Environment.CurrentDirectory;

            //Act
            string actual = Project.CreateNewProject(testName).GetFilePathWithoutExtension();
            string expected = System.IO.Path.Combine(testDirectory, testName);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CreateNewProject_MultipleCallReturnSameInstance()
        {
            //Arrange
            Project.CreateNewProject("firstTestName");
            Project secondProject = Project.CreateNewProject("secondTestName");
            
            //Act
            //Assert
            Assert.AreEqual(Project.Instance.GetFilePathWithoutExtension(), secondProject.GetFilePathWithoutExtension());
        }

        [TestMethod()]
        public void ValidateDirectory_BadDirectoryReturnsFalse()
        {
            //Arrange
            Project testProject = Project.CreateNewProject("testProject");

            //Act
            //Assert
            Assert.AreEqual(false, Project.Instance.ValidateDirectory("BadPath"));
        }

        [TestMethod()]
        public void ValidateDirectory_GoodDirectoryReturnsTrue()
        {
            //Arrange
            Project testProject = Project.CreateNewProject("testProject");

            //Act
            //Assert
            Assert.AreEqual(true, Project.Instance.ValidateDirectory(@"C:\Users"));
        }

        [TestMethod()]
        public void ValidateDirectory_GoodDirectoryPlusFileNameReturnsFalse()
        {
            //Arrange
            Project testProject = Project.CreateNewProject("testProject");
            string goodPathPlusFileName = System.IO.Path.Combine(Environment.CurrentDirectory, @"Model.dll");

            //Act
            //Assert
            if (new System.IO.FileInfo(goodPathPlusFileName).Exists) Assert.AreEqual(false, Project.Instance.ValidateDirectory(goodPathPlusFileName));
            else Assert.Fail("The test directory plus file path do not exist");
        }

        [TestMethod()]
        public void ValidateFileName_FileNamePlusExtensionReturnsFalse()
        {
            //Arrange
            Project testProject = Project.CreateNewProject("testProject.sqlite");

            //Act
            bool actual = Project.Instance.ValidateName("testProject.sqlite");

            //Assert
            Assert.AreEqual(false, actual);
        }

        [TestMethod()]
        public void ValidateFileName_FileNamePlusDirectoryReturnsFalse()
        {
            //Arrange
            Project testProject = Project.CreateNewProject("Project\\testProject");

            //Act
            bool actual = Project.Instance.ValidateName("Project\\testProject");

            //Assert
            Assert.AreEqual(false, actual);
        }

        [TestMethod()]
        public void ValidateFileName_FileNamePlusWeirdCharacterReturnsFalse()
        {
            //Arrange
            Project testProject = Project.CreateNewProject("*testProject");
            
            //Act
            bool actual = Project.Instance.ValidateName("*testProject");

            //Assert
            Assert.AreEqual(false, actual);
        }

        [TestMethod()]
        public void ValidateFileName_GoodNameReturnsTrue()
        {
            //Arrange
            Project testProject = Project.CreateNewProject("testProject");

            //Act
            bool actual = Project.Instance.ValidateName();

            //Assert
            Assert.AreEqual(true, actual);
        }

        [TestMethod()]
        public void Write_CreatesNewFileIfNonExists()
        {
            //Arrange
            bool fileExists;
            Project testProject = Project.CreateNewProject("testProject");

            double[] xs = new double[2] { 0, 1 }, ys = new double[2] { 2, 3 };
            Statistics.CurveIncreasing testFunction = new Statistics.CurveIncreasing(xs, ys, true, false);
            OutflowFrequencyDecorator testDecorator = (OutflowFrequencyDecorator)FunctionFactory.CreateNew(testFunction, Model.Inputs.Functions.FunctionType.OutflowFrequency);

            //Act
            //if (new System.IO.FileInfo(Project.Instance.GetFilePathWithoutExtension() + ".sqlite").Exists) new System.IO.FileInfo(Project.Instance.GetFilePathWithoutExtension() + ".sqlite").Delete();
            System.IO.FileInfo temporaryDatabase = new System.IO.FileInfo(Project.Instance.GetFilePathWithoutExtension() + ".sqlite");

            
            //Project.Instance.Write();
            //Model.Inputs.Functions.DecoratorList.Instance.Write();
            //testDecorator.Write();
            fileExists = temporaryDatabase.Exists;

            //Assert
            Assert.IsTrue(fileExists);
        }

        [TestMethod()]
        public void Write_DoesnotFailIfFileAlreadyExists()
        {
            //Arrange
            Project testProject = Project.CreateNewProject("testProject");

            //Act
            if (!new System.IO.FileInfo(Project.Instance.GetFilePathWithoutExtension() + "sqlite").Exists) Project.Instance.Write();
            System.IO.FileInfo temporaryDatabase = new System.IO.FileInfo(Project.Instance.GetFilePathWithoutExtension() + ".sqlite");
            if (temporaryDatabase.Exists) Project.Instance.Write();

            //Assert
            Assert.AreEqual(true, temporaryDatabase.Exists);
        }

        [TestMethod()]
        public void IsValidGetter_ReturnsValidateMethodValue()
        {
            //Arrange
            Project testProject = Project.CreateNewProject("testProject");

            //Act
            bool isValid = Project.Instance.IsValid;

            //Assert
            Assert.AreEqual(isValid, testProject.Validate());
        }
    }
}
