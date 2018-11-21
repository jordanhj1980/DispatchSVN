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

        // 软交换设备列表
        ObservableCollection<SWDEV> swList;

        // 用户列表
        ObservableCollection<User> m_UserList;
        private User _selectedUserItem;
        public User SelectedUserItem {
            get { return _selectedUserItem; }
            set
            {
                _selectedUserItem = value;
                //SetAndNotifyIfChanged("SelectedUserItem", ref _selectedUserItem, value);
            }
        }  /* 保存临时的用户item */

        // 当对话框打开添加软交换时，临时存储软交换的信息
        public SWDEV swdevobj;      // 新增软交换

        private SWDEV _selectedSW;  // 列表中当前选中项
        /// <summary>
        /// function 列表中选中行的对象
        /// </summary>
        public SWDEV SelectedSW
        {
            get { return _selectedSW; }
            set
            {
                if (_selectedSW != value)
                {
                    _selectedSW = value;
                    RaisePropertyChanged("SelectedSW");
                }
            }
        }

        // 当对话框打开添加用户时，临时存储用户的信息
        public User userobj;

        public string ImageSource { get; set; }
        //public Image img_collapse { get; set; }

        public bool isCollapse;
        // 标识当前行是否为选中
        private bool _rowSelectionChanged;

        public CallManagerControl(MainWindow mmainWindow)
        {
            DataContext = this;

            mainWindow = mmainWindow;
            InitializeComponent();

            // 初始化左侧资源服务列表
            //ToolTipTestWindow();
            isCollapse = true;

            var viewModel = new ExpanderListViewModel();
            StackPanelLeft.DataContext = viewModel;

            viewModel.SelectedExpander = 1;

            Expander exp = new Expander();
            exp.Header = "exp1";
            StackPanel sp1 = new StackPanel();
            exp.Content = sp1;
            RadioButton rb = new RadioButton();
            rb.Content = "rb1";
            sp1.Children.Add(rb);

            //StackPanelLeft.Children.Add(exp);

            // 初始化软交换设备列表
            InitialSwitches();
        }

        public void ToolTipTestWindow()
        {
            LoadData();
            Expander ep;
            foreach (TestClass ts in classlist)
            {
               ep = new Expander();
               ep.Header = ts.ClassTypeId;
               ep.FontSize = 14;
               ep.Content = new ListBox()
               {
                   ItemsSource = ts.btnList,
                   DisplayMemberPath = "Content"
               };

               StackPanelLeft.Children.Add(ep);
            }
        }
       
        /// <summary>
        /// 接收信息
        /// </summary>
        public void InitialSwitches()
        {
            swdevobj = new SWDEV();
            SelectedSW = new SWDEV();
            SelectedSW.name = "";
            SelectedSW.ip = "";

            userobj = new User();

            swList = new ObservableCollection<SWDEV>();
            m_UserList = new ObservableCollection<User>();

            switchGrid.ItemsSource = swList;
            //switchGrid.MouseDown += DataGrid_MouseDown;
            switchGrid.SelectionChanged += DataGrid_Click;


            userGrid.ItemsSource = m_UserList;
            userGrid.MouseDown += DataGrid_MouseDown;
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
                case "DELALLKEYBOARD":
                    AnsDelKeyBoard(data);
                    break;
                case "ADDSW":
                    addSwitchDevice(data);
                    break;
                case "QUERYSW":
                    freshSwitchDevice(data);
                    break;
                case "DELSW":
                    delSwitchDevice(data);
                    break;
                case "EDITSW":
                    editSwitchDevice(data);
                    break;
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
                case "GETALLREGISTERDEV":
                    ShowGroupMember(data); 
                    showGroupDate = data;
                    break;
                default: 
                    break;
            }              
        }
        /// ==============================================================
        /// 
        /// <summary>
        /// 删除调度键盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_desk_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                getDesk.keyboardlist.Remove(getDesk.keyboardlist[keyBoardNum.index - 1]);
                keyBoardNum.count--;

                string sendMsg = JsonConvert.SerializeObject(getDesk);
                DeskImage(sendMsg);

                string strMsg = "MAN#DELKEYBOARD#" + sendMsg;
                mainWindow.ws.Send(strMsg);


                //tabControl1.SelectedIndex = 2;  // 跳转到键盘的树
                tabControl_mgt.SelectedIndex = 2;  // 跳转到键盘的树
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + "b");
            }
        }


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
        
        private void LoadData()
        {
            classlist = new List<TestClass>();
            TestClass class1 = new TestClass();
            class1.ClassTypeId = 1;
            List<RadioButton> list1 = new List<RadioButton>()
            {
                new RadioButton(){Content="1a", GroupName="btnGrp" },
                new RadioButton(){Content="1b", GroupName="btnGrp" },
                new RadioButton(){Content="1c", GroupName="btnGrp" },
            };
            class1.btnList = list1;
            classlist.Add(class1);
            TestClass class2 = new TestClass();
            class2.ClassTypeId = 2;
            List<RadioButton> list2 = new List<RadioButton>()
            {
                new RadioButton(){Content="2a", GroupName="btnGrp"},
                new RadioButton(){Content="2b", GroupName="btnGrp"},
                new RadioButton(){Content="2c", GroupName="btnGrp"},
            };
            class2.btnList = list2;
            classlist.Add(class2);
        }

        private void callManager_click(object sender, RoutedEventArgs e)
        {
            if (CtrlSwitchEvent != null)
            {
                CtrlSwitchEvent();
            }
        }

        public void radioBtn_man_click(object sender, RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn == null)
                return;
            switch (btn.Name)
            {
                case "sw":
                    tabControl_mgt.SelectedIndex = 0;
                    break;
                case "user":
                    tabControl_mgt.SelectedIndex = 1;
                    break;
                case "desk":
                    tabControl_mgt.SelectedIndex = 2;
                    // 查询调度键盘 20181024 xiaozi
                    InitialDeskes();
                    break;
                default:
                    break;
            }

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

            Trace.WriteLine("SEND: " + sb.ToString());
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

            Trace.WriteLine("SEND: " + sb.ToString());
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

            Trace.WriteLine("SEND: " + sb.ToString());
            sendMsg(this, "net", sb.ToString());
        }

        public void delUserReq(string name)
        {
            /* 向服务器请求列表 */
            USERQUERY req = new USERQUERY();
            req.sequence = GlobalFunAndVar.sequenceGenerator();
            req.name = name;
            StringBuilder sb = new StringBuilder(100);
            sb.Append("MAN#DELUSER#");
            sb.Append(JsonConvert.SerializeObject(req));

            Trace.WriteLine("SEND: " + sb.ToString());
            sendMsg(this, "net", sb.ToString());
            /* 临时保存用户名 */
            userobj.name = name;
        }

        // 查询调度键盘 未用
        public void queryDesk()
        {
            SearchDesk searchDesk = new SearchDesk();
            searchDesk.sequence = GlobalFunAndVar.sequenceGenerator();
            StringBuilder sb = new StringBuilder(100);
            sb.Append("MAN#GETALLKEYBOARD#");
            sb.Append(JsonConvert.SerializeObject(searchDesk));

            Trace.WriteLine("SEND：" + sb.ToString());
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

            Trace.WriteLine("SEND：" + sb.ToString());
            sendMsg(this, "net", sb.ToString());
        }




        public void recvNetMsg(string msg)
        {
            int startpos = msg.IndexOf("#");
            string msgTpye = msg.Substring(0, startpos);
            string data = msg.Substring(startpos + 1);

            switch (msgTpye)
            {
                case "ADDSW":
                    addSwitchDevice(data);
                    break;
                case "QUERYSW":
                    freshSwitchDevice(data);
                    break;
                case "DELSW":
                    delSwitchDevice(data);
                    break;
                case "EDITSW":
                    editSwitchDevice(data);
                    break;
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
        #endregion


        /* 添加软交换设备 */
        /// <summary>
        /// 20181025 xf Add
        /// </summary>
        /// <param name="data"></param>
        private void addSwitchDevice(string data)
        {

            try
            {
                /* 比较临时存储的obj的sequence是否一致 */
                SW_ADDRESULT res = JsonConvert.DeserializeObject<SW_ADDRESULT>(data);
                if (res != null && swdevobj.sequence == res.sequence)
                {
                    if (res.result == "Failed")
                    {
                        Trace.WriteLine(res.reason);
                        return;
                    }

                    SWDEV item = new SWDEV();
                    //item.idstr = swdevobj.name;
                    item.name = swdevobj.name;
                    item.ip = swdevobj.ip;
                    item.port = swdevobj.port;
                    item.index = swdevobj.index;
                    item.type = swdevobj.type;

                    swList.Add(item);
                }
            }
            catch (System.Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        private void delSwitchDevice(string data)
        {
            /* 比较临时存储的obj的sequence是否一致 */
            SW_ADDRESULT res = JsonConvert.DeserializeObject<SW_ADDRESULT>(data);
            if (res != null /* && swdevobj.sequence == res.sequence */)
            {
                if (res.result == "Failed")
                {
                    Trace.WriteLine(res.reason);
                    return;
                }

                /* 查询index */
                for (int i = 0; i < swList.Count; i++)
                {
                    if ((swList[i].index).ToString() == res.index)
                    {
                        swList.RemoveAt(i);
                    }
                }
            }
        }

        /* 接收到服务器的回应消息，更新软交换设备列表 */
        private void editSwitchDevice(string data)
        {
            /* 比较临时存储的obj的sequence是否一致 */
            SW_ADDRESULT res = JsonConvert.DeserializeObject<SW_ADDRESULT>(data);
            if (res != null && swdevobj.sequence == res.sequence)
            {
                if (res.result == "Failed")
                {
                    Trace.WriteLine(res.reason);
                    return;
                }

                // 在swList中查询对应的item
                for (int i = 0; i < swList.Count; i++)
                {
                    // 根据index来判断
                    if (swList[i].index == swdevobj.index)
                        {
                        SWDEV dev = swList[i];
                        // index 不允许更新
                        dev.name = swdevobj.name;
                        dev.ip = swdevobj.ip;
                        dev.port = swdevobj.port;
                        dev.type = swdevobj.type;
                    }
                }
            }

        }

        /* 刷新软交换设备列表 */
        private void freshSwitchDevice(string data)
        {
            //swList.Clear();
            SW_QUERYRESULT res = JsonConvert.DeserializeObject<SW_QUERYRESULT>(data);
            if (res != null)
            {
                ObservableCollection<SWDEV> list = new ObservableCollection<SWDEV>();
                foreach (SWDEVITEM member in res.switchlist)
                {
                    SWDEV item = new SWDEV();
                    //item.idstr = member.name;
                    item.name = member.name;
                    item.index = member.index;
                    item.type = member.type;
                    item.ip = member.ip;
                    item.port = member.port.ToString();

                    swList.Add(item);
                }
                //  = list;
            }
        }

        /* 添加用户应答 */
        private void addUser(string data)
        {
            /* 比较临时存储的obj的sequence是否一致 */
            USER_ADDRESULT res = JsonConvert.DeserializeObject<USER_ADDRESULT>(data);
            if (res != null && userobj.sequence == res.sequence)
            {
                if (res.result == "Failed")
                {
                    Trace.WriteLine(res.reason);
                    return;
                }

                //retrievedItems is the data you received from the service
                //foreach (object item in retrievedItems)
                //Dispatcher.BeginInvoke(DispatcherPriority.Background, new ParameterizedThreadStart(AddItem), item);  
                //Dispatcher.BeginInvoke( new Action( ()  =>  {
                m_UserList.Add(userobj);
                //}));
            }
        }

        // 查询用户请求
        private void freshUser(string data)
        {
            m_UserList.Clear();
            USER_QUERYRESULT res = JsonConvert.DeserializeObject<USER_QUERYRESULT>(data);
            if (res != null)
            {
                ObservableCollection<USERITEM> list = new ObservableCollection<USERITEM>();
                foreach (USERITEM member in res.userlist)
                {
                    User item = new User();
                    item.index = m_UserList.Count;  // 列表序号，从0开始
                    item.idstr = member.name;
                    item.name = member.name;
                    item.password = member.password;
                    item.status = Convert.ToInt16("0" + member.status);
                    item.privilege = member.privilege;
                    item.description = member.description;
                    item.desk = member.desk;
                    item.IsDetailsExpanded = false;

                    m_UserList.Add(item);
                }
                //  = list;
            }

        }

        private void delUser(string data)
        {
            /* 比较临时存储的obj的sequence是否一致 */
            SW_ADDRESULT res = JsonConvert.DeserializeObject<SW_ADDRESULT>(data);
            if (res != null /* && swdevobj.sequence == res.sequence */)
            {
                if (res.result == "Failed")
                {
                    Trace.WriteLine(res.reason);
                    return;
                }

                /* 查询index */
                for (int i = 0; i < m_UserList.Count; i++)
                {
                    if (m_UserList[i].name == userobj.name)
                    {
                        m_UserList.RemoveAt(i);
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
                if (res.result == "Failed")
                {
                    Trace.WriteLine(res.reason);
                    return;
                }

                User item = new User();
                item.idstr = userobj.name;
                item.name = userobj.name;
                item.password = userobj.password;
                item.privilege = userobj.privilege;
                item.description = userobj.description;
                item.role = userobj.role;
                item.index = userobj.index;
                item.status = userobj.status;
                item.desk = userobj.desk;

                // 在swList中查询对应的item
                for (int i = 0; i < m_UserList.Count; i++)
                {
                    // 用户名来判断
                    if (m_UserList[i].name == item.name)
                    {
                        // 保留之前的展开状态
                        DataGridRow row;
                        Visibility visib = System.Windows.Visibility.Visible;
                        if (m_UserList[i].IsDetailsExpanded == false)
                        {
                            visib = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            visib = System.Windows.Visibility.Hidden;
                        }
                        item.IsDetailsExpanded = (visib == System.Windows.Visibility.Collapsed) ? true : false;
                        // using modified version will not notify the RowDetailsTemplate
                        m_UserList.RemoveAt(i);
                        if (i == 0)
                        {
                            m_UserList.Insert(0, item);
                        }
                        else
                        {
                            m_UserList.Insert(i - 1, item);
                        }

                        Trace.WriteLine("current selected index: " + i);
                        //row = (DataGridRow)switchGrid.ItemContainerGenerator.ContainerFromIndex(i);

                        //switchGrid.Focus();
                        //switchGrid.CurrentCell = 
                        userGrid.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        /* 向服务器发送网络协议 */
        /// <summary>
        /// 20181025 xf Add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        /// <param name="obj"></param>
        private void sendMsg(object sender, string msg, object obj)
        {
            if (msg == "net")
            {
                /* 网络传输协议 */
                mainWindow.ws.Send(obj as string);
            }
            else if (msg == "swdev")
            {
                swdevobj = obj as SWDEV;
            }
            else if (msg == "user")
            {
                userobj = obj as User;
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
                // 新建用户
                CreateUserWindow cuw = new CreateUserWindow();

                // 传递消息事件
                cuw.msgevent += new CreateUserWindow.CWHandler(sendMsg);
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
            Trace.WriteLine(index);
            /* 删除软交换页面 */
            if (0 == index)
            {
                int i = switchGrid.SelectedIndex;
                if (MessageBox.Show("确定是否要删除设备 " + swList[i].name, "提示消息",
                MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    delSWReq(swList[i].index.ToString());
                }
            }
            else if (1 == index)
            {
                int i = userGrid.SelectedIndex;
                if (MessageBox.Show("确定是否要删除用户 " + m_UserList[i].name, "提示消息",
                MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    delUserReq(m_UserList[i].name);
                }
            }
            else
            {
                delDeskReq(getDesk.keyboardlist[keyBoardNum.index - 1].index.ToString());
                desk_delete_Click();
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
                Trace.WriteLine("current index: " + index);
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

        /// <summary>
        /// 更新软交换数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                Int16 index = Convert.ToInt16(btn.Tag);
                Trace.WriteLine("update index: " + btn.Tag);

                SWDEV tempDev = new SWDEV();
                tempDev.sequence = GlobalFunAndVar.sequenceGenerator();
                tempDev.index = swIndex.Text;
                tempDev.name = swName.Text;
                tempDev.ip = swIP.Text;
                tempDev.port = swPort.Text;
                tempDev.type = swType.Text;

                /* 向服务器发送更新消息 */
                StringBuilder sb = new StringBuilder(100);
                sb.Append("MAN#EDITSW#");
                sb.Append(JsonConvert.SerializeObject(tempDev));

                Trace.WriteLine("SEND: " + sb.ToString());
                sendMsg(this, "net", sb.ToString());
                sendMsg(this, "swdev", tempDev);
            }
        }

        private void btn_expOrCollapse_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage imgSource;
            if (isCollapse)
            {
                imgSource = new BitmapImage(new Uri("../Resources/expand.png", UriKind.Relative));
                isCollapse = false;
            }
            else
            {
                imgSource = new BitmapImage(new Uri("../Resources/collapse.png", UriKind.Relative));
                isCollapse = true;
            }
            img_collapse.Source = imgSource;
        }

        private void DataGrid_Click(object sender, SelectionChangedEventArgs e)
        {
            int index = switchGrid.SelectedIndex;

            if (index >= 0 && index < swList.Count)
            {
                //SelectedSW = swList[index];
            }
        }

        private void DataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                int index = switchGrid.SelectedIndex;

                if (index >= 0 && index < swList.Count)
                {
                //    SelectedSW = swList[index];
                }

                /*
                var se = switchGrid.SelectedItem;

                FrameworkElement el = switchGrid.Columns[0].GetCellContent(se);
                DataGridRow row = DataGridRow.GetRowContainingElement(el.Parent as FrameworkElement);
                row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                
                */
                //if (objElement != null)
                //{
                //    TextBlock objChk = (TextBlock)objElement;
                //    //objChk.IsChecked = !objChk.IsChecked;
                //    Console.WriteLine("MouseDown");
                //}
            }
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
                Trace.WriteLine("update index: " + btn.Tag);

                DataGridRow dgr = (DataGridRow)userGrid.ItemContainerGenerator.ContainerFromIndex(userGrid.SelectedIndex);

                TextBox tb_idstr = FindVisualChildByName<TextBox>(dgr, "detailidstr") as TextBox;
                TextBox tb_name = FindVisualChildByName<TextBox>(dgr, "detailname") as TextBox;
                TextBox tb_pass = FindVisualChildByName<TextBox>(dgr, "detailpass") as TextBox;
                TextBox tb_status = FindVisualChildByName<TextBox>(dgr, "detailstatus") as TextBox;
                TextBox tb_desp = FindVisualChildByName<TextBox>(dgr, "detaildesp") as TextBox;
                TextBox tb_desk = FindVisualChildByName<TextBox>(dgr, "detaildesk") as TextBox;
                TextBox tb_role = FindVisualChildByName<TextBox>(dgr, "detailrole") as TextBox;
                TextBox tb_priv = FindVisualChildByName<TextBox>(dgr, "detailpriv") as TextBox;

                USEREDITITEM tempUser = new USEREDITITEM();
                tempUser.sequence = GlobalFunAndVar.sequenceGenerator();
                tempUser.name = tb_name.Text.Trim();
                tempUser.password = tb_pass.Text.Trim();
                tempUser.status = tb_status.Text.Trim();
                tempUser.description = tb_desp.Text.Trim();
                tempUser.desk = tb_desk.Text.Trim();
                tempUser.role = tb_role.Text.Trim();
                tempUser.privilege = tb_priv.Text.Trim();

                // 用于界面显示
                User uiUser = new User();
                uiUser.sequence = tempUser.sequence;
                uiUser.name = tempUser.name;
                uiUser.password = tempUser.password;
                uiUser.status = Convert.ToInt16(tempUser.status);
                uiUser.description = tempUser.description;
                uiUser.desk = tempUser.desk;
                uiUser.role = Convert.ToInt16(tempUser.role);
                uiUser.privilege = tempUser.privilege;
                // 保存List的序号
                uiUser.index = index;

                /* 向服务器发送删除消息 */

                StringBuilder sb = new StringBuilder(100);
                sb.Append("MAN#EDITUSER#");
                sb.Append(JsonConvert.SerializeObject(tempUser));

                Trace.WriteLine("SEND: " + sb.ToString());
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
