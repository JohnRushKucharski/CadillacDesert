using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBasedPerformance.ViewModel
{
    public abstract class BaseViewModel: System.ComponentModel.INotifyPropertyChanged
    {

        #region Constructor
        public BaseViewModel()
        {
        }
        #endregion

        #region INotifyPropertyChangedMembers
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion



    }


}
