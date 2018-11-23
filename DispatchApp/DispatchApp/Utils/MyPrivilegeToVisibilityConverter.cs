using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DispatchApp
{
    public class MyPrivilegeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            if (value.ToString() == "1")   // 管理员
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }

            return (value.ToString() == parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (System.Convert.ToBoolean(value)) return parameter;

            return null;
        }
    }
}
