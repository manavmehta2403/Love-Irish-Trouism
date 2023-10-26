using System.Windows;
using System.Windows.Controls;

namespace LIT.Core.Controls
{
    public class ResponsivePanel : Panel
    {
        public static readonly DependencyProperty RequestedItemWidthProperty =
            DependencyProperty.RegisterAttached("RequestedItemWidth", typeof(double), typeof(ResponsivePanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        public double RequestedItemWidth
        {
            get { return (double)GetValue(RequestedItemWidthProperty); }
            set { SetValue(RequestedItemWidthProperty, value); }
        }
        public static readonly DependencyProperty RequestedItemHeightProperty =
            DependencyProperty.RegisterAttached("RequestedItemHeight", typeof(double), typeof(ResponsivePanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        public double RequestedItemHeight
        {
            get { return (double)GetValue(RequestedItemHeightProperty); }
            set { SetValue(RequestedItemHeightProperty, value); }
        }
        public static readonly DependencyProperty IsCenteredProperty =
            DependencyProperty.Register("IsCentered", typeof(bool), typeof(ResponsivePanel), new PropertyMetadata(false, OnIsCenteredChanged));

        public bool IsCentered
        {
            get { return (bool)GetValue(IsCenteredProperty); }
            set { SetValue(IsCenteredProperty, value); }
        }

        private static void OnIsCenteredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ResponsivePanel panel)
            {
                panel.InvalidateArrange();
            }
        }

        public static double GetRequestedItemWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(RequestedItemWidthProperty);
        }

        public static void SetRequestedItemWidth(DependencyObject obj, double value)
        {
            obj.SetValue(RequestedItemWidthProperty, value);
        }

        public static double GetRequestedItemHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(RequestedItemHeightProperty);
        }

        public static void SetRequestedItemHeight(DependencyObject obj, double value)
        {
            obj.SetValue(RequestedItemHeightProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if ((!double.IsNaN(RequestedItemWidth)) || (!double.IsNaN(RequestedItemHeight)))
            {
                double requestedWidth = RequestedItemWidth;
                double requestedHeight = RequestedItemHeight;
                double panelWidth = availableSize.Width;
                double panelHeight = availableSize.Height;

                if (panelWidth < requestedWidth)
                {
                    requestedWidth = panelWidth;
                }

                if (panelHeight < requestedHeight)
                {
                    requestedHeight = panelHeight;
                }

                foreach (UIElement child in InternalChildren)
                {
                    Thickness margin = (Thickness)child.GetValue(MarginProperty);

                    double width = requestedWidth - margin.Left - margin.Right;
                    double height = requestedHeight - margin.Top - margin.Bottom;
                    if (width < 0)
                    {
                        width = 0;
                    }
                    if (height < 0)
                    {
                        height = 0;
                    }

                    child.SetValue(WidthProperty, width);
                    child.SetValue(HeightProperty, height);

                }
                return base.MeasureOverride(availableSize);
            }

            else
            {
                foreach (UIElement child in InternalChildren)
                {
                    double requestedWidth = GetRequestedItemWidth(child);
                    double requestedHeight = GetRequestedItemHeight(child);

                    if (!double.IsNaN(requestedWidth) || !double.IsNaN(requestedHeight))
                    {
                        child.Measure(new Size(!double.IsNaN(requestedWidth) ? requestedWidth : availableSize.Width,
                                               !double.IsNaN(requestedHeight) ? requestedHeight : availableSize.Height));
                    }
                    else
                    {
                        child.Measure(availableSize);
                    }
                }

                return availableSize;
            }
            
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (IsCentered)
            {
                double accumulatedWidth = 0;
                double accumulatedHeight = 0;

                foreach (UIElement child in InternalChildren)
                {
                    double requestedWidth = GetRequestedItemWidth(child);
                    double requestedHeight = GetRequestedItemHeight(child);

                    double childWidth = !double.IsNaN(requestedWidth) ? requestedWidth : child.DesiredSize.Width;
                    double childHeight = !double.IsNaN(requestedHeight) ? requestedHeight : child.DesiredSize.Height;

                    double childX = (finalSize.Width - accumulatedWidth - childWidth) / 2;
                    double childY = (finalSize.Height - accumulatedHeight - childHeight) / 2;

                    child.Arrange(new Rect(childX, childY, childWidth, childHeight));

                    accumulatedWidth += childWidth;
                    accumulatedHeight += childHeight;
                }
            }
            else
            {
                double xOffset = 0;
                double yOffset = 0;

                foreach (UIElement child in InternalChildren)
                {
                    double requestedWidth = GetRequestedItemWidth(child);
                    double requestedHeight = GetRequestedItemHeight(child);

                    double childWidth = !double.IsNaN(requestedWidth) ? requestedWidth : child.DesiredSize.Width;
                    double childHeight = !double.IsNaN(requestedHeight) ? requestedHeight : child.DesiredSize.Height;

                    child.Arrange(new Rect(xOffset, yOffset, childWidth, childHeight));

                    if (xOffset + childWidth > finalSize.Width)
                    {
                        xOffset = 0;
                        yOffset += childHeight;
                    }
                    else
                    {
                        xOffset += childWidth;
                    }
                }
            }

            return finalSize;
        }
    }
}
