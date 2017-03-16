using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimateRiskToolkit.ViewModel
{
    public class NamedAction: BaseViewModel
    {
        #region Fields
        private string _Name;
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
                NotifyPropertyChanged(nameof(Name));
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
                NotifyPropertyChanged(nameof(Action));
            }
        }
        #endregion



        #region Constructor
        public NamedAction(string actionName)
        {
            Name = actionName;
            Action = delegate (object sender, EventArgs e)
            {
                System.Diagnostics.Debug.WriteLine("This worked.");
            };
        }
        #endregion

        public void Execute(object o)
        {
            
        }
    }
}
