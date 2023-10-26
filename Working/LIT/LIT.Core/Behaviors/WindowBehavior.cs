using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;

namespace LIT.Core.Behaviors
{
    public class WindowBehavior
    {
        private static readonly Type OwnerType = typeof(WindowBehavior);

        #region HideCloseButton (attached property)

        public static readonly DependencyProperty HideCloseButtonProperty =
            DependencyProperty.RegisterAttached(
                "HideCloseButton",
                typeof(bool),
                OwnerType,
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(HideCloseButtonChangedCallback)));

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHideCloseButton(Window obj)
        {
            return (bool)obj.GetValue(HideCloseButtonProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHideCloseButton(Window obj, bool value)
        {
            obj.SetValue(HideCloseButtonProperty, value);
        }

        private static void HideCloseButtonChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window == null) return;

            var hideCloseButton = (bool)e.NewValue;
            if (hideCloseButton && !GetIsHiddenCloseButton(window))
            {
                if (!window.IsLoaded)
                {
                    window.Loaded += HideWhenLoadedDelegate;
                }
                else
                {
                    HideCloseButton(window);
                }
                SetIsHiddenCloseButton(window, true);
            }
            else if (!hideCloseButton && GetIsHiddenCloseButton(window))
            {
                if (!window.IsLoaded)
                {
                    window.Loaded -= ShowWhenLoadedDelegate;
                }
                else
                {
                    ShowCloseButton(window);
                }
                SetIsHiddenCloseButton(window, false);
            }
        }

        #region Win32 imports

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        private const int WS_MINIMIZEBOX = 0x20000; // Add this line for minimizing button
        private const int WS_MAXIMIZEBOX = 0x10000; // Add this line for maximize/restore button

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        #endregion

        private static readonly RoutedEventHandler HideWhenLoadedDelegate = (sender, args) => {
            if (sender is Window == false) return;
            var w = (Window)sender;
            HideCloseButton(w);
            w.Loaded -= HideWhenLoadedDelegate;
        };

        private static readonly RoutedEventHandler ShowWhenLoadedDelegate = (sender, args) => {
            if (sender is Window == false) return;
            var w = (Window)sender;
            ShowCloseButton(w);
            w.Loaded -= ShowWhenLoadedDelegate;
        };

        private static void HideCloseButton(Window w)
        {
            var hwnd = new WindowInteropHelper(w).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private static void ShowCloseButton(Window w)
        {
            var hwnd = new WindowInteropHelper(w).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) | WS_SYSMENU);
        }

        #endregion

        #region HideMinimizeButton (attached property)

        public static readonly DependencyProperty HideMinimizeButtonProperty =
            DependencyProperty.RegisterAttached(
                "HideMinimizeButton",
                typeof(bool),
                OwnerType,
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(HideMinimizeButtonChangedCallback)));

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHideMinimizeButton(Window obj)
        {
            return (bool)obj.GetValue(HideMinimizeButtonProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHideMinimizeButton(Window obj, bool value)
        {
            obj.SetValue(HideMinimizeButtonProperty, value);
        }

        private static void HideMinimizeButtonChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window == null) return;

            var hideMinimizeButton = (bool)e.NewValue;
            if (hideMinimizeButton && !GetIsHiddenMinimizeButton(window))
            {
                if (!window.IsLoaded)
                {
                    window.Loaded += HideMinimizeButtonWhenLoadedDelegate;
                }
                else
                {
                    HideMinimizeButton(window);
                }
                SetIsHiddenMinimizeButton(window, true);
            }
            else if (!hideMinimizeButton && GetIsHiddenMinimizeButton(window))
            {
                if (!window.IsLoaded)
                {
                    window.Loaded -= ShowMinimizeButtonWhenLoadedDelegate;
                }
                else
                {
                    ShowMinimizeButton(window);
                }
                SetIsHiddenMinimizeButton(window, false);
            }
        }

        private static readonly RoutedEventHandler HideMinimizeButtonWhenLoadedDelegate = (sender, args) =>
        {
            if (sender is Window == false) return;
            var w = (Window)sender;
            HideMinimizeButton(w);
            w.Loaded -= HideMinimizeButtonWhenLoadedDelegate;
        };

        private static readonly RoutedEventHandler ShowMinimizeButtonWhenLoadedDelegate = (sender, args) =>
        {
            if (sender is Window == false) return;
            var w = (Window)sender;
            ShowMinimizeButton(w);
            w.Loaded -= ShowMinimizeButtonWhenLoadedDelegate;
        };

        private static void HideMinimizeButton(Window w)
        {
            var hwnd = new WindowInteropHelper(w).Handle;
            var style = GetWindowLong(hwnd, GWL_STYLE);
            style &= ~WS_MINIMIZEBOX; // Hide minimize button
            SetWindowLong(hwnd, GWL_STYLE, style);
        }

        private static void ShowMinimizeButton(Window w)
        {
            var hwnd = new WindowInteropHelper(w).Handle;
            var style = GetWindowLong(hwnd, GWL_STYLE);
            style |= WS_MINIMIZEBOX; // Show minimize button
            SetWindowLong(hwnd, GWL_STYLE, style);
        }

        #endregion

        #region HideMaximizeRestoreButton (attached property)

        public static readonly DependencyProperty HideMaximizeRestoreButtonProperty =
            DependencyProperty.RegisterAttached(
                "HideMaximizeRestoreButton",
                typeof(bool),
                OwnerType,
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(HideMaximizeRestoreButtonChangedCallback)));

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetHideMaximizeRestoreButton(Window obj)
        {
            return (bool)obj.GetValue(HideMaximizeRestoreButtonProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static void SetHideMaximizeRestoreButton(Window obj, bool value)
        {
            obj.SetValue(HideMaximizeRestoreButtonProperty, value);
        }

        private static void HideMaximizeRestoreButtonChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window == null) return;

            var hideMaximizeRestoreButton = (bool)e.NewValue;
            if (hideMaximizeRestoreButton && !GetIsHiddenMaximizeRestoreButton(window))
            {
                if (!window.IsLoaded)
                {
                    window.Loaded += HideMaximizeRestoreButtonWhenLoadedDelegate;
                }
                else
                {
                    HideMaximizeRestoreButton(window);
                }
                SetIsHiddenMaximizeRestoreButton(window, true);
            }
            else if (!hideMaximizeRestoreButton && GetIsHiddenMaximizeRestoreButton(window))
            {
                if (!window.IsLoaded)
                {
                    window.Loaded -= ShowMaximizeRestoreButtonWhenLoadedDelegate;
                }
                else
                {
                    ShowMaximizeRestoreButton(window);
                }
                SetIsHiddenMaximizeRestoreButton(window, false);
            }
        }

        private static readonly RoutedEventHandler HideMaximizeRestoreButtonWhenLoadedDelegate = (sender, args) =>
        {
            if (sender is Window == false) return;
            var w = (Window)sender;
            HideMaximizeRestoreButton(w);
            w.Loaded -= HideMaximizeRestoreButtonWhenLoadedDelegate;
        };

        private static readonly RoutedEventHandler ShowMaximizeRestoreButtonWhenLoadedDelegate = (sender, args) =>
        {
            if (sender is Window == false) return;
            var w = (Window)sender;
            ShowMaximizeRestoreButton(w);
            w.Loaded -= ShowMaximizeRestoreButtonWhenLoadedDelegate;
        };

        private static void HideMaximizeRestoreButton(Window w)
        {
            var hwnd = new WindowInteropHelper(w).Handle;
            var style = GetWindowLong(hwnd, GWL_STYLE);
            style &= ~WS_MAXIMIZEBOX; // Hide maximize/restore button
            SetWindowLong(hwnd, GWL_STYLE, style);
        }

        private static void ShowMaximizeRestoreButton(Window w)
        {
            var hwnd = new WindowInteropHelper(w).Handle;
            var style = GetWindowLong(hwnd, GWL_STYLE);
            style |= WS_MAXIMIZEBOX; // Show maximize/restore button
            SetWindowLong(hwnd, GWL_STYLE, style);
        }

        #region IsHiddenMinimizeButton (readonly attached property)

        private static readonly DependencyPropertyKey IsHiddenMinimizeButtonKey =
            DependencyProperty.RegisterAttachedReadOnly(
                "IsHiddenMinimizeButton",
                typeof(bool),
                OwnerType,
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsHiddenMinimizeButtonProperty =
            IsHiddenMinimizeButtonKey.DependencyProperty;

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetIsHiddenMinimizeButton(Window obj)
        {
            return (bool)obj.GetValue(IsHiddenMinimizeButtonProperty);
        }

        private static void SetIsHiddenMinimizeButton(Window obj, bool value)
        {
            obj.SetValue(IsHiddenMinimizeButtonKey, value);
        }

        #endregion

        #region IsHiddenMaximizeRestoreButton (readonly attached property)

        private static readonly DependencyPropertyKey IsHiddenMaximizeRestoreButtonKey =
            DependencyProperty.RegisterAttachedReadOnly(
                "IsHiddenMaximizeRestoreButton",
                typeof(bool),
                OwnerType,
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsHiddenMaximizeRestoreButtonProperty =
            IsHiddenMaximizeRestoreButtonKey.DependencyProperty;

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetIsHiddenMaximizeRestoreButton(Window obj)
        {
            return (bool)obj.GetValue(IsHiddenMaximizeRestoreButtonProperty);
        }

        private static void SetIsHiddenMaximizeRestoreButton(Window obj, bool value)
        {
            obj.SetValue(IsHiddenMaximizeRestoreButtonKey, value);
        }

        #endregion


        #endregion

        #region IsHiddenCloseButton (readonly attached property)

        private static readonly DependencyPropertyKey IsHiddenCloseButtonKey =
            DependencyProperty.RegisterAttachedReadOnly(
                "IsHiddenCloseButton",
                typeof(bool),
                OwnerType,
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsHiddenCloseButtonProperty =
            IsHiddenCloseButtonKey.DependencyProperty;

        [AttachedPropertyBrowsableForType(typeof(Window))]
        public static bool GetIsHiddenCloseButton(Window obj)
        {
            return (bool)obj.GetValue(IsHiddenCloseButtonProperty);
        }

        private static void SetIsHiddenCloseButton(Window obj, bool value)
        {
            obj.SetValue(IsHiddenCloseButtonKey, value);
        }

        #endregion

    }

}
