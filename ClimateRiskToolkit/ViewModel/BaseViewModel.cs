using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ClimateRiskToolkit.ViewModel
{
    public abstract class BaseViewModel: INotifyPropertyChanged
    {
        #region Fields
        
        #endregion


        #region Properties
        #endregion


        #region Constructors
        public BaseViewModel()
        {
        }
        #endregion


        #region INotifyPropertyChangedMembers
        public event PropertyChangedEventHandler PropertyChanged;

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
