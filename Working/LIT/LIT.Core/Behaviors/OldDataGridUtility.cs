using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

public static class OldDataGridUtility
{
    public static void FocusLastEditableCellAndEdit(DataGrid dataGrid)
    {
        if (dataGrid.Items.Count == 0)
            return;

        // Get the last item in the DataGrid
        var lastItem = dataGrid.Items[dataGrid.Items.Count - 1];

        // Select the last item (row)
        dataGrid.SelectedItem = lastItem;

        // Ensure the row is brought into view
        dataGrid.UpdateLayout();
        dataGrid.ScrollIntoView(lastItem);
        dataGrid.UpdateLayout();

        // Find the last row container
        DataGridRow lastRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(lastItem);
        if (lastRow == null)
            return;

        // Find the first editable cell in the selected row and focus on it
        foreach (DataGridColumn column in dataGrid.Columns)
        {
            if (column.Visibility == Visibility.Visible && !column.IsReadOnly)
            {
                DataGridCell cell = GetCell(lastRow, dataGrid);
                if (cell != null)
                {
                    cell.Focus();

                    // Programmatically start editing the cell
                    dataGrid.BeginEdit();
                    break;
                }
            }
        }
    }

    private static DataGridCell GetCell(DataGridRow row, DataGrid dataGrid)
    {
        if (row != null)
        {
            DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

            if (presenter == null)
            {
                row.ApplyTemplate();
                presenter = GetVisualChild<DataGridCellsPresenter>(row);
            }

            if (presenter != null)
            {
                foreach (DataGridColumn dataGridColumn in dataGrid.Columns)
                {
                    if (dataGridColumn.Visibility == Visibility.Visible && !dataGridColumn.IsReadOnly)
                    {
                        DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(dataGridColumn.DisplayIndex);

                        // Check if the cell is editable and visible
                        if (cell != null)
                        {
                            return cell;
                        }
                    }
                }
            }
        }

        return null;
    }


    private static T GetVisualChild<T>(Visual parent) where T : Visual
    {
        T child = default(T);

        int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < numVisuals; i++)
        {
            Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
            child = v as T;
            if (child == null)
            {
                child = GetVisualChild<T>(v);
            }
            if (child != null)
            {
                break;
            }
        }
        return child;
    }
}
