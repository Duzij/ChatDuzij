using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace OpenChatClient
{
    public class MessageAlignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return HorizontalAlignment.Right;

            return HorizontalAlignment.Left;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((HorizontalAlignment)value == HorizontalAlignment.Right)
                return true;

            return false;
        }
    }
}
