using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SystemBasedPerformance.View
{
    public class CustomListBox: ListBox, System.ComponentModel.INotifyPropertyChanged
    {
        public static readonly DependencyProperty SelectedItemsListProperty = DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(ListBox), new PropertyMetadata(null));

        public IList SelectedItemsList
        {
            get
            {
                return (IList)GetValue(SelectedItemsListProperty);
            }
            set
            {
                SetValue(SelectedItemsListProperty, value);
                //NotifyPropertyChanged("SelectedItemsList");
            }
        }

        public CustomListBox()
        {
            this.SelectionChanged += CustomListBox_SelectionChanged;
        }

        public void CustomListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItemsList;
        }

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
