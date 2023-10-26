using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LIT.Core.Behaviors
{
    public static class NewDataGridAutoFocusBehavior
    {
        public static bool GetAutoFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoFocusProperty);
        }

        public static void SetAutoFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoFocusProperty, value);
        }

        public static readonly DependencyProperty AutoFocusProperty =
            DependencyProperty.RegisterAttached(
                "AutoFocus",
                typeof(bool),
                typeof(NewDataGridAutoFocusBehavior),
                new PropertyMetadata(false, OnAutoFocusPropertyChanged));

        private static void OnAutoFocusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                dataGrid.Loaded += (sender, args) =>
                {
                    if (GetAutoFocus(dataGrid))
                    {
                        dataGrid.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
                            dataGrid.Focus();
                            dataGrid.CurrentCell = new DataGridCellInfo(
                                dataGrid.Items[dataGrid.Items.Count - 1],
                                dataGrid.Columns[0]);
                            dataGrid.BeginEdit();
                        }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                    }
                };
            }
        }
    }

}
