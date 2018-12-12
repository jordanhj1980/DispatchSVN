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

using Npgsql;

using System.Windows.Interop;
using System.Runtime.InteropServices;
using WebSocket4Net;
using Newtonsoft.Json;

using System.Threading;
using System.Diagnostics;
using MaterialDesignThemes.Wpf.Transitions;
using MaterialDesignThemes.Wpf;

// 终端用户界面
namespace DispatchApp
{
    /// <summary>
    /// 主机电话/呼叫电话/中继号码
    /// </summary>
    public struct callRel
    {
        public string fromid;
        public string toid;
        public string trunkid;
    }

    /// <summary>
    /// 主机电话/呼叫电话
    /// </summary>
    public struct call
    {
        public string fromid;
        public string toid;
    }

    /// <summary>
    /// 中继电话状态
    /// </summary>
    public struct RelayState
    {
        public string type;
        public string data;
    }

    public class KeyCallDate
    {
        public string id;
        public string state;
        public string nightId;
        public string nightState;
        public int index;
    }

    /* add by twinkle start 20181106*/
    public enum KeyStatus
    {
        OFFLINE,        /* 离线状态 */
        IDLE,           /* 空闲状态 */
        CALLING,        /* 正在呼叫 */
        ESTABLISHED,    /* 通话连接 */
    }

    public class keyphoneinfo
    {
        public KeyStatus Status { get; set; }
        public string extid { get; set; }
    }

    /* 用于外部呼入事件 */
    public class CallSession
    {
        public string trunkid { get; set; }
        public string visitorid { get; set; }
        public string fromnumber { get; set; }
        public string tonumber { get; set; }
        public string callid { get; set; }
    }
    
    public class UI_CallSession : NotifyObject
    {
        public string trunkid { get; set; }
        public string visitorid { get; set; }
        public string fromnumber { get; set; }
        public string tonumber { get; set; }
        public string callid { get; set; }

        /* 呼叫状态 */
        public string _CurrentState;
        public string CurrentState
        {
            get { return _CurrentState; }
            set
            {
                _CurrentState = value;
                OnPropertyChanged("CurrentState");
            }
        }
    }
    /* add by twinkle end */    

    /// <summary>
    /// Interaction logic for CallUserControl.xaml
    /// </summary>
    public partial class CallUserControl : UserControl
    {
        private MainWindow mainWindow;

        public CallBoard callBoard;
        //public OutLine outLine;

        public string serverCall = "0";
        public string clientCall = "0";
        public string nightId = "213";
        public string trunkCall;
        

        public List<KeyCallDate> keyCallDateList = new List<KeyCallDate>(); // 存所有键权电话信息
        public RelayState relayState = new RelayState();
        /* add by twinkle 20181106 start  */
        public List<keyphoneinfo> m_keyphone;
        public int m_keyIndex;  /* 确定当前选择的键权电话序号，从0开始 */
        public ObservableCollection<UI_CallSession> m_callQueue;
        /* add by twinkle 20181106 end  */

        public event CtrlSwitchHandler CtrlSwitchEvent;
        public LogWindow logWindow;



        /* add by xiaozi 20181128 start  */
        public OutLineCall outLineCall;
        /* add by xiaozi 20181128 end  */


        public CallUserControl(MainWindow mmainWindow)
        {
            mainWindow = mmainWindow;
            callBoard = new CallBoard(this);
            //outLine = new OutLine();
            InitializeComponent();

            DataContext = this;
         
            logWindow = new LogWindow(this);
            logWindow.clossCDR += new CtrlSwitchHandler(ClossCDR);

            keyCallDateList.Clear();

            /* added by twinkle 20181107 start */
            /* 初始化键权列表的list */
            m_keyIndex = 0;
            m_keyphone = new List<keyphoneinfo>(4);
            for (int i = 0; i < 4; i++)
            {
                keyphoneinfo info = new keyphoneinfo();
                info.extid = "";   /* 赋值为空字符串*/
                info.Status = KeyStatus.OFFLINE;
                m_keyphone.Add(info);
            }

            /* 隐藏“呼叫保持”按钮 */
            //btn_holdoff.Visibility = Visibility.Hidden;
            //btn_holdoff.Click += new RoutedEventHandler(btn_holdoff_click);

            /* 清空呼叫队列 */
            lbCallQueue.Items.Clear();
            m_callQueue = new ObservableCollection<UI_CallSession>();
            lbCallQueue.ItemsSource = m_callQueue;
            /* added by twinkle 20181107 end */

            /* add by xiaozi 20181128 start  */
            outLineCall = new OutLineCall();
            /* add by xiaozi 20181128 end  */

            // 初始化界面应隐藏日志界面
            LogCtrl.Visibility = Visibility.Hidden;


        }

        private void ClossCDR()
        {
            //tabCtrl_CDR.Visibility = System.Windows.Visibility.Hidden;
            LogCtrl.Visibility = System.Windows.Visibility.Hidden;
            tabCtrl_User.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void callUser_click(object sender, RoutedEventArgs e)
        {
            if (CtrlSwitchEvent != null)
            {
                CtrlSwitchEvent();
            }
        }


        /// =======================页面上布置用户号码======================
        // 20181008 XiaoZi Add 传参的线程
        /// <summary>
        /// 将从服务器传来的参数传入页面，布置用户按钮
        /// </summary>
        /// <param name="msg"></param>
        private delegate void outputDelegate(List<GroupData> msg);
        public void output(List<GroupData> msg)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new outputDelegate(getButtons), msg);
        }

        private void getButtons(List<GroupData> clickpage)
        {
            // 查询各组名称
            List<string> groupName = new List<string>();
            var queryResuilts = clickpage.Select(s => s.groupid).Distinct();
            foreach (var item in queryResuilts)
            {
                groupName.Add(item);
            }

            // 找到键权电话/中继电话/广播电话/用户电话
            foreach (string name in groupName)
            {
                List<GroupData> item = clickpage.FindAll((GroupData s) => s.groupid == name);
                switch (name)
                {
                    case "0":
                        PageKey = item;
                        c_callTypeInfo.status = e_CallType.Key;
                        SetKeyCall();
                        break;
                    //case "T":
                    //    PageRelay = item;
                    //    c_callTypeInfo.status = e_CallType.RELAY;
                    //    SetRelayCall();
                    //    break;
                    //case "B":
                    //    PageRadio = item;
                    //    c_callTypeInfo.status = e_CallType.RADIO;
                    //    SetRadioCall();
                    //    break;
                    default:
                        s_ListUser user = new s_ListUser();
                        user.GroupUser = item;
                        user.Header = name;
                        PageUser.Add(user);
                        c_callTypeInfo.status = e_CallType.USER;
                        SetUserCall();
                        break;
                }
            }
        
            int KeyNum = PageKey.Count;            // 键权电话个数
            
            /* add by twinkle start */
            //int keyphoneNum = PageKey.Count;
            int keyphoneNum = 2;
            for (int i = 0; i < keyphoneNum; i++)
            {
                if (i < KeyNum)
                {
                    m_keyphone[i].extid = PageKey[i].extid;
                }
                else
                {
                    m_keyphone[i].extid = "";
                }

                
                m_keyphone[i].Status = KeyStatus.IDLE;
            }
            /* add by twinkle end */
        }
        //============================================================


        /// =====================STATE命令字接收事件==================
        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// 
        public void Second_State_Word(string word)
        {
            string strMsg = (string)word;
            int indexstart = 0;
            int indexend = 0;
            indexend = strMsg.IndexOf('#');
            string type = strMsg.Substring(indexstart, indexend - indexstart);
            //去第二个#之后的数据
            indexstart = indexend + 1;
            string data = strMsg.Substring(indexstart);

            /* add by twinkle start */
            /* 匹配了ANSWER和ANSWERED*/
            if (type.StartsWith("ANSWER"))
            {
                call callinstance = JsonConvert.DeserializeObject<call>(data);

                /* 判断fromid和toid是否是键权电话 */
                for (int i = 0; i < m_keyphone.Count; i++)
                {
                    if (m_keyphone[i].extid == callinstance.fromid || m_keyphone[i].extid == callinstance.toid)
                    {
                        /* 如果是键权电话，置状态为通话建立中 */
                        m_keyphone[i].Status = KeyStatus.ESTABLISHED;
                        break;
                    }
                }
            }
            else if (type == "INVITE")
            {
                /* 显示在呼叫队列表中 */
                CallSession cals = JsonConvert.DeserializeObject<CallSession>(data);
                if (cals != null)
                {
                    //lbCallQueue.Items.Add(cals);
                    UI_CallSession uicall = new UI_CallSession();
                    uicall.callid = cals.callid;
                    uicall.trunkid = cals.trunkid;
                    uicall.visitorid = cals.visitorid;
                    uicall.fromnumber = cals.fromnumber;
                    uicall.tonumber = cals.tonumber;
                    uicall.CurrentState = "IDLE";   /**/
                    m_callQueue.Add(uicall);
                }
            }
            else if (type == "BYE" || type == "FAIL")   /* 对方挂断电话或者拒接 */
            {
                call callinstance = JsonConvert.DeserializeObject<call>(data);
                /* 判断队列中是否有对应号码 */
                for (int i = 0; i < m_callQueue.Count; i++)
                {
                    // Step1:判断是否在呼叫队列中
                    if (m_callQueue[i].fromnumber == callinstance.fromid || m_callQueue[i].fromnumber == callinstance.toid)
                    {
                        /* 去掉列表 */
                        m_callQueue.RemoveAt(i);
                        break;
                    }                    
                }

                // Step2:判断是否为键权电话
                /**
                 * 对方挂断电话 STATE#BYE#{"fromid":"18163350377","toid":"220"}
                 * 217挂断电话 STATE#BYE#{"fromid":"217","toid":"220"}
                 * isHolding有针对的对方号码
                 */

                for (int i = 0; i < m_keyphone.Count; i++)
                {
                    /*  如果是键权电话挂机，则无holdoff按钮 */
                    if (m_keyphone[i].extid == callinstance.fromid  )
                    {
                        m_keyphone[i].Status = KeyStatus.IDLE;
                        /* 隐藏 */
                        if (!isHolding)
                        {
                            return;
                        }
                        else
                        {
                            /* 如果是键权电话，将当前键权电话的状态置位IDLE */
                            isHolding = false;                            
                            btn_expand();
                        }
                    }
                    else if (m_keyphone[i].extid == callinstance.toid) 
                    {
                        /* 如果是对方拒接，或者沟通后挂机，则holdoff */
                        if (isHolding)
                        {
                            Operation_unhold(this, null);
                        }
                    }
                }
                // Step3:判断是否为内部电话
            }
            /* add by twinkle end */

            //如果num不只是数字，以下Try..Catch..读取客户端号码
            string clientNum;
            call tempName = new call();
            try
            {
                int intNum = int.Parse(data);
                clientNum = data;
            }
            catch (Exception)
            {
                tempName = JsonConvert.DeserializeObject<call>(data);
                clientNum = tempName.fromid;
            }
            // 查看是否为键权电话
            //GroupData groupDate = new GroupData() { groupid = "0", extid = clientNum };
            //GroupData groupDateT = new GroupData() { groupid = "T", extid = clientNum };
            //if (PageKey.Contains(groupDate))
            //{
                KeyCommondWord_State(type, data);
            //}
            //else if (PageRelay.Contains(groupDateT))
            //{
                RelayCommondWord_State(type, data);
            //}
            //else
            //{
                //判断命令字 20181010 xiaozi add,终端电话状态判断
                CommondWord_State(type, data);
            //}
        }

      

        /// <summary>
        /// 对键权电话状态的处理
        /// </summary>
        /// <param name="state"></param>
        /// <param name="num"></param>
        private void KeyCommondWord_State(string state, string num)
        {
            string clientNum;
            call tempName = new call();

            //如果num不只是数字，以下Try..Catch..读取客户端号码
            try
            {
                int intNum = int.Parse(num);
                clientNum = num;
            }
            catch (Exception)
            {
                tempName = JsonConvert.DeserializeObject<call>(num);
                clientNum = tempName.fromid;
            }

            KeyCall item = new KeyCall();
            for (int i = 0; i < 2; i++ )
            {
                item = (KeyCall)KeyCallListBox.Items[i];
                if (clientNum == item.KeyText.Text)
                {
                    item.CurrentState = state;
                    mainWindow.CurrentState = state;
                    keyCallDateList[i].state = state;
                    if ("BUSY" == state)
                    {
                        KeyClickEvent(clientNum); // 摘机同单击
                        nightServerCloseBtn.NightServerClose.Command.Execute(nightServerCloseBtn.NightServerClose.CommandTarget);           // 自动触发button命令事件。 
                        //CommandBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));  // 自动触发button单击事件。
                    }
                    else if ("ALERT" == state)
                    {
                        if ((e_OperaState.INTER != operaState) && (e_OperaState.LISTEN != operaState))
                        { 
                            mainWindow.ShowKeyLabel.Content = tempName.toid + "与" + tempName.fromid + "通话连接中...";
                            mainWindow.ShowKeyLabel.Foreground = Brushes.Black;
                        }
                    }
                    else if ("RING" == state)
                    {
                        if ((e_OperaState.INTER != operaState) && (e_OperaState.LISTEN != operaState))
                        {
                            mainWindow.ShowKeyLabel.Content = tempName.fromid + "与" + tempName.toid + "通话连接中...";
                            mainWindow.ShowKeyLabel.Visibility = Visibility.Visible;
                            mainWindow.ShowKeyLabel.Foreground = Brushes.RosyBrown;
                            //var converter = new System.Windows.Media.BrushConverter();
                            //mainWindow.ShowKeyLabel.Foreground = (Brush)converter.ConvertFromString("#FFFF0090");
                        }
                    }
                    else if (("ANSWERED" == state) || ("ANSWER" == state))
                    {
                        if ((e_OperaState.INTER != operaState) && (e_OperaState.LISTEN != operaState))
                        {
                            mainWindow.ShowKeyLabel.Content = "键权电话与" + tempName.toid + "正在通话";
                            mainWindow.ShowKeyLabel.Foreground = Brushes.Black;
                        }
                    }
                    else
                    {
                        mainWindow.ShowKeyLabel.Content = "";
                    }
                }
                else
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 中继电话的状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="num"></param>
        private void RelayCommondWord_State(string state, string num)
        {
            foreach(RelayNum item in mainWindow.outLine.outLineViewModel.outLineCall.relayNumList)
            {
                if (item.relayNum == num)
                {
                    item.strRelayState = state;
                    Debug.WriteLine("中继电话"+num+"状态："+state);
                }
            }

            int pageRelayBoxNum = ((ListBox)((TabItem)tabCtrl_User.Items[PageRelayIndex]).Content).Items.Count;
            for (int jdex = 0; jdex < pageRelayBoxNum; jdex++)
            {
                // 直呼界面中继电话
                UserRelay t = (UserRelay)(((ListBox)((TabItem)tabCtrl_User.Items[PageRelayIndex]).Content).Items[jdex]);
                //if (t.trunkid.Text == num)
                if (t.Soures.trunkid == num)
                {
                    t.CurrentState = state;
                    Debug.WriteLine("中继电话aaaaa" + num + "状态：" + state);
                }
            } 
        }

        /// <summary>
        /// 查找中继电话在界面的位置
        /// </summary>
        public int relayCallStateIdex;
        private void FindRelayCall(string callNum)
        {
            for (int idex = 0; idex < callBoard.RelayList.Items.Count; idex++)
            {
                RelayCall temp = (RelayCall)callBoard.RelayList.Items[idex];
                //if (temp.RelaylabelNumFromId.Text.ToString() == callNum)
                if (temp.ButtonRelay.Content.ToString() == callNum)
                {
                    relayCallStateIdex = idex;
                }
                else
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 用来判断用户名片上是否刷新时间
        /// </summary>
        public struct StateUser
        {
            public string state;
            public string num;
        }
        public StateUser stateUser;
        private void CommondWord_State(string state, string num)
        {
            string clientNum = "0";
            string clientToid = "0";
            call tempName = new call();

            //如果num不只是数字，以下Try..Catch..读取客户端号码
            try
            {
                int intNum = int.Parse(num);
                clientNum = num;
            }
            catch (Exception)
            {
                tempName = JsonConvert.DeserializeObject<call>(num);
                clientNum = tempName.fromid;
                clientToid = tempName.toid;
            }

            // 查找0：终端号码分布的组数，即TabItem的个数                                   
            int pageUserTabNum = tabCtrl_User.Items.Count;
            // 查找0的解释
            //List<TabItem> PageUserTab = FindChirldHelper.FindVisualChild<TabItem>(this);// 查找CallUserControl里的所有TabItem
            //int pageUserTabNum = PageUserTab.Count; // TabItem的个数

            for (int idex = 0; idex < pageUserTabNum; idex++)
            {
                if ((idex != PageRelayIndex) && (idex != PageRadioIndex))
                {
                // 查找1：每一页的每一个UserCall
                //int pageUserBoxNum = ((ListBox)((TabItem)tabCtrl_User.Items[idex]).Content).Items.Count;
                //for (int jdex = 0; jdex < pageUserBoxNum; jdex++)
                //{
                //    UserCall temp = (UserCall) ((ListBox)((TabItem)tabCtrl_User.Items[idex]).Content).Items[jdex];
                //}
                // 查找1的解释
                TabItem tabItem = (TabItem)tabCtrl_User.Items[idex]; // 一个页面选项
                ListBox listBox = (ListBox)tabItem.Content; // 一个页面选项的内容是ListBox
                int pageUserBoxNum = listBox.Items.Count;   // ListBox中UserCall的个数
                for (int jdex = 0; jdex < pageUserBoxNum; jdex++)
                {
                        UserCall temp = (UserCall)listBox.Items[jdex];  // ListBox中的一个UserCall 

                        string callNum = temp.labelNumFromId.Content.ToString(); // 读取当前UserCall的本机号码                  
                        // 布置本机号码对应的状态
                        //if ((callNum == clientNum) || (callNum == clientToid))
                        if (callNum == clientNum)
                        {
                            temp.CurrentState = state;

                            switch (state)
                            {
                                case "Ready":
                                case "Active":
                                case "Progress":
                                case "Offline":
                                case "Offhook":
                                    break;
                                case "BUSY":
                                    break;
                                case "IDLE":
                                    temp.timer_Stop();
                                    temp.labelNumFromId.Content = callNum;
                                    temp.labelNumToId.Content = "no";
                                    temp.ButtonBack.Background = (Brush)new BrushConverter().ConvertFromString("#CACDDA");
                                    break;
                                case "ONLINE":
                                    temp.ButtonBack.Background = (Brush)new BrushConverter().ConvertFromString("#CACDDA");
                                    break;
                                case "OFFLINE":
                                    temp.ButtonBack.Background = (Brush)new BrushConverter().ConvertFromString("#898B94");
                                    break;
                                case "FAILED":
                                    break;
                                case "BYE":
                                    temp.timer_Stop();
                                    temp.labelNumFromId.Content = callNum;
                                    temp.labelNumToId.Content = "no";
                                    break;
                                case "RING":
                                    temp.labelNumFromId.Content = tempName.fromid;
                                    temp.labelNumToId.Content = tempName.toid;
                                    break;
                                case "ALERT":
                                    temp.labelNumFromId.Content = tempName.fromid;
                                    temp.labelNumToId.Content = tempName.toid;
                                    break;
                                case "ANSWER":
                                    if (((e_OperaState.INTER != operaState) && (e_OperaState.LISTEN != operaState)) || (callNum != clientCall))
                                    {
                                        temp.labelNumFromId.Content = tempName.fromid;
                                        temp.labelNumToId.Content = tempName.toid;
                                        temp.timer_Stop();
                                        temp.ShowCallTime();
                                    }
                                    break;
                                case "ANSWERED":
                                    //if ((("Insert" != stateUser.state) && ("Monitor" != stateUser.state)) || (num != stateUser.num))
                                    if (((e_OperaState.INTER != operaState) && (e_OperaState.LISTEN != operaState)) || (callNum != clientCall))
                                    {
                                        temp.labelNumFromId.Content = tempName.fromid;
                                        temp.labelNumToId.Content = tempName.toid;
                                        temp.timer_Stop();
                                        temp.ShowCallTime();
                                        //stateUser.state = "";
                                        //stateUser.num = num;
                                    }
                                    break;
                                default: break;
                            }
                        }
                    }
                }
            }
        }
        //============================================================

        /// =====================CMD命令字接收事件====================
        public void Second_Cmd_Word(string word)
        {
            string strMsg = (string)word;
            int indexstart = 0;
            int indexend = 0;
            indexend = strMsg.IndexOf('#');
            string type = strMsg.Substring(indexstart, indexend - indexstart);
            //去第二个#之后的数据
            indexstart = indexend + 1;
            string data = strMsg.Substring(indexstart);

            switch (type)
            {
                case "GETCDR":
                    /*List<DateCDR> itemList = JsonConvert.DeserializeObject<List<DateCDR>>(data);
                    tabCtrl_User.Visibility = Visibility.Collapsed;
                    tabCtrl_CDR.Items.Clear();
                    logWindow.DetailMsg.ItemsSource = itemList;
                    tabCtrl_CDR.Items.Add(logWindow);
                    tabCtrl_CDR.Visibility = Visibility.Visible;*/
                    break;
                case "GetUserlog":
                    List<UserLog> userlog = JsonConvert.DeserializeObject<List<UserLog>>(data);
                    DataGridPageViewModel logmodel = new DataGridPageViewModel(userlog);
                    datagridpage.DataContext = logmodel;
                    this.datagridpage.msgevent += new DataGridPage.CWHandler(ClossCDR);
                    break;
                case "GETPHONEBOOK":                    
                    PhoneBook pb = JsonConvert.DeserializeObject<PhoneBook>(data);
                    if (pb != null)
                    {
                        mainWindow.outLine.outLineViewModel.ContactList.Clear();
                        foreach (Department member in pb.departmentlist)
                        {
                            Department item = new Department();
                            item.department = member.department;
                            item.memberlist = new List<PhoneItem>();
                            foreach (PhoneItem it in member.memberlist) {
                                PhoneItem newItem = new PhoneItem();
                                newItem.callno = it.callno;
                                newItem.name = it.name;
                                item.memberlist.Add(newItem);
                            }

                            mainWindow.outLine.outLineViewModel.ContactList.Add(item);
                        }
                    }
                    break;
                case "Call":
                case "CallOut":
                case "Visitor":
                case "Bargein":
                case "Monitor":
                case "NightServiceOn":
                case "NightServiceOff":
                    ShowKeyLabelScreen(type, data);
                    break;
                default:
                    break;
            }
        }

        private void ShowKeyLabelScreen(string type,string data)
        {
            Reply reply = JsonConvert.DeserializeObject<Reply>(data);
            if (reply.result == "Fail")
            {
                switch (type)
                {
                    case "Call":
                    case "CallOut":
                        mainWindow.ShowKeyLabel.Content = "键权电话呼叫失败：" + reply.reason;
                        break;
                    case "Visitor":
                        mainWindow.ShowKeyLabel.Content = "转接失败：" + reply.reason;
                        break;
                    case "Bargein":
                        mainWindow.ShowKeyLabel.Content = "键权电话强插失败：" + reply.reason;
                        break;
                    case "Monitor":
                        mainWindow.ShowKeyLabel.Content = "键权电话监听失败：" + reply.reason;
                        break;
                    case "NightServiceOn":
                        mainWindow.ShowKeyLabel.Content = "夜服开启失败：" + reply.reason;
                        break;
                    case "NightServiceOff":
                        mainWindow.ShowKeyLabel.Content = "夜服关闭失败：" + reply.reason;
                        break;
                    default:
                        mainWindow.ShowKeyLabel.Content = "发生未知错误，检查CMD应答";
                        break;
                }
            }
        }
        ///============================================================


        /// ======================鼠标滑动事件========================
        /// 未用到
        private void ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ItemsControl items = (ItemsControl)sender;
            ScrollViewer scroll = FindVisualChild<ScrollViewer>(items);
            if (scroll != null)
            {
                int d = e.Delta;
                if (d > 0)
                {
                    scroll.LineUp();
                }
                if (d < 0)
                {
                    scroll.LineDown();
                }
                scroll.ScrollToTop();
            }


        }
        public static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }
        //============================================================

        /// ======================功能区按键===========================
        /// <summary>
        /// 呼叫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Call(object sender, RoutedEventArgs e)
        {
            operaState = e_OperaState.CALL;
            FunKeysBorderBrush("btn_call");
            if ("0" == serverCall)
            {
                var view = new MessageBoxShow();
                view.MsgBoxShowText.Text = "当前键权电话空,请点击键权电话或拿起键权电话！";
                var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);

                Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
                //MessageBox.Show("当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "呼叫信息");
            }
            //else
            //{
            //    if ("0" == clientCall)
            //    {
            //        var view = new MessageBoxShow();
            //        view.MsgBoxShowText.Text = "当前终端电话空\r\n请点击终端电话！";
            //        var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
            //        Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
            //        //MessageBox.Show("当前终端电话空\r\n请点击终端电话！", "呼叫信息");
            //    }
            //    else
            //    {
            //        call tellCall = new call() { fromid = serverCall, toid = clientCall };
            //        string strMsg = "CMD#Call#" + JsonConvert.SerializeObject(tellCall);
            //        mainWindow.ws.Send(strMsg);
            //    }
            //}
        }
        /// <summary>
        /// add by twinkle 20181106
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Transfer(object sender, RoutedEventArgs e)
        {
            operaState = e_OperaState.TRANS;
            FunKeysBorderBrush("btn_trans");
            string extid = m_keyphone[m_keyIndex].extid;
            if ("0" == extid)
            {
                var view = new MessageBoxShow();
                view.MsgBoxShowText.Text = "当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！";
                var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
                //MessageBox.Show("当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "呼叫信息");
            }
            //else if ("0" == clientCall)
            //{
            //    var view = new MessageBoxShow();
            //    view.MsgBoxShowText.Text = "当前终端电话空\r\n请点击终端电话！";
            //    var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
            //    //MessageBox.Show("当前终端电话空\r\n请点击终端电话！", "呼叫信息");
            //}
            //else
            //{
            //    /* 如果当前选择的键权电话正处于通话过程中，hold */
            //    if (m_keyphone[m_keyIndex].Status == KeyStatus.ESTABLISHED ||
            //        m_keyphone[m_keyIndex].Status == KeyStatus.CALLING)
            //    {
            //        Operation_hold(extid, this);
            //    }

            //    call tellCall = new call() { fromid = serverCall, toid = clientCall };
            //    string strMsg = "CMD#Call#" + JsonConvert.SerializeObject(tellCall);
            //    mainWindow.ws.Send(strMsg);
            //}
        }


        private void Button_Hold(object sender, RoutedEventArgs e)
        {

        }

        private async void Button_Insert(object sender, RoutedEventArgs e)
        {
            if (operaState == e_OperaState.INTER)
            {
                string strMsg = "CMD#Clear#" + serverCall;
                mainWindow.ws.Send(strMsg);
                operaState = e_OperaState.NULL;
                FunKeysBorderBrush("");
            }
            else
            {
                operaState = e_OperaState.INTER;
                FunKeysBorderBrush("btn_insert");
                if ("0" == serverCall)
                {
                    var view = new MessageBoxShow();
                    view.MsgBoxShowText.Text = "错误操作\r\n当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！";
                    var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
                    //MessageBox.Show("错误操作\r\n当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "强插信息");
                }
            }
            
            //else
            //{
            //    if ("0" == clientCall)
            //    {
            //        var view = new MessageBoxShow();
            //        view.MsgBoxShowText.Text = "错误操作\r\n当前终端电话空\r\n请在通话过程中强插！";
            //        var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
            //        //MessageBox.Show("错误操作\r\n当前终端电话空\r\n请在通话过程中强插！", "强插信息");
            //    }
            //    else
            //    {
            //        call insertCall = new call() { fromid = serverCall, toid = clientCall };
            //        string strMsg = "CMD#Bargein#" + JsonConvert.SerializeObject(insertCall);
            //        mainWindow.ws.Send(strMsg);
            //    }
            //}
        }

        private async void Button_Split(object sender, RoutedEventArgs e)
        {
            operaState = e_OperaState.SPLIT;
            FunKeysBorderBrush("btn_split");
            if ("0" == serverCall)
            {
                var view = new MessageBoxShow();
                view.MsgBoxShowText.Text = "错误操作\r\n当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话!";
                var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
                //MessageBox.Show("错误操作\r\n当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "强拆信息");
            }
            //else
            //{
            //    if ("0" == clientCall)
            //    {
            //        var view = new MessageBoxShow();
            //        view.MsgBoxShowText.Text = "错误操作\r\n当前终端电话空\r\n请在通话过程中强拆！";
            //        var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
            //        //MessageBox.Show("错误操作\r\n当前终端电话空\r\n请在通话过程中强拆！", "强拆信息");
            //    }
            //    else
            //    {
            //        string strMsg = "CMD#Clear#" + clientCall;
            //        mainWindow.ws.Send(strMsg);
            //    }
            //}
        }

        private void Button_Hangoff(object sender, RoutedEventArgs e)
        {

        }

        private async void Button_Monitor(object sender, RoutedEventArgs e)
        {
            if (operaState == e_OperaState.LISTEN)
            {
                string strMsg = "CMD#Clear#" + serverCall;
                mainWindow.ws.Send(strMsg);
                operaState = e_OperaState.NULL;
                FunKeysBorderBrush("");
            }
            else
            {
                operaState = e_OperaState.LISTEN;
                FunKeysBorderBrush("btn_monitor");
                if ("0" == serverCall)
                {
                    var view = new MessageBoxShow();
                    view.MsgBoxShowText.Text = "错误操作\r\n当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！";
                    var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
                    //MessageBox.Show("错误操作\r\n当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "监听信息");
                }
            }
            
            //else
            //{
            //    if ("0" == clientCall)
            //    {
            //        var view = new MessageBoxShow();
            //        view.MsgBoxShowText.Text = "错误操作\r\n当前终端电话空\r\n请在通话过程中监听！";
            //        var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
            //        //MessageBox.Show("错误操作\r\n当前终端电话空\r\n请在通话过程中监听！", "监听信息");
            //    }
            //    else
            //    {
            //        call monitorCall = new call() { fromid = serverCall, toid = clientCall };
            //        string strMsg = "CMD#Monitor#" + JsonConvert.SerializeObject(monitorCall);
            //        mainWindow.ws.Send(strMsg);        
            //    }
            //}
        }

        private void Button_Urgentconf(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Cal(object sender, RoutedEventArgs e)
        {

        }

        
        //private void Night_Click(object sender, RoutedEventArgs e)
        //{
        //    DialogTab.SelectedIndex = 0;
        //    call tellCall = new call() { fromid = serverCall, toid = clientCall };
        //    string strState = "";

        //    foreach (var item in keyCallDateList)
        //    {
        //        if ((serverCall == item.id) && ("0" != item.nightId) && (null != item.nightId))
        //        {
        //            strState = "True";
        //        }
        //    }

        //    if (strState == "True")
        //    {
        //        foreach (var item in keyCallDateList)
        //        {
        //            item.nightState = "CMD#NightServiceOn#";     // 键权电话的夜服开启状态
        //            tellCall.fromid = item.id;
        //            tellCall.toid = item.nightId;
        //            string strMsg = "CMD#NightServiceOn#" + JsonConvert.SerializeObject(tellCall);
        //            mainWindow.ws.Send(strMsg);

        //            mainWindow.ShowKeyLabel.Content = "夜服已开启";
        //            // MessageBox.Show("键权话机" + item.id + "\r\n" + "夜服开启" + "\r\n" + "呼转至" + item.nightId, "夜服信息");     
        //        }               
        //    }

        //    //foreach (var item in keyCallDateList)
        //    //{
        //    //    if ((serverCall == item.id) && ("0" != item.nightId) && (null != item.nightId))
        //    //    { 
        //    //        //if ("CMD#NightServiceOn#" == item.nightState)
        //    //        //{
        //    //        //    tellCall.fromid = item.id;
        //    //        //    tellCall.toid = item.nightId;
        //    //        //    item.nightState = "CMD#NightServiceOff#";
        //    //        //    string strMsg = "CMD#NightServiceOff#" + JsonConvert.SerializeObject(tellCall);
        //    //        //    mainWindow.ws.Send(strMsg);
        //    //        //    MessageBox.Show("键权话机" + item.id + "\r\n" + "夜服关闭", "夜服信息");
        //    //        //    //NightService.Background = ((Brush)new BrushConverter().ConvertFromString("#FFCBC7C7"));
        //    //        //}
        //    //        //else
        //    //        //{    
        //    //            item.nightState = "CMD#NightServiceOn#";     // 键权电话的夜服开启状态
        //    //            tellCall.fromid = item.id;
        //    //            tellCall.toid = item.nightId;
        //    //            string strMsg = "CMD#NightServiceOn#" + JsonConvert.SerializeObject(tellCall);
        //    //            mainWindow.ws.Send(strMsg);
        //    //            MessageBox.Show("键权话机" + item.id + "\r\n" + "夜服开启" + "\r\n" + "呼转至" + item.nightId, "夜服信息");                  
        //    //        //}
        //    //    }
        //    //}
        //}

        public void CloseNightServer(object sender, RoutedEventArgs e)
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
                    //MessageBox.Show("键权话机" + item.id + "\r\n" + "夜服关闭", "夜服信息");
                    //NightService.Background = ((Brush)new BrushConverter().ConvertFromString("#FFCBC7C7"));
                }
            }
        }

        /// <summary>
        /// 功能键高亮功能
        /// </summary>
        /// <param name="name"></param>
        private void FunKeysBorderBrush(string name)
        {
            var converter = new System.Windows.Media.BrushConverter();
            if (name == "btn_call")
                btn_call.Background = (Brush)converter.ConvertFromString("#FFDCDCB8");
            else
                btn_call.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
            if (name == "btn_trans")
                btn_trans.Background = (Brush)converter.ConvertFromString("#FFDCDCB8");
            else
                btn_trans.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
            if (name == "btn_insert")
                btn_insert.Background = (Brush)converter.ConvertFromString("#FFDCDCB8");
            else
                btn_insert.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
            if (name == "btn_split")
                btn_split.Background = (Brush)converter.ConvertFromString("#FFDCDCB8");
            else
                btn_split.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
            if (name == "btn_monitor")
                btn_monitor.Background = (Brush)converter.ConvertFromString("#FFDCDCB8");
            else
                btn_monitor.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
            if (name == "btn_outline")
                btn_outline.Background = (Brush)converter.ConvertFromString("#FFDCDCB8");
            else
                btn_outline.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
            if (name == "btn_relay")
                btn_relay.Background = (Brush)converter.ConvertFromString("#FFDCDCB8");
            else
                btn_relay.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
            if (name == "btn_radio")
                btn_radio.Background = (Brush)converter.ConvertFromString("#FFDCDCB8");
            else
                btn_radio.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
            if (name == "btn_night")
                btn_night.Background = (Brush)converter.ConvertFromString("#FFDCDCB8");
            else
                btn_night.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
            if (name == "btn_log")
                btn_log.Background = (Brush)converter.ConvertFromString("#FFDCDCB8");
            else
                btn_log.Background = (Brush)converter.ConvertFromString("#FFDDDDDD");
        }

        //=========================================================================



        /// ===========================右侧功能区按键===============================
        /// <summary>
        /// 日志功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Log(object sender, RoutedEventArgs e)
        {
            FunKeysBorderBrush("btn_log");
            // 更改GETCDR到GetUserlog
            string strMsg = "CMD#GetUserlog#ALL";
            mainWindow.ws.Send(strMsg);

            tabCtrl_User.Visibility = Visibility.Collapsed;
            LogCtrl.Visibility = Visibility.Visible;// 显示日志界面
        }

        /// <summary>
        /// 查询功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Search(object sender, RoutedEventArgs e)
        {
            //LogWindow Log = new LogWindow();
            //Log.Show();
        }

        /// <summary>
        /// 邮件功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Email(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 会议功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Meeting(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 广播功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //public int radioState = 0; 
        private void Radio_Click(object sender, RoutedEventArgs e)
        {
            FunKeysBorderBrush("btn_radio");
            tabCtrl_User.SelectedIndex = PageRadioIndex;        // 直呼区跳转广播界面
        }

        /// <summary>
        /// 中继直呼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Relay_Click(object sender, RoutedEventArgs e)
        {
            FunKeysBorderBrush("btn_relay");
            tabCtrl_User.SelectedIndex = PageRelayIndex;        // 直呼区跳转中继直呼界面
        }

        /// <summary>
        /// 强挂键权电话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if ("0" == serverCall)
            {
                var view = new MessageBoxShow();
                view.MsgBoxShowText.Text = "错误操作\r\n当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！";
                var result = await DialogHost.Show(view, "MessageBox", ListViewClosingEventHandler);
                //MessageBox.Show("错误操作\r\n当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "强拆信息");
            }
            else
            {
                string strMsg = "CMD#Clear#" + clientCall;
                mainWindow.ws.Send(strMsg);
            }
        }

        /// <summary>
        /// 外线按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_CallKeyBord(object sender, RoutedEventArgs e)
        {
            //DialogTab.SelectedIndex = 1;
            //card.Children.Clear();
            //card.Children.Add(exteriorLine);
            
            
            //callBoard.CallText.Text = "";

            //if (callBoard.RelayList.Items.Count == 0)
            //{
            //    callBoard.deskTabControl.SelectedIndex = 0;   
            //    ((TabItem)(callBoard.deskTabControl.Items[0])).Visibility = Visibility.Hidden;
            //    ((TabItem)(callBoard.deskTabControl.Items[1])).Visibility = Visibility.Hidden;
            //    callBoard.RelayList.Items.Clear();
            //    for (int Idx = 0; Idx < PageRelay.Count; Idx++) // 布置页面按钮
            //    {
            //        string name = PageRelay[Idx].extid;
            //        //string called = "no";
            //        RelayCall relayCall = new RelayCall();

            //        relayCall.setContent(name);                        //Id
            //        //relayCall.SetValue(called);
            //        callBoard.RelayList.Items.Add(relayCall);

            //        relayCall.ImageSouresHandle += new RelayCall.ImageEventHandler(callBoard.ReLaySigleEvent);
            //        string strMsg = "CMD#GETSTATE#" + name;           //获取电话初始状态
            //        mainWindow.ws.Send(strMsg);
            //        //relayCall.ImageSouresDoubleHandle += new RelayCall.ImageEventHandler(ReLaDoubleEvent);
            //    }
            //}
            //callBoard.ShowDialog();
        }








        //=========================================================================


       
       
    }
}
