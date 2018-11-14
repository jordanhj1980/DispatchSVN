using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace DispatchApp
{
    /// <summary>
    /// 状态转换为颜色
    /// </summary>
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str_value = value.ToString();
            Brush bru_return = (Brush)new BrushConverter().ConvertFromString("#585F80");

            switch (str_value)
            {
                case "BUSY":
                case "ALERT":
                case "RING":
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("#FFE70E"));
                    break;
                case "ANSWER":         
                case "ANSWERED":
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("#4FA92E"));
                    break;
                case "BYE":
                case "IDLE":
                case "ONLINE":     
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("#585F80"));
                    break;
                case "FAILED":
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("#E60416"));
                    break;
                case "OFFLINE":
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("#4D4D4F"));
                    break;
                default: break;
            }
            return (bru_return);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 用户状态转换为图片
    /// </summary>
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str_value = value.ToString();
            BitmapImage img = new BitmapImage(new Uri("../Resources/PhoneKey.png", UriKind.RelativeOrAbsolute));

            try
            {
                switch (str_value)
                {
                    case "Ready":
                    case "Active":
                    case "Progress":
                    case "Offline":
                    case "Offhook":
                        img = (new BitmapImage(new Uri("../Resources/PhoneKey.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "BUSY":
                        img = (new BitmapImage(new Uri("../Resources/PhoneKey.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "IDLE":
                        img = (new BitmapImage(new Uri("../Resources/ThePhone.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "ONLINE":
                        img = (new BitmapImage(new Uri("../Resources/ThePhone.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "BYE":
                        img = (new BitmapImage(new Uri("../Resources/ThePhone.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "OFFLINE":
                        img = (new BitmapImage(new Uri("../Resources/PhoneOut.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "FAILED":
                        img = (new BitmapImage(new Uri("../Resources/PhoneOut.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "RING":
                         img = (new BitmapImage(new Uri("../Resources/PhoneKey.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "ALERT":
                        img =  (new BitmapImage(new Uri("../Resources/PhoneKey.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "ANSWER":
                        img = (new BitmapImage(new Uri("../Resources/PhoneIn.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "ANSWERED":
                        img = (new BitmapImage(new Uri("../Resources/PhoneIn.png", UriKind.RelativeOrAbsolute)));
                        break;
                    default: break;
                }
                return img;
             }
            catch
            {
                return new BitmapImage();
            } 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 键权状态转换为图片
    /// </summary>
    public class KeyImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str_value = value.ToString();
            BitmapImage img = new BitmapImage(new Uri("../Resources/dianhuaNo.png", UriKind.RelativeOrAbsolute));

            try
            {
                switch (str_value)
                {
                    // 灰色表示空闲，可操作
                    case "BUSY":
                    case "BYE":
                    case "IDLE":
                    case "ONLINE":
                        img = (new BitmapImage(new Uri("../Resources/dianhuaNo.png", UriKind.RelativeOrAbsolute)));
                        break;
                    // 黄色表示在呼叫，不可操作
                    case "ALERT":
                    case "RING":
                        img = (new BitmapImage(new Uri("../Resources/dianhuaRing.png", UriKind.RelativeOrAbsolute)));
                        break;
                    // 绿色表示在通话，不可操作
                    case "ANSWER":
                    case "ANSWERED":
                        img = (new BitmapImage(new Uri("../Resources/dianhuaAnswer.png", UriKind.RelativeOrAbsolute)));
                        break;      
                    // 红色标识故障。不可操作
                    case "FAILED":
                    case "OFFLINE":
                        img = (new BitmapImage(new Uri("../Resources/dianhuaFailed.png", UriKind.RelativeOrAbsolute)));
                        break;
                    default:
                        img = new BitmapImage(new Uri("../Resources/dianhuaBusy.png", UriKind.RelativeOrAbsolute));
                        break;
                }
                return img;
            }
            catch
            {
                return new BitmapImage();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
