using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LIT.Core.Converters
{

    public class ConvertHeightRespectedWithMainWindow : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height)
            {
                double subtractValue = 0;

                if (parameter != null && double.TryParse(parameter.ToString(), out double parsedValue))
                {
                    subtractValue = parsedValue;
                }

                return Math.Max(height - subtractValue, 0);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}