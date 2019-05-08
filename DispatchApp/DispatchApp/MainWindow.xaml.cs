using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
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
using System.Threading;
using System.Windows.Forms;
using System.Configuration;

using Npgsql;

using System.Windows.Interop;
using System.Runtime.InteropServices;
using WebSocket4Net;
using Newtonsoft.Json;
using System.Diagnostics;
using System.ComponentModel;
using System.Security.Cryptography;


public struct GroupData
{
    public string groupid;
    public string extid;
    public string name;
}

namespace DispatchApp
{
    public enum WINDOWTYPE  //201801010 xf Add
    {
        LOGINWIN,
        MAINWIN
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public OutLine outLine;
        public SettingDetail settingDetail;

        //public PhoneOffHookState phoneOffHookState;
        private DispatcherTimer ShowTimer;
        public DispatcherTimer HeartBeatTimer;

        public WebSocket ws;
        private NotifyIcon _notifyIcon = null;
        private LoginWindow logwin;

        private Brush icon_hilight;
        private Brush icon_pale;
        
        private string m_ServerIP;
        public string serverip
        {
            get { return m_ServerIP; }
            set { 
                m_ServerIP = value;
                // 界面左下角显示服务器IP
                this.serverIP.Text = value;
                saveUserOption();
            }
        }

        private string m_ServerPort;
        private int m_HeartBeat;

        private LoadingWindow loadingWin;
        private LockScreen lockScreen;  // 锁屏窗口

        private string _account;
        public string account
        {
            get { return _account; }
            set { _account = value; }
        }

        private string _password;
        public string password
        {
            get { return _password; }
            set {
                var buffer = Encoding.UTF8.GetBytes(value);
                var data = SHA1.Create().ComputeHash(buffer);
                var sb = new StringBuilder();
                foreach (var t in data)
                {
                    sb.Append(t.ToString("X2"));
                }
                _password = sb.ToString();
            }  // 记录用户密码
        }

        /* 调度usercontrol */
        //UserControl1 callUserCtrl;//weituo 20181013
        public CallUserControl callUserCtrl;
        public CallManagerControl callManagerCtrl;

        // 状态转换器参数绑定
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

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
            // 关闭窗口执行前，触发的可以取消关闭窗口的操作
            this.Closing += Window_Closing;

            DataContext = this;
            CurrentState = "RING";

            icon_hilight = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            icon_pale = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC2C2C2"));


            // 最小化到系统托盘
            //Initial();

            //demoUserControl.Children.Add(new UserControl1());
            //InitBitkyPoleShow();

            //phoneOffHookState = new PhoneOffHookState(this);  // 查询电话未挂机状态 add by xiaozi 20190126

            // 初始化用户界面和管理界面
            callUserCtrl = new CallUserControl(this);
            //callUserCtrl.CtrlSwitchEvent += new CtrlSwitchHandler(CtrlSwitch_callUser);
            callManagerCtrl = new CallManagerControl(this);
            //callManagerCtrl.CtrlSwitchEvent += new CtrlSwitchHandler(CtrlSwitch_callManager);
            outLine = new OutLine(this);
            settingDetail = new SettingDetail(this);
            //CtrlSwitch_callUser();            

            /* 隐藏公共工具栏的button */
            sw_button.Visibility = Visibility.Hidden;
            user_button.Visibility = Visibility.Hidden;
            desk_button.Visibility = Visibility.Hidden;
            contact_button.Visibility = Visibility.Hidden;            

            // 创建websocket
            m_ServerIP = ConfigurationManager.AppSettings["serverip"];
            m_ServerPort = ConfigurationManager.AppSettings["serverport"];
            string heartbeat = ConfigurationManager.AppSettings["heartbeat"];
            m_HeartBeat = Convert.ToInt16(heartbeat);
            Debug.WriteLine("serverip = " + m_ServerIP);

            // 显示登录界面，验证后返回，未登陆前不会触发Window_Loaded事件
            logwin = new LoginWindow(this);
            logwin.setIPaddr(m_ServerIP);
            logwin.Show();

            this.Hide();

            // 创建socket和ui交互机制 // 20181010 xf Add
            PeerCallBack.Instance = new PeerCallBack(SynchronizationContext.Current, this);

            ShowCurTimer(this, null);    //在这里窗体加载的时候不执行文本框赋值，窗体上不会及时的把时间显示出来，而是等待了片刻才显示了出来
            ShowTimer = new DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurTimer);//起个Timer一直获取当前时间
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ShowTimer.Start();

            HeartBeatTimer = new DispatcherTimer();
            HeartBeatTimer.Tick += new EventHandler(HeartBeat);//开启监听
            HeartBeatTimer.Interval = new TimeSpan(0, 0, m_HeartBeat);  

            // 初始化loading界面
            loadingWin = new LoadingWindow();
            lockScreen = new LockScreen(this);
            lockScreen.msgevent += new LockScreen.LockHandler(lockWinClose);
        }

        public void initSocket(string serverip)
        {
            // 创建websocket
            string serveruri = "ws://" + serverip + ":" + m_ServerPort.Trim();
            try
            {
                ws = new WebSocket(serveruri);
                ws.Opened += websocket_Opened;
                ws.Closed += websocket_Closed;
                ws.MessageReceived += websocket_MessageReceived;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public void lockWinClose(object sender, string msg)
        {
            this.Show();
            lockScreen.TxPassword.Clear();
        }

        /// <summary>
        ///     初始化信息显示标签界面
        /// </summary>
        //private void InitBitkyPoleShow()
        //{
        //    //var controls = new List<callButton>();
            
        //        var bitkyPoleControl = new callButton();
        //        //在 Grid 中动态添加控件
        //        grid_bottom.Children.Add(bitkyPoleControl);
        //        //设定控件在 Grid 中的位置
        //        Grid.SetRow(bitkyPoleControl, 0);
        //        Grid.SetColumn(bitkyPoleControl, 0);
        //        var bitkyPole = new transferButton();
        //        //在 Grid 中动态添加控件
        //        grid_bottom.Children.Add(bitkyPole);
        //        //设定控件在 Grid 中的位置
        //        Grid.SetRow(bitkyPole, 0);
        //        Grid.SetColumn(bitkyPole, 2);

        //        //将控件添加到集合中，方便下一步的使用
        //        //controls.Add(bitkyPoleControl);     
        //}

        /// =======================websocket======================
        /// <summary>        
        /// </summary>     
        //20180930 xiaozi Add
        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            //实际代码
            MessageReceivedEventArgs responseMsg = (MessageReceivedEventArgs)e; //接收服务端发来的消息
            string strMsg = responseMsg.Message;
            Debug.WriteLine("接收" + strMsg);
            this.Dispatcher.Invoke(new Action(delegate
            {
                typedata(strMsg);
            }));  
        }

        private void typedata(Object date)
        {
            //Debug.WriteLine("typedata接收" + date);
            // lets see the thread id 20181010 xf Add
            int id = Thread.CurrentThread.ManagedThreadId;
            Debug.WriteLine("Run thread: " + id);

            // 实际代码
            List<GroupData> temp = new List<GroupData>();
            string strMsg = (string)date;
            int indexstart = 0;
            int indexend = 0;

            //去第一个#之后的数据
            indexend = strMsg.IndexOf('#');
            string type = strMsg.Substring(indexstart, indexend - indexstart);
            indexstart = indexend + 1;
            string word = strMsg.Substring(indexstart);
            
            //判断命令字 20181010 xiaozi add
            switch (type)
            {
                case "LOG":
                    PeerCallBack.Instance.GetLoginFeedBack(Convert.ToInt32(WINDOWTYPE.LOGINWIN), word);
                    break; // 20181010 xf Add 
                default:
                    PeerCallBack.Instance.GetOperationMsg(strMsg);
                    break; // 20181010 xf Add   
            }           
        }

        //20180930 xiaozi Add
        private void websocket_Closed(object sender, EventArgs e)
        {
            //websocket.Send("一个客户端 下线");
            Debug.WriteLine("client is closed!!!");
            //Debug.WriteLine("client is closed!!!");
            // 20181010 xf Add
            if (App.isLogin)
            {
                // 网络断开连接
                PeerCallBack.Instance.GetOperationMsg("login"); // commented by twinkle
                // 当用户已经登录的时候，才停止定时器
                HeartBeatTimer.Stop();
            }
            else
            {
                PeerCallBack.Instance.GetLoginFeedBack(Convert.ToInt32(WINDOWTYPE.LOGINWIN), "服务器无法连接！");
            }
        }

        //20180930 xiaozi Add
        void websocket_Opened(object sender, EventArgs e)
        {
            //用登陆界面登陆
            PeerCallBack.Instance.GetLoginFeedBack(Convert.ToInt32(WINDOWTYPE.LOGINWIN), "connected");
        }
        //============================================================

        // add by twinkle 10.10
        public void GetLoginFeedBack(int type, string msg)
        {
            try
            {
                // let's see the thread id
                int id = Thread.CurrentThread.ManagedThreadId;
                Debug.WriteLine("mToolStripButtonThreads_Click thread: " + id);

                logwin.Delog(msg);

                /* 进入用户界面
                if ("Admin" == logwin.logIn)
                {
                    CtrlSwitch_callUser();
                    Debug.WriteLine("用户界面" + logwin.logIn);
                }
                else
                {
                    CtrlSwitch_callManager();
                    Debug.WriteLine("用户界面" + logwin.logIn);
                }*/
                }
            catch (System.Exception exc)
            {
                //System.Windows.MessageBox.Show(exc.Message, Title);
                System.Windows.MessageBox.Show(exc.Message + "axa");
            }
        }

        public void GetOperationMsg(string msg)
        {
            try
            {
                // let's see the thread id
                int id = Thread.CurrentThread.ManagedThreadId;
                Debug.WriteLine("mToolStripButtonThreads_Click thread: " + id);

                if (msg == "login")
                {
                    //logwin.Show();
                    //this.Hide();

                    // add by twinkle 2018.11.23
                    // 用户在当前界面，如果网线断开，停留在当前界面，制作动画，并重新连接后台
                    //loadWin.Show();

                    if (App.isLogin) { 
                        loadingWin.ShowDialog(this);
                        callUserCtrl.KeyCallListBox.Items.Clear();           // 清理键权电话区
                        callUserCtrl.tabCtrl_User.Items.Clear();             // 清理直呼键区
                    }
                } 
                else 
                {
                    string strMsg = (string)msg;
                    int indexstart = 0;
                    int indexend = 0;

                    //去第一个#之后的数据
                    indexend = strMsg.IndexOf('#');
                    string type = strMsg.Substring(indexstart, indexend - indexstart);
                    indexstart = indexend + 1;
                    string word = strMsg.Substring(indexstart);
                    switch (type)
                    {
                        case "GroupExt":
                            List<GroupData> tempExt = JsonConvert.DeserializeObject<List<GroupData>>(word);
                            //temp = JsonConvert.DeserializeObject(word);//测试代码
                            callUserCtrl.output(tempExt);
                            //getButtons(temp);
                            //output(temp);
                            break;
                        case "GroupTrunk": //中继直呼页面
                            List<GroupTrunk> tempTru = JsonConvert.DeserializeObject<List<GroupTrunk>>(word);
                            callUserCtrl.PageRelay = tempTru;
                            callUserCtrl.c_callTypeInfo.status = e_CallType.RELAY;
                            callUserCtrl.SetRelayCall();
                            Debug.WriteLine("中继直呼" + word);
                            break;
                        case "GroupBroadcast": // 广播页面
                            List<GroupBroadcast> tempBro = JsonConvert.DeserializeObject<List<GroupBroadcast>>(word);
                            callUserCtrl.PageRadio = tempBro;
                            callUserCtrl.c_callTypeInfo.status = e_CallType.RADIO;
                            callUserCtrl.SetRadioCall();
                            Debug.WriteLine("广播分组" + word);
                            break;
                        case "ACTIVESTATE":
                            callUserCtrl.Second_ActiveState_Word(word);
                            Debug.WriteLine("主动上报状态问题？？？？？？" + word);
                            break;
                        case "STATE":
                            callUserCtrl.Second_State_Word(word);
                            Debug.WriteLine("状态问题？？？？？？" + word);
                            break;
                        case "MAN": // 20181024 xiaozi 调度信息
                            callManagerCtrl.Second_State_Word(word);
                            //Debug.WriteLine("aaaaaaaaaaaaaaaaa" + word);
                            break;
                        case "CMD":
                            callUserCtrl.Second_Cmd_Word(word);
                            break;
                        default:
                            break;  
                    }           
                }
            }
            catch (System.Exception exc)
            {
                //System.Windows.MessageBox.Show(exc.Message, Title);
                System.Windows.MessageBox.Show(exc.Message + "xixi");
            }
        }

        /// =======================初始化托盘区图标======================
        /// <summary>        
        /// </summary>     
        public void Initial()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.BalloonTipText = "Hello,模拟键盘";
            _notifyIcon.Text = "模拟键盘";

            //程序图标
            //_notifyIcon.Icon = new System.Drawing.Icon(@"Resources/Appicon.ico");
            _notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);//当前程序图标
            _notifyIcon.Visible = true;

            //打开菜单项            
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("Open");
            open.Click += new EventHandler(Show);
            //隐藏菜单项            
            System.Windows.Forms.MenuItem hide = new System.Windows.Forms.MenuItem("Hide");
            hide.Click += new EventHandler(Hide);
            //退出菜单项            
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("Exit");
            exit.Click += new EventHandler(Close);
            //关联托盘控件            
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, hide, exit };
            _notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //双击图标            
            _notifyIcon.MouseDoubleClick += OnMouseDoubleClickHandler;
        }

        private void OnMouseDoubleClickHandler(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Show(null, null);
        }

        private void Show(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;

            this.ShowInTaskbar = true;
            this.Activate();
        }

        private void Hide(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Close(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        //============================================================

        /// ======================= 定时器 ======================
        /// <summary>        
        /// </summary>     
        public void ShowCurTimer(object sender, EventArgs e)
        {
            //获得年月日
            this.label_Date.Content = DateTime.Now.ToString("yyyy-MM-dd");   //yyyy/MM/dd
            //获得时分秒
            this.label_Time.Content = DateTime.Now.ToString("HH:mm:ss");
        }


        //heartBeat方法
        private void HeartBeat(object sender, EventArgs e)
        {
            if (ws.State == WebSocketState.Open) {
                ws.Send("BEAT");
                Debug.WriteLine("heartbeat packet");
            }
        }
        //============================================================


        private void Btn_logOut(object sender, RoutedEventArgs e)
        {
            /* 隐藏公共工具栏的button */
            sw_button.Visibility = Visibility.Hidden;
            user_button.Visibility = Visibility.Hidden;
            desk_button.Visibility = Visibility.Hidden;
            contact_button.Visibility = Visibility.Hidden;

            logwin.Show();

            this.Hide();

            // 20181010 xf Add   必须关闭ws，在登陆界面重新连接
            App.isLogin = false;    // 用户主动选择登出
            ws.Close(); // important

            callUserCtrl.KeyCallListBox.Items.Clear();           // 清理键权电话区
            callUserCtrl.tabCtrl_User.Items.Clear();             // 清理直呼键区
        }

        public void showLogWin()
        {
            logwin.Show();
            this.Hide();
        }

        public void usercontrol_click(object sender, RoutedEventArgs e)
        {
            this.CenterPanel2.Content = callUserCtrl;
            // 启动心跳
            HeartBeatTimer.Start();            
        }

        public void managercontrol_click(object sender, RoutedEventArgs e)
        {
            this.CenterPanel2.Content = callManagerCtrl;
            // 启动心跳
            HeartBeatTimer.Start();

            /* 打开服务端界面钱首先查询软交换设备、用户列表 */
            callManagerCtrl.querySWDevice();
            callManagerCtrl.queryUSER();

            /* 显示公共工具栏的button */
            sw_button.Visibility = Visibility.Visible;
            user_button.Visibility = Visibility.Visible;
            desk_button.Visibility = Visibility.Visible;
            contact_button.Visibility = Visibility.Visible;

            /* 初始化管理方式选择 */
            man_sw_click(this, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ExpanderClick(object sender, MouseButtonEventArgs e)
        {
            Expander item = sender as Expander;
            Canvas parent = item.Parent as Canvas;
            if (parent != null)
             {
                 IEnumerable<UIElement> uiE = parent.Children.OfType<UIElement>().Where(x => x != sender);//枚举类型定义
                   if (uiE.Count()>0)//判断 除去用户选择的控件，是否还有其他控件。
                   {
                        var maxZ = uiE.Select(x => Canvas.GetZIndex(x)).Max();
                        Canvas.SetZIndex(item, maxZ + 1);//置于最顶层
                   }
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USERSystemCloseButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /* 重新登陆 */
        public void reLogin()
        {
            if (!App.isLogin)
            {
                try {
                    // 先断开之前的连接
                    ws.Close();

                ws.Open();
            }
                catch (System.Exception exc)
                {
                    System.Windows.MessageBox.Show(exc.Message + "xixirelogin");
                }
            }
        }

        /* 关闭重连窗口 */
        public void closeLoadingWin()
        {
            // 如果之前为离线状态，并且当前的界面显示为mainwindow
            if (!App.isLogin && this.Visibility == Visibility.Visible)
            {
                loadingWin.reloginOk();
            }
        }

        private void MessageBoxDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {

        }      



        //private void CtrlSwitch_callUser()
        //{
        //    Debug.WriteLine("OK");
        //    call tellCall = new call() { fromid = serverCall, toid = clientCall };
        //    string strMsg = "CMD#Call#" + JsonConvert.SerializeObject(tellCall);
        //   // ws.Send(strMsg);
        //    Debug.WriteLine(strMsg);
        //    Debug.WriteLine("client is clicked!!!");
        //}
    }
}
