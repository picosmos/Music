using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Koopakiller.Apps.MusicManager.Converter
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = false;
            if (value is bool)
            {
                val = (bool)value;
            }
            if (parameter != null)
            {
                val = !val;
            }

            return val ? this.VisibleValue : this.HiddenValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public Visibility VisibleValue { get; set; } = Visibility.Visible;
        public Visibility HiddenValue { get; set; } = Visibility.Collapsed;
    }
}
