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
using System.Collections.ObjectModel;
using System.ComponentModel;

using Npgsql;

using System.Windows.Interop;
using System.Runtime.InteropServices;
using WebSocket4Net;
using Newtonsoft.Json;

using System.Threading;
using System.Diagnostics;
using MaterialDesignThemes.Wpf.Transitions;
using MaterialDesignThemes.Wpf;

namespace DispatchApp
{

    public enum e_CallType
    {
        Key,            // 键权
        USER,           // 用户
        RADIO,          // 广播
        RELAY,          // 中继
    }

    public class c_CallTypeInfo
    {
        public e_CallType status { get; set; }
        public int index { get; set; }
    }

    public struct s_ListUser
    {
        public List<GroupData> GroupUser;
        public string Header;
    }


    public partial class CallUserControl : UserControl
    {

        /* 电话List分类 start  */
        public List<s_ListUser> PageUser = new List<s_ListUser>();                  // 用户电话
        public List<GroupData> PageKey = new List<GroupData>();                     // 键权电话
        public List<GroupData> PageRelay = new List<GroupData>();                   // 直呼中继电话
        public int PageRelayIndex = 0;                                              // 直呼中继电话所在直呼区的页数
        public List<GroupData> PageRadio = new List<GroupData>();                   // 广播电话
        public int PageRadioIndex = 0;                                              // 广播电话所在直呼区的页数
        /* 电话List分类 end  */

        public c_CallTypeInfo c_callTypeInfo = new c_CallTypeInfo();

        /// =====================键权电话代码==================
        /// <summary>
        /// 键权电话的分布
        /// </summary>
        private void SetKeyCall()
        {
            int KeyNum = PageKey.Count;            // 键权电话个数
            for (int i = 0; i < 2; i++)
            {
                KeyCall keycall = new KeyCall();
                if (i < KeyNum)
                {
                    keycall.KeyText.Text = PageKey[i].extid;
                    keycall.index = i + 1;
                    keycall.ImageSouresHandle += new KeyCall.ImageEventHandler(KeyClickEvent);
                    string strMsg = "CMD#GETSTATE#" + keycall.KeyText.Text;  //获取电话初始状态
                    mainWindow.ws.Send(strMsg);
                    Debug.WriteLine("查键权电话状态" + keycall.KeyText.Text);
                    // 清除夜服设置
                    call tellCall = new call();
                    tellCall.fromid = PageKey[i].extid;
                    tellCall.toid = nightId;
                    strMsg = "CMD#NightServiceOff#" + JsonConvert.SerializeObject(tellCall);
                    mainWindow.ws.Send(strMsg);
                    // 记录键权电话信息。
                    KeyCallDate keyCallDate = new KeyCallDate();
                    keyCallDate.id = PageKey[i].extid;
                    keyCallDate.index = i + 1;
                    keyCallDate.nightId = nightId;
                    keyCallDateList.Add(keyCallDate);
                }
                else
                {
                    keycall.KeyText.Text = "null";
                }

                KeyCallListBox.Items.Add(keycall);
            }
        }

        /// <summary>
        /// 键权电话单击事件
        /// </summary>
        /// <param name="word"></param>
        private void KeyClickEvent(string word)
        {
            //mainWindow.outLine.outLineViewModel.outLineCall.serverNum = word;
            //outLineCall.serverNum = word;
            serverCall = word;
            int idex = 0;

            //单击高亮
            List<KeyCall> pageKeyCall = FindChirldHelper.FindVisualChild<KeyCall>(this);
            foreach (var item in pageKeyCall)
            {
                if (word == item.KeyText.Text)
                {
                    item.KeyButton.BorderBrush = Brushes.Yellow;
                    idex = item.index;
                }
                else
                {
                    item.KeyButton.BorderBrush = Brushes.Gray;
                }
            }

            // 指示当前选择的键权电话index
            m_keyIndex = idex - 1;
        }
        //============================================================

        /// =====================直呼键区中继代码=====================
        /// <summary>
        /// 直呼中继电话的分布
        /// </summary>
        private void SetRelayCall()
        {
            /* 创建容器 */
            TabItem tab = new TabItem();            // 造一个新选项卡
            tab.Visibility = Visibility.Collapsed;
            ListBox MyWrapPanel2 = new ListBox();   // 选项卡中的容器，用于存放每一个元素
            MyWrapPanel2.Style = FindResource("WrapListBoxStyle") as Style;     // 设定ListBox样式为定义好的样式 WrapListBoxStyle

            /* 将造好的新选项卡扔进TabControl1里 */
            tab.Content = MyWrapPanel2;
            tabCtrl_User.Items.Add(tab);

            /* 获取直呼中继电话所在直呼区的页数 */
            PageRelayIndex = tabCtrl_User.Items.Count - 1;
        }
        //============================================================

        /// =====================直呼键区广播代码=====================
        /// <summary>
        /// 直呼区广播电话的分布
        /// </summary>
        private void SetRadioCall()
        {
            /* 创建容器 */
            TabItem tab = new TabItem();            // 造一个新选项卡
            tab.Visibility = Visibility.Collapsed;
            ListBox MyWrapPanel2 = new ListBox();   // 选项卡中的容器，用于存放每一个元素
            MyWrapPanel2.Style = FindResource("WrapListBoxStyle") as Style;

            /* 将造好的新选项卡扔进TabControl1里 */
            tab.Content = MyWrapPanel2;
            tabCtrl_User.Items.Add(tab);

            /* 获取广播电话所在直呼区的页数 */
            PageRadioIndex = tabCtrl_User.Items.Count - 1;
        }
        //============================================================


        /// =====================直呼键区用户代码=====================
        /// <summary>
        /// 直呼区不同组用户电话的分布
        /// </summary>
        private void SetUserCall()
        {
            /* 创建容器 */
            TabItem tab = new TabItem();            // 造一个新选项卡
            int idex = PageUser.Count - 1;         
            tab.Header = PageUser[idex].Header;     // 当前组组名
          
            ListBox MyWrapPanel2 = new ListBox();   // 选项卡中的容器，用于存放每一个元素
            MyWrapPanel2.Style = FindResource("WrapListBoxStyle") as Style;

            /* 处理组员 */
            int groupNum = PageUser[idex].GroupUser.Count;      // 组员个数
            for (int i = 0; i < groupNum; i++)
            {
                /* 一个组员的信息 */
                UserCall userCall = new UserCall();
                string name = PageUser[idex].GroupUser[i].extid; 
                userCall.setContent(name);                      //Id
                userCall.SetValue("");                          //被叫号码
                /* 存放组员 */
                MyWrapPanel2.Items.Add(userCall);
                /* 组员被点击后的操作 */
                userCall.ImageSouresHandle += new UserCall.ImageEventHandler(ImageEvent);
                userCall.ImageSouresDoubleHandle += new UserCall.ImageEventHandler(ImageDoubleEvent);
                /* 获取组员初始状态 */
                string strMsg = "CMD#GETSTATE#" + name;         
                mainWindow.ws.Send(strMsg);
            }

            /* 将造好的新选项卡扔进TabControl1里 */
            tab.Content = MyWrapPanel2;
            tabCtrl_User.Items.Add(tab);
            /* 默认TabControl1当前页为用户分组页的第一页 */
            tabCtrl_User.SelectedIndex = tabCtrl_User.Items.Count - 1;          
        }

        /// <summary>
        /// 点击事件进行传用户号码
        /// 对控件高亮，其余变暗
        /// </summary>
        private void ImageEvent(string word)
        {
            clientCall = word;
            Debug.WriteLine("clientCall" + clientCall);

            List<UserCall> firstPageUserCall = FindChirldHelper.FindVisualChild<UserCall>(this);

            //string upName = "name" + word;
            foreach (var item in firstPageUserCall)
            {
                //UserCall btn = new UserCall();
                if (word == item.phoneNum)
                {
                    item.ButtonBack.BorderBrush = Brushes.Yellow;
                }
                else
                {
                    item.ButtonBack.BorderBrush = Brushes.Gray;
                }
            }
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

        /// <summary>
        /// 双击用户号码
        /// </summary>
        /// <param name="word"></param>
        private void ImageDoubleEvent(string word)
        {
            if ("0" == serverCall)
            {
                MessageBox.Show("当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "呼叫信息");
            }
            else
            {
                if ("0" == clientCall)
                {
                    MessageBox.Show("当前终端电话空\r\n请点击终端电话！", "呼叫信息");
                }
                else
                {
                    call tellCall = new call() { fromid = serverCall, toid = clientCall };
                    string strMsg = "CMD#Call#" + JsonConvert.SerializeObject(tellCall);
                    mainWindow.ws.Send(strMsg);
                }
            }
        }
        //============================================================


        //=======================夜服分隔线==============================
        //===============================================================
        /// <summary>
        /// 关闭夜服Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void NightServeDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            call tellCall = new call() { fromid = serverCall, toid = clientCall };
            foreach (var item in keyCallDateList)
            {
                if ("CMD#NightServiceOn#" == item.nightState)
                {
                    tellCall.fromid = item.id;
                    tellCall.toid = item.nightId;
                    item.nightState = "CMD#NightServiceOff#";
                    string strMsg = "CMD#NightServiceOff#" + JsonConvert.SerializeObject(tellCall);
                    mainWindow.ws.Send(strMsg);

                    mainWindow.ShowKeyLabel.Content = "";
                    mainWindow.ShowKeyLabel.Foreground = Brushes.Red;
                    Debug.WriteLine("键权话机" + item.id + "开启夜服");
                    //MessageBox.Show("键权话机" + item.id + "\r\n" + "夜服关闭", "夜服信息");
                    //NightService.Background = ((Brush)new BrushConverter().ConvertFromString("#FFCBC7C7"));
                }
            }
        }


        /// <summary>
        /// 开启夜服Dialog
        /// </summary>
        private MyCommand _nightServerCommand;
        public MyCommand NightServerCommand
        {
            get
            {
                if (_nightServerCommand == null)
                    _nightServerCommand = new MyCommand(new Action<object>
                    (
                        o =>
                        {
                            NightServerCommandViewDialog(o);
                        }
                    ));
                return _nightServerCommand;
            }
        }


        public NightServerCloseBtn nightServerCloseBtn = new NightServerCloseBtn();
        private async void NightServerCommandViewDialog(object o)
        {
            call tellCall = new call() { fromid = serverCall, toid = clientCall };
            string strState = "";

            foreach (var item in keyCallDateList)
            {
                if ((serverCall == item.id) && ("0" != item.nightId) && (null != item.nightId))
                {
                    strState = "True";
                }
            }

            if (strState == "True")
            {
                foreach (var item in keyCallDateList)
                {
                    item.nightState = "CMD#NightServiceOn#";     // 键权电话的夜服开启状态
                    tellCall.fromid = item.id;
                    tellCall.toid = item.nightId;
                    string strMsg = "CMD#NightServiceOn#" + JsonConvert.SerializeObject(tellCall);
                    mainWindow.ws.Send(strMsg);

                    mainWindow.ShowKeyLabel.Content = "夜服已开启";
                    //MessageBox.Show("键权话机" + item.id + "\r\n" + "夜服开启" + "\r\n" + "呼转至" + item.nightId, "夜服信息");
                    Debug.WriteLine("键权话机" + item.id + "关闭夜服");
                }
            }

            var view = nightServerCloseBtn;
            // var view = new NightServerCloseBtn();
            var result = await DialogHost.Show(view, "UserNightServerDialog", ListViewClosingEventHandler);
        }
        private void ListViewClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }
        //===============================================================


        //=======================外线分隔线==============================
        //===============================================================
        /// <summary>
        /// 关闭呼叫外线Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OutLineDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            mainWindow.outLine.deskTabControl.SelectedIndex = 0;
        }

        private MyCommand _outLineComand;
        public MyCommand OutLineComand
        {
            get
            {
                if (_outLineComand == null)
                    _outLineComand = new MyCommand(new Action<object>
                    (
                        o =>
                        {
                            OutLineComandViewDialog(o);
                        }
                    ));
                return _outLineComand;
            }
        }

        private async void OutLineComandViewDialog(object o)
        {
            //var view = new OutLine(mainWindow);
            var view = mainWindow.outLine;
            view.outLineViewModel.outLineCall.serverNum = serverCall;
            if ((view.outLineViewModel.outLineCall.serverNum == null) || (view.outLineViewModel.outLineCall.serverNum == "0"))
            {
                MessageBox.Show("键权电话为空" +"\r\n"+"请点击键权电话");
            }
            else
            {
                view.outLineViewModel.outLineCall.outLineNum = "";

                if (view.RelayList.Items.Count == 0)
                {
                    view.deskTabControl.SelectedIndex = 0;
                    view.outLineViewModel.callLogList.Clear();
                    //((TabItem)(view.deskTabControl.Items[0])).Visibility = Visibility.Hidden;
                    //((TabItem)(view.deskTabControl.Items[1])).Visibility = Visibility.Hidden;
                    callBoard.RelayList.Items.Clear();
                    for (int Idx = 0; Idx < PageRelay.Count; Idx++) // 布置页面按钮
                    {
                        RelayNum relaynum = new RelayNum();
                        relaynum.relayNum = PageRelay[Idx].extid;
                        relaynum.isSelected = false;
                        view.outLineViewModel.outLineCall.relayNumList.Add(relaynum);

                        string strMsg = "CMD#GETSTATE#" + relaynum.relayNum;           //获取电话初始状态
                        mainWindow.ws.Send(strMsg);
                        //relayCall.ImageSouresDoubleHandle += new RelayCall.ImageEventHandler(ReLaDoubleEvent);
                    }
                }
                var result = await DialogHost.Show(view, "UserOutLineDialog", ListViewClosingEventHandler);

            }

        }














    }

     
}
