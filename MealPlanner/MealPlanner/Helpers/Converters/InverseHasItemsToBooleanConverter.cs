using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MealPlanner.Helpers.Converters
{
    public class InverseHasItemsToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value == 0)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
