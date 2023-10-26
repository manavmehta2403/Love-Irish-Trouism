using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LIT.Core.Converters
{
    public class StringNullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                // Convert empty or null string to Visibility.Collapsed, otherwise, Visible.
                return string.IsNullOrEmpty(stringValue) ? Visibility.Collapsed : Visibility.Visible;
            }

            // Handle non-string values (optional)
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(); // This converter doesn't support two-way binding.
        }
    }

}
