using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LIT.Core.Behaviors
{
    public static class SingleClickDataGridBehavior
    {
        public static readonly DependencyProperty EnableSingleClickEditingProperty =
            DependencyProperty.RegisterAttached(
                "EnableSingleClickEditing",
                typeof(bool),
                typeof(SingleClickDataGridBehavior),
                new PropertyMetadata(false, OnEnableSingleClickEditingChanged));

        public static bool GetEnableSingleClickEditing(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableSingleClickEditingProperty);
        }

        public static void SetEnableSingleClickEditing(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableSingleClickEditingProperty, value);
        }

        private static void OnEnableSingleClickEditingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                if ((bool)e.NewValue)
                {
                    dataGrid.PreviewMouseLeftButtonDown += DataGridCell_PreviewMouseLeftButtonDown;
                    dataGrid.GotFocus += DataGrid_CellGotFocus;
                }
                else
                {
                    dataGrid.PreviewMouseLeftButtonDown -= DataGridCell_PreviewMouseLeftButtonDown;
                    dataGrid.GotFocus -= DataGrid_CellGotFocus;
                }
            }
        }

        private static void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep is DataGridCell cell && !cell.IsEditing)
            {
                if (cell.IsFocused)
                {
                    if (cell.Content is TextBlock textBlock)
                    {
                        DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
                        if (dataGrid != null)
                        {
                            dataGrid.BeginEdit();
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private static void DataGrid_CellGotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);

                Control control = GetFirstChildByType<Control>(e.OriginalSource as DataGridCell);
                if (control != null)
                {
                    control.Focus();
                }
            }
        }

        private static T GetFirstChildByType<T>(DependencyObject prop) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(prop); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild((prop), i) as DependencyObject;
                if (child == null)
                    continue;

                T castedProp = child as T;
                if (castedProp != null)
                    return castedProp;

                castedProp = GetFirstChildByType<T>(child);

                if (castedProp != null)
                    return castedProp;
            }
            return null;
        }

        private static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            // Helper method to find the parent DataGrid of a visual element
            UIElement parent = element;
            while (parent != null)
            {
                if (parent is T correctlyTyped)
                {
                    return correctlyTyped;
                }
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }
    }
}
