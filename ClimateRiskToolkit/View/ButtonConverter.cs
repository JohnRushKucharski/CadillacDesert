using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace ClimateRiskToolkit.View
{
    
    
    
    //class ButtonConverter : System.Windows.Data.IValueConverter
    //{
    //    object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        System.Windows.Controls.ContextMenu c = new System.Windows.Controls.ContextMenu();
    //        if (value == null)
    //        {
    //            c.Visibility = System.Windows.Visibility.Collapsed;
    //            c.IsEnabled = false;
    //            return c;
    //        }
    //        if (value.GetType() == typeof(List<FdaViewModel.Utilities.NamedAction>))
    //        {
    //            c = new System.Windows.Controls.ContextMenu();
    //            List<FdaViewModel.Utilities.NamedAction> Actions = (List<FdaViewModel.Utilities.NamedAction>)value;
    //            foreach (FdaViewModel.Utilities.NamedAction Action in Actions)
    //            {
    //                System.Windows.Controls.MenuItem mi = new System.Windows.Controls.MenuItem();
    //                if (Action.Path != null)
    //                {
    //                    string[] names = Action.Path.Split(new Char[] { '.' });
    //                    string firstheader = names[0];
    //                    bool isNewMI = true;
    //                    foreach (System.Windows.Controls.MenuItem item in c.Items)
    //                    {
    //                        if (item.Header.Equals(firstheader))
    //                        {
    //                            mi = item;
    //                            isNewMI = false;
    //                        }
    //                    }
    //                    if (isNewMI) mi.Header = firstheader;
    //                    System.Windows.Controls.MenuItem basemi = new System.Windows.Controls.MenuItem();
    //                    //basemi.DataContext = Action;
    //                    Binding mybinding = new Binding("IsEnabled");
    //                    mybinding.Source = Action;
    //                    mybinding.Mode = BindingMode.TwoWay;
    //                    mybinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
    //                    BindingOperations.SetBinding(basemi, System.Windows.Controls.MenuItem.IsEnabledProperty, mybinding);
    //                    Binding visibilityBinding = new Binding("IsVisible");
    //                    visibilityBinding.Source = Action;
    //                    visibilityBinding.Mode = BindingMode.OneWay;
    //                    visibilityBinding.Converter = new System.Windows.Controls.BooleanToVisibilityConverter();
    //                    visibilityBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
    //                    BindingOperations.SetBinding(basemi, System.Windows.Controls.MenuItem.VisibilityProperty, visibilityBinding);
    //                    Binding headerBinding = new Binding("Header");
    //                    headerBinding.Source = Action;
    //                    headerBinding.Mode = BindingMode.OneWay;
    //                    headerBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
    //                    BindingOperations.SetBinding(basemi, System.Windows.Controls.MenuItem.HeaderProperty, headerBinding);
    //                    //basemi.Header = names.Last();
    //                    basemi.Click += (ob, ev) => Action.Action(ob, ev);
    //                    System.Windows.Controls.MenuItem tmpmi = new System.Windows.Controls.MenuItem();
    //                    tmpmi = mi;
    //                    for (int i = 1; i < names.Count(); i++)
    //                    {
    //                        bool miExisted = false;
    //                        foreach (System.Windows.Controls.MenuItem item in tmpmi.Items)
    //                        {
    //                            if (item.Header.Equals(names[i]))
    //                            {
    //                                tmpmi = item;
    //                                miExisted = true;
    //                                isNewMI = false;
    //                            }
    //                        }
    //                        if (!miExisted)
    //                        {
    //                            System.Windows.Controls.MenuItem submi = new System.Windows.Controls.MenuItem();
    //                            submi.Header = names[i];
    //                            tmpmi.Items.Add(submi);
    //                            tmpmi = submi;
    //                        }
    //                    }
    //                    tmpmi.Items.Add(basemi);

    //                    if (isNewMI) { c.Items.Add(mi); }
    //                }
    //                else
    //                {
    //                    Binding headerBinding = new Binding("Header");
    //                    headerBinding.Source = Action;
    //                    headerBinding.Mode = BindingMode.OneWay;
    //                    headerBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
    //                    BindingOperations.SetBinding(mi, System.Windows.Controls.MenuItem.HeaderProperty, headerBinding);
    //                    Binding enabledBinding = new Binding("IsEnabled");
    //                    enabledBinding.Source = Action;
    //                    enabledBinding.Mode = BindingMode.OneWay;
    //                    enabledBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
    //                    BindingOperations.SetBinding(mi, System.Windows.Controls.MenuItem.IsEnabledProperty, enabledBinding);
    //                    Binding visibilityBinding = new Binding("IsVisible");
    //                    visibilityBinding.Source = Action;
    //                    visibilityBinding.Mode = BindingMode.OneWay;
    //                    visibilityBinding.Converter = new System.Windows.Controls.BooleanToVisibilityConverter();
    //                    visibilityBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
    //                    BindingOperations.SetBinding(mi, System.Windows.Controls.MenuItem.VisibilityProperty, visibilityBinding);
    //                    mi.Click += (ob, ev) => Action.Action(ob, ev);
    //                    c.Items.Add(mi);
    //                }
    //            }
    //            if (!c.HasItems)
    //            {
    //                c.Visibility = System.Windows.Visibility.Collapsed;
    //                c.IsEnabled = false;
    //            }
    //            return c;
    //        }
    //        return c;
    //    }

    //    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }

    //}
}
