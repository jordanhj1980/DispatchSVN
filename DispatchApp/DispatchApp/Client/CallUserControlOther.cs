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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Configuration;
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

    public enum e_OperaState
    {
        CALL,            // 呼叫
        COMMUNICATION,   // 正在通信
        TRANS,           // 转接
        LISTEN,          // 监听
        INTER,           // 强插
        SPLIT,           // 强拆
        NULL,            // 空
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

    /// <summary>
    /// 直呼键区单击后的显示状态
    /// add by xiaozi 20190124
    /// </summary>
    public enum ClickStateEnum
    {
        CALL,       // 用户拨号状态
        RELAY,      // 中继直呼状态
        RADIO,      // 广播直呼状态
        STOP,
    }

    /// <summary>
    /// 一个电话/中继直呼/广播直呼的名称和状态
    /// add by xiaozi 20190124
    /// </summary>
    public class ClickState
    {
        public ClickStateEnum clickState { get; set; }
        public string name { get; set; }

        public DispatcherTimer callClickTimer;      // 呼叫点击事件定时器
        public ClickState()
        {
            string strCallClickDelay = ConfigurationManager.AppSettings["callclickdelay"];
            int intCallClickDelay = Convert.ToInt16(strCallClickDelay);
            callClickTimer = new DispatcherTimer();
            callClickTimer.Tick += new EventHandler(CallClickDelay);//开启监听
            callClickTimer.Interval = new TimeSpan(0, 0, intCallClickDelay);  
        }

        /// <summary>
        /// 开启监听方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CallClickDelay(object sender, EventArgs e)
        {
            callClickTimer.Stop();
            clickState = ClickStateEnum.STOP;
        }
    }

    public partial class CallUserControl : UserControl
    {
        public e_OperaState operaState = e_OperaState.NULL; // 操作状态

        /* 电话List分类 start  */
        public List<GroupData> PageKey = new List<GroupData>();                     // 键权电话

        public List<s_ListUser> PageUser = new List<s_ListUser>();                  // 用户电话
        public List<int> PageUserIndexList = new List<int>();                       // 用户分组所在的页数
        public List<ClickState> UserPhoneClickStateList = new List<ClickState>();   // 记录单击用户号码的状态和号码

        public List<GroupTrunk> PageRelay = new List<GroupTrunk>();                 // 直呼中继电话
        public int PageRelayIndex = 0;                                              // 直呼中继电话所在直呼区的页数
        public List<ClickState> RelayPhoneClickStateList = new List<ClickState>();  // 记录单击中继直呼号码的状态和号码

        public List<GroupBroadcast> PageRadio = new List<GroupBroadcast>();         // 广播电话
        public int PageRadioIndex = 0;                                              // 广播电话所在直呼区的页数
        public List<ClickState> RadioPhoneClickStateList = new List<ClickState>();  // 记录单击中继直呼号码的状态和号码
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

                    /* 给键权电话匹配定时器，查询电话未挂线状态 */
                    PhoneOffHookState phoneOffHookStateTimer = new PhoneOffHookState(mainWindow);
                    phoneOffHookStateTimer.strPhoneId = PageKey[i].extid;
                    phoneOffHookStateTimer.strState = "";
                    phoneOffHookStateList.Add(phoneOffHookStateTimer);
                    SearchOffHookState("", PageKey[i].extid);    // 查询电话不挂线状态
                }
                else
                {
                    keycall.KeyText.Text = "null";
                }

                KeyCallListBox.Items.Add(keycall);
            }
        }

        public List<string> serverCallList = new List<string>();
        /// <summary>
        /// 键权电话单击事件
        /// </summary>
        /// <param name="word"></param>
        private void KeyClickEvent(string word)
        {
            /* 获取键权 */
            /* 查找到该键权电话在不在获取键权的电话号码行列 */
            int intKeyPhoneCount = serverCallList.Count;
            if (intKeyPhoneCount == 0)
            {
                serverCallList.Add(word);
                int intLastNumber = serverCallList.Count - 1;
                serverCall = serverCallList[intLastNumber];
            }
            else
            {
                int intKeyPhoneIndex = 0;
                bool isSeted = false;
                for (int intIndex = 0; intIndex < serverCallList.Count; intIndex++)
                {
                    /* 在键权电话列表中，获取所在的位置索引 */
                    if (word == serverCallList[intIndex])
                    {
                        isSeted = true;
                        intKeyPhoneIndex = intIndex;
                    }
                }

                if (true == isSeted)
                {
                    /* 在获取键权的名单里去除该键权号码 */
                    serverCallList.RemoveAt(intKeyPhoneIndex);
                    /*再添加该键权电话并获取键权 */
                    serverCallList.Add(word);
                    int intLastNumber = serverCallList.Count - 1;
                    serverCall = serverCallList[intLastNumber];
                }
                else 
                {
                    /*再添加该键权电话并获取键权 */
                    serverCallList.Add(word);
                    int intLastNumber = serverCallList.Count - 1;
                    serverCall = serverCallList[intLastNumber];
                }
            }
           
           
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

        /// =====================直呼键区广播代码=====================
        /// <summary>
        /// 直呼区广播电话的分布
        /// </summary>
        public void SetRadioCall()
        {
            /* 创建容器 */
            TabItem tab = new TabItem();            // 造一个新选项卡
            tab.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            //tab.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("{x:Null}"));
            tab.Visibility = Visibility.Collapsed;
            //tab.Visibility = Visibility.Visible;
            ListBox MyWrapPanel2 = new ListBox();   // 选项卡中的容器，用于存放每一个元素
            MyWrapPanel2.Style = FindResource("WrapListBoxStyle") as Style;     // 设定ListBox样式为定义好的样式 WrapListBoxStyle

            /* 处理组员 */
            int groupNum = PageRadio.Count;      // 组员个数
            for (int i = 0; i < groupNum; i++)
            {
                /* 一个组员的信息 */
                UserRadio userRadio = new UserRadio();
                userRadio.name.Text = PageRadio[i].name;
                /* 存放组员 */
                MyWrapPanel2.Items.Add(userRadio);
                /* 组员被点击后的操作 */
                userRadio.ImageSouresHandle += new UserRadio.ImageEventHandler(RadioImageEvent);
                //userRadio.ImageSouresDoubleHandle += new UserRadio.ImageEventHandler(RadioImageDoubleEvent);
            }

            /* 将造好的新选项卡扔进TabControl1里 */
            tab.Content = MyWrapPanel2;
            tabCtrl_User.Items.Add(tab);

            /* 获取广播电话所在直呼区的页数 */
            PageRadioIndex = tabCtrl_User.Items.Count - 1;
        }

        private void RadioImageEvent(string word)
        {
            /* 功能键释放选中 */
            operaState = e_OperaState.NULL;
            FunKeysBorderBrush("");

            RadioHighLight(word);           // 点击该广播组，该广播组高亮
            RadioImageDoubleEvent(word);    // 对每个广播号码发送呼叫命令
            //AddRadioClickItem(word);        // 记录该广播组被点击的状态
        }

        /// <summary>
        /// 广播组单击事件
        /// </summary>
        /// <param name="word">广播组名</param>
        //private void RadioImageEvent(string word)
        //{
        //    /* 功能键释放选中 */
        //    operaState = e_OperaState.NULL;
        //    FunKeysBorderBrush("");

        //    /* 判断该广播组是否已被点击 */
        //    /* 被点击的广播组数为0，确认触发点击该广播组事件 */
        //    if (UserPhoneClickStateList.Count == 0)
        //    {
        //        RadioHighLight(word);           // 点击该广播组，该广播组高亮
        //        RadioImageDoubleEvent(word);    // 对每个广播号码发送呼叫命令
        //        AddRadioClickItem(word);        // 记录该广播组被点击的状态
        //    }
        //    else
        //    {
        //        bool isClickedState = false;

        //        /* 有广播组被点击，判断是否为当前广播组 */
        //        foreach (var item in UserPhoneClickStateList)
        //        {
        //            if ((item.name == word) && (item.clickState == ClickStateEnum.RADIO))
        //            {
        //                isClickedState = true;
        //                Debug.WriteLine(word + "已被点击");
        //            }
        //        }

        //        /* 有广播组被点击，但当前广播组未记录被点击的状态，则触发点击事件 */
        //        if (isClickedState == false)
        //        {
        //            RadioHighLight(word);           // 点击该广播组，该广播组高亮
        //            RadioImageDoubleEvent(word);    // 对每个广播号码发送呼叫命令
        //            AddRadioClickItem(word);        // 记录该广播组被点击的状态
        //        }
        //        /* 查询该广播组的状态，如果所有的电话都是空闲或不在线状态，则可以触发点击事件进行广播 */
        //        else
        //        {
        //        }

        //    }
        //}

        /// <summary>
        /// 广播组高亮
        /// </summary>
        /// <param name="word"></param>
        private void RadioHighLight(string word)
        {
            List<UserRadio> firstPageUserCall = FindChirldHelper.FindVisualChild<UserRadio>(this);

            foreach (var item in firstPageUserCall)
            {
                if (word == item.name.Text)
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
        /// 双击广播号码
        /// </summary>
        /// <param name="word"></param>
        private void RadioImageDoubleEvent(string word)
        {
            GroupBroadcast callRadio = new GroupBroadcast();
            callRadio = PageRadio.Find(c => c.name.Equals(word));

            call callBoard = new call();
            foreach (GroupMember item in callRadio.bmemberlist)
            {
                callBoard.fromid = "1";
                callBoard.toid = item.callno;
                string strMsg = "CMD#MenuToExt#" + JsonConvert.SerializeObject(callBoard);
                mainWindow.ws.Send(strMsg);
            }
        }

        /// <summary>
        /// 添加广播组被点击的状态
        /// </summary>
        /// <param name="word"></param>
        private void AddRadioClickItem(string word)
        {
            ClickState clickStateItem = new ClickState();
            clickStateItem.name = word;
            clickStateItem.clickState = ClickStateEnum.RADIO;
            UserPhoneClickStateList.Add(clickStateItem);
        }
        //============================================================

        /// =====================直呼键区中继代码=====================
        /// <summary>
        /// 直呼中继电话的分布
        /// </summary>
        public void SetRelayCall()
        {
            /* 创建容器 */
            TabItem tab = new TabItem();            // 造一个新选项卡
            tab.Visibility = Visibility.Collapsed;
            tab.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            //tab.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("{x:Null}"));
            //tab.Visibility = Visibility.Visible;
            ListBox MyWrapPanel2 = new ListBox();   // 选项卡中的容器，用于存放每一个元素
            MyWrapPanel2.Style = FindResource("UserRelayWrapListBoxStyle") as Style;

            /* 处理组员 */
            int groupNum = PageRelay.Count;      // 组员个数
            for (int i = 0; i < groupNum; i++)
            {
                /* 一个组员的信息 */
                UserRelay userRelay = new UserRelay();
                userRelay.Soures = PageRelay[i];
                //userRelay.name.Text = PageRelay[i].name;
                //userRelay.trunkid.Text = PageRelay[i].trunkid;
                //userRelay.bindingnumber.Text = PageRelay[i].bindingnumber;
                /* 存放组员 */
                MyWrapPanel2.Items.Add(userRelay);
                /* 组员被点击后的操作 */
                userRelay.ImageSouresHandle += new UserRelay.ImageEventHandler(RelayImageEvent);
                //userRelay.ImageSouresDoubleHandle += new UserRelay.ImageEventHandler(RelayImageDoubleEvent);
                /* 获取组员初始状态 */
                //string strMsg = "CMD#GETSTATE#" + userRelay.trunkid.Text;
                string strMsg = "CMD#GETSTATE#" + userRelay.Soures.trunkid;
                mainWindow.ws.Send(strMsg);
            }

            /* 将造好的新选项卡扔进TabControl1里 */
            tab.Content = MyWrapPanel2;
            tabCtrl_User.Items.Add(tab);

            /* 获取直呼中继电话所在直呼区的页数 */
            PageRelayIndex = tabCtrl_User.Items.Count - 1;
        }

        private void RelayImageEvent(GroupTrunk word)
        {
            operaState = e_OperaState.NULL;
            FunKeysBorderBrush("");

            List<UserRelay> firstPageUserCall = FindChirldHelper.FindVisualChild<UserRelay>(this);

            foreach (var item in firstPageUserCall)
            {
                //if (word.trunkid == item.trunkid.Text)
                if (word.trunkid == item.Soures.trunkid)
                {
                    item.ButtonBack.BorderBrush = Brushes.Yellow;
                }
                else
                {
                    item.ButtonBack.BorderBrush = Brushes.Gray;
                }
            }
            RelayImageDoubleEvent(word);
        }

        /// <summary>
        /// 双击中继号码
        /// </summary>
        /// <param name="word"></param>
        private async void RelayImageDoubleEvent(GroupTrunk word)
        {
            if ("0" == serverCall)
            {
                var view = new MessageBoxShow();
                view.MsgBoxShowText.Text = "当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！";
                var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
            }
            else
            {
                if (("0" == word.bindingnumber) || (null == word.bindingnumber))
                {
                    var view = new MessageBoxShow();
                    view.MsgBoxShowText.Text = "当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！";
                    var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
                    //MessageBox.Show("当前中继未绑定终端电话！", "呼叫信息");
                }
                else
                {
                    callRel tellCall = new callRel() { fromid = serverCall, toid = word.bindingnumber, trunkid = word.trunkid };
                    string strMsg = "CMD#CallOut#" + JsonConvert.SerializeObject(tellCall);
                    mainWindow.ws.Send(strMsg);
                }
            }
        }
        //============================================================


        /// =====================直呼键区用户代码=====================
        /// <summary>
        /// 直呼区不同组用户电话的分布
        /// </summary>
        private void SetUserCall()
        {
            /* 创建容器 */
            int idex = PageUser.Count - 1;
            TabItem tab = new TabItem(){Header = PageUser[idex].Header, FontSize = 24, Height = 40, Width = 100};            // 造一个新选项卡       
            tab.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            ListBox MyWrapPanel2 = new ListBox();   // 选项卡中的容器，用于存放每一个元素
            MyWrapPanel2.Name = "name"+tab.Header.ToString();
            MyWrapPanel2.Style = FindResource("WrapListBoxStyle") as Style;

            /* 处理组员 */
            int groupNum = PageUser[idex].GroupUser.Count;      // 组员个数
            for (int i = 0; i < groupNum; i++)
            {
                /* 一个组员的信息  */
                UserCall userCall = new UserCall();
                string name = PageUser[idex].GroupUser[i].name;
                userCall.setContent(name);                      // Id
                userCall.SetValue("");                          // 被叫号码
                userCall.ContentFrom = PageUser[idex].GroupUser[i].extid;

                /* 存放组员 */
                MyWrapPanel2.Items.Add(userCall);
                /* 组员被点击后的操作 */
                userCall.ImageSouresHandle += new UserCall.ImageEventHandler(ImageEvent);
                //userCall.ImageSouresDoubleHandle += new UserCall.ImageEventHandler(ImageDoubleEvent);
                /* 获取组员初始状态 */
                string strMsg = "CMD#GETSTATE#" + userCall.ContentFrom;         
                mainWindow.ws.Send(strMsg);

                /* 给用户电话匹配定时器，查询电话未挂线状态 */
                PhoneOffHookState phoneOffHookStateTimer = new PhoneOffHookState(mainWindow);
                phoneOffHookStateTimer.strPhoneId = name;
                phoneOffHookStateTimer.strState = "";
                phoneOffHookStateList.Add(phoneOffHookStateTimer);
                SearchOffHookState("", name);    // 查询电话不挂线状态
            }

            /* 将造好的新选项卡扔进TabControl1里 */
            tab.Content = MyWrapPanel2;
            tabCtrl_User.Items.Add(tab);
            /* 默认TabControl1当前页为用户分组页的第一页 */                      
        }


        /// <summary>
        /// 点击事件进行传用户号码
        /// 对控件高亮，其余变暗
        /// </summary>
        private async void ImageEvent(string word)
        {
            clientCall = word;
            Debug.WriteLine("clientCall" + clientCall);
            call tellCall = new call();
            string strMsg;

            // 高亮
            List<UserCall> firstPageUserCall = FindChirldHelper.FindVisualChild<UserCall>(this);
            foreach (var item in firstPageUserCall)
            {
                //if (word == item.phoneNum)
                if (word == item.ContentFrom)
                {
                    item.ButtonBack.BorderBrush = Brushes.Yellow;
                }
                else
                {
                    item.ButtonBack.BorderBrush = Brushes.Gray;
                }
            }

            UserCall userNow = firstPageUserCall.Find((UserCall s) => s.ContentFrom.ToString() == word); // 当前用户电话
            // 按键触发事件
            switch (operaState)
            {
                case e_OperaState.CALL:
                case e_OperaState.NULL:
                    switch(userNow.ThreeSideCallState)
                    {
                        case "INSTER":
                            if (serverCall == userNow.insterNum)
                            {
                                /* 发送强拆对方电话消息 */
                                call sendCall = new call();
                                sendCall.fromid = userNow.insterNum.ToString();
                                sendCall.toid = userNow.NameToId.ToString();
                                strMsg = "CMD#Clear#" + JsonConvert.SerializeObject(sendCall);
                                mainWindow.ws.Send(strMsg);
                                /* 显示强拆消息 */
                                mainWindow.ShowKeyLabel.Content = "键权电话强拆" + userNow.NameToId;
                                mainWindow.ShowKeyLabel.Foreground = Brushes.Black;
                                /* 呼叫方变为键权电话 */ 
                                userNow.NameToId = serverCall;
                                /* 清除强插代码 */ 
                                userNow.insterNum = "0";
                                userNow.ThreeSideCallState = "";
                            }
                            else
                            {
                            //    tellCall = new call() { fromid = serverCall, toid = clientCall };
                            //    strMsg = "CMD#Bargein#" + JsonConvert.SerializeObject(tellCall);
                            //    mainWindow.ws.Send(strMsg);
                            //    mainWindow.ShowKeyLabel.Content = "键权电话强插" + clientCall;
                            //    mainWindow.ShowKeyLabel.Foreground = Brushes.Black;
                            //    userNow.ThreeSideCallState = "INSTER";
                            //    userNow.insterNum = serverCall;
                            //    Debug.WriteLine("键权电话强插" + clientCall);
                            }
                            break;
                        case "LISTEN":
                            if (serverCall == userNow.insterNum)
                            {
                                /* 发送强拆键权电话 */
                                call sendCall = new call();
                                sendCall.fromid = serverCall;
                                sendCall.toid = serverCall;
                                strMsg = "CMD#Clear#" + JsonConvert.SerializeObject(sendCall);
                                mainWindow.ws.Send(strMsg);
                                /* 显示强拆消息 */
                                mainWindow.ShowKeyLabel.Content = "监听取消";
                                mainWindow.ShowKeyLabel.Foreground = Brushes.Black;
                                /* 清除强插代码 */
                                userNow.insterNum = "0";
                                userNow.ThreeSideCallState = "";
                            }
                            else
                            {
                            }
                            break;
                        default:
                            if ((userNow.CurrentState == "ANSWER") || (userNow.CurrentState == "ANSWERED"))
                            {
                                bool isClickedState = false;
                                /* 判断呼叫方是否为键权电话 */
                                foreach (var item in PageKey)
                                {
                                    if (userNow.NameToId.ToString() == item.name)
                                    {
                                        isClickedState = true;
                                        Debug.WriteLine("键权电话" + word + "已被点击");
                                    }
                                }
                                /* 呼叫方若为键权电话，则再次点击用户电话就强拆通话 */
                                if (isClickedState == true)
                                {
                                    tellCall = new call() { fromid = serverCall, toid = clientCall };
                                    strMsg = "CMD#Clear#" + JsonConvert.SerializeObject(tellCall);
                                    mainWindow.ws.Send(strMsg);
                                    mainWindow.ShowKeyLabel.Content = "键权电话强拆" + userNow.NameFromId;
                                    mainWindow.ShowKeyLabel.Foreground = Brushes.Black;
                                }
                                /* 呼叫方不为键权电话，则再次点击用户电话就时键权电话强插通话 */
                                else
                                {
                                    tellCall = new call() { fromid = serverCall, toid = clientCall };
                                    strMsg = "CMD#Bargein#" + JsonConvert.SerializeObject(tellCall);
                                    mainWindow.ws.Send(strMsg);
                                    mainWindow.ShowKeyLabel.Content = "键权电话强插" + clientCall;
                                    mainWindow.ShowKeyLabel.Foreground = Brushes.Black;
                                    userNow.ThreeSideCallState = "INSTER";
                                    userNow.insterNum = serverCall;
                                    Debug.WriteLine("键权电话强插" + clientCall);
                                }
                            }
                            else
                            {
                                if (UserPhoneClickStateList.Count == 0)
                                {
                                    SendCallManage();
                                }
                                else
                                {
                                    bool isClickedState = false;

                                    /* 有电话被点击，判断是否为当前电话 */
                                    foreach (var item in UserPhoneClickStateList)
                                    {
                                        if ((item.name == clientCall) && (item.clickState == ClickStateEnum.CALL))
                                        {
                                            if (item.clickState == ClickStateEnum.CALL)
                                            {
                                                isClickedState = true;
                                                Debug.WriteLine(word + "已被点击");
                                            }
                                            else if (item.clickState == ClickStateEnum.STOP)
                                            {
                                                isClickedState = false;
                                                UserPhoneClickStateList.Remove(item);
                                            }
                                        }
                                    }

                                    /* 有电话被点击，但当前电话未记录被点击的状态，则触发点击事件 */
                                    if (isClickedState == false)
                                    {
                                        SendCallManage();
                                    }
                                }
                            }
                        break;
                    }
                    break;
                case e_OperaState.INTER:
                    tellCall = new call() { fromid = serverCall, toid = clientCall };
                    strMsg = "CMD#Bargein#" + JsonConvert.SerializeObject(tellCall);
                    mainWindow.ws.Send(strMsg);
                    mainWindow.ShowKeyLabel.Content = "键权电话强插" + clientCall;
                    mainWindow.ShowKeyLabel.Foreground = Brushes.Black;
                    userNow.ThreeSideCallState = "INSTER";
                    userNow.insterNum = serverCall;
                    Debug.WriteLine("键权电话强插" + clientCall);
                    break;
                case e_OperaState.LISTEN:
                    tellCall = new call() { fromid = serverCall, toid = clientCall };
                    strMsg = "CMD#Monitor#" + JsonConvert.SerializeObject(tellCall);
                    mainWindow.ws.Send(strMsg);
                    mainWindow.ShowKeyLabel.Content = "键权电话监听" + clientCall;
                    mainWindow.ShowKeyLabel.Foreground = Brushes.Black;
                    userNow.ThreeSideCallState = "LISTEN";
                    userNow.insterNum = serverCall;
                    Debug.WriteLine("键权电话监听" + clientCall);
                    break;
                case e_OperaState.SPLIT:
                    call sendCallMessage = new call();
                    //sendCallMessage.fromid = userNow.insterNum.ToString();
                    sendCallMessage.fromid = serverCall;
                    sendCallMessage.toid = userNow.NameFromId;
                    strMsg = "CMD#Clear#" + JsonConvert.SerializeObject(sendCallMessage);
                    mainWindow.ws.Send(strMsg);
                    //operaState = e_OperaState.NULL;
                    //FunKeysBorderBrush("");
                    mainWindow.ShowKeyLabel.Content = "键权电话强拆" + clientCall;
                    mainWindow.ShowKeyLabel.Foreground = Brushes.Black;
                    Debug.WriteLine("键权电话强拆" + clientCall);
                    /* 清除强插代码 */
                    userNow.insterNum = "0";
                    userNow.ThreeSideCallState = "";
                    mainWindow.ShowKeyLabel.Content = "";
                    break;
                case e_OperaState.TRANS:
                    /* 如果当前选择的键权电话正处于通话过程中，hold */
                    if (m_keyphone[m_keyIndex].Status == KeyStatus.ESTABLISHED ||
                        m_keyphone[m_keyIndex].Status == KeyStatus.CALLING)
                    {
                        Operation_hold(m_keyphone[m_keyIndex].extid, this);
                    }
                    tellCall = new call() { fromid = serverCall, toid = clientCall };
                    strMsg = "CMD#Call#" + JsonConvert.SerializeObject(tellCall);
                    mainWindow.ws.Send(strMsg);
                    //operaState = e_OperaState.NULL;
                    //FunKeysBorderBrush("");
                    break;
                default:
                    var view = new MessageBoxShow();
                    view.MsgBoxShowText.Text = "错误操作,请选择功能项！";
                    var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
                    //operaState = e_OperaState.NULL;
                    //FunKeysBorderBrush("");
                    break;
            }
            operaState = e_OperaState.NULL;
            FunKeysBorderBrush("");
        }

        private void SendCallManage()
        {
            call tellCall = new call() { fromid = serverCall, toid = clientCall };
            string strMsg = "CMD#Call#" + JsonConvert.SerializeObject(tellCall);
            mainWindow.ws.Send(strMsg);
            Debug.WriteLine("键权电话呼叫AAAAAA" + clientCall);
            ClickState clickStateItem = new ClickState();
            clickStateItem.clickState = ClickStateEnum.CALL;
            clickStateItem.name = clientCall;
            clickStateItem.callClickTimer.Start();
            UserPhoneClickStateList.Add(clickStateItem);
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
        private async void ImageDoubleEvent(string word)
        {
            if ("0" == serverCall)
            {
                var view = new MessageBoxShow();
                view.MsgBoxShowText.Text = "当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！";
                var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
                //MessageBox.Show("当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "呼叫信息");
            }
            else
            {
                if ("0" == clientCall)
                {
                    var view = new MessageBoxShow();
                    view.MsgBoxShowText.Text = "当前终端电话空\r\n请点击终端电话！";
                    var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
                    //MessageBox.Show("当前终端电话空\r\n请点击终端电话！", "呼叫信息");
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
                    //mainWindow.settingDetail.settingDetailViewModel.settingDetail.nightServerId = "0";
                    //item.nightId = "0";
                    tellCall.fromid = item.id;
                    tellCall.toid = item.nightId;
                    item.nightState = "CMD#NightServiceOff#";
                    string strMsg = "CMD#NightServiceOff#" + JsonConvert.SerializeObject(tellCall);
                    mainWindow.ws.Send(strMsg);

                    mainWindow.ShowKeyLabel.Content = "";
                    Debug.WriteLine("键权话机" + item.id + "关闭夜服");
                    //MessageBox.Show("键权话机" + item.id + "\r\n" + "夜服关闭", "夜服信息");
                    //NightService.Background = ((Brush)new BrushConverter().ConvertFromString("#FFCBC7C7"));
                }
            }
            operaState = e_OperaState.NULL;
            FunKeysBorderBrush("");
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
            FunKeysBorderBrush("btn_night");
            call tellCall = new call() { fromid = serverCall, toid = clientCall };
            string strState = "";

            if ((serverCall == "0") || (serverCall == null))
            {
                strState = "serverFalse";
            }

            foreach (var item in keyCallDateList)
            {
                if ((serverCall != "0") && (serverCall != null) && ("0" != item.nightId) && (null != item.nightId))
                {
                    strState = "True";
                }
                //if ((serverCall != item.id))
                //{
                //    strState = "serverFalse";
                //}
                if (("0" == item.nightId) || (null == item.nightId))
                {
                    strState = "nightIdFalse";
                }
            }

            if (strState == "True")
            {
                string nightIdShow = "0";
                foreach (var item in keyCallDateList)
                {
                    item.nightState = "CMD#NightServiceOn#";     // 键权电话的夜服开启状态
                    tellCall.fromid = item.id;
                    tellCall.toid = item.nightId;
                    string strMsg = "CMD#NightServiceOn#" + JsonConvert.SerializeObject(tellCall);
                    mainWindow.ws.Send(strMsg);
                    Debug.WriteLine("键权话机" + item.id + "夜服已开启");
                    nightIdShow = item.nightId;
                }

                mainWindow.ShowKeyLabel.Foreground = Brushes.Red;
                mainWindow.ShowKeyLabel.Content = "夜服已开启，呼叫转接到" + nightIdShow;
                var view = nightServerCloseBtn;
                // var view = new NightServerCloseBtn();
                var result = await DialogHost.Show(view, "UserNightServerDialog", ListViewClosingEventHandler);
            }
            else if (strState == "serverFalse")
            {
                var view = new MessageBoxShow();
                view.MsgBoxShowText.Text = "当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！";
                var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler1);
            }
            else if (strState == "nightIdFalse")
            {
                var view = new MessageBoxShow();
                view.MsgBoxShowText.Text = "点击设置按键设置正确的夜服号码";
                var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler1);
            }
            operaState = e_OperaState.NULL;
            FunKeysBorderBrush("");
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
            operaState = e_OperaState.NULL;
            FunKeysBorderBrush("");
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
            FunKeysBorderBrush("btn_outline");
            //var view = new OutLine(mainWindow);
            var view = mainWindow.outLine;
            view.outLineViewModel.outLineCall.serverNum = serverCall;
            if ((view.outLineViewModel.outLineCall.serverNum == null) || (view.outLineViewModel.outLineCall.serverNum == "0"))
            {
                var view2 = new MessageBoxShow();
                view2.MsgBoxShowText.Text = "键权电话为空！";
                var result = await DialogHost.Show(view2, "MessageBox", ListViewClosingEventHandler1);
                //MessageBox.Show("键权电话为空" +"\r\n"+"请点击键权电话");
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
                        relaynum.relayNum = PageRelay[Idx].trunkid;
                        relaynum.isSelected = false;
                        view.outLineViewModel.outLineCall.relayNumList.Add(relaynum);

                        string strMsg = "CMD#GETSTATE#" + relaynum.relayNum;           //获取电话初始状态
                        mainWindow.ws.Send(strMsg);
                        //relayCall.ImageSouresDoubleHandle += new RelayCall.ImageEventHandler(ReLaDoubleEvent);
                    }
                }

                /* 发送电话簿查询请求 */
                string sequence = GlobalFunAndVar.sequenceGenerator();
                StringBuilder sb = new StringBuilder(100);

                sb.Append("CMD#GETPHONEBOOK#{\"sequence\":");
                sb.Append(JsonConvert.SerializeObject(sequence));
                sb.Append("}");

                Debug.WriteLine("SEND: " + sb.ToString());
                mainWindow.ws.Send(sb.ToString());

                var result = await DialogHost.Show(view, "UserOutLineDialog", ListViewClosingEventHandler1);

            }
        }
        private void ListViewClosingEventHandler1(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }
   
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Setting_Click(object sender, RoutedEventArgs e)
        {
            FunKeysBorderBrush("btn_set");
            var view = mainWindow.settingDetail;
            var result = await DialogHost.Show(view, "UserSettingDetailDialog", ListViewClosingEventHandler1);
        }

        private void SettingDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            mainWindow.outLine.deskTabControl.SelectedIndex = 0;
            foreach (var item in keyCallDateList)
            {
                item.nightState = "CMD#NightServiceOn#";     // 键权电话的夜服开启状态
                item.nightId = mainWindow.settingDetail.settingDetailViewModel.settingDetail.nightServerId;
            }
            operaState = e_OperaState.NULL;
            FunKeysBorderBrush("");
        }

    }
     
}
