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
using System.ComponentModel;

using System.Windows.Threading;

namespace DispatchApp
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class KeyCall : UserControl , INotifyPropertyChanged
    { 
        //公开点击事件(传参)
        public delegate void ImageEventHandler(string ImageSoures);
        public event ImageEventHandler ImageSouresHandle;
        //public event ImageEventHandler ImageSouresDoubleHandle;

        public int index{ get; set; }
        

        public event PropertyChangedEventHandler PropertyChanged;
        public string _CurrentState;
        public string CurrentState
        {
            get { return _CurrentState; }
            set
            {
                _CurrentState = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentState"));
            }
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {   
                //假设属性发生了改变，则触发这个事件
                PropertyChanged(this, e);
            }
        }


        public KeyCall()
        {
            InitializeComponent();
            DataContext = this;
            CurrentState = "";
        }

        /// <summary>
        /// 按钮点击触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Key(object sender, RoutedEventArgs e)//weituo 20181013
        {
            if (ImageSouresHandle != null)
            {
                ImageSouresHandle(KeyText.Text);
            }
        }


   

    }
}

