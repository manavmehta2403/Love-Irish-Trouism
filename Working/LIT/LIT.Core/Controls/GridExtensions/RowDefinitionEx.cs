using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;

namespace LIT.Core.Controls.GridExtensions
{
    public class RowDefinitionEx : RowDefinition
    {
        // Variables
        public static DependencyProperty VisibilityProperty;
        private readonly HashSet<UIElement> MonitoredChildren;

        // Properties
        public Visibility Visibility { get { return (Visibility)GetValue(VisibilityProperty); } set { SetValue(VisibilityProperty, value); } }

        // Constructors
        static RowDefinitionEx()
        {
            VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(RowDefinitionEx), new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));
        }
        public RowDefinitionEx()
        {
            MonitoredChildren = new HashSet<UIElement>();

            // Register the loaded event
            Loaded += OnRowDefinitionExperimentalLoaded;
        }

        void OnRowDefinitionExperimentalLoaded(Object sender, RoutedEventArgs e)
        {
            GridExperimental grid = GetUIParentCore() as GridExperimental;
            if (grid == null)
                return;

            // Need to pick up existing pre-loaded children
            FindMonitoredChildren(grid);

            // Register for further changes 
            grid.ChildrenChanged += OnGridExtendedChildrenChanged;
        }

        void OnGridExtendedChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            GridExperimental grid = GetUIParentCore() as GridExperimental;
            if (grid == null)
                return;

            UIElement va = visualAdded as UIElement;
            if (va != null)
                MonitorChild(grid, va);

            UIElement vr = visualAdded as UIElement;
            if (vr != null)
                MonitoredChildren.Remove(vr);
        }

        static void OnVisibilityChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            RowDefinitionEx rde = obj as RowDefinitionEx;
            if (rde != null)
                rde.OnVisibilityChanged((Visibility)e.NewValue);
        }

        void OnVisibilityChanged(Visibility nVisibility)
        {
            GridExperimental grid = GetUIParentCore() as GridExperimental;
            if (grid == null)
                return;

            ProcessMonitoredChildrenVisibility(grid, nVisibility);
        }

        void FindMonitoredChildren(GridExperimental nGrid)
        {
            int rowindex = nGrid.RowDefinitions.IndexOf(this);
            foreach (UIElement child in nGrid.Children)
            {
                if (Grid.GetRow(child) == rowindex)
                    MonitoredChildren.Add(child);
            }

            ProcessMonitoredChildrenVisibility(nGrid, Visibility);
        }

        void MonitorChild(GridExperimental nGrid, UIElement nChild)
        {
            int rowindex = nGrid.RowDefinitions.IndexOf(this);
            if (Grid.GetRow(nChild) == rowindex)
            {
                MonitoredChildren.Add(nChild);
                ProcessChildVisibility(nGrid, nChild, Visibility);
            }
        }

        void ProcessMonitoredChildrenVisibility(GridExperimental nGrid, Visibility nVisibility)
        {
            foreach (UIElement child in MonitoredChildren)
                ProcessVisibility(nGrid, child, nVisibility);
        }

        void ProcessChildVisibility(GridExperimental nGrid, UIElement nChild, Visibility nVisibility)
        {
            int rowindex = nGrid.RowDefinitions.IndexOf(this);
            if (Grid.GetRow(nChild) == rowindex)
                ProcessVisibility(nGrid, nChild, nVisibility);
        }

        void ProcessVisibility(GridExperimental nGrid, UIElement nChild, Visibility nVisibility)
        {
            int colindex = Grid.GetColumn(nChild);
            ColumnDefinitionEx cde = colindex < nGrid.ColumnDefinitions.Count ? nGrid.ColumnDefinitions[colindex] as ColumnDefinitionEx : null;
            nChild.Visibility = cde != null ? VisibilityPrecedence(nVisibility, cde.Visibility) : nVisibility;
        }

        Visibility VisibilityPrecedence(Visibility nA, Visibility nB)
        {
            return (Visibility)Math.Max((int)nA, (int)nB);
        }
    }
}
