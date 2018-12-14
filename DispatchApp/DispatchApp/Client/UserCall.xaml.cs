﻿using System;
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


namespace DispatchApp
{

    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserCall : UserControl , INotifyPropertyChanged
    { 
        //公开点击事件(传参)
        public delegate void ImageEventHandler(string ImageSoures);
        public event ImageEventHandler ImageSouresHandle;
        public event ImageEventHandler ImageSouresDoubleHandle;


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

        /// <summary>
        /// 记录通话的信息
        /// </summary>
        public call callNum = new call();
        public string insterNum;
        public UserCall()
        {
            InitializeComponent();
            DataContext = this;
            CurrentState = "OFFLINE";
            

            // 依赖项属性测试
            //StateBackgroundDependencyProperty test = new StateBackgroundDependencyProperty();
            //Binding binding = new Binding("StateBackground") { Source = Time };
            //BindingOperations.SetBinding(test, StateBackgroundDependencyProperty.StateBackgroundProperty, binding);

            //Brush a =(Brush)test.GetValue(StateBackgroundDependencyProperty.StateBackgroundProperty);

            //Time.SetBinding(TextBox.BackgroundProperty, new Binding("StateBackground") { Source = test });
            //CurrentState = new SolidColorBrush(Colors.Orange);
        


        }

       
        public string phoneNum = "0";

        /// <summary>
        /// 按钮点击触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Style_click(object sender, RoutedEventArgs e)//weituo 20181013
        {
            if (ImageSouresHandle != null)
            {
                ImageSouresHandle(phoneNum);
            }
        }
        private void MouseDouble_Click(object sender, MouseButtonEventArgs e)
        {
            if (ImageSouresDoubleHandle != null)
            {
                ImageSouresDoubleHandle(phoneNum);
            }
        }

        /// <summary>
        ///     根据参数初始化该控件
        /// </summary>
        /// <param name="id">输入的参数</param>
        public void setContent(string num)
        {
            labelNumFromId.Content = num.ToString();
            phoneNum = num;
        }

        /// <summary>
        /// label接收号码
        /// </summary>
        /// <param name="num"></param>
        public void SetValue(string num)
        {
            labelNumToId.Content = num.ToString();
        }



        private DispatcherTimer timer;
        private ProcessCount processCount;
        public delegate bool CountDownHandler();
        public void ShowCallTime()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(10000000);   //时间间隔为一秒
            timer.Tick += new EventHandler(timer_Tick);
            processCount = new ProcessCount(0);
            CountDown += new CountDownHandler(processCount.ProcessCountUp);
            timer.Start();
            callState = "1";
        }
        public string callState = "0";

        /// <summary>
        /// Timer触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            //if (CountDown != null)
            //{
            //    Time.Text = processCount.GetHour() + ":" + processCount.GetMinute() + ":" + processCount.GetSecond();
            //}
            //else
            //{
            //    timer.Stop();
            //}
            if (OnCountDown())
            {
                if (processCount.GetHour() == "00")
                {
                    Time.Text = processCount.GetMinute() + ":" + processCount.GetSecond();
                }
                else
                {
                    Time.Text = processCount.GetHour() + ":" + processCount.GetMinute() + ":" + processCount.GetSecond();
                }
            }
            else
            {
                timer.Stop();
                Time.Text = "";
            }
        }

        public void timer_Stop()
        {
            if ("1" == callState)
            {
                timer.Stop();
                callState = "0";
            }
            else
            {
                callState = "0";
            }
            Time.Text = "";
        }

        /// <summary>
        /// 处理事件
        /// </summary>
        public event CountDownHandler CountDown;
        public bool OnCountDown()
        {
            if (CountDown != null)
               return CountDown();
            return false;
        }

        private void SizeChange(object sender, SizeChangedEventArgs e)
        {
            int i = 1;
            i++;         
        }


        /// <summary>
        /// 处理倒计时的委托
        /// </summary>
        /// <returns></returns>
 
        //private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        //{
        //    //转换成秒数
        //    Int32 hour = Convert.ToInt32(HourArea.Text);
        //    Int32 minute = Convert.ToInt32(MinuteArea.Text);
        //    Int32 second = Convert.ToInt32(SecondArea.Text);
            
        //    //处理倒计时的类
        //    processCount = new ProcessCount(hour * 3600 + minute * 60 + second);
        //    CountDown += new CountDownHandler(processCount.ProcessCountDown);
        //    //开启定时器
        //    timer.Start();
        //}


    }
}

