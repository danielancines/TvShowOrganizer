using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Labs.WPF.Core.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Hidden;

            var convertedValue = false;
            if (bool.TryParse(value.ToString(), out convertedValue))
            {
                if (convertedValue)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
