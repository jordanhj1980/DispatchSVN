using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace DispatchApp
{
    /// <summary>
    /// UserManage.xaml 的交互逻辑
    /// </summary>
    public partial class UserManage : UserControl
    {
        public User userobj;      // /* 保存临时的用户item */

        private MainWindow mainWindow;
        private UserViewModel userDataModel;
        public UserManage(MainWindow mainwindow)
        {
            InitializeComponent();
            this.mainWindow = mainwindow;
            userDataModel = new UserViewModel();
            this.DataContext = userDataModel;

            userobj = new User();   // 临时变量
        }               

        public void recv(string state, string data)
        {
            switch (state)
            {
                case "ADDUSER":
                    addUser(data);
                    break;
                case "GETUSER":
                    freshUser(data);
                    break;
                case "DELUSER":
                    delUser(data);
                    break;
                case "EDITUSER":
                    editUser(data);
                    break;
                default:
                    break;
            }
        }
        
        #region recv_operation

        /* 添加用户应答 */
        private void addUser(string data)
        {
            /* 比较临时存储的obj的sequence是否一致 */
            USER_ADDRESULT res = JsonConvert.DeserializeObject<USER_ADDRESULT>(data);
            if (res != null && userobj.sequence == res.sequence)
            {
                if (res.result == "Fail")
                {
                    Debug.WriteLine(res.reason);
                    this.result.Content = "添加失败";
                    return;
                }
                this.result.Content = "添加成功";
                //retrievedItems is the data you received from the service
                //foreach (object item in retrievedItems)
                //Dispatcher.BeginInvoke(DispatcherPriority.Background, new ParameterizedThreadStart(AddItem), item);  
                //Dispatcher.BeginInvoke( new Action( ()  =>  {
                userDataModel.UserList.Add(userobj);
                //}));
            }
        }

        // 查询用户请求
        private void freshUser(string data)
        {
            userDataModel.UserList.Clear();
            USER_QUERYRESULT res = JsonConvert.DeserializeObject<USER_QUERYRESULT>(data);
            if (res != null)
            {
                ObservableCollection<USERITEM> list = new ObservableCollection<USERITEM>();
                foreach (USERITEM member in res.userlist)
                {
                    User item = new User();
                    item.index = userDataModel.UserList.Count;  // 列表序号，从0开始
                    item.name = member.name;
                    item.password = member.password;
                    item.status = member.status;
                    item.privilege = member.privilege;
                    item.description = member.description;
                    item.desk = member.desk;

                    userDataModel.UserList.Add(item);
                }
            }
        }

        private void delUser(string data)
        {
            /* 比较临时存储的obj的sequence是否一致 */
            SW_ADDRESULT res = JsonConvert.DeserializeObject<SW_ADDRESULT>(data);
            if (res != null /* && swdevobj.sequence == res.sequence */)
            {
                if (res.result == "Fail")
                {
                    Debug.WriteLine(res.reason);
                    this.result.Content = "删除失败";
                    return;
                }
                this.result.Content = "删除成功";

                /* 查询index */
                for (int i = 0; i < userDataModel.UserList.Count; i++)
                {
                    if (userDataModel.UserList[i].name == userobj.name)
                    {
                        userDataModel.UserList.RemoveAt(i);
                    }
                }
            }
        }

        private void editUser(string data)
        {
            /* 比较临时存储的obj的sequence是否一致 */
            SW_ADDRESULT res = JsonConvert.DeserializeObject<SW_ADDRESULT>(data);
            if (res != null && userobj.sequence == res.sequence)
            {
                if (res.result == "Fail")
                {
                    Debug.WriteLine(res.reason);
                    this.result.Content = "保存失败";
                    return;
                }

                /* 显示修改成功 */
                this.result.Content = "保存成功";

                // 在swList中查询对应的item
                for (int i = 0; i < userDataModel.UserList.Count; i++)
                {
                    // 用户名来判断
                    if (userDataModel.UserList[i].name == userobj.name)
                    {
                        User item = userDataModel.UserList[i];
                        item.name = userobj.name;
                        item.password = userobj.password;
                        item.privilege = userobj.privilege;
                        item.description = userobj.description;
                        item.role = userobj.role;
                        item.index = userobj.index;
                        item.status = userobj.status;
                        item.desk = userobj.desk;

                        Trace.WriteLine("current selected name: " + item.name);
                        break;
                    }
                }
            }
        }
        #endregion

        public void delUserReq(string name)
        {
            /* 向服务器请求列表 */
            USERQUERY req = new USERQUERY();
            req.sequence = GlobalFunAndVar.sequenceGenerator();
            req.name = name;
            StringBuilder sb = new StringBuilder(100);
            sb.Append("MAN#DELUSER#");
            sb.Append(JsonConvert.SerializeObject(req));

            Debug.WriteLine("SEND: " + sb.ToString());
            //sendMsg(this, "net", sb.ToString());
            mainWindow.ws.Send(sb.ToString());

            /* 临时保存用户名 */
            userobj.name = name;
        }

        public void freshDeskList(ObservableCollection<UserStatus> list)
        {
            /* 清空列表 */
            userDataModel.deskList.Clear();
            /*foreach (UserStatus item in list)
            {
                userDataModel.deskList.Add(item);
            }*/
            userDataModel.deskList = list;
        }


        /* 添加用户 */
        private void add_Click(object sender, RoutedEventArgs e)
        {
            userDataModel.SelectedUser = new User();
            userDataModel.SelectedUser.name = "新用户";
            userDataModel.SelectedUser.privilege = "2";
            userDataModel.NewUser = true;
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            User user = userDataModel.SelectedUser; 
            if (MessageBox.Show("确定是否要删除用户 " + user.name, "提示消息",
                MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Debug.WriteLine("delete user: "+user.name);                
                delUserReq(user.name);
            }
        }

        private void user_Click_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            if (tvi.Header is User)
            {
                User modelkey = tvi.Header as User;
                // 赋值给UI的临时对象，用于显示
                if (modelkey != null)
                {
                    userDataModel.SelectedUser = modelkey;
                    /* 表示是否是新建用户 */
                    userDataModel.NewUser = false;
                }                
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                /* 用于数据库协议交互 */
                USEREDITITEM item = new USEREDITITEM();
                item.sequence = GlobalFunAndVar.sequenceGenerator();
                item.name = userName.Text.Trim();
                item.password = userPass.Text.Trim();
                item.status = userStatus.Text.Trim();
                //item.privilege = userPriv.Text.Trim();
                item.privilege = userPriv.SelectedValue as string;
                int ind = userDesk.SelectedIndex;
                if (item.privilege == "2")
                {
                    item.desk = userDataModel.deskList[ind].id;// userDesk.SelectedValue.ToString(); //userDesk.Text.Trim();
                }
                else
                {
                    item.desk = "";
                }              
                item.role = userRole.Text.Trim();                
                item.description = userDesp.Text.Trim();

                // 用于界面显示
                User uiUser = new User();
                uiUser.sequence = item.sequence;
                uiUser.name = item.name;
                uiUser.password = item.password;
                uiUser.status = item.status;
                uiUser.privilege = item.privilege;
                uiUser.role = item.role;
                uiUser.description = item.description;
                uiUser.desk = item.desk;

                // 保存临时User变量
                userobj = uiUser;

                /* 向服务器发送删除消息 */

                StringBuilder sb = new StringBuilder(100);
                if (true == userDataModel.NewUser)
                {
                    sb.Append("MAN#ADDUSER#");
                }
                else
                {
                    sb.Append("MAN#EDITUSER#");
                }
                
                sb.Append(JsonConvert.SerializeObject(item));

                Debug.WriteLine("SEND: " + sb.ToString());
                //sendMsg(this, "net", sb.ToString());
                //sendMsg(this, "user", uiUser);
                mainWindow.ws.Send(sb.ToString());
                
            }
        }
    }
}
