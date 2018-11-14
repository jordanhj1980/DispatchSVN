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
using System.Windows.Shapes;
using System.ComponentModel;

using Newtonsoft.Json;
using WebSocket4Net;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
namespace DispatchApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private MainWindow m_mainWindow;
        ////20181010 xiaozi Add
        //private WebSocket ws;
      
        public LoginWindow(MainWindow mainWindow)
        {
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
            switch (data)
            {
                case "Success#Control":
                    App.isLogin = true;
                    this.Hide();
                    m_mainWindow.usercontrol_click(this, null);
                    m_mainWindow.Show();
                    logIn = "Control";
                    break;
                case "Success#Admin":
                    App.isLogin = true;
                    this.Hide();
                    m_mainWindow.managercontrol_click(this, null);
                    m_mainWindow.Show();
                    logIn = "Admin";
                    break;
                case "connected":// ws 连接成功，暂未登陆 //2018101 xf Add
                    if (!App.isLogin)
                    {
                        label_msg.Content = "";

                        //联网登陆，有问题
                        List<string> logdata = new List<string>();
                        logdata.Add(TxUserName.Text);
                        logdata.Add(TxPassword.Password);

                        string logstr = "LOG#" + JsonConvert.SerializeObject(logdata);
                        m_mainWindow.ws.Send(logstr);
                    }
                    else
                    {
                        Console.WriteLine("App.isLogin");
                    }
                    break;
                case "Wrong":
                    //label_msg.Content = data;
                    //MessageBox.Show("账号或密码错误！");
                    
                    message = "账号或密码错误！";
                    //messageQueue.Enqueue(message);
                    //the message queue can be called from any thread
                    Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                    break;
                case "Already":
                    //label_msg.Content = data;
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
        /// 确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtLogin_Click(object sender, RoutedEventArgs e)
        {
            /* 尝试连接远程服务器 */
            //2018101 xf Add
            if (!App.isLogin)
            {
                m_mainWindow.ws.Open();
            }

            //本地登陆
            //if (TxUserName.Text == "admin" && TxPassword.Text == "admin")
            //{
            //    //this.DialogResult = Convert.ToBoolean(1);                
            //    //this.Close();      
            //    this.Hide();
            //    m_mainWindow.Show();
            //}
            //else
            //{
            //    MessageBox.Show("账号或密码错误！");
            //}
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
            label_msg.Content = msg;
        }

        private void BtExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
