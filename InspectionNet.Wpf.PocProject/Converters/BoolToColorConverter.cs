using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace InspectionNet.Wpf.PocProject.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isOn && parameter is SolidColorBrush ColorBrush)
            {
                return isOn ? ColorBrush : Brushes.DimGray;
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
