using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Exercice3
{
    public class ChargementCouleurConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool enChargement)
            {
                return enChargement ? Brushes.Orange : Brushes.Green;
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
