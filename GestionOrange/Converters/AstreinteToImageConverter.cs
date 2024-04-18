using System.Globalization;
using Microsoft.Maui.Controls;

namespace GestionOrange.Converters
{
    public class AstreinteToImageConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isTrue && isTrue)
            {
                return "true_astreinte.png";
            }
            else
            {
                return "false_astreinte.png";
            }
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
