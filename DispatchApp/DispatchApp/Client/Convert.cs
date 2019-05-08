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
            Brush bru_return = (Brush)new BrushConverter().ConvertFromString("#4D4D4F");

            switch (str_value)
            {
                case "BUSY":
                case "ALERT":
                case "RING":
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("#FFE70E"));
                    break;
                case "ANSWER":         
                case "ANSWERED":
                case "INSTER":
                case "LISTEN":
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("#4FA92E"));
                    break;
                case "BYE":
                case "IDLE":
                case "ONLINE":     
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("#4d56ad"));//585F80
                    break;
                case "FAILED":
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("#E60416"));
                    break;
                case "OFFLINE":
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("#4D4D4F")); //4D4D4F
                    break;
                case "OFFHOOK":
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("#E60416")); //4D4D4F
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
                    case "BUSY":
                        img = (new BitmapImage(new Uri("../Resources/dianhuaHung.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "ALERT":
                    case "RING":
                        img = (new BitmapImage(new Uri("../Resources/dianhuaHung.png", UriKind.RelativeOrAbsolute)));
                        //img = (new BitmapImage(new Uri("../Resources/dianhuaRing2.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "ANSWER":
                    case "ANSWERED":
                    case "INSTER":
                    case "LISTEN":
                        img = (new BitmapImage(new Uri("../Resources/dianhuaCom.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "BYE":
                    case "IDLE":
                    case "ONLINE":
                        img = (new BitmapImage(new Uri("../Resources/dianhuaOn.png", UriKind.RelativeOrAbsolute)));
                        break;
                    case "FAILED":
                    case "OFFLINE":
                    case "OFFHOOK":
                        img = (new BitmapImage(new Uri("../Resources/dianhuaOFF.png", UriKind.RelativeOrAbsolute)));
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
                    case "BYE":
                    case "IDLE":
                    case "ONLINE":
                        img = (new BitmapImage(new Uri("../Resources/dianhuaNo.png", UriKind.RelativeOrAbsolute)));
                        break;
                    // 黄色表示在呼叫，不可操作
                    case "BUSY":
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
                    case "OFFHOOK":
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


    /// <summary>
    /// 来电显示区颜色信息，没用
    /// </summary>
    public class ComeCallConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str_value = value.ToString();
            Brush bru_return = (Brush)new BrushConverter().ConvertFromString("#4D4D4F");

            switch (str_value)
            {
                case "ALERT":
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("Black"));
                    break;
                case "RING":
                    bru_return = ((Brush)new BrushConverter().ConvertFromString("Red"));
                    break;
                case "BUSY":
                case "ANSWER":
                case "ANSWERED":
                case "BYE":
                case "IDLE":
                case "ONLINE":                  
                case "FAILED":                  
                case "OFFLINE":
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

}
