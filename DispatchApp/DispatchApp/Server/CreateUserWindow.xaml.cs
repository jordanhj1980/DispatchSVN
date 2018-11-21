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

using Newtonsoft.Json;
using System.Diagnostics;

namespace DispatchApp
{
    /// <summary>
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        public delegate void CWHandler(object sender, string msg, object obj);
        public event CWHandler msgevent;

        public CreateUserWindow()
        {
            InitializeComponent();

            /* 用户角色列表 */
            List<UserStatus> allstatus = new List<UserStatus>
            {
                new UserStatus(1, "启用"),
                new UserStatus(2, "停用"),
            };

            comBox_status.ItemsSource = allstatus;
            comBox_status.DisplayMemberPath = "description";
            comBox_status.SelectedValuePath = "id";
            comBox_status.SelectedIndex = 0;

            /* 用户角色列表 */
            List<UserRole> allrole = new List<UserRole>
            {
                new UserRole(2, "普通调度员"),
                new UserRole(1, "全权限调度员"),
            };

            comBox_privilege.ItemsSource = allrole;
            comBox_privilege.DisplayMemberPath = "description";
            comBox_privilege.SelectedValuePath = "id";
            comBox_privilege.SelectedIndex = 0;

        }

        private void bt_Click_apply(object sender, RoutedEventArgs e)
        {
            /* 首先校验用户输入 */
            //if (!IsValid(this)) 
            //    return;

            // 保存当前的软交换配置，并发送给服务器

            //联网登陆，有问题
            USEREDITITEM item = new USEREDITITEM();
            item.sequence = GlobalFunAndVar.sequenceGenerator();
            item.name = tb_name.Text.Trim();
            item.password = tb_pass.Text.Trim();            
            item.status = comBox_status.SelectedValue.ToString();
            item.privilege = comBox_privilege.SelectedValue.ToString();
            item.description = tb_description.Text.Trim();
            // 需要从调度台列表中查询
            //item.desk = comBox_desk.SelectedValue.ToString();
            item.desk = "1";

            /* 临时保存待修改的用户信息 */
            User user = new User();
            user.sequence = item.sequence;
            user.name = item.name;
            user.password = item.password;
            user.status = item.status;
            user.privilege = item.privilege;
            user.role = item.role;
            user.description = item.description;
            user.desk = item.desk;

            StringBuilder sb = new StringBuilder(100);
            sb.Append("MAN#ADDUSER#");
            sb.Append(JsonConvert.SerializeObject(item));

            Debug.WriteLine("SEND: " + sb.ToString());

            /* 发送网络信息 */
            if (msgevent != null)
            {
                msgevent(this, "net", sb.ToString());
                msgevent(this, "user", user);
            }
        }

        private void bt_Click_cancel(object sender, RoutedEventArgs e)
        {
            // 取消配置并且关闭窗口
            Close();
        }

        private void bt_Click_ok(object sender, RoutedEventArgs e)
        {
            // 确定配置并关闭当前窗口
            Close();
        }

    }
}
