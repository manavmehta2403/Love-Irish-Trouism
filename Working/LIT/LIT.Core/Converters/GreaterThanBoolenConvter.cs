using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LIT.Core.Converters
{ 

    public class GreaterThanBoolenConvter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            if (value.Length > 0 && value[0] != DependencyProperty.UnsetValue && value[1] != DependencyProperty.UnsetValue)
            {
                double valueLeft = System.Convert.ToDouble(value[0], CultureInfo.InvariantCulture);
                double valueRight = System.Convert.ToDouble(value[1], CultureInfo.InvariantCulture);
                if (valueLeft > 0 && valueRight > 0)
                {
                    result = valueLeft >= valueRight;
                }
            }

            return result;
        }

        public object ConvertBack(object[] value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
