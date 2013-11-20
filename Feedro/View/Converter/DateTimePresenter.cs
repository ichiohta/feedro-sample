using System;
using Windows.UI.Xaml.Data;
using Windows.Globalization.DateTimeFormatting;

namespace Feedro.View.Converter
{
    public class DateTimePresenter : IValueConverter
    {
        private static DateTimeFormatter _defaultDateFormat;

        private static DateTimeFormatter DefaultDateFormat
        {
            get
            {
                return _defaultDateFormat ?? (_defaultDateFormat = new DateTimeFormatter("shortdate"));
            }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var datetime = (DateTimeOffset)value;

            if (datetime == default(DateTimeOffset))
                return null;

            var formatName = parameter as string;
            var formatter = formatName != null ? new DateTimeFormatter(formatName) : DefaultDateFormat;

            return formatter.Format(datetime.LocalDateTime);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
