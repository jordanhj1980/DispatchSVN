using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DispatchApp
{
class UserImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string imgName = "user_normal";
            string index = value as string;

            if ("1" == index)
            {
                // 管理员
                imgName = "user_admin";
            }

            imgName = string.Format("../../Resources/{0}.png", imgName);
            return new BitmapImage(new Uri(imgName, UriKind.Relative));
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
