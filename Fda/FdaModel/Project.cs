using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Model
{

    [Author("John Kucharski", "23 May 2017")]
    public sealed class Project : IProject, DataBase.IEntityData, IValidateData
    {

        #region Notes
        // Rename option?
        // Messaging when project singleton is called incorrectly.
        #endregion

        #region Fields
        private bool _IsValid;
        #endregion

        #region Properties
        public int Id { get; private set; }
        public static Project Instance { get; private set; }
        public string Directory { get; private set; }
        public string Name { get; private set; }
        public bool IsValid
        {
            get
            {
                return Validate();
            }
            private set
            {
                _IsValid = value;
            }
        }          
        #endregion

        #region Constructor
        private Project(string name, string directory = null)
        {
            Name = name;
            if (Directory == null) Directory = Environment.CurrentDirectory;
            else Directory = directory;
            IsValid = Validate();
        }
        #endregion

        #region Methods
        public static Project CreateNewProject(string projectName, string projectDirectory = null)
        {
            if (Instance == null) Instance = new Project(projectName, projectDirectory);
            return Instance; 
        }

        public string GetFilePathWithoutExtension()
        {
            return System.IO.Path.Combine(Instance.Directory, Instance.Name);
        }

        public bool Validate()
        {
            if (ValidateDirectory(Directory) && ValidateName(Name)) return true;
            else return false;
        }

        public IEnumerable<string> ReportValidationErrors()
        {
            throw new NotImplementedException();
        }

        public bool ValidateDirectory(string fileDirectory = null)
        {
            if (fileDirectory == null) fileDirectory = Directory;
            return new System.IO.DirectoryInfo(fileDirectory).Exists;
        }

        public bool ValidateName(string fileName = null)
        {
            if (fileName == null) fileName = Name;
            if (fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1 ||
                fileName.Contains(".") ||
                fileName.Contains("\\")) return false;
            else return true;
        }

        public void Write()
        {
            string dbPath = String.Format("Data Source={0};Version=3;", GetFilePathWithoutExtension() + ".sqlite");
            var sqLiteConnection = new System.Data.SQLite.SQLiteConnection(dbPath);
            using (var context = new DataBase.DataContext(sqLiteConnection, false))
            {
                //context.Projects.Add(this);
                context.SaveChanges();
            }
        }

        public void Read()
        {
            string dbPath = String.Format("Data Source={0};Version=3;", System.IO.Path.Combine(GetFilePathWithoutExtension(), ".sqlite"));
            var sqLiteConnection = new System.Data.SQLite.SQLiteConnection(dbPath);
            using (var context = new DataBase.DataContext(sqLiteConnection, false))
            {
                foreach(var project in context.Set<Project>())
                {

                }
            }
        }
        #endregion
    }
}
