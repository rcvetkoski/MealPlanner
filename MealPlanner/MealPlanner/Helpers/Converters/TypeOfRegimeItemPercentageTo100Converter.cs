using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MealPlanner.Helpers.Converters
{
    public class TypeOfRegimeItemPercentageTo100Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value * 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0;

            return (double)value / 100;
        }
    }
}

