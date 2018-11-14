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

using Npgsql;

using System.Windows.Interop;
using System.Runtime.InteropServices;
using WebSocket4Net;
using Newtonsoft.Json;
using System.Diagnostics;



public struct GroupData
{
    public string groupid;
    public string extid;
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
        private DispatcherTimer ShowTimer;
        NpgsqlConnection conn;
        DBHelper dbHelper;

        //private WebSocket ws;
        public WebSocket ws;
        private NotifyIcon _notifyIcon = null;
        private LoginWindow logwin;
        public CallBoard callBoard;
        private string m_ServerIP;

        /* 调度usercontrol */
        //UserControl1 callUserCtrl;//weituo 20181013
        public CallUserControl callUserCtrl;
        CallManagerControl callManagerCtrl;



        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
            // 关闭窗口执行前，触发的可以取消关闭窗口的操作
            this.Closing += Window_Closing;

 
            // 最小化到系统托盘
            //Initial();

            //demoUserControl.Children.Add(new UserControl1());
            //InitBitkyPoleShow();

            // 初始化用户界面和管理界面
            callUserCtrl = new CallUserControl(this);
            //callUserCtrl.CtrlSwitchEvent += new CtrlSwitchHandler(CtrlSwitch_callUser);
            callManagerCtrl = new CallManagerControl(this);
            //callManagerCtrl.CtrlSwitchEvent += new CtrlSwitchHandler(CtrlSwitch_callManager);
            callBoard = new CallBoard(this);
            //CtrlSwitch_callUser();

            // 显示登录界面，验证后返回，未登陆前不会触发Window_Loaded事件
            logwin = new LoginWindow(this);
            logwin.Show();
            this.Hide();

            // 创建socket和ui交互机制 // 20181010 xf Add
            PeerCallBack.Instance = new PeerCallBack(SynchronizationContext.Current, this);    

            ShowTime();    //在这里窗体加载的时候不执行文本框赋值，窗体上不会及时的把时间显示出来，而是等待了片刻才显示了出来
            ShowTimer = new DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurTimer);//起个Timer一直获取当前时间
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ShowTimer.Start();

            // 创建websocket
            string serverip = "192.168.2.101";
            string serverport = "1020";

            string serveruri = "ws://" + serverip + ":" + serverport.Trim();
            try
            {
                ws = new WebSocket(serveruri);
                ws.Opened += websocket_Opened;
                ws.Closed += websocket_Closed;
                ws.MessageReceived += websocket_MessageReceived;

                //不联网时测试代码，联网注释
                //List<GroupData> temp = new List<GroupData>();
                //List<GroupData> DateJsonCon = new List<GroupData>();
                //GroupData group1 = new GroupData() { groupid = "动力科室", extid = "204" };
                //GroupData group2 = new GroupData() { groupid = "动力科室", extid = "205" };
                //GroupData group3 = new GroupData() { groupid = "动力科室", extid = "206" };
                //GroupData group4 = new GroupData() { groupid = "动力科室", extid = "207" };
                //GroupData group5 = new GroupData() { groupid = "动力科室", extid = "208" };
                //GroupData group6 = new GroupData() { groupid = "运输部门", extid = "209" };
                //GroupData group7 = new GroupData() { groupid = "运输部门", extid = "210" };
                //GroupData group8 = new GroupData() { groupid = "运输部门", extid = "211" };
                //GroupData group9 = new GroupData() { groupid = "运输部门", extid = "220" };
                //GroupData group10 = new GroupData() { groupid = "运输部门", extid = "213" };
                //GroupData group11 = new GroupData() { groupid = "调度中心", extid = "214" };
                //GroupData group12 = new GroupData() { groupid = "调度中心", extid = "215" };
                //GroupData group13 = new GroupData() { groupid = "调度中心", extid = "211" };
                //GroupData group14 = new GroupData() { groupid = "调度中心", extid = "217" };
                //GroupData group15 = new GroupData() { groupid = "调度中心", extid = "218" };
                //GroupData group16 = new GroupData() { groupid = "调度中心", extid = "219" };
                //GroupData group17 = new GroupData() { groupid = "调度中心", extid = "220" };
                //GroupData group18 = new GroupData() { groupid = "调度中心", extid = "221" };
                //GroupData group19 = new GroupData() { groupid = "调度中心", extid = "222" };

                //DateJsonCon.Add(group1);
                //DateJsonCon.Add(group2);
                //DateJsonCon.Add(group3);
                //DateJsonCon.Add(group4);
                //DateJsonCon.Add(group5);
                //DateJsonCon.Add(group6);
                //DateJsonCon.Add(group7);
                //DateJsonCon.Add(group8);
                //DateJsonCon.Add(group9);
                //DateJsonCon.Add(group10);
                //DateJsonCon.Add(group11);
                //DateJsonCon.Add(group12);
                //DateJsonCon.Add(group13);
                //DateJsonCon.Add(group14);
                //DateJsonCon.Add(group15);
                //DateJsonCon.Add(group16);
                //DateJsonCon.Add(group17);
                //DateJsonCon.Add(group18);
                //DateJsonCon.Add(group19);
               

                //string strMsg = "GroupExt#" + JsonConvert.SerializeObject(DateJsonCon);
                //Console.WriteLine(group1.extid);
                //Console.WriteLine(group2.extid);
                //typedata(strMsg);
                //string strword = "STATE#BUSY#220";
                //typedata(strword);

                //call ringcall = new call() { fromid = "215", toid = "214" };
                //string strring = "STATE#RING#" + JsonConvert.SerializeObject(ringcall); ;
                //typedata(strring);

                //服务端调度台页面布置
                //Member member1 = new Member() { callno = "212", type = "", name = "", level = "", description = "" };
                //Member member2 = new Member() { callno = "213", type = "", name = "", level = "", description = "" };
                //Member member3 = new Member() { callno = "214", type = "", name = "", level = "", description = "" };
                //Member member4 = new Member() { callno = "215", type = "", name = "", level = "", description = "" };
                //Member member5 = new Member() { callno = "220", type = "", name = "", level = "", description = "" };
                //Member member6 = new Member() { callno = "221", type = "", name = "", level = "", description = "" };
                //Member member7 = new Member() { callno = "204", type = "", name = "", level = "", description = "" };
                //List<Member> memberList1 = new List<Member>();
                //memberList1.Add(member1);
                //memberList1.Add(member2);
                //memberList1.Add(member3);
                //memberList1.Add(member4);
                //memberList1.Add(member5);
                //memberList1.Add(member6);
                //Group group1 = new Group() { index = "1", groupname = "test1", column = "1", description = "11111", memberlist = memberList1 };
                //List<Member> memberList2 = new List<Member>();
                //memberList2.Add(member5);
                //memberList2.Add(member2);
                //Group group2 = new Group() { index = "2", groupname = "test2", column = "2", description = "22222", memberlist = memberList2 };
                //List<Group> groupList1 = new List<Group>();
                //List<Group> groupList2 = new List<Group>();
                //groupList1.Add(group1);
                //groupList1.Add(group2);
                //Hotline hotline1 = new Hotline() { callno = "204", type = "", name = "", level = "", description = "" };
                //List<Hotline> hotlineList1 = new List<Hotline>();
                //hotlineList1.Add(hotline1);
                //KeyBoard keyBoard1 = new KeyBoard() { index = "1", name = "keyboard", mac = "", ip = "", grouplist = groupList1, hotlinelist = hotlineList1 };
                //List<KeyBoard> keyBoardList = new List<KeyBoard>();
                //keyBoardList.Add(keyBoard1);


                //List<Member> memberList3 = new List<Member>();
                //memberList3.Add(member7);
                //memberList3.Add(member5);
                //memberList3.Add(member2);
                //Group group3 = new Group() { index = "3", groupname = "test3", column = "1", description = "33333", memberlist = memberList3 };
                //groupList2.Add(group3);
                //Hotline hotline2 = new Hotline() { callno = "204", type = "", name = "", level = "", description = "" };
                //List<Hotline> hotlineList2 = new List<Hotline>();
                //hotlineList1.Add(hotline2);
                //KeyBoard keyBoard2 = new KeyBoard() { index = "2", name = "keyboard", mac = "", ip = "", grouplist = groupList2, hotlinelist = hotlineList2 };
                //keyBoardList.Add(keyBoard2);

                //GetDesk getDesk1 = new GetDesk() { sequence = "1", keyboardlist = keyBoardList };
                //string strsing = "MAN#GETALLKEYBOARD#" + JsonConvert.SerializeObject(getDesk1); ;
                //typedata(strsing);

                // 联网注释
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            // 初始化数据库handle
            dbHelper = new PostgreHelper();
            Console.WriteLine("Hello PostgreSQL");

            //INIHelper.WriteIni("DEV", "port", "80");
            // 初始化IP
            m_ServerIP = Properties.Settings.Default.serverip;

            // Host info
            string host = "localhost";
            string user = "xf";
            string password = "xf";
            string dbname = "dispatch";
            var connString = string.Format("Host={0};Port=5432;Username={1};Password={2};Database={3}", host, user, password, dbname);
            conn = new NpgsqlConnection(connString);

            //在主页面中把usercontrol画出来或者说引用出来
            //this.CenterPanel.Content = callUserCtrl;
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
            Console.WriteLine("接收" + strMsg);
            this.Dispatcher.Invoke(new Action(delegate
            {
                typedata(strMsg);
            }));
            
        }

        private void typedata(Object date)
        {
            //Console.WriteLine("typedata接收" + date);
            // lets see the thread id 20181010 xf Add
            int id = Thread.CurrentThread.ManagedThreadId;
            Trace.WriteLine("Run thread: " + id);

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
            Console.WriteLine("client is closed!!!");
            // 20181010 xf Add
            if (App.isLogin)
            {
                PeerCallBack.Instance.GetOperationMsg("login");
                App.isLogin = false;
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
                Trace.WriteLine("mToolStripButtonThreads_Click thread: " + id);

                logwin.Delog(msg);

                // 进入用户界面
                if ("Admin" == logwin.logIn)
                {
                    CtrlSwitch_callUser();
                    Console.WriteLine("用户界面" + logwin.logIn);
                }
                else
                {
                    CtrlSwitch_callManager();
                    Console.WriteLine("用户界面" + logwin.logIn);
                }
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
                Trace.WriteLine("mToolStripButtonThreads_Click thread: " + id);

                if (msg == "login")
                {
                    logwin.Show();
                    this.Hide();
                } 
                else 
                {
                    List<GroupData> temp = new List<GroupData>();
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
                            temp = JsonConvert.DeserializeObject<List<GroupData>>(word);
                            //temp = JsonConvert.DeserializeObject(word);//测试代码
                            callUserCtrl.output(temp);
                            //getButtons(temp);
                            //output(temp);
                            break;
                        case "STATE":
                            callUserCtrl.Second_State_Word(word);
                            break;
                        case "MAN": // 20181024 xiaozi 调度信息
                            callManagerCtrl.Second_State_Word(word); 
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



        /// =======================时间======================
        /// <summary>        
        /// </summary>     
        public void ShowCurTimer(object sender, EventArgs e)
        {
            ShowTime();
        }


        //ShowTime方法
        private void ShowTime()
        {
            //获得年月日
            this.label_Date.Content = DateTime.Now.ToString("yyyy-MM-dd");   //yyyy/MM/dd
            //获得时分秒
            this.label_Time.Content = DateTime.Now.ToString("HH:mm:ss");
        }
        //============================================================


        private void Btn_logOut(object sender, RoutedEventArgs e)
        {
            //logwin.Show();
            //this.Hide();

            // 20181010 xf Add
            ws.Close();
        }


        public void usercontrol_click(object sender, RoutedEventArgs e)
        {
            this.CenterPanel2.Content = callUserCtrl;
        }

        public void managercontrol_click(object sender, RoutedEventArgs e)
        {
            this.CenterPanel2.Content = callManagerCtrl;
            /* 打开服务端界面钱首先查询软交换设备、用户列表 */
            callManagerCtrl.querySWDevice();
            callManagerCtrl.queryUSER();
        }

        #region 跨页面间的 事件委托
        public void CtrlSwitch_callUser()
        {
            this.CenterPanel2.Content = callManagerCtrl;
        }

        public void CtrlSwitch_callManager()
        {
            this.CenterPanel2.Content = callUserCtrl;
        }
        #endregion

        

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




        //private void CtrlSwitch_callUser()
        //{
        //    Console.WriteLine("OK");
        //    call tellCall = new call() { fromid = serverCall, toid = clientCall };
        //    string strMsg = "CMD#Call#" + JsonConvert.SerializeObject(tellCall);
        //   // ws.Send(strMsg);
        //    Console.WriteLine(strMsg);
        //    Console.WriteLine("client is clicked!!!");
        //}



    }
}
