using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MenuiserieApp.UI.Converters
{
    public class StatutCouleurConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var statut = value as string;

            return statut switch
            {
                "Devis" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8")),
                "En production" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B")),
                "Terminé" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981")),
                "Livré" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B82F6")),
                _ => new SolidColorBrush(Colors.Black)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
