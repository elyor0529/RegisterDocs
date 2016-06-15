using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RegisterDocs.GUI.Converters
{
  [ValueConversion(typeof(double), typeof(string))]
  public class DoubleConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value == null
        ? null
        : System.Convert.ToDouble(value).ToString("N");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      double d;

      if (double.TryParse(value as string, out d))
      {
        return d;
      }

      return DependencyProperty.UnsetValue;
    }
  }
}
