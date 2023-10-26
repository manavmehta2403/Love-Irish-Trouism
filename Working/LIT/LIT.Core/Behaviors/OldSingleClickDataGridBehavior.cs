using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace LIT.Core.Behaviors
{
    public static class OldSingleClickDataGridBehavior
    {
        public static readonly DependencyProperty EnableSingleClickEditingProperty =
            DependencyProperty.RegisterAttached(
                "EnableSingleClickEditing",
                typeof(bool),
                typeof(OldSingleClickDataGridBehavior),
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
            DataGridCell cell = GetCellFromEvent(sender, e);

            if (cell != null && !cell.IsEditing)
            {
                DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
                DataGridRow row = FindVisualParent<DataGridRow>(cell);

                if (row != null)
                {
                    if (IsDoubleClick(e))
                    {
                        // Handle double-click action here
                    }
                    else if (e.OriginalSource is ButtonBase button)
                    {
                        // Select the entire row if not already selected
                        if (!row.IsSelected)
                        {
                            // Unselect all other rows
                            foreach (object item in dataGrid.Items)
                            {
                                DataGridRow otherRow = dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                                if (otherRow != null && otherRow != row)
                                {
                                    otherRow.IsSelected = false;
                                }
                            }
                            row.IsSelected = true;
                        }

                        // Handle button click here
                        // You can raise an event or handle it as needed
                        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, button));
                        e.Handled = true;
                    }
                    else if (e.OriginalSource is CheckBox checkBox)
                    {
                        // Automatically check the checkbox when it's clicked
                        checkBox.IsChecked = !checkBox.IsChecked;
                        e.Handled = true;
                    }
                    else if (e.OriginalSource is DatePicker datePicker)
                    {
                        if (!datePicker.IsDropDownOpen)
                        {
                            // Open the DatePicker calendar
                            datePicker.IsDropDownOpen = true;
                            e.Handled = true;
                        }
                    }
                    else if (e.OriginalSource is TextBlock textBlock)
                    {
                        // Check if the TextBlock is inside a Button
                        ButtonBase TextBoxbutton = FindVisualParent<ButtonBase>(textBlock);
                        if (TextBoxbutton != null)
                        {
                            // Select the entire row if not already selected
                            if (!row.IsSelected)
                            {
                                // Unselect all other rows
                                foreach (object item in dataGrid.Items)
                                {
                                    DataGridRow otherRow = dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                                    if (otherRow != null && otherRow != row)
                                    {
                                        otherRow.IsSelected = false;
                                    }
                                }
                                row.IsSelected = true;
                            }

                            // Handle button click here
                            // You can raise an event or handle it as needed
                            TextBoxbutton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, TextBoxbutton));
                            e.Handled = true;
                        }
                    }
                    else if (e.OriginalSource is System.Windows.Shapes.Rectangle rectangle)
                    {
                        // Check if the Rectangle is inside a CheckBox
                        CheckBox rectangleCheckBox = FindVisualParent<CheckBox>(rectangle);

                        if (rectangleCheckBox != null)
                        {
                            // Toggle the CheckBox state
                            rectangleCheckBox.IsChecked = !rectangleCheckBox.IsChecked;
                            e.Handled = true;
                        }
                    }
                    else if (e.OriginalSource is Border border)
                    {
                        // Check if the border is inside a CheckBox
                        CheckBox borderCheckBox = FindVisualParent<CheckBox>(border);

                        if (borderCheckBox != null)
                        {
                            // Toggle the CheckBox state
                            borderCheckBox.IsChecked = !borderCheckBox.IsChecked;
                            e.Handled = true;
                        }

                        // Check if the border is inside the DatePicker
                        DatePicker borderDatePicker = FindVisualParent<DatePicker>(border);
                        
                        if (borderDatePicker != null)
                        {
                            if (!borderDatePicker.IsDropDownOpen)
                            {
                                // Open the DatePicker calendar
                                borderDatePicker.IsDropDownOpen = true;
                                e.Handled = true;
                            }
                        }
                    }
                    else
                    {
                        // Handle other UI elements inside the cell as needed
                    }
                }
            }
        }



        private static DataGridCell GetCellFromEvent(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            return dep as DataGridCell;
        }

        private static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
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

        private static bool IsDoubleClick(MouseButtonEventArgs e)
        {
            return e.ClickCount >= 2;
        }

        private static void DataGrid_CellGotFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is DataGridCell cell)
            {
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);

                Control control = GetFirstChildByType<Control>(cell);
                if (control != null)
                {
                    control.Focus();
                }
            }
        }

        private static T GetFirstChildByType<T>(DependencyObject prop) where T : DependencyObject
        {
            if (prop == null)
            {
                return null;
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
