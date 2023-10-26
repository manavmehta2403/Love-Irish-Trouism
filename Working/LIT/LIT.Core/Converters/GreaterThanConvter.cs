using System;
using System.Globalization;
using System.Windows.Data;

namespace LIT.Core.Converters
{
    public class GreaterThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                double doubleValue;
                double doubleParameter;

                if (double.TryParse(value.ToString(), out doubleValue) && double.TryParse(parameter.ToString(), out doubleParameter))
                {
                    return doubleValue > doubleParameter;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
