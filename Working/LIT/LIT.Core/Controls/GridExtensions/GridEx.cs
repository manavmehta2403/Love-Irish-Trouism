﻿/* OPTIMISE THE CODE AND REMOVE THIS COMMENT */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LIT.Core.Controls.GridExtensions
{
    using LayoutUpdateEventHandler = EventHandler;

    public static class GridEx
    {
        public static Orientation GetAutoFillOrientation(DependencyObject obj)
        {
            return (Orientation)obj.GetValue(AutoFillOrientationProperty);
        }
        public static void SetAutoFillOrientation(DependencyObject obj, Orientation value)
        {
            obj.SetValue(AutoFillOrientationProperty, value);
        }
        // Using a DependencyProperty as the backing store for AutoFillOrientation. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoFillOrientationProperty =
            DependencyProperty.RegisterAttached("AutoFillOrientation", typeof(Orientation), typeof(GridEx), new PropertyMetadata(Orientation.Horizontal));

        public static bool GetAutoFillChildren(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoFillChildrenProperty);
        }
        public static void SetAutoFillChildren(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoFillChildrenProperty, value);
        }
        // Using a DependencyProperty as the backing store for AutoFillChildren. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoFillChildrenProperty =
            DependencyProperty.RegisterAttached("AutoFillChildren", typeof(bool), typeof(GridEx), new PropertyMetadata(false, OnAutoFillChildrenChanged));

        private static void OnAutoFillChildrenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var isEnabled = (bool)e.NewValue;
            if (grid == null) { return; }

            if (isEnabled)
            {
                var layoutUpdateCallback = CreateLayoutUpdateHandler(grid);
                // event registration
                grid.LayoutUpdated += layoutUpdateCallback;
                SetLayoutUpdatedCallback(grid, layoutUpdateCallback);

                // Perform Auto Fill processing
                AutoFill(grid);
            }
            else
            {
                // clear the event
                var callback = GetLayoutUpdatedCallback(grid);
                grid.LayoutUpdated -= callback;

                // Reset Auto Fill processing
                ClearAutoFill(grid);
            }
        }

        private static LayoutUpdateEventHandler CreateLayoutUpdateHandler(Grid grid)
        {
            var prevCount = 0;
            var prevColumn = grid.ColumnDefinitions.Count;
            var prevRow = grid.RowDefinitions.Count;
            var prevOrientation = GetAutoFillOrientation(grid);

            var layoutUpdateCallback = new LayoutUpdateEventHandler((sender, args) =>
            {
                var count = grid.Children.Count;
                var column = grid.ColumnDefinitions.Count;
                var row = grid.RowDefinitions.Count;
                var orientation = GetAutoFillOrientation(grid);

                if (count != prevCount ||
                    column != prevColumn ||
                    row != prevRow ||
                    orientation != prevOrientation)
                {
                    AutoFill(grid);
                    prevCount = count;
                    prevColumn = column;
                    prevRow = row;
                    prevOrientation = orientation;
                }
            });

            return layoutUpdateCallback;
        }

        public static LayoutUpdateEventHandler GetLayoutUpdatedCallback(DependencyObject obj)
        {
            return (LayoutUpdateEventHandler)obj.GetValue(LayoutUpdatedCallbackProperty);
        }
        private static void SetLayoutUpdatedCallback(DependencyObject obj, LayoutUpdateEventHandler value)
        {
            obj.SetValue(LayoutUpdatedCallbackProperty, value);
        }
        // Using a DependencyProperty as the backing store for LayoutUpdatedCallback.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LayoutUpdatedCallbackProperty =
            DependencyProperty.RegisterAttached("LayoutUpdatedCallback", typeof(LayoutUpdateEventHandler), typeof(GridEx), new PropertyMetadata(null));

        private static void AutoFill(Grid grid)
        {
            var isEnabled = GetAutoFillChildren(grid);
            var rowCount = grid.RowDefinitions.Count;
            var columnCount = grid.ColumnDefinitions.Count;
            var orientation = GetAutoFillOrientation(grid);

            if (!isEnabled || rowCount == 0 || columnCount == 0) return;

            var area = new bool[rowCount, columnCount];

            var autoLayoutList = new List<FrameworkElement>();
            // Check for position fixed elements in Grid
            foreach (FrameworkElement child in grid.Children)
            {
                // Set the grid position according to Area Name ⇒ Area priority
                var region = GetAreaNameRegion(child) ?? GetAreaRegion(child);
                var isFixed = region != null;

                if (isFixed)
                {
                    // Since the position is specified, record in the Auto Fill Reserved Area
                    var row = region.Row;
                    var column = region.Column;
                    var rowSpan = region.RowSpan;
                    var columnSpan = region.ColumnSpan;

                    for (var i = row; i < row + rowSpan; i++)
                        for (var j = column; j < column + columnSpan; j++)
                        {
                            if (columnCount <= j || rowCount <= i) { continue; }
                            area[i, j] = true;
                        }
                }
                else
                {
                    // Grid's unpositioned elements are added to the list for automatic layout
                    autoLayoutList.Add(child);
                }

            }

            var count = 0;
            var numOfCell = rowCount * columnCount;
            var isHorizontal = orientation == Orientation.Horizontal;
            var isOverflow = false;
            // Arrange child elements of Grid in order in Grid
            foreach (FrameworkElement child in autoLayoutList)
            {
                // Exclude Visibility.Collapsed items
                if (child.Visibility == Visibility.Collapsed)
                {
                    continue;
                }

                while (true)
                {
                    var x = isHorizontal ? count % columnCount : count / rowCount;
                    var y = isHorizontal ? count / columnCount : count % rowCount;
                    var canArrange = isOverflow ? true : !area[y, x];
                    if (canArrange)
                    {
                        Grid.SetRow(child, y);
                        Grid.SetColumn(child, x);
                        Grid.SetRowSpan(child, 1);
                        Grid.SetColumnSpan(child, 1);
                    }

                    if (count + 1 < numOfCell)
                    {
                        count++;
                    }
                    else
                    {
                        isOverflow = true;
                    }

                    if (canArrange)
                    {
                        break;
                    }
                }

            }
        }

        private static void ClearAutoFill(Grid grid)
        {
            foreach (FrameworkElement child in grid.Children)
            {
                child.ClearValue(Grid.RowProperty);
                child.ClearValue(Grid.ColumnProperty);
                child.ClearValue(Grid.RowSpanProperty);
                child.ClearValue(Grid.ColumnSpanProperty);

                UpdateItemPosition(child);
            }
        }


        public static string GetColumnDefinition(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnDefinitionProperty);
        }
        public static void SetColumnDefinition(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnDefinitionProperty, value);
        }
        // Using a DependencyProperty as the backing store for ColumnDefinition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnDefinitionProperty =
            DependencyProperty.RegisterAttached("ColumnDefinition", typeof(string), typeof(GridEx), new PropertyMetadata(null, OnColumnDefinitionChanged));

        private static void OnColumnDefinitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var param = e.NewValue as string;

            InitializeColumnDefinition(grid, param);

            var template = GetTemplateArea(grid);
            if (template != null)
            {
                InitializeTemplateArea(grid, template);
            }
        }

        private static void InitializeColumnDefinition(Grid grid, string param)
        {
            if (grid == null || param == null)
            {
                return;
            }

            grid.ColumnDefinitions.Clear();

            var list = param.Split(',')
                            .Select(o => o.Trim());

            foreach (var item in list)
            {
                var def = StringToGridLengthDefinition(item);
                var columnDefinition = new ColumnDefinition() { Width = def.GridLength };
                if (def.Max != null) { columnDefinition.MaxWidth = def.Max.Value; }
                if (def.Min != null) { columnDefinition.MinWidth = def.Min.Value; }
                grid.ColumnDefinitions.Add(columnDefinition);
            }
        }

        public static string GetRowDefinition(DependencyObject obj)
        {
            return (string)obj.GetValue(RowDefinitionProperty);
        }
        public static void SetRowDefinition(DependencyObject obj, string value)
        {
            obj.SetValue(RowDefinitionProperty, value);
        }
        // Using a DependencyProperty as the backing store for RowDefinition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowDefinitionProperty =
            DependencyProperty.RegisterAttached("RowDefinition", typeof(string), typeof(GridEx), new PropertyMetadata(null, OnRowDefinitionChanged));

        private static void OnRowDefinitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var param = e.NewValue as string;

            InitializeRowDefinition(grid, param);

            var template = GetTemplateArea(grid);
            if (template != null)
            {
                InitializeTemplateArea(grid, template);
            }
        }

        private static void InitializeRowDefinition(Grid grid, string param)
        {
            if (grid == null || param == null)
            {
                return;
            }

            grid.RowDefinitions.Clear();

            var list = param.Split(',')
                            .Select(o => o.Trim());

            foreach (var item in list)
            {
                var def = StringToGridLengthDefinition(item);
                var rowDefinition = new RowDefinition() { Height = def.GridLength };
                if (def.Max != null) { rowDefinition.MaxHeight = def.Max.Value; }
                if (def.Min != null) { rowDefinition.MinHeight = def.Min.Value; }
                grid.RowDefinitions.Add(rowDefinition);
            }
        }

        // Private attached property used only inside ↓Grid Ex
        public static IList<NamedAreaDefinition> GetAreaDefinitions(DependencyObject obj)
        {
            return (IList<NamedAreaDefinition>)obj.GetValue(AreaDefinitionsProperty);
        }
        private static void SetAreaDefinitions(DependencyObject obj, IList<NamedAreaDefinition> value)
        {
            obj.SetValue(AreaDefinitionsProperty, value);
        }
        // Using a DependencyProperty as the backing store for AreaDefinitions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AreaDefinitionsProperty =
            DependencyProperty.RegisterAttached("AreaDefinitions", typeof(IList<NamedAreaDefinition>), typeof(GridEx), new PropertyMetadata(null));



        public static string GetTemplateArea(DependencyObject obj)
        {
            return (string)obj.GetValue(TemplateAreaProperty);
        }
        public static void SetTemplateArea(DependencyObject obj, string value)
        {
            obj.SetValue(TemplateAreaProperty, value);
        }
        // Using a DependencyProperty as the backing store for TemplateArea.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemplateAreaProperty =
            DependencyProperty.RegisterAttached("TemplateArea", typeof(string), typeof(GridEx), new PropertyMetadata(null, OnTemplateAreaChanged));

        private static void OnTemplateAreaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            var param = e.NewValue as string;

            if (d == null)
            {
                return;
            }

            // Initialize the grid once
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            // Initialize rows/columns with Grid Ex.Row Definition/Grid Ex.Column Definition settings
            InitializeRowDefinition(grid, GetRowDefinition(grid));
            InitializeColumnDefinition(grid, GetColumnDefinition(grid));

            if (param != null)
            {
                InitializeTemplateArea(grid, param);
            }
        }

        private static void InitializeTemplateArea(Grid grid, string param)
        {
            // Check number of rows x columns
            // Empty lines and spaces are removed and transformed into a row x column data structure
            var columns = param.Split(new[] { '\n', '/' })
                               .Select(o => o.Trim())
                               .Where(o => !string.IsNullOrWhiteSpace(o))
                               .Select(o => o.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            // Check number of rows x columns
            var num = columns.FirstOrDefault().Count();
            var isValidRowColumn = columns.All(o => o.Count() == num);
            if (!isValidRowColumn)
            {
                // Invalid Row Columns...
                throw new ArgumentException("Invalid Row/Column definition.");
            }

            // adjust the number of grids (add missing rows/columns)
            var rowShortage = columns.Count() - grid.RowDefinitions.Count;
            for (var i = 0; i < rowShortage; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            var columnShortage = num - grid.ColumnDefinitions.Count;
            for (var i = 0; i < columnShortage; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Parse the Area definition
            var areaList = ParseAreaDefinition(columns);
            SetAreaDefinitions(grid, areaList);

            // Since the definition of the overall layout has changed,
            // Re-reflect all Region settings of child elements of Grid
            foreach (FrameworkElement child in grid.Children)
            {
                UpdateItemPosition(child);
            }
        }


        private static IList<NamedAreaDefinition> ParseAreaDefinition(IEnumerable<string[]> columns)
        {
            var result = new List<NamedAreaDefinition>();

            // Check if the Regions are correctly concatenated
            var flatten = columns.SelectMany(
                    (item, index) => item.Select((o, xIndex) => new { row = index, column = xIndex, name = o })
                );

            var groups = flatten.GroupBy(o => o.name);
            foreach (var group in groups)
            {
                var left = group.Min(o => o.column);
                var top = group.Min(o => o.row);
                var right = group.Max(o => o.column);
                var bottom = group.Max(o => o.row);

                var isValid = true;
                for (var y = top; y <= bottom; y++)
                    for (var x = left; x <= right; x++)
                    {
                        isValid = isValid && group.Any(o => o.column == x && o.row == y);
                    }

                if (!isValid)
                {
                    throw new ArgumentException($"\"{group.Key}\" is invalid area definition.");
                }

                result.Add(new NamedAreaDefinition(group.Key, top, left, bottom - top + 1, right - left + 1));
            }

            return result;
        }

        private static GridLengthDefinition StringToGridLengthDefinition(string source)
        {
            var r = new System.Text.RegularExpressions.Regex(@"(^[^\(\)]+)(?:\((.*)-(.*)\))?");
            var m = r.Match(source);

            var length = m.Groups[1].Value;
            var min = m.Groups[2].Value;
            var max = m.Groups[3].Value;

            double temp;
            var result = new GridLengthDefinition()
            {
                GridLength = StringToGridLength(length),
                Min = double.TryParse(min, out temp) ? temp : null,
                Max = double.TryParse(max, out temp) ? temp : null
            };

            return result;
        }
        private static GridLength StringToGridLength(string source)
        {
            var glc = TypeDescriptor.GetConverter(typeof(GridLength));
            return (GridLength)glc.ConvertFromString(source);
        }


        //================================================ =====================
        // Attached properties to apply to child elements in Grid
        //================================================ =====================
        public static string GetAreaName(DependencyObject obj)
        {
            return (string)obj.GetValue(AreaNameProperty);
        }
        public static void SetAreaName(DependencyObject obj, string value)
        {
            obj.SetValue(AreaNameProperty, value);
        }
        // Using a DependencyProperty as the backing store for AreaName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AreaNameProperty =
            DependencyProperty.RegisterAttached("AreaName", typeof(string), typeof(GridEx), new PropertyMetadata(null, OnAreaNameChanged));

        private static void OnAreaNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FrameworkElement;

            if (ctrl == null)
            {
                return;
            }

            UpdateItemPosition(ctrl);

            // Recalculate Auto Fill for all child elements
            var grid = ctrl.Parent as Grid;
            var isAutoFill = GetAutoFillChildren(grid);
            if (isAutoFill)
            {
                AutoFill(grid);
            }
        }

        public static string GetArea(DependencyObject obj)
        {
            return (string)obj.GetValue(AreaProperty);
        }
        public static void SetArea(DependencyObject obj, string value)
        {
            obj.SetValue(AreaProperty, value);
        }
        // Using a DependencyProperty as the backing store for Area.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AreaProperty =
            DependencyProperty.RegisterAttached("Area", typeof(string), typeof(GridEx), new PropertyMetadata(null, OnAreaChanged));

        private static void OnAreaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as FrameworkElement;

            if (d == null)
            {
                return;
            }

            UpdateItemPosition(ctrl);

            // Recalculate Auto Fill for all child elements
            var grid = ctrl.Parent as Grid;
            if (grid == null)
            {
                return;
            }

            var isAutoFill = GetAutoFillChildren(grid);
            if (isAutoFill)
            {
                AutoFill(grid);
            }
        }


        private static void UpdateItemPosition(FrameworkElement element)
        {
            // Set the grid position according to Area Name ⇒ Area priority
            var area = GetAreaNameRegion(element) ?? GetAreaRegion(element);
            if (area != null)
            {
                Grid.SetRow(element, area.Row);
                Grid.SetColumn(element, area.Column);
                Grid.SetRowSpan(element, area.RowSpan);
                Grid.SetColumnSpan(element, area.ColumnSpan);
            }
        }


        private static AreaDefinition GetAreaNameRegion(FrameworkElement element)
        {
            var name = GetAreaName(element);
            var grid = element.Parent as Grid;
            if (grid == null || name == null) { return null; }
            var areaList = GetAreaDefinitions(grid);
            if (areaList == null) { return null; }

            var area = areaList.FirstOrDefault(o => o.Name == name);
            if (area == null) { return null; }

            return new AreaDefinition(area.Row, area.Column, area.RowSpan, area.ColumnSpan);
        }

        private static AreaDefinition GetAreaRegion(FrameworkElement element)
        {
            var param = GetArea(element);
            if (param == null) { return null; }

            var list = param.Split(',')
                .Select(o => o.Trim())
                .Select(o => int.Parse(o))
                .ToList();

            // Row, Column, RowSpan, ColumnSpan
            if (list.Count() != 4) { return null; }

            return new AreaDefinition(list[0], list[1], list[2], list[3]);
        }
    }

    public class GridExperimental : Grid
    {
        // Delegates
        public delegate void ChildrenEventHandler(DependencyObject visualAdded, DependencyObject visualRemoved);

        // Events
        public event ChildrenEventHandler ChildrenChanged;

        // Callbacks
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            if (ChildrenChanged != null)
                ChildrenChanged(visualAdded, visualRemoved);
        }
    }

    struct GridLengthDefinition
    {
        public GridLength GridLength;
        public double? Min;
        public double? Max;
    }

    public class AreaDefinition
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }

        public AreaDefinition(int row, int column, int rowSpan, int columnSpan)
        {
            Row = row;
            Column = column;
            RowSpan = rowSpan;
            ColumnSpan = columnSpan;
        }
    }

    public class NamedAreaDefinition : AreaDefinition
    {
        public string Name { get; set; }

        public NamedAreaDefinition(string name, int row, int column, int rowSpan, int columnSpan)
            : base(row, column, rowSpan, columnSpan)
        {
            Name = name;
        }
    }
}
