using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Lib.Tools
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        enum Parameter
        {
            Normal,
            Inverted,
            Hidden,
            HiddenInverted
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool) value;
            Parameter param = Parameter.Normal;

            if (parameter is string paramString)
                Enum.TryParse(paramString, true, out param);

            switch (param)
            {
                case Parameter.Normal:
                {
                    return boolValue ? Visibility.Visible : Visibility.Collapsed;
                }
                case Parameter.Inverted:
                {
                    return !boolValue ? Visibility.Visible : Visibility.Collapsed;
                }
                case Parameter.Hidden:
                {
                    return boolValue ? Visibility.Visible : Visibility.Hidden;
                }
                case Parameter.HiddenInverted:
                {
                    return !boolValue ? Visibility.Visible : Visibility.Hidden;
                }
                default:
                {
                    throw new InvalidOperationException($"Can not convert to Parameter {param}.");
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}