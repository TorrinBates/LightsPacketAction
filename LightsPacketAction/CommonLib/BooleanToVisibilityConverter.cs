using System;
using System.Windows;
using System.Windows.Data;

namespace LightsPacketAction
{
    public class BooleanToVisibility : IValueConverter
    {
        public bool Inverted { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return Inverted ? InvertedGetValue(value) : GetValue(value);
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Visibility InvertedGetValue(object value) => (bool)value ? Visibility.Collapsed : Visibility.Visible;

        private Visibility GetValue(object value) => (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }
}
