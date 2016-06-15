using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RegisterDocs.GUI.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]

    public class DateTimeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var date = (DateTime)value;

                return date.ToLongDateString();
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                DateTime resultDateTime;

                if (DateTime.TryParseExact(value.ToString(), "dd.MM.yyyy", null, DateTimeStyles.None, out resultDateTime))
                {
                    return resultDateTime;
                }
            }

            return DependencyProperty.UnsetValue;
        }

    }
}
