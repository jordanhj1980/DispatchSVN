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

using Newtonsoft.Json;

namespace DispatchApp
{
    /// <summary>
    /// CreateDeskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CallBoard : Window
    {
        private CallUserControl callUserCtrl;

        public CallBoard(CallUserControl mm)
        {
            callUserCtrl = mm;
            InitializeComponent();
        }

        private void CallOne(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "1";
        }

        private void CallTwo(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "2";
        }

        private void CallThree(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "3";
        }

        private void CallFour(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "4";
        }

        private void CallFive(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "5";
        }

        private void CallSix(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "6";
        }

        private void CallSeven(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "7";
        }

        private void CallEight(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "8";
        }

        private void CallNine(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "9";
        }

        private void CallStar(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "*";
        }

        private void CallZero(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "0";
        }
        private void CallAlarm(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "#";
        }

        private void CallClear(object sender, RoutedEventArgs e)
        {
            CallText.Text = "";
        }

        private void CallBackspace(object sender, RoutedEventArgs e)
        {
            if (CallText.Text.Length > 0)
            {
                CallText.Text = CallText.Text.Substring(0, CallText.Text.Length - 1);
            }
            else
            {
                CallText.Text = "";
            }
        }

        private void CallAdd(object sender, RoutedEventArgs e)
        {
            CallText.Text = CallText.Text + "+";
        }

        private void ClossBoard(object sender, RoutedEventArgs e)
        {
            this.Hide();
           //deskTabControl.SelectedIndex = 0;
        }

        /// <summary>
        /// 呼叫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void CallKeyBoard(object sender, RoutedEventArgs e)
        //{
        //    if ("0" == callUserCtrl.serverCall)
        //    {
        //        MessageBox.Show("当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "呼叫信息");
        //    }
        //    else
        //    {
        //        //if ((null == mainWindow.callUserCtrl.trunkCall) || ("" == mainWindow.callUserCtrl.trunkCall))
        //        //{
        //        //    MessageBox.Show("当前中继电话空\r\n请输入中继电话！", "呼叫信息");
        //        //}
        //        //else if ("" == CallText.Text)
        //        //{
        //        //    MessageBox.Show("当前呼叫电话空\r\n请输入呼叫电话！", "呼叫信息");
        //        //}
        //        if ("" == CallText.Text)
        //        {
        //            MessageBox.Show("当前呼叫电话空\r\n请输入呼叫电话！", "呼叫信息");
        //        }
        //        else
        //        {
        //            callRel tellCall = new callRel() { fromid = callUserCtrl.serverCall, toid = CallText.Text, trunkid = callUserCtrl.trunkCall };
        //            string strMsg = "CMD#CallOut#" + JsonConvert.SerializeObject(tellCall);
        //            mainWindow.ws.Send(strMsg);
        //            this.Hide();
        //        }  
        //    }


        //}

        /// <summary>
        /// 中继呼叫布置电话，查初始状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RelayCallKeyBoard(object sender, RoutedEventArgs e)
        {
            if ("0" == callUserCtrl.serverCall)
            {
                MessageBox.Show("当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "呼叫信息");
            }
            else
            {
                if (("" == CallText.Text) || (null == CallText.Text))
                {
                    MessageBox.Show("当前呼叫电话空\r\n请输入呼叫电话！", "呼叫信息");
                }
                else
                {
                    deskTabControl.SelectedIndex = 1;

                    ((TabItem)(deskTabControl.Items[0])).Visibility = Visibility.Visible;
                    ((TabItem)(deskTabControl.Items[1])).Visibility = Visibility.Visible;

                    //RelayList.Items.Clear();
                    //for (int Idx = 0; Idx < mainWindow.callUserCtrl.PageRelay.Count; Idx++) // 布置页面按钮
                    //{
                    //    string name = mainWindow.callUserCtrl.PageRelay[Idx].extid;
                    //    string called = "no";
                    //    RelayCall relayCall = new RelayCall();

                    //    relayCall.setContent(name);                        //Id
                    //    //relayCall.SetValue(called);
                    //    RelayList.Items.Add(relayCall);

                    //    relayCall.ImageSouresHandle += new RelayCall.ImageEventHandler(ReLaySigleEvent);
                    //    string strMsg = "CMD#GETSTATE#" + name;           //获取电话初始状态
                    //    mainWindow.ws.Send(strMsg);
                    //    //relayCall.ImageSouresDoubleHandle += new RelayCall.ImageEventHandler(ReLaDoubleEvent);
                    //}
                }
            }
        }

        /// <summary>
        /// 单击中继号码事件
        /// </summary>
        /// <param name="word"></param>
        public void ReLaySigleEvent(string word)
        {
            callUserCtrl.trunkCall = word;
            List<RelayCall> pageRelayCall = FindChirldHelper.FindVisualChild<RelayCall>(this);

            foreach (var item in pageRelayCall)
            {
                //UserCall btn = new UserCall();
                if (word == item.phoneNum)
                {
                    item.ButtonRelay.BorderBrush = Brushes.Yellow;
                }
                else
                {
                    item.ButtonRelay.BorderBrush = Brushes.Gray;
                }
            }

            callRel tellCall = new callRel() { fromid = callUserCtrl.serverCall, toid = CallText.Text, trunkid = callUserCtrl.trunkCall };
            string strMsg = "CMD#CallOut#" + JsonConvert.SerializeObject(tellCall);
            //mainWindow.ws.Send(strMsg);
            this.Hide();
        }

        /// <summary>
        /// 遍历容器内控件
        /// </summary>
        public static class FindChirldHelper
        {
            public static List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
            {
                try
                {
                    List<T> TList = new List<T> { };
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                        if (child != null && child is T)
                        {
                            TList.Add((T)child);
                            List<T> childOfChildren = FindVisualChild<T>(child);
                            if (childOfChildren != null)
                            {
                                TList.AddRange(childOfChildren);
                            }
                        }
                        else
                        {
                            List<T> childOfChildren = FindVisualChild<T>(child);
                            if (childOfChildren != null)
                            {
                                TList.AddRange(childOfChildren);
                            }
                        }
                    }
                    return TList;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message + "a");
                    return null;
                }
            }
        }












      

    }
}
