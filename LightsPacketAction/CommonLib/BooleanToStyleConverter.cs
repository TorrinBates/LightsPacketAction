using System;
using System.Windows;
using System.Windows.Data;

namespace LightsPacketAction
{
    public class BooleanToStyleConverter : IValueConverter
    {
        public Style TrueStyle { get; set; }
        public Style FalseStyle { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool && (bool)value)
            {
                return TrueStyle;
            }
            return FalseStyle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
