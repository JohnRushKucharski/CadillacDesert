using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClimateRiskToolkit.Model;

namespace ClimateRiskToolkit.ViewModel
{
    public class ProjectVM : BaseViewModel
    {
        #region Fields
        private Project _ProjectSingleton;
        #endregion


        #region Properties
        public Project ProjectSingleton
        {
            get
            {
                return _ProjectSingleton;
            }
            private set
            {
                _ProjectSingleton = value;
                NotifyPropertyChanged(nameof(ProjectSingleton));
            }
        }
        #endregion


        #region Constructor
        public ProjectVM()
        {
            Model.Project.ProjectSingleton();
        }

        public ProjectVM(string filePath, string fileName)
        {
            if (Project.Instance == null)
            {
                Project.ProjectSingleton(filePath, fileName);
            }
            else
            {
                Project.Instance.FolderPath = filePath;
                Project.Instance.Name = fileName;
            }
        }
        #endregion
    }
}
