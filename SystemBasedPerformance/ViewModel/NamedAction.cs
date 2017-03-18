using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBasedPerformance.ViewModel
{
    public class NamedAction: BaseViewModel
    {
        #region Fields
        private string _Name;
        private bool _IsEnabled = true;
        private Action<object, EventArgs> _Action;
        #endregion


        #region Properties
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
                NotifyPropertyChanged();
            }
        }

        public Action<object, EventArgs> Action
        {
            get
            {
                return _Action;
            }
            set
            {
                _Action = value;
                NotifyPropertyChanged();
            }
        }
        #endregion


        #region Constructor
        public NamedAction(string nameOfAction)
        {
            Name = nameOfAction;
            Action = delegate (object sender, EventArgs e)
            {
            };
        }
        #endregion

        #region Voids
        public void ExecuteAction(object sender)
        {
        }
        #endregion

    }
}
