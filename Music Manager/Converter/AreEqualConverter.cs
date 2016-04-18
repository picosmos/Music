using System;
using System.Globalization;
using System.Windows.Data;

namespace Koopakiller.Apps.MusicManager.Converter
{
    public class AreEqualConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return false;
            }
            if (values.Length <= 1)
            {
                return true;
            }
            for (var i = 1; i < values.Length; ++i)
            {
                if (!values[0].Equals(values[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
