using System;
using Windows.UI.Xaml.Data;

namespace Feedro.View.Converter
{
    public class BooleanInverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
                return !((bool)value);

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
