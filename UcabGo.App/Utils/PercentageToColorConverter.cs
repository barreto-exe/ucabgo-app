using System.Globalization;

namespace UcabGo.App.Utils
{
    public class PercentageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double percentage)
            {
                if (percentage >= 0.8)
                    return Colors.Green;
                else if (percentage >= 0.4)
                    return Colors.SteelBlue;
                else
                    return Colors.IndianRed;
            }
            
            return Colors.IndianRed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
