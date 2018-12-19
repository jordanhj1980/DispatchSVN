using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

using Newtonsoft.Json;
using WebSocket4Net;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;


namespace DispatchApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private bool isExpand;
        private bool isRegistered;
        private MainWindow m_mainWindow;
        private string _machineCode;
        ////20181010 xiaozi Add
        //private WebSocket ws;
      
        public LoginWindow(MainWindow mainWindow)
        {
            isExpand = false;
            isRegistered = false;
            m_mainWindow = mainWindow;
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowState = System.Windows.WindowState.Maximized;
        }

        public string logIn = "0"; 
        public void Delog(string data)
        {
            var messageQueue = SnackbarOne.MessageQueue;
            string message;

            /* 如果登陆成功 */
            if (data.StartsWith("Success"))
            {
                m_mainWindow.closeLoadingWin();
            }

            switch (data)
            {
                case "Success#Control":
                    App.isLogin = true;
                    m_mainWindow.account = this.TxUserName.Text.Trim();
                    m_mainWindow.password = this.TxPassword.Password;
                    m_mainWindow.serverip = this.IPAddr.Text.Trim();    // 登陆成功则保存ip地址
                    this.Hide();
                    m_mainWindow.usercontrol_click(this, null);
                    m_mainWindow.Show();
                    logIn = "Control";
                    break;
                case "Success#Admin":
                    App.isLogin = true;
                    m_mainWindow.account = this.TxUserName.Text.Trim();
                    m_mainWindow.password = this.TxPassword.Password;
                    m_mainWindow.serverip = this.IPAddr.Text.Trim();
                    this.Hide();
                    m_mainWindow.managercontrol_click(this, null);
                    m_mainWindow.Show();
                    logIn = "Admin";
                    break;
                case "connected":// ws 连接成功，暂未登陆 //2018101 xf Add
                    if (!App.isLogin)
                    {
                        //联网登陆，有问题
                        List<string> logdata = new List<string>();
                        logdata.Add(TxUserName.Text);
                        logdata.Add(TxPassword.Password);

                        string logstr = "LOG#" + JsonConvert.SerializeObject(logdata);
                        m_mainWindow.ws.Send(logstr);
                    }
                    else
                    {
                        Debug.WriteLine("App.isLogin");
                    }
                    break;
                case "Wrong":
                    //MessageBox.Show("账号或密码错误！");
                    
                    message = "账号或密码错误！";
                    //messageQueue.Enqueue(message);
                    //the message queue can be called from any thread
                    Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                    break;
                case "Already":
                    //MessageBox.Show("用户已登陆！");

                    message = "用户已登陆！";
                    //messageQueue.Enqueue(message);
                    Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                    break;

                    
                default: 
                    //MessageBox.Show("服务器连接失败！");
                    break;
            }
        }


        /// <summary>
        /// 登陆按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!isRegistered)
            {
                var messageQueue = SnackbarOne.MessageQueue;               
                Task.Factory.StartNew(() => messageQueue.Enqueue("当前未注册，请输入注册码"));
                this.serialBox.Visibility = Visibility.Visible;
                return;
            }

            /* 尝试连接远程服务器 */
            //2018101 xf Add
            if (!App.isLogin)
            {
                try
                {
                    /* 初始化socket连接 */
                    m_mainWindow.initSocket(this.IPAddr.Text.Trim());

                    m_mainWindow.ws.Open();
                }
                catch (System.Exception exc)
                {
                    //System.Windows.MessageBox.Show(exc.Message, Title);
                    System.Windows.MessageBox.Show(exc.Message + "BtLogin_Click");
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;  // cancels the window close
            //this.Hide();      // Programmatically hides the window
            System.Windows.Application.Current.Shutdown();
        }

        //2018101 xf Add
        public void setStatus(string msg)
        {
        }

        private void BtExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void BtAdvance_Click(object sender, RoutedEventArgs e)
        {
            if (isExpand)
            {
                isExpand = false;
                this.loginBox.Height -= 40;
                advBox.Visibility = Visibility.Collapsed;
                image_adv.Source = new BitmapImage(new Uri("Resources/adv_expand.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                isExpand = true;
                advBox.Visibility = Visibility.Visible;
                this.loginBox.Height += 40;
                image_adv.Source = new BitmapImage(new Uri("Resources/adv_collapse.png", UriKind.RelativeOrAbsolute));
            }
            
        }

        private void Form_Loaded(object sender, RoutedEventArgs e)
        {
            this.loginBox.Height -= 40;
            advBox.Visibility = Visibility.Collapsed;

            // obtain the machine code
            _machineCode = MachineCode.getRNum();

            StreamReader sr = null;
            try
            {
                //string appDataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
                string appDataPath = "license.lic";
                sr = new StreamReader(appDataPath, Encoding.Default);
                if (sr == null)
                {
                    return;
                }
                string content;
                while ((content = sr.ReadLine()) != null)
                {
                    content = content.Trim();
                    if (content.ToUpper().StartsWith("SERIAL:"))
                    {
                        string code = content.Substring(content.IndexOf(":") + 1);

                        if (code == _machineCode)
                        {
                            isRegistered = true;
                        }

                    }
                }
                sr.Close();
            }
            catch (SystemException exc)
            {
                //System.Windows.MessageBox.Show(exc.Message + "Form_Loaded");
                if (sr != null)
                {
                    sr.Close();
                }
                return;
            }

            // 如果未成功注册，显示注册码输入界面
            if (!isRegistered)
            {
                serialBox.Visibility = Visibility.Visible;
            }
            else
            {
                serialBox.Visibility = Visibility.Hidden;
            }
            
        }

        public void setIPaddr(string ipAddr)
        {
            IPAddr.Text = ipAddr;
        }

        private void register_click(object sender, RoutedEventArgs e)
        {
            var messageQueue = SnackbarOne.MessageQueue;
            string msg;
            if (serial.Text.Trim()  == _machineCode)  {
                // 注册成功
                serialBox.Visibility = Visibility.Collapsed;
                isRegistered = true;
                msg = "注册成功";
                // 写入注册码
                string appDataPath = "license.lic";
                StreamWriter sw = null; 
                try
                {
                    sw = new StreamWriter(appDataPath);
                    sw.Write("SERIAL:");
                    sw.Write(_machineCode);
                    sw.Close();

                } catch (SystemException exc) {
                    if (sw != null) {
                        sw.Close();
                    }
                    msg = "写入注册文件失败";
                }                
            }
            else
            {
                msg = "注册失败";
            }
            Task.Factory.StartNew(() => messageQueue.Enqueue(msg));
        }

        private void SelectAllText(object sender, KeyboardFocusChangedEventArgs e)
        {
            PasswordBox pb = sender as PasswordBox;
            if(pb!=null)
            {
                pb.SelectAll();
            }
        }
    }
}
