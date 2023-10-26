using System.Linq;
using System.Windows;

namespace LIT.Core.Behaviors
{
    public class ResponsivenessBehavior
    {
        public static readonly DependencyProperty IsResponsiveProperty =
            DependencyProperty.RegisterAttached("IsResponsive", typeof(bool), typeof(ResponsivenessBehavior),
                new PropertyMetadata(false, OnIsResponsiveChanged));

        public static bool GetIsResponsive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsResponsiveProperty);
        }

        public static void SetIsResponsive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsResponsiveProperty, value);
        }

        public static readonly DependencyProperty HorizontalBreakpointProperty =
            DependencyProperty.RegisterAttached("HorizontalBreakpoint", typeof(double), typeof(ResponsivenessBehavior),
                new PropertyMetadata(double.MaxValue));

        public static double GetHorizontalBreakpoint(DependencyObject obj)
        {
            return (double)obj.GetValue(HorizontalBreakpointProperty);
        }

        public static void SetHorizontalBreakpoint(DependencyObject obj, double value)
        {
            obj.SetValue(HorizontalBreakpointProperty, value);
        }

        public static readonly DependencyProperty VerticalBreakpointProperty =
            DependencyProperty.RegisterAttached("VerticalBreakpoint", typeof(double), typeof(ResponsivenessBehavior),
                new PropertyMetadata(double.MaxValue));

        public static double GetVerticalBreakpoint(DependencyObject obj)
        {
            return (double)obj.GetValue(VerticalBreakpointProperty);
        }

        public static void SetVerticalBreakpoint(DependencyObject obj, double value)
        {
            obj.SetValue(VerticalBreakpointProperty, value);
        }

        public static readonly DependencyProperty HorizontalBreakpointSettersProperty =
            DependencyProperty.RegisterAttached("HorizontalBreakpointSetters", typeof(SetterBaseCollection), typeof(ResponsivenessBehavior),
                new PropertyMetadata(new SetterBaseCollection()));

        public static SetterBaseCollection GetHorizontalBreakpointSetters(DependencyObject obj)
        {
            return (SetterBaseCollection)obj.GetValue(HorizontalBreakpointSettersProperty);
        }

        public static void SetHorizontalBreakpointSetters(DependencyObject obj, SetterBaseCollection value)
        {
            obj.SetValue(HorizontalBreakpointSettersProperty, value);
        }

        public static readonly DependencyProperty VerticalBreakpointSettersProperty =
            DependencyProperty.RegisterAttached("VerticalBreakpointSetters", typeof(SetterBaseCollection), typeof(ResponsivenessBehavior),
                new PropertyMetadata(new SetterBaseCollection()));

        public static SetterBaseCollection GetVerticalBreakpointSetters(DependencyObject obj)
        {
            return (SetterBaseCollection)obj.GetValue(VerticalBreakpointSettersProperty);
        }

        public static void SetVerticalBreakpointSetters(DependencyObject obj, SetterBaseCollection value)
        {
            obj.SetValue(VerticalBreakpointSettersProperty, value);
        }

        public static readonly DependencyProperty IsHorizontalBreakpointSettersActiveProperty =
            DependencyProperty.RegisterAttached("IsHorizontalBreakpointSettersActive", typeof(bool), typeof(ResponsivenessBehavior),
                new PropertyMetadata(false));

        public static bool GetIsHorizontalBreakpointSettersActive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsHorizontalBreakpointSettersActiveProperty);
        }

        public static void SetIsHorizontalBreakpointSettersActive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHorizontalBreakpointSettersActiveProperty, value);
        }

        public static readonly DependencyProperty IsVerticalBreakpointSettersActiveProperty =
            DependencyProperty.RegisterAttached("IsVerticalBreakpointSettersActive", typeof(bool), typeof(ResponsivenessBehavior),
                               new PropertyMetadata(false));
        public static bool GetIsVerticalBreakpointSettersActive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsVerticalBreakpointSettersActiveProperty);
        }

        public static void SetIsVerticalBreakpointSettersActive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsVerticalBreakpointSettersActiveProperty, value);
        }

        private static void OnIsResponsiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                Window window = Application.Current.MainWindow;

                if (GetIsResponsive(element))
                {
                    window.SizeChanged += (s, e) => UpdateElement(window, element);
                    UpdateElement(window, element); // Initial update
                }
                else
                {
                    window.SizeChanged -= (s, e) => UpdateElement(window, element);
                }
            }
        }

        private static void UpdateElement(Window window, FrameworkElement element)
        {
            double windowWidth = window.Width;
            double windowHeight = window.Height;

            double horizontalBreakpoint = GetHorizontalBreakpoint(element);
            double verticalBreakpoint = GetVerticalBreakpoint(element);

            if (windowWidth >= horizontalBreakpoint)
            {
                ApplySetters(element, GetHorizontalBreakpointSetters(element));
            }
            else if (windowWidth >= horizontalBreakpoint && !GetIsHorizontalBreakpointSettersActive(element))
            {
                SetIsHorizontalBreakpointSettersActive(element, true);
                element.Style = CreateResponsivenessStyle(element);
            }
            else if (windowWidth < horizontalBreakpoint && GetIsHorizontalBreakpointSettersActive(element))
            {
                SetIsHorizontalBreakpointSettersActive(element, false);
                element.Style = element.Style.BasedOn;
            }
            else
            {
                // If the horizontal breakpoint is not met, reset to original style
                element.Style = null;
            }


            if (windowHeight >= verticalBreakpoint)
            {
                ApplySetters(element, GetVerticalBreakpointSetters(element));
            }
            else if (windowWidth >= verticalBreakpoint && !GetIsVerticalBreakpointSettersActive(element))
            {
                SetIsVerticalBreakpointSettersActive(element, true);
                element.Style = CreateResponsivenessStyle(element);
            }
            else if (windowWidth < verticalBreakpoint && GetIsVerticalBreakpointSettersActive(element))
            {
                SetIsVerticalBreakpointSettersActive(element, false);
                element.Style = element.Style.BasedOn;
            }
            else
            {
                // If the vertical breakpoint is not met, reset to original style
                element.Style = null;
            }
        }

        private static void ApplySetters(FrameworkElement element, SetterBaseCollection setters)
        {
            if (setters != null && setters.Count > 0)
            {
                Style responsivenessStyle = new Style(element.GetType(), element.Style);

                foreach (SetterBase setterBase in setters)
                {
                    if (setterBase is Setter setter)
                    {
                        responsivenessStyle.Setters.Add(setter);
                    }
                }

                element.Style = responsivenessStyle;
            }
        }

        private static Style CreateResponsivenessStyle(FrameworkElement element)
        {
            Style responsivenessStyle = new Style(element.GetType(), element.Style);

            foreach (Setter setter in GetHorizontalBreakpointSetters(element).Concat(GetVerticalBreakpointSetters(element)))
            {
                responsivenessStyle.Setters.Add(setter);
            }

            return responsivenessStyle;
        }
    }
}
