using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace GameZilla.Helpers
{
    public class FavoriteToSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isFavorite && isFavorite)
            {
                return "\uE735";// Symbol.SolidStar; // Utilisez un symbole d'étoile jaune
            }
            else
            {
                return "\uE734";// Symbol.OutlineStar; // Utilisez un symbole d'étoile vide
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
