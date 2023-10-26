using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;

namespace LIT.Core.Controls.GridExtensions
{
    public class ColumnDefinitionEx : ColumnDefinition
    {
        // Variables
        public static DependencyProperty VisibilityProperty;
        private readonly HashSet<UIElement> MonitoredChildren;

        // Properties
        public Visibility Visibility { get { return (Visibility)GetValue(VisibilityProperty); } set { SetValue(VisibilityProperty, value); } }

        // Constructors
        static ColumnDefinitionEx()
        {
            VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(ColumnDefinitionEx), new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));
        }
        public ColumnDefinitionEx()
        {
            MonitoredChildren = new HashSet<UIElement>();

            // Register the loaded event
            Loaded += OnColumnDefinitionExperimentalLoaded;
        }

        void OnColumnDefinitionExperimentalLoaded(object sender, RoutedEventArgs e)
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
            ColumnDefinitionEx cde = obj as ColumnDefinitionEx;
            if (cde != null)
                cde.OnVisibilityChanged((Visibility)e.NewValue);
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
            int colindex = nGrid.ColumnDefinitions.IndexOf(this);
            foreach (UIElement child in nGrid.Children)
            {
                if (Grid.GetColumn(child) == colindex)
                    MonitoredChildren.Add(child);
            }

            ProcessMonitoredChildrenVisibility(nGrid, Visibility);
        }

        void MonitorChild(GridExperimental nGrid, UIElement nChild)
        {
            int colindex = nGrid.ColumnDefinitions.IndexOf(this);
            if (Grid.GetColumn(nChild) == colindex)
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
            int colindex = nGrid.ColumnDefinitions.IndexOf(this);
            if (Grid.GetColumn(nChild) == colindex)
                ProcessVisibility(nGrid, nChild, nVisibility);
        }

        void ProcessVisibility(GridExperimental nGrid, UIElement nChild, Visibility nVisibility)
        {
            int rowindex = Grid.GetRow(nChild);
            RowDefinitionEx rde = rowindex < nGrid.RowDefinitions.Count ? nGrid.RowDefinitions[rowindex] as RowDefinitionEx : null;
            nChild.Visibility = rde != null ? VisibilityPrecedence(nVisibility, rde.Visibility) : nVisibility;
        }

        Visibility VisibilityPrecedence(Visibility nA, Visibility nB)
        {
            return (Visibility)Math.Max((int)nA, (int)nB);
        }
    }
}