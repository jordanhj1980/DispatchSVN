using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Npgsql;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using WebSocket4Net;
using Newtonsoft.Json;

namespace DispatchApp
{

    public class TestClass
    {
        public int ClassTypeId { get; set; }
        public List<RadioButton> btnList;
    }

    public class TestClassRoom
    {
        public string classroomId { get; set; }
    }

    /// <summary>
    /// Interaction logic for CallManagerControl.xaml
    /// </summary>
    public partial class CallManagerControl : UserControl, INotifyPropertyChanged
    {
        #region 事件通知
        public event PropertyChangedEventHandler PropertyChanged;

        /* propertyName为属性的名称 */
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                //假设属性发生了改变，则触发这个事件
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void SetAndNotifyIfChanged<T>(string propertyName, ref T oldValue, T newValue)
        {
            if (oldValue == null && newValue == null) return;
            if (oldValue != null && oldValue.Equals(newValue)) return;
            if (newValue != null && newValue.Equals(oldValue)) return;
            oldValue = newValue;
            RaisePropertyChanged(propertyName);
        }

        private MainWindow mainWindow; // 20181024 xiaozi
        
        public event CtrlSwitchHandler CtrlSwitchEvent;
        List<TestClass> classlist;
        
        
        
        #endregion

        public string ImageSource { get; set; }
        //public Image img_collapse { get; set; }

        public bool isCollapse;

        // 新建用户窗口，可以反复打开
        CreateUserWindow cuw;
        // 添加调度台信息  add by twinkle 20181122 
        private ObservableCollection<UserStatus> deskList;
        /*
        public List<UserStatus> deskList
        {
            get { return _deskList; }
            set
            {
                if (_deskList != value)
                {
                    _deskList = value;
                    RaisePropertyChanged("deskList");
                }
            }
        }*/
        //public KeyBoardManageViewModel keyboardmanagedata; //调度键盘界面数据
        KeyBoardManage keyboardmanagetab;
        /* 软交换界面TabItem */
        SwitchManage swmanaagetab;
        /* 用户界面TabItem */
        UserManage usermanagetab;
        /* 电话簿界面TabItem */
        ContactManage contactmanaagetab;

        public CallManagerControl(MainWindow mmainWindow)
        {
            mainWindow = mmainWindow;
            //keyboardmanagetab.keyboardmanagedata = new KeyBoardManageViewModel();
            InitializeComponent();

            DataContext = this;

            /* 赋值软交换界面 */
            swmanaagetab = new SwitchManage(mainWindow);
            this.tabItem_switch.Content = swmanaagetab;

            /* 赋值用户界面 */
            usermanagetab = new UserManage(mainWindow);
            this.tabItem_user.Content = usermanagetab;

            /* 赋值电话簿界面 */
            contactmanaagetab = new ContactManage(mainWindow);
            this.tabItem_contact.Content = contactmanaagetab;

            /* 赋值调度台界面 */
            keyboardmanagetab = new KeyBoardManage(mainWindow);
            this.keymanage.Content = keyboardmanagetab;            

            // 初始化左侧资源服务列表
            //ToolTipTestWindow();
            isCollapse = true;

            //StackPanelLeft.Children.Add(exp);

            // 初始化软交换设备列表
            InitialSwitches();            

            // 新建用户
            cuw = new CreateUserWindow();
            // 传递消息事件
            cuw.msgevent += new CreateUserWindow.CWHandler(sendMsg);

            deskList = new ObservableCollection<UserStatus>();
            // 绑定数据
            userDesk.ItemsSource = deskList;
        }        
       
        /// <summary>
        /// 接收信息
        /// </summary>
        public void InitialSwitches()
        {
            //switchGrid.ItemsSource = swList;
            //switchGrid.SelectionChanged += DataGrid_Click;

            //userGrid.ItemsSource = m_UserList;
            //userGrid.MouseDown += DataGrid_MouseDown;
        }

        /// =======================获取查询的调度台信息======================
        /// 20181024 xiaozi Add
        /// <summary>
        /// 接收调度键盘信息
        /// </summary>
        /// <param name="word"></param>
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

            //判断命令字 
            CommondWord_State(type, data);
        }

       
        /// <summary>
        /// 获取管理界面状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="num"></param>
        public string showGroupDate;
        private void CommondWord_State(string state, string data)
        {
            
            switch (state)
            {
                case "ADDKEYBOARD":
                    AnsAddKeyBoard(data);            
                    break;
                case "GETALLKEYBOARD":
                    DeskImage(data);
                    break;
                case "DELKEYBOARD":
                    AnsDelKeyBoard(data);
                    break;
                case "ADDSW":
                case "QUERYSW":
                case "DELSW":
                case "EDITSW":
                    swmanaagetab.recv(state,data);
                    break;
                case "ADDUSER":
                case "GETUSER":
                case "DELUSER":
                case "EDITUSER":
                    usermanagetab.recv(state,data);
                    break;
                case "GETPHONEBOOK":
                case "ADDPHONEBOOK":
                case "EDITPHONEBOOK":
                case "DELPHONEBOOK":
                    contactmanaagetab.recv(state, data);
                    break;
                case "GETALLREGISTERDEV":
                    ShowGroupMember(data); 
                    showGroupDate = data;
                    break;
                default: 
                    break;
            }              
        }
        /// ==============================================================
        

        public CreateDeskWindow myprofile = new CreateDeskWindow();
        private void GetDeskMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //var se = switchGrid.SelectedItem;

                //FrameworkElement el = switchGrid.Columns[0].GetCellContent(se);
                //DataGridRow row = DataGridRow.GetRowContainingElement(el.Parent as FrameworkElement);
                //row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                //MyProfile myprofile = new MyProfile();
                //myprofile.ShowDialog();
                myprofile.Show();
            }
        }   

        private void callManager_click(object sender, RoutedEventArgs e)
        {
            if (CtrlSwitchEvent != null)
            {
                CtrlSwitchEvent();
            }
        }

        public void man_header_click(string name)
        {
            switch (name)
            {
                case "sw":
                    tabControl_mgt.SelectedIndex = 0;
                    break;
                case "user":
                    tabControl_mgt.SelectedIndex = 1;

                    /* 发送调度台查询命令 */
                    queryDesk();

                    break;                
                case "desk":
                    //tabControl_mgt.SelectedIndex = 2;
                    tabControl_mgt.SelectedIndex = 5;
                    // 查询调度键盘 20181024 xiaozi
                    //keyboardmanagetab.DataContext = keyboardmanagedata;
                    InitialDeskes();
                    break;
                case "contact":
                    tabControl_mgt.SelectedIndex = 2;

                    /* 发送电话簿查询命令 */
                    queryContact();

                    break;
                default:
                    break;
            }
        }

        public void radioBtn_man_click(object sender, RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn == null)
                return;

            man_header_click(btn.Name);           

        }


        #region 网络收发
        public void querySWDevice()
        {
            /* 向服务器请求列表 */
            SWQUERY swquery = new SWQUERY();
            swquery.sequence = GlobalFunAndVar.sequenceGenerator();
            StringBuilder sb = new StringBuilder(100);
            sb.Append("MAN#QUERYSW#");
            sb.Append(JsonConvert.SerializeObject(swquery));

            Debug.WriteLine("SEND: " + sb.ToString());
            sendMsg(this, "net", sb.ToString());
        }

        public void delSWReq(string index)
        {
            /* 向服务器请求列表 */
            SWQUERY swquery = new SWQUERY();
            swquery.sequence = GlobalFunAndVar.sequenceGenerator();
            swquery.index = index;
            StringBuilder sb = new StringBuilder(100);
            sb.Append("MAN#DELSW#");
            sb.Append(JsonConvert.SerializeObject(swquery));

            Debug.WriteLine("SEND: " + sb.ToString());
            sendMsg(this, "net", sb.ToString());
        }

        public void queryUSER()
        {
            /* 向服务器请求列表 */
            string sequence = GlobalFunAndVar.sequenceGenerator();
            StringBuilder sb = new StringBuilder(100);

            sb.Append("MAN#GETUSER#{\"sequence\":");
            sb.Append(JsonConvert.SerializeObject(sequence));
            sb.Append("}");

            Debug.WriteLine("SEND: " + sb.ToString());
            sendMsg(this, "net", sb.ToString());
        }

        public void queryContact()
        {
            /* 向服务器请求列表 */
            string sequence = GlobalFunAndVar.sequenceGenerator();
            StringBuilder sb = new StringBuilder(100);

            sb.Append("MAN#GETPHONEBOOK#{\"sequence\":");
            sb.Append(JsonConvert.SerializeObject(sequence));
            sb.Append("}");

            Debug.WriteLine("SEND: " + sb.ToString());
            sendMsg(this, "net", sb.ToString());
        }  


        // 查询调度键盘 未用
        public void queryDesk()
        {
            SearchDesk searchDesk = new SearchDesk();
            searchDesk.sequence = GlobalFunAndVar.sequenceGenerator();
            StringBuilder sb = new StringBuilder(100);
            sb.Append("MAN#GETALLKEYBOARD#");
            sb.Append(JsonConvert.SerializeObject(searchDesk));

            Debug.WriteLine("SEND：" + sb.ToString());
            sendMsg(this, "net", sb.ToString());

        }

        /// <summary>
        /// 向服务器请求删除第index调度键盘
        /// </summary>
        /// <param name="index"></param>
        public void delDeskReq(string index)
        {
            DelKeyBoard req = new DelKeyBoard();
            req.index = index;
            req.sequence = GlobalFunAndVar.sequenceGenerator();
            StringBuilder sb = new StringBuilder(100);
            sb.Append("MAN#DELKEYBOARD#");
            sb.Append(JsonConvert.SerializeObject(req));

            Debug.WriteLine("SEND：" + sb.ToString());
            sendMsg(this, "net", sb.ToString());
        }

        #endregion        

        /* 向服务器发送网络协议 */
        /// <summary>
        /// 20181025 xf Add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        /// <param name="obj"></param>
        public void sendMsg(object sender, string msg, object obj)
        {
            if (msg == "net")
            {
                /* 网络传输协议 */
                mainWindow.ws.Send(obj as string);
            }
            else if (msg == "swdev")
            {
                //swdevobj = obj as SWDEV;
            }
            else if (msg == "user")
            {
                //userobj = obj as User;
            }

        }

        /* 响应添加按钮事件 */
        private void btn_sw_add_Click(object sender, RoutedEventArgs e)
        {
            /* 响应添加软交换设备按钮事件 */
            //int index = tabItem_sw.TabIndex;
            int index = tabControl_mgt.SelectedIndex;
            /* 创建软交换页面 */
            if (0 == index)
            {
                // 新建软交换服务设备
                CreateSwitchWindow csw = new CreateSwitchWindow();
                // 传递消息事件
                csw.msgevent += new CreateSwitchWindow.CWHandler(sendMsg);

                csw.ShowDialog();
            }
            else if (1 == index)  // 创建用户页面
            {
                cuw.ShowDialog();                
            }
            else
            {
                desk_add_Click();
                //tabControl_mgt.SelectedIndex = index;
            }
            //else
            //{
            //    CreateDeskWindow cdw = new CreateDeskWindow();
            //    cdw.ShowDialog();
            //}
        }

        /* 响应删除按钮事件  */
        private void btn_sw_delete_Click(object sender, RoutedEventArgs e)
        {
            int index = tabControl_mgt.SelectedIndex;
            Debug.WriteLine(index);
            /* 删除软交换页面 */
            if (0 == index)
            {
                //int i = switchGrid.SelectedIndex;
                
            }
            else if (1 == index)
            {
                int i = userGrid.SelectedIndex;
                
            }
            else
            {
                delDeskReq(getDesk.keyboardlist[keyBoardNum.index - 1].index.ToString());
                //desk_delete_Click();
            }
        }
        

#region 软交换设备表格

        /// <summary>
        /// 软交换列表中处理查看信息按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SWGrid_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Int16 index = Convert.ToInt16(btn.Tag);
                Debug.WriteLine("current index: " + index);
            }
        }     

        /// <summary>    
        /// 找到行明细中嵌套的控件名称    
        /// </summary>    
        /// <typeparam name="T"></typeparam>    
        /// <param name="parent"></param>    
        /// <param name="name"></param>    
        /// <returns></returns>    
        public T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i) as DependencyObject;
                    string controlName = child.GetValue(Control.NameProperty) as string;
                    if (controlName == name)
                    {
                        return child as T;
                    }
                    else
                    {
                        T result = FindVisualChildByName<T>(child, name);
                        if (result != null)
                            return result;
                    }
                }
            }
            return null;
        }   
        
#endregion

        // 查看用户信息
        private void Btn_UserGrid_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {

            }
        }

        // 更新用户信息        
        private void Btn_UserUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                // 获取当前的list序号
                Int16 index = Convert.ToInt16(btn.Tag);
                Debug.WriteLine("update index: " + btn.Tag);

                USEREDITITEM tempUser = new USEREDITITEM();
                tempUser.sequence = GlobalFunAndVar.sequenceGenerator();
                tempUser.name = userName.Text.Trim();
                tempUser.password = userPass.Text.Trim();
                tempUser.status = userStatus.Text.Trim();
                tempUser.description = userDesp.Text.Trim();
                int ind = userDesk.SelectedIndex;
                tempUser.desk = deskList[ind].id.ToString();// userDesk.SelectedValue.ToString(); //userDesk.Text.Trim();
                tempUser.role = userRole.Text.Trim();
                tempUser.privilege = userPriv.Text.Trim();

                // 用于界面显示
                User uiUser = new User();
                uiUser.sequence = tempUser.sequence;
                uiUser.name = tempUser.name;
                uiUser.password = tempUser.password;
                uiUser.status = tempUser.status;
                uiUser.description = tempUser.description;
                uiUser.desk = tempUser.desk;
                uiUser.role = tempUser.role;
                uiUser.privilege = tempUser.privilege;
                // 保存List的序号
                uiUser.index = index;

                /* 向服务器发送删除消息 */

                StringBuilder sb = new StringBuilder(100);
                sb.Append("MAN#EDITUSER#");
                sb.Append(JsonConvert.SerializeObject(tempUser));

                Debug.WriteLine("SEND: " + sb.ToString());
                sendMsg(this, "net", sb.ToString());
                sendMsg(this, "user", uiUser);
                /*
                BindingExpression be = temp.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
                */
                //MainWindow.swList[index].name = switchGrid.
            }
        }


        private void desk_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void sw_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void NextStepHotLine(object sender, RoutedEventArgs e)
        {

        }
    }
}
