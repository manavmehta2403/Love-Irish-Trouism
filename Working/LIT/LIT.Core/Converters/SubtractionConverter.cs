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
    public class SubtractionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != DependencyProperty.UnsetValue && values[1] != DependencyProperty.UnsetValue)
            {
                if (values.Length == 2)
                {
                    decimal subtract = (decimal)values[1];
                    bool canSubtract = subtract > 0;
                    if (values[0] is decimal totalAmount && values[1] is decimal paid && canSubtract)
                    {
                        return totalAmount - paid;
                    }
                    else
                    {
                        return 0.00;
                    }
                }

            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
