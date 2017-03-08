using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateRiskToolkit.Model
{
    public class Project
    {
        #region Fields
        private static Project _Instance;
        private string _ProjectFolderPath;
        private string _ProjectName;
        private string _ProjectDescription;
        #endregion


        #region Properties
        public static Project Instance
        {
            get
            {
                return _Instance;
            }
        }
        public string FolderPath
        {
            get
            {
                return _ProjectFolderPath;
            }
            set
            {
                _ProjectFolderPath = value;
            }
        }
        public string Name
        {
            get
            {
                return _ProjectName;
            }
            set
            {
                _ProjectName = value;
            }
        }
        public string Description
        {
            get
            {
                return _ProjectDescription;
            }
            set
            {
                _ProjectDescription = value;
            }
        }
        #endregion


        #region Constructor
        private Project(string filePath, string projectName, string projectDescription)
        {
            FolderPath = filePath;
            Name = projectName;
            Description = projectDescription;
        }
        #endregion


        #region Functions
        public static Project ProjectSingleton(string filePath = null, string projectName = null, string projectDescription = null)
        {
            if (Instance == null)
            {
                _Instance = new Project(filePath, projectName, projectDescription);
            }
            return Instance;
        }
        #endregion
    }
}
