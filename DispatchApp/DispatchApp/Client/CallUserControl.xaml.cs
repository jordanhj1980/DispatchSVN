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

    /* 用于呼入事件 */
    public class CallSession
    {
        public string trunkid { get; set; }
        public string visitorid { get; set; }
        public string fromnumber { get; set; }
        public string tonumber { get; set; }
        public string callid { get; set; }
    }
    /* add by twinkle end */
    

    /// <summary>
    /// Interaction logic for CallUserControl.xaml
    /// </summary>
    public partial class CallUserControl : UserControl
    {
        private MainWindow mainWindow;

        public string serverCall = "0";
        public string clientCall = "0";
        public string nightId = "213";
        public string trunkCall;
        public List<GroupData> PageKey = new List<GroupData>();     // 键权电话
        public List<GroupData> PageRelay = new List<GroupData>();   // 中继电话
        public List<GroupData> PageRadio = new List<GroupData>();   // 广播电话

        public List<KeyCallDate> keyCallDateList = new List<KeyCallDate>(); // 存所有键权电话信息

        public RelayState relayState = new RelayState();
        /* add by twinkle 20181106 start  */
        public List<keyphoneinfo> m_keyphone;
        public int m_keyIndex;  /* 确定当前选择的键权电话序号，从0开始 */
        public ObservableCollection<CallSession> m_callQueue;
        /* add by twinkle 20181106 end  */

        public event CtrlSwitchHandler CtrlSwitchEvent;
        public LogWindow logWindow;


        public CallUserControl(MainWindow mmainWindow)
        {
            mainWindow = mmainWindow;
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
            btn_holdoff.Visibility = Visibility.Hidden;
            btn_holdoff.Click += new RoutedEventHandler(btn_holdoff_click);

            /* 清空呼叫队列 */
            lbCallQueue.Items.Clear();
            m_callQueue = new ObservableCollection<CallSession>();
            lbCallQueue.ItemsSource = m_callQueue;
            /* added by twinkle 20181107 end */
        }

        private void ClossCDR()
        {
            tabCtrl_CDR.Visibility = System.Windows.Visibility.Hidden;
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
                //Console.WriteLine(item);
            }

            // 组的总数
            int groupNum = groupName.Count;
            
            // 给各组电话重排序
            List<List<GroupData>> PageChange = new List<List<GroupData>>();
            // 确认键权电话所在的组
            int k0 = 0;
            // 确认中继电话所在的组
            int kT = 0;
            // 确认广播电话所在的组
            int kB = 0;
            // 查找键权电话/中继电话/广播电话所在的位置
            for (int i = 0; i < groupNum; i++)
            {
                PageChange.Add(clickpage.FindAll((GroupData s) => s.groupid == groupName[i]));
                if (groupName[i] == "0")  // 重要：键权电话组的组名为“0”，可能需要改
                {
                    k0 = i;
                }
                else
                {
                    if (groupName[i] == "T")
                    {
                        kT = i;
                        
                    }
                    else
                    {
                        if (groupName[i] == "B")
                        {
                            kB = i;
                        }
                        else
                        {
                            continue;
                        }
                        
                    }
                }
            }
            
            int iK = 3;
            for (int i = 0; i < groupNum; i++)
            {
                if (k0 == i)// 将键权电话放在第一组,中继电话放在第二组
                {
                    PageChange[0] = clickpage.FindAll((GroupData s) => s.groupid == groupName[i]);
                }
                else
                {
                    if (kT == i)// 将中继电话放在第二组
                    {
                        PageChange[1] = clickpage.FindAll((GroupData s) => s.groupid == groupName[i]);
                    }
                    else
                    {
                        if (kB == i)// 将广播电话放在第三组
                        {
                            PageChange[2] = clickpage.FindAll((GroupData s) => s.groupid == groupName[i]);
                            PageRadio = PageChange[2];
                        }
                        else
                        {
                            PageChange[iK] = clickpage.FindAll((GroupData s) => s.groupid == groupName[i]);
                            iK++;
                        }                     
                    }
                }
            }
           
                    
            //第一组电话  键权电话
            //PageKey = clickpage.FindAll((GroupData s) => s.groupid == groupName[0]);
            PageKey = PageChange[0];
            int KeyNum = PageKey.Count;            // 键权电话个数
            KeyCallListBox.Items.Clear();
            for (int i = 0; i < 4; i++ )
            {
                KeyCall keycall = new KeyCall();
                if (i < KeyNum)
                {
                    keycall.KeyText.Text = PageKey[i].extid;
                    keycall.index = i + 1;
                    keycall.ImageSouresHandle += new KeyCall.ImageEventHandler(KeyClickEvent);
                    string strMsg = "CMD#GETSTATE#" + keycall.KeyText.Text;  //获取电话初始状态
                    mainWindow.ws.Send(strMsg);
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
                

            // 第二组电话 中继电话
            PageRelay = PageChange[1];


            /* add by twinkle start */
            int keyphoneNum = PageKey.Count;
            for (int i = 0; i < keyphoneNum; i++)
            {
                m_keyphone[i].extid = PageKey[i].extid;
                m_keyphone[i].Status = KeyStatus.IDLE;
            }
            /* add by twinkle end */

            tabCtrl_User.Items.Clear();
            //其余组作为终端电话
            for (int i = 3; i < groupNum; i++)
            {
                TabItem ti = new TabItem(); //造一个新选项卡
                ti.Header = groupName[i];   //新选项卡的名字
                tabCtrl_User.Items.Add(ti); //将造好的新选项卡扔进TabControl1里

                List<GroupData> PageNow = new List<GroupData>();  
                //PageNow = clickpage.FindAll((GroupData s) => s.groupid == groupName[i]);//把相同组的用户放在同一页面
                PageNow = PageChange[i];
                int buttonNum = PageNow.Count;            //当前页面用户总数

                ListBox MyWrapPanel2 = new ListBox();
                MyWrapPanel2.Style = FindResource("WrapListBoxStyle") as Style;     // 设定ListBox样式为定义好的样式 WrapListBoxStyle

                for (int Idx = 0; Idx < buttonNum; Idx++) // 布置页面按钮
                {
                    string name = PageNow[Idx].extid;
                    string called = "no";
                    UserCall userCall = new UserCall();

                    userCall.setContent(name);                        //Id
                    userCall.SetValue(called);                        //被叫号码
                    MyWrapPanel2.Items.Add(userCall);
               
                    userCall.ImageSouresHandle += new UserCall.ImageEventHandler(ImageEvent);
                    userCall.ImageSouresDoubleHandle += new UserCall.ImageEventHandler(ImageDoubleEvent);
                    string strMsg = "CMD#GETSTATE#" + name;           //获取电话初始状态
                    //Thread.Sleep(100);
                    mainWindow.ws.Send(strMsg);
                }

                ti.Content = MyWrapPanel2;
                // 每造一个新窗口便默认突出显示为新窗口
                tabCtrl_User.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 键权电话单击事件
        /// </summary>
        /// <param name="word"></param>
        private void KeyClickEvent(string word)
        {
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
        }

        /// <summary>
        /// 点击事件进行传用户号码
        /// 对控件高亮，其余变暗
        /// </summary>
        private void ImageEvent(string word)
        {
            clientCall = word;
            Console.WriteLine("clientCall" + clientCall);

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
                    m_callQueue.Add(cals);
                }
            }
            else if (type == "BYE")
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
                for (int i = 0; i < m_keyphone.Count; i++)
                {
                    if (m_keyphone[i].extid == callinstance.fromid || m_keyphone[i].extid == callinstance.toid )
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
            GroupData groupDate = new GroupData() { groupid = "0", extid = clientNum };
            GroupData groupDateT = new GroupData() { groupid = "T", extid = clientNum };
            if (PageKey.Contains(groupDate))
            {
                KeyCommondWord_State(type, data);
            }
            else if (PageRelay.Contains(groupDateT))
            {
                RelayCommondWord_State(type, data);
            }
            else
            {
                //判断命令字 20181010 xiaozi add,终端电话状态判断
                CommondWord_State(type, data);
            }
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
            for (int i = 0; i < 4; i++ )
            {
                item = (KeyCall)KeyCallListBox.Items[i];
                if (clientNum == item.KeyText.Text)
                {
                    item.CurrentState = state;
                    keyCallDateList[i].state = state;
                    if ("BUSY" == state)
                    {
                        KeyClickEvent(clientNum); // 摘机同单击
                        //keyCallDateList[i].nightId = "213";
                        //serverCall = clientNum;                 // 摘机即获得键权电话
                        if ("CMD#NightServiceOn#" == keyCallDateList[i].nightState)
                        {
                            call tellCall = new call() { fromid = "0", toid = "0" };
                            tellCall.fromid = keyCallDateList[i].id;
                            tellCall.toid = keyCallDateList[i].nightId;
                            string strMsg = "CMD#NightServiceOff#" + JsonConvert.SerializeObject(tellCall);
                            mainWindow.ws.Send(strMsg);
                            MessageBox.Show("键权话机" + keyCallDateList[i].id + "\r\n" + "夜服关闭" + "夜服信息");
                        }
                        keyCallDateList[i].nightState = "CMD#NightServiceOff#";
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
            string trunkNum;
            callRel tempName = new callRel();
            RelayCall temp = new RelayCall();
            switch (state)
            {
                case "Ready":
                case "Active":
                case "Progress":
                case "Offline":
                case "Offhook":
                    trunkNum = num.ToString();
                    FindRelayCall(trunkNum);
                    temp.RelaylabelPoleId.Background = Brushes.Yellow;
                    //temp.RelaylabelPoleId.Background = state;
                    break;
                case "BUSY":
                    trunkNum = num.ToString();
                    FindRelayCall(trunkNum);
                    temp = (RelayCall)mainWindow.callBoard.RelayList.Items[relayCallStateIdex];
                    temp.RelaylabelPoleId.Background = Brushes.Yellow;
                    break;
                case "IDLE":
                    trunkNum = num.ToString();
                    FindRelayCall(trunkNum);
                    temp = (RelayCall)mainWindow.callBoard.RelayList.Items[relayCallStateIdex];
                    temp.RelaylabelPoleId.Background = Brushes.Green;
                    break;
                case "ONLINE":
                    trunkNum = num.ToString();
                    FindRelayCall(trunkNum);
                    temp = (RelayCall)mainWindow.callBoard.RelayList.Items[relayCallStateIdex];
                    temp.RelaylabelPoleId.Background = Brushes.Green;
                    break;
                case "OFFLINE":
                    trunkNum = num.ToString();
                    FindRelayCall(trunkNum);
                    temp = (RelayCall)mainWindow.callBoard.RelayList.Items[relayCallStateIdex];
                    temp.RelaylabelPoleId.Background = Brushes.Gray;
                    break;
                case "FAILED":
                    tempName = JsonConvert.DeserializeObject<callRel>(num);
                    FindRelayCall(tempName.trunkid);
                    temp = (RelayCall)mainWindow.callBoard.RelayList.Items[relayCallStateIdex];
                    temp.RelaylabelPoleId.Background = Brushes.Gray;
                    break;
                case "BYE":
                    tempName = JsonConvert.DeserializeObject<callRel>(num);
                    FindRelayCall(tempName.trunkid);
                    temp = (RelayCall)mainWindow.callBoard.RelayList.Items[relayCallStateIdex];
                    temp.RelaylabelPoleId.Background = Brushes.Green;
                    break;
                case "RING":
                    tempName = JsonConvert.DeserializeObject<callRel>(num);
                    FindRelayCall(tempName.trunkid);
                    temp = (RelayCall)mainWindow.callBoard.RelayList.Items[relayCallStateIdex];
                    temp.RelaylabelPoleId.Background = Brushes.Blue;
                    break;
                case "ALERT":
                    tempName = JsonConvert.DeserializeObject<callRel>(num);
                    FindRelayCall(tempName.trunkid);
                    temp = (RelayCall)mainWindow.callBoard.RelayList.Items[relayCallStateIdex];
                    temp.RelaylabelPoleId.Background = Brushes.Blue;
                    break;
                case "ANSWER":
                    tempName = JsonConvert.DeserializeObject<callRel>(num);
                    FindRelayCall(tempName.trunkid);
                    temp = (RelayCall)mainWindow.callBoard.RelayList.Items[relayCallStateIdex];
                    temp.RelaylabelPoleId.Background = Brushes.Red;
                    break;
                case "ANSWERED":
                    tempName = JsonConvert.DeserializeObject<callRel>(num);
                    FindRelayCall(tempName.trunkid);
                    temp = (RelayCall)mainWindow.callBoard.RelayList.Items[relayCallStateIdex];
                    temp.RelaylabelPoleId.Background = Brushes.Red;
                    break;
                default: break;
            }
        }

        /// <summary>
        /// 查找中继电话再界面的位置
        /// </summary>
        public int relayCallStateIdex;
        private void FindRelayCall(string callNum)
        {
            for (int idex = 0; idex < mainWindow.callBoard.RelayList.Items.Count; idex++)
            {
                RelayCall temp = (RelayCall)mainWindow.callBoard.RelayList.Items[idex];
                if (temp.RelaylabelNumFromId.Text.ToString() == callNum)
                {
                    relayCallStateIdex = idex;
                }
                else
                {
                    continue;
                }
            }
        }


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
                    if ((callNum == clientNum) || (callNum == clientToid))
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
                                break;
                            case "ONLINE":
                                break;
                            case "OFFLINE":
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
                                temp.labelNumFromId.Content = tempName.fromid;
                                temp.labelNumToId.Content = tempName.toid;
                                temp.timer_Stop();
                                temp.ShowCallTime();
                                break;
                            case "ANSWERED":
                                temp.labelNumFromId.Content =  tempName.fromid;
                                temp.labelNumToId.Content = tempName.toid;
                                temp.timer_Stop();
                                temp.ShowCallTime();
                                break;
                            default: break;
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
                    List<DateCDR> itemList = JsonConvert.DeserializeObject<List<DateCDR>>(data);
                    tabCtrl_CDR.Items.Clear();
                    logWindow.DetialMsg.ItemsSource = itemList;
                    tabCtrl_CDR.Items.Add(logWindow);
                    tabCtrl_CDR.Visibility = System.Windows.Visibility.Visible;
                    //logWin.Show();
                    break;
                default:
                    break;
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
        private void Button_Call(object sender, RoutedEventArgs e)
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
                    //if (MessageBox.Show("主叫：" + serverCall + "\r\n" + "被叫：" + clientCall, "呼叫信息", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    //{
                    //    mainWindow.ws.Send(strMsg);
                       
                    //}
                    //else
                    //{
                    //    MessageBox.Show("请点击“确认”键后输入正确的键权电话和终端电话", "呼叫信息");
                    //}
                }
            }
        }
        /// <summary>
        /// add by twinkle 20181106
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Transfer(object sender, RoutedEventArgs e)
        {
            string extid = m_keyphone[m_keyIndex].extid;
            if ("0" == extid)
            {
                MessageBox.Show("当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "呼叫信息");
            }
            else if ("0" == clientCall)
            {
                MessageBox.Show("当前终端电话空\r\n请点击终端电话！", "呼叫信息");
            }
            else
            {
                /* 如果当前选择的键权电话正处于通话过程中，hold */
                if (m_keyphone[m_keyIndex].Status == KeyStatus.ESTABLISHED ||
                    m_keyphone[m_keyIndex].Status == KeyStatus.CALLING)
                {
                    Operation_hold(extid, this);
                }

                call tellCall = new call() { fromid = serverCall, toid = clientCall };
                string strMsg = "CMD#Call#" + JsonConvert.SerializeObject(tellCall);
                mainWindow.ws.Send(strMsg);
            }
        }

        //private void Button_I(object sender, RoutedEventArgs e)
        //{

        //}

        private void Button_Hold(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Insert(object sender, RoutedEventArgs e)
        {
            if ("0" == serverCall)
            {
                MessageBox.Show("错误操作\r\n当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "强插信息");
            }
            else
            {
                if ("0" == clientCall)
                {
                    MessageBox.Show("错误操作\r\n当前终端电话空\r\n请在通话过程中强插！", "强插信息");
                }
                else
                {
                    call insertCall = new call() { fromid = serverCall, toid = clientCall };
                    string strMsg = "CMD#Bargein#" + JsonConvert.SerializeObject(insertCall);
                    mainWindow.ws.Send(strMsg);
                    //if (MessageBox.Show("键权电话：" + serverCall + "\r\n" + "被叫：" + clientCall, "强插信息", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    //{
                    //    mainWindow.ws.Send(strMsg);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("请点击“确认”键后输入正确的键权电话和终端电话", "强插信息");
                    //}
                }
            }
        }

        private void Button_Split(object sender, RoutedEventArgs e)
        {
            if ("0" == serverCall)
            {
                MessageBox.Show("错误操作\r\n当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "强拆信息");
            }
            else
            {
                if ("0" == clientCall)
                {
                    MessageBox.Show("错误操作\r\n当前终端电话空\r\n请在通话过程中强拆！", "强拆信息");
                }
                else
                {
                    string strMsg = "CMD#Clear#" + clientCall;
                    mainWindow.ws.Send(strMsg);
                    //if (MessageBox.Show("强拆终端：" + clientCall, "强拆信息", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    //{
                    //    mainWindow.ws.Send(strMsg);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("请点击“确认”键后输入正确的终端电话", "强拆信息");
                    //}
                }
            }
        }

        private void Button_Hangoff(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Monitor(object sender, RoutedEventArgs e)
        {
            if ("0" == serverCall)
            {
                MessageBox.Show("错误操作\r\n当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "监听信息");
            }
            else
            {
                if ("0" == clientCall)
                {
                    MessageBox.Show("错误操作\r\n当前终端电话空\r\n请在通话过程中监听！", "监听信息");
                }
                else
                {
                    call monitorCall = new call() { fromid = serverCall, toid = clientCall };
                    string strMsg = "CMD#Monitor#" + JsonConvert.SerializeObject(monitorCall);
                    mainWindow.ws.Send(strMsg);
                    //if (MessageBox.Show("键权电话：" + serverCall + "\r\n" + "被监听电话：" + clientCall, "强拆信息", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    //{
                    //    mainWindow.ws.Send(strMsg);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("请点击“确认”键后输入正确的键权电话和终端电话", "监听信息");
                    //}
                }
            }
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


        //private void Button_KeyRightCallFirst(object sender, RoutedEventArgs e)
        //{
        //    serverCall = KeyLeft1.Text;
        //    KeyCallState.id = KeyLeft1.Text;
        //    KeyCallState.nightId = "215";
        //    KeyCallState.index = 0;
        //    KeyLeftImage1.Source = (new BitmapImage(new Uri("../Resources/phone_on.png", UriKind.RelativeOrAbsolute)));
        //}

        //private void Button_KeyRightCallSecond(object sender, RoutedEventArgs e)
        //{
        //    serverCall = KeyRight1.Text;
        //    KeyCallState.id = KeyRight1.Text;
        //    KeyCallState.nightId = "215";
        //    KeyCallState.index = 1;
        //    KeyRightImage1.Source = (new BitmapImage(new Uri("../Resources/phone_on.png", UriKind.RelativeOrAbsolute)));
        //}

        //private void Button_KeyRightCallThird(object sender, RoutedEventArgs e)
        //{
        //    serverCall = KeyLeft2.Text;
        //    KeyCallState.id = KeyLeft2.Text;
        //    KeyCallState.nightId = "215";
        //    KeyCallState.index = 2;
        //    KeyLeftImage2.Source = (new BitmapImage(new Uri("../Resources/phone_on.png", UriKind.RelativeOrAbsolute)));
        //}
        //private void Button_KeyRightCallSFourth(object sender, RoutedEventArgs e)
        //{
        //    serverCall = KeyRight2.Text;
        //    KeyCallState.id = KeyRight2.Text;
        //    KeyCallState.nightId = "215";
        //    KeyCallState.index = 3;
        //    KeyRightImage2.Source = (new BitmapImage(new Uri("../Resources/phone_on.png", UriKind.RelativeOrAbsolute)));
        //}

        
        private void Night_Click(object sender, RoutedEventArgs e)
        {
            call tellCall = new call() { fromid = serverCall, toid = clientCall };

            foreach (var item in keyCallDateList)
            {
                if ((serverCall == item.id) && ("0" != item.nightId) && (null != item.nightId))
                { 
                    if ("CMD#NightServiceOn#" == item.nightState)
                    {
                        tellCall.fromid = item.id;
                        tellCall.toid = item.nightId;
                        item.nightState = "CMD#NightServiceOff#";
                        string strMsg = "CMD#NightServiceOff#" + JsonConvert.SerializeObject(tellCall);
                        mainWindow.ws.Send(strMsg);
                        MessageBox.Show("键权话机" + item.id + "\r\n" + "夜服关闭", "夜服信息");
                        //NightService.Background = ((Brush)new BrushConverter().ConvertFromString("#FFCBC7C7"));
                    }
                    else
                    {    
                        //NightService.Background = Brushes.Green;              // 夜服按键背景颜色变红
                        item.nightState = "CMD#NightServiceOn#";     // 键权电话的夜服开启状态
                        tellCall.fromid = item.id;
                        tellCall.toid = item.nightId;
                        string strMsg = "CMD#NightServiceOn#" + JsonConvert.SerializeObject(tellCall);
                        mainWindow.ws.Send(strMsg);
                        MessageBox.Show("键权话机" + item.id + "\r\n" + "夜服开启" + "\r\n" + "呼转至" + item.nightId, "夜服信息");                  
                    }
                }
            }
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
            string strMsg = "CMD#GETCDR#ALL";
            mainWindow.ws.Send(strMsg);
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
            call callRadio = new call();

            //if (0 == radioState)
            foreach (GroupData item in PageRadio)
            {
                callRadio.fromid = "1";
                callRadio.toid = item.extid;
                string strMsg = "CMD#MenuToExt#" + JsonConvert.SerializeObject(callRadio);
                mainWindow.ws.Send(strMsg);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }


        //=========================================================================


       
       
    }
}
