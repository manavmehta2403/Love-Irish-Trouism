using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LIT.Core.Converters
{
    public class SelectedToBackgroundConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected && isSelected)
            {

                
                return new BrushConverter().ConvertFrom("#000") as Brush; // Lighter color when not selected (do not go with the logic it works all opposite in UI)
            }
            else
            {
                return  new BrushConverter().ConvertFrom("#899ea9") as Brush; // Darker color when selected (do not go with the logic it works all opposite in UI)
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
