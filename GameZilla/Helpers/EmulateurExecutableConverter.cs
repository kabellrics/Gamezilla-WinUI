using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace GameZilla.Helpers;
public class EmulateurExecutableConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string stringarray)
        {
            return stringarray.Replace("^", "").Replace("\\", "").Replace("$", "").Replace("*", "").Replace("..", ".");
        }
        return string.Empty;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value.ToString();
        //throw new NotImplementedException();
    }
}
