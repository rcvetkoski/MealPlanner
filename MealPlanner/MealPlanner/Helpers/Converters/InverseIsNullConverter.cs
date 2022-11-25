using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MealPlanner.Helpers.Converters
{
    public class InverseIsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;
            else
                return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
