using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
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
using System.Configuration;

using Npgsql;
using System.Diagnostics;
using Newtonsoft.Json;

namespace DispatchApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        /************************************************************************/
        /* 关闭窗口执行前，触发保存设置参数                                       */
        /************************************************************************/
        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("是否要关闭？", "确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
                saveUserOption();

                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void saveUserOption()
        {
            // 写，保存 
            // User则运行时可更改，Applicatiion则运行时不可更改
            //Properties.Settings.Default.serverip = m_ServerIP;
            //Properties.Settings.Default.Save();
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["serverip"].Value = m_ServerIP;
            cfa.Save();
        }


        /************************************************************************/
        /* 用于软交换服务器  端口控件按下过滤数字按键                             */
        /************************************************************************/
        private void sw_port_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;

            // 过滤按键
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9)))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btn_lock_Clicked(object sender, RoutedEventArgs e)
        {
            lockScreen.account = this.account;
            lockScreen.Show();
            this.Hide();
        }

        private void btnMask_Click(object sender, RoutedEventArgs e)
        {
            CreateAlbum.ShowDialog(this);
        }

        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("menu open clicked!");
        }

        private void man_sw_click(object sender, RoutedEventArgs e)
        {
            callManagerCtrl.man_header_click("sw");

            icon_contact.Foreground = icon_pale;
            icon_key.Foreground = icon_pale;
            icon_sw.Foreground = icon_hilight;
            icon_user.Foreground = icon_pale;
        }

        private void man_user_click(object sender, RoutedEventArgs e)
        {
            callManagerCtrl.man_header_click("user");

            icon_contact.Foreground = icon_pale;
            icon_key.Foreground = icon_pale;
            icon_sw.Foreground = icon_pale;
            icon_user.Foreground = icon_hilight;

            /* 发送调度台查询命令 */
            queryDesk();
        }

        private void man_desk_click(object sender, RoutedEventArgs e)
        {
            callManagerCtrl.man_header_click("desk");

            icon_contact.Foreground = icon_pale;
            icon_key.Foreground = icon_hilight;
            icon_sw.Foreground = icon_pale;
            icon_user.Foreground = icon_pale;
        }

        private void man_contact_click(object sender, RoutedEventArgs e)
        {
            callManagerCtrl.man_header_click("contact");

            icon_contact.Foreground = icon_hilight;
            icon_key.Foreground = icon_pale;
            icon_sw.Foreground = icon_pale;
            icon_user.Foreground = icon_pale;
        }

        // 查询调度键盘
        public void queryDesk()
        {
            SearchDesk searchDesk = new SearchDesk();
            searchDesk.sequence = GlobalFunAndVar.sequenceGenerator();
            StringBuilder sb = new StringBuilder(100);
            sb.Append("MAN#GETALLKEYBOARD#");
            sb.Append(JsonConvert.SerializeObject(searchDesk));

            Debug.WriteLine("SEND：" + sb.ToString());
            ws.Send(sb.ToString());

        }
    }
}

