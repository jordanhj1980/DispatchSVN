using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace DispatchApp
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class RelayCall : UserControl
    { 
        //公开点击事件(传参)
        public delegate void ImageEventHandler(string ImageSoures);
        public event ImageEventHandler ImageSouresHandle;
        public event ImageEventHandler ImageSouresDoubleHandle;

        public RelayCall()
        {
            InitializeComponent();
        }

        public string phoneNum = "0";

        /// <summary>
        /// 按钮点击触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RelayStyle_click(object sender, RoutedEventArgs e)//weituo 20181013
        {
            if (ImageSouresHandle != null)
            {
                ImageSouresHandle(phoneNum);
            }
        }

        /// <summary>
        ///     根据参数初始化该控件
        /// </summary>
        /// <param name="id">输入的参数</param>
        public void setContent(string num)
        {
            //RelaylabelNumFromId.Text = num.ToString();
            ButtonRelay.Content = num.ToString();
            phoneNum = num;
        }

        /// <summary>
        /// label接收号码
        /// </summary>
        /// <param name="num"></param>
        //public void SetValue(string num)
        //{
        //    RelaylabelNumToId.Content = num.ToString(); 
        //}

        private void RelayMouseDouble_Click(object sender, MouseButtonEventArgs e)
        {
            if (ImageSouresDoubleHandle != null)
            {
                ImageSouresDoubleHandle(phoneNum);
            }
        }

        


  


    }
}

