using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace LIT.Core.Behaviors
{
    public static class NewSingleClickDataGridBehavior
    {
        public static readonly DependencyProperty EnableSingleClickEditingProperty =
            DependencyProperty.RegisterAttached(
                "EnableSingleClickEditing",
                typeof(bool),
                typeof(NewSingleClickDataGridBehavior),
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
                            if (IsDoubleClick(e))
                            {
                                // Handle double-click action here
                            }
                            else
                            {
                                // Close any open DatePicker and ComboBox popups
                                ClosePopups(dataGrid);

                                // Check if the original source is a DatePicker
                                if (e.OriginalSource is DatePicker datePicker)
                                {
                                    if (!datePicker.IsDropDownOpen)
                                    {
                                        // Open the DatePicker calendar
                                        datePicker.IsDropDownOpen = true;
                                        e.Handled = true;
                                    }
                                    else
                                    {
                                        // DatePicker is already open, handle date selection
                                        datePicker.SelectedDateChanged += (s, args) =>
                                        {
                                            // Handle the selected date here
                                            datePicker.IsDropDownOpen = false; // Close the DatePicker
                                            e.Handled = true;
                                        };
                                    }
                                }
                                else if (e.OriginalSource is ComboBox comboBox)
                                {
                                    if (!comboBox.IsDropDownOpen)
                                    {
                                        // Open the ComboBox drop-down
                                        comboBox.IsDropDownOpen = true;
                                        e.Handled = true;
                                    }
                                    else
                                    {
                                        // ComboBox is already open, handle selection
                                        comboBox.SelectionChanged += (s, args) =>
                                        {
                                            // Handle the selected item here
                                            comboBox.IsDropDownOpen = false; // Close the ComboBox
                                            e.Handled = true;
                                        };
                                    }
                                }
                                else if (e.OriginalSource is ButtonBase button)
                                {
                                    if (IsDoubleClick(e))
                                    {
                                        // Handle double-click action here
                                    }
                                    else
                                    {
                                        // Handle single-click action for buttons here
                                        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, button));
                                        e.Handled = true;
                                    }
                                }
                                else
                                {
                                    // Handle other UI elements inside the cell as needed
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void ClosePopups(DataGrid dataGrid)
        {
            foreach (var cell in FindVisualChildren<DataGridCell>(dataGrid))
            {
                if (cell.Content is TextBlock textBlock)
                {
                    if (VisualTreeHelper.GetChildrenCount(textBlock) > 0)
                    {
                        var child = VisualTreeHelper.GetChild(textBlock, 0);
                        if (child is FrameworkElement frameworkElement && frameworkElement is IInputElement inputElement)
                        {
                            if (inputElement is DatePicker datePicker && datePicker.IsDropDownOpen)
                            {
                                datePicker.IsDropDownOpen = false; // Close the DatePicker
                            }
                            else if (inputElement is ComboBox comboBox && comboBox.IsDropDownOpen)
                            {
                                comboBox.IsDropDownOpen = false; // Close the ComboBox
                            }
                        }
                    }
                }
            }
        }


        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
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

        // Helper method to detect a double click
        private static bool IsDoubleClick(MouseButtonEventArgs e)
        {
            return e.ClickCount >= 2;
        }

        private static void DataGrid_CellGotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource is DataGridCell cell)
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                if (grd.Items.Count > 0)
                {
                    grd.BeginEdit(e);
                }

                Control control = GetFirstChildByType<Control>(cell);
                if (control != null)
                {
                    control.Focus();
                }
            }
            else if (e.OriginalSource is ButtonBase || e.OriginalSource is CheckBox || e.OriginalSource is DatePicker)
            {
                // Handle the case where the original source is a button, checkbox, or datepicker
                // You can add custom handling for these cases if needed
            }
        }

        private static T GetFirstChildByType<T>(DependencyObject prop) where T : DependencyObject
        {
            if (prop == null)
            {
                return null; // Add a null check here to avoid null reference exceptions
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(prop); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(prop, i) as DependencyObject;
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
    }
}
