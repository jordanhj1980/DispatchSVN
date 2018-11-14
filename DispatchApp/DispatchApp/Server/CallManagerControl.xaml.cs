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
    public partial class CallManagerControl : UserControl
    {
        private MainWindow mainWindow; // 20181024 xiaozi

        public event CtrlSwitchHandler CtrlSwitchEvent;
        List<TestClass> classlist;

        // 软交换设备列表
        ObservableCollection<SwitchDevice> swList;

        // 用户列表
        ObservableCollection<User> m_UserList;

        // 当对话框打开添加软交换时，临时存储软交换的信息
        public SWDEV swdevobj;
        // 当对话框打开添加用户时，临时存储用户的信息
        public User userobj;

        public string ImageSource { get; set; }
        //public Image img_collapse { get; set; }

        public bool isCollapse;
        // 标识当前行是否为选中
        private bool _rowSelectionChanged;

        public CallManagerControl(MainWindow mmainWindow)
        {
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
        /// 初始化软交换设备列表
        /// </summary>
        public void InitialSwitches()
        {
            swdevobj = new SWDEV();
            userobj = new User();

            swList = new ObservableCollection<SwitchDevice>();
            m_UserList = new ObservableCollection<User>();

            switchGrid.ItemsSource = swList;
            switchGrid.MouseDown += DataGrid_MouseDown;

            userGrid.ItemsSource = m_UserList;
            userGrid.MouseDown += DataGrid_MouseDown;
        }

        // =======================发送查询调度台信息指令====================
        /// 20181024 xiaozi Add
        /// <summary>
        /// 查询调度键盘 20181024 xiaozi
        /// </summary>
        public void InitialDeskes()
        {
            // 查询调度键盘
            SearchDesk searchDesk = new SearchDesk() { sequence = GlobalFunAndVar.sequenceGenerator() };
            string strMsg = "MAN#GETALLKEYBOARD#" + JsonConvert.SerializeObject(searchDesk);
            mainWindow.ws.Send(strMsg);
            // 查询所有注册的电话
            searchAllRegest.sequence = GlobalFunAndVar.sequenceGenerator();
            string searchAllCallNum = "MAN#GETALLREGISTERDEV#" + JsonConvert.SerializeObject(searchAllRegest);
            mainWindow.ws.Send(searchAllCallNum);
            // 当前组的序列 和 组总数
            keyBoardNum.groupNum.index = keyBoardNum.groupNum.count + 1;
            keyBoardNum.groupNum.count++;           
        }
        //==================================================================

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
        /// 
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
                    //AddDeskDecice(date);
                    //AddDesk(date);
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
                    //ShowGroupMember(date);
                    showGroupDate = data;
                    break;
                default: 
                    break;
            }              
        }


        /// =====================
        /// 用page实现分层功能
        ///
        //private void AddDesk(string date)
        //{
        //    // step1 调度台
        //    getDesk = JsonConvert.DeserializeObject<GetDesk>(date);
        //    // step2 一个调度台管理的所有调度键盘
        //    int keyBoardCount = getDesk.keyboardlist.Count;
        //    for (int idex = 0; idex < keyBoardCount; idex++)
        //    {
        //        KeyBoard keyBoard = getDesk.keyboardlist[idex]; // 一个调度键盘
        //        //////////////////////////////////// 加入页面超链接
        //        TextBlock textBlock = new TextBlock();
        //        Hyperlink hyperlink = new Hyperlink();
        //        Run run = new Run();
        //        run.Text = keyBoard.name;
        //        hyperlink.Inlines.Add(run);
        //        textBlock.Inlines.Add(hyperlink);
        //        hyperlink.Name = "name" + idex.ToString();
        //        hyperlink.MouseEnter += new MouseEventHandler(link_MouseEnter);//为超链接文本添加鼠标进入触发事件
        //        hyperlink.MouseLeave += new MouseEventHandler(link_MouseLeave);//为超链接文本添加鼠标离开触发时间                     
        //        //NavigationService.GetNavigationService(this).Navigate(new PageKeyBoard(keyBoard));
        //        hyperlink.Click += new RoutedEventHandler(link_Click);//为超链接文本添加鼠标单击事件   
        //        boardStep1.Children.Add(textBlock);
        //        ////////////////////////////////////

        //        // step3 一个调度键盘下的所有组
        //        int grouplistCount = keyBoard.grouplist.Count;
        //        for (int inex = 0; inex < grouplistCount; inex++)
        //        {
        //            Group group = keyBoard.grouplist[inex];
        //            // step4 一个组下的所有成员
        //            int memberListCount = group.memberlist.Count;
        //            for (int ikex = 0; ikex < memberListCount; ikex++)
        //            {
        //                Member member = group.memberlist[ikex];
        //            }
        //        }
        //        // step3 一个调度键盘下的所有线路
        //        int hotlinelistCount = keyBoard.hotlinelist.Count;
        //        for (int iqex = 0; iqex < hotlinelistCount; iqex++)
        //        {
        //            Hotline holine = keyBoard.hotlinelist[iqex];
        //        }
        //    }
        //}


        //private void link_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    Hyperlink link = (Hyperlink)e.OriginalSource;
        //    link.Foreground = System.Windows.Media.Brushes.Red;
        //}

        //private void link_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    Hyperlink link = (Hyperlink)e.OriginalSource;
        //    link.Foreground = System.Windows.Media.Brushes.White;
        //}

        //private void link_Click(object sender, RoutedEventArgs e)
        //{
        //    Hyperlink hyperlink = sender as Hyperlink;

        //    for (int i=0; i<getDesk.keyboardlist.Count; i++)
        //    {
        //        if (hyperlink.Name==("name" + i.ToString()))
        //        {
        //            PageKeyBoard pageKeyBoard = new PageKeyBoard(getDesk.keyboardlist[i]);
        //            keyBoardPage.Content = pageKeyBoard;
        //        }
        //    }
         
        //    //怎么切页面。
        //    //NavigationService.GetNavigationService(this).Navigate(new PageKeyBoard(keyBoard));
        //}
      

        /// <summary>
        /// 用treeview实现分层功能
        /// </summary>
        /// <param name="date"></param>
        //private void AddDeskDecice(string date)
        //{
        //    TreeView treeView = new TreeView();
        //    List<propertyNodeItem> listItem = new List<propertyNodeItem>();
        //    // step1 调度台
        //    getDesk = JsonConvert.DeserializeObject<GetDesk>(date);

        //    // step2 一个调度台管理的所有调度键盘
        //    int keyBoardCount = getDesk.keyboardlist.Count;
        //    for (int idex = 0; idex < keyBoardCount; idex++)
        //    {
        //        KeyBoard keyBoard = getDesk.keyboardlist[idex]; // 一个调度键盘

        //        TabItem ti = new TabItem();
        //        ti.Header = keyBoard.index;

        //        List<TextBlock> dataGrid = new List<TextBlock>();
        //        ti.Content = dataGrid;
        //        //KeyBoardTab.Items.Add(ti);

        //        propertyNodeItem keyBoardNode = new propertyNodeItem() { DisplayName = keyBoard.name };
        //        listItem.Add(keyBoardNode);

        //        propertyNodeItem keyBoardNameNode = new propertyNodeItem() { DisplayName = "MAC:" + keyBoard.name };
        //        propertyNodeItem keyBoardIpNode = new propertyNodeItem() { DisplayName = "IP:" + keyBoard.ip };
        //        propertyNodeItem keyBoardMacNode = new propertyNodeItem() { DisplayName = "MAC:" + keyBoard.mac };
        //        propertyNodeItem keyBoardSeqNode = new propertyNodeItem() { DisplayName = "Sequence:" + keyBoard.sequence };
        //        propertyNodeItem keyBoardGroNode = new propertyNodeItem() { DisplayName = "Grouplist:" };
        //        propertyNodeItem keyBoardHotNode = new propertyNodeItem() { DisplayName = "Hotlinelist:" };
        //        keyBoardNode.Children.Add(keyBoardNameNode);
        //        keyBoardNode.Children.Add(keyBoardIpNode);
        //        keyBoardNode.Children.Add(keyBoardMacNode);
        //        keyBoardNode.Children.Add(keyBoardSeqNode);
        //        keyBoardNode.Children.Add(keyBoardGroNode);
        //        keyBoardNode.Children.Add(keyBoardHotNode);

        //        // step3 一个调度键盘下的所有组
        //        int grouplistCount = keyBoard.grouplist.Count;
        //        for (int inex = 0; inex < grouplistCount; inex++)
        //        {
        //            Group group = keyBoard.grouplist[inex];

        //            propertyNodeItem groupNode = new propertyNodeItem() { DisplayName = "Group:" + group.index };
        //            keyBoardGroNode.Children.Add(groupNode);

        //            propertyNodeItem groupIndexNode = new propertyNodeItem() { DisplayName = "Index:" + group.index };
        //            propertyNodeItem groupDesNode = new propertyNodeItem() { DisplayName = "Description:" + group.description };
        //            propertyNodeItem groupColNode = new propertyNodeItem() { DisplayName = "Column:" + group.column };
        //            propertyNodeItem groupNamNode = new propertyNodeItem() { DisplayName = "Groupname:" + group.groupname };
        //            propertyNodeItem groupMemNode = new propertyNodeItem() { DisplayName = "memberlist:" };
        //            groupNode.Children.Add(groupIndexNode);
        //            groupNode.Children.Add(groupDesNode);
        //            groupNode.Children.Add(groupColNode);
        //            groupNode.Children.Add(groupNamNode);
        //            groupNode.Children.Add(groupMemNode);

        //            // step4 一个组下的所有成员
        //            int memberListCount = group.memberlist.Count;
        //            for (int ikex = 0; ikex < memberListCount; ikex++)
        //            {
        //                Member member = group.memberlist[ikex];
        //                propertyNodeItem memberNode = new propertyNodeItem() { DisplayName = "Member:" + member.callno };
        //                groupMemNode.Children.Add(memberNode);

        //                propertyNodeItem memberCalNode = new propertyNodeItem() { DisplayName = "CallNum:" + member.callno };
        //                propertyNodeItem memberDesNode = new propertyNodeItem() { DisplayName = "Description:" + member.description };
        //                propertyNodeItem memberLevelNode = new propertyNodeItem() { DisplayName = "Level:" + member.level };
        //                propertyNodeItem memberNameNode = new propertyNodeItem() { DisplayName = "Name:" + member.name };
        //                propertyNodeItem memberTypeNode = new propertyNodeItem() { DisplayName = "Type:" + member.type };
        //                memberNode.Children.Add(memberCalNode);
        //                memberNode.Children.Add(memberDesNode);
        //                memberNode.Children.Add(memberLevelNode);
        //                memberNode.Children.Add(memberNameNode);
        //                memberNode.Children.Add(memberTypeNode);
        //            }

        //        }
        //        // step3 一个调度键盘下的所有线路
        //        int hotlinelistCount = keyBoard.hotlinelist.Count;
        //        for (int iqex = 0; iqex < hotlinelistCount; iqex++)
        //        {
        //            Hotline holine = keyBoard.hotlinelist[iqex];

        //            propertyNodeItem holineNode = new propertyNodeItem() { DisplayName = holine.callno };
        //            keyBoardHotNode.Children.Add(holineNode);

        //            propertyNodeItem holineCallNode = new propertyNodeItem() { DisplayName = "CallNum:" + holine.callno };
        //            propertyNodeItem holineDesNode = new propertyNodeItem() { DisplayName = "Description:" + holine.description };
        //            propertyNodeItem holineLevelNode = new propertyNodeItem() { DisplayName = "Level:" + holine.level };
        //            propertyNodeItem holineTypeNode = new propertyNodeItem() { DisplayName = "Name:" + holine.type };
        //            propertyNodeItem holineNameNode = new propertyNodeItem() { DisplayName = "Type:" + holine.name };
        //            holineNode.Children.Add(holineCallNode);
        //            holineNode.Children.Add(holineDesNode);
        //            holineNode.Children.Add(holineLevelNode);
        //            holineNode.Children.Add(holineTypeNode);
        //            holineNode.Children.Add(holineNameNode);
        //        }
        //    }
        //    this.tvProperty.ItemsSource = listItem;

        //    // 根据信息进行页面布置

        //    // Console.WriteLine("keyBoard" + getDesk);
        //}
        /// =====================
        /// 

        /// <summary>
        /// 添加调度键盘的应答
        /// </summary>
        private void AnsAddKeyBoard(string date)
        {
            AnsAddKeyBoard ans = JsonConvert.DeserializeObject<AnsAddKeyBoard>(date);
            Console.WriteLine(ans);
        }

        private void AnsDelKeyBoard(string date)
        {
            AnsDelKeyBoard ans = JsonConvert.DeserializeObject<AnsDelKeyBoard>(date);
        }

        public GetDesk getDesk = new GetDesk();
        public KeyBoardNum keyBoardNum = new KeyBoardNum();
        private void DeskImage(string date)
        {
            // 一棵树
            List<propertyNodeItem> listItem = new List<propertyNodeItem>();

            /* 调度台查询到的信息 */ 
            getDesk = JsonConvert.DeserializeObject<GetDesk>(date); // 一个调度台
            keyBoardNum.count = getDesk.keyboardlist.Count;         // 调度台里的调度键盘个数

            // 遍历所有的调度键盘
            for (int idex = 0; idex < keyBoardNum.count; idex++)
            {
                // 第一级树枝名称
                KeyBoard keyBoard = getDesk.keyboardlist[idex];
                propertyNodeItem keyBoardNode = new propertyNodeItem() { DisplayName = keyBoard.name, Tag = idex + 1};
                // 第一级树枝加入树中
                listItem.Add(keyBoardNode);
            }
            this.tvProperty.ItemsSource = listItem;
        }

        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock newTextBlock = sender as TextBlock;

            Int16 index = Convert.ToInt16(newTextBlock.Tag); // 查找到该键盘的序号
            keyBoardNum.index = index;
            keyBoardNum.groupNum.count = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist.Count;       // 该键盘下组员个数
            keyBoardNum.hotlineNum.count = getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist.Count;   // 该键盘下线路个数

            // 该键盘基础信息
            biaoshitext.Text = getDesk.keyboardlist[keyBoardNum.index - 1].sequence;
            xulietext.Text = getDesk.keyboardlist[keyBoardNum.index - 1].index;
            IPtext.Text = getDesk.keyboardlist[keyBoardNum.index-1].ip;
            MACtext.Text = getDesk.keyboardlist[keyBoardNum.index-1].mac;

            GroupShowGrid.ItemsSource = getDesk.keyboardlist[keyBoardNum.index-1].grouplist;
            LineShowGrid.ItemsSource = getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist;

            // 初始化该键盘组内信息
            List<DeskDevice> deskList = new List<DeskDevice>();
            for (int i = 0; i < keyBoardNum.groupNum.count; i++)
            {
                DeskDevice tmp = new DeskDevice();
                tmp.index = i + 1;
                tmp.groupname = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[i].groupname;
                tmp.memberlist = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[i].memberlist;
                tmp.description = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[i].description;
                deskList.Add(tmp);
            }
            GroupShowGrid.ItemsSource = deskList;

            // 初始化该线路信息
            List<DeskDevice> deskLineList = new List<DeskDevice>();
            for (int i = 0; i < keyBoardNum.hotlineNum.count; i++)
            {
                DeskDevice tmp = new DeskDevice();
                tmp.index = i + 1;
                tmp.name = getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist[i].name;
                tmp.callno = getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist[i].callno;
                tmp.description = getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist[i].description;
                deskLineList.Add(tmp);
            }
            LineShowGrid.ItemsSource = deskList;
            ShowLineMember(showGroupDate);

            Console.WriteLine("打开某个键盘" + getDesk);

            tabControl_mgt.SelectedIndex = 3;  // 跳转到键盘的详细信息
        }
        //==================================================================
        private void NewAddGroupShow()
        {
            RegesterStack.Items.Clear();
            AddCallStack.Items.Clear();
            newGroupNameText.Text = "";
            newGroupIdexText.Text = "";
            newGroupDesText.Text = "";
        }

        private void ClearKeyBoardShow()
        {
            // 调度键盘基本信息
            biaoshitext.Text = "";
            xulietext.Text = "";
            IPtext.Text = "";
            MACtext.Text = "";
            queding.Background = Brushes.Wheat;
            // 分组信息
            GroupShowGrid.ItemsSource = "";
            NewAddGroupShow();
            // 线路信息
            ShowLineMember(showGroupDate);
            LineShowGrid.ItemsSource = "";
        }
        /// ============================初始化界面信息======================
        
        //==================================================================


        /// ====================增加删除修改的调度键盘信息==================
        /// <summary>
        /// 添加调度键盘的按键，跳转到添加界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_desk_add_Click(object sender, RoutedEventArgs e)
        {
            KeyBoard newKeyBoard = new KeyBoard();
            getDesk.keyboardlist.Add(newKeyBoard);      // 新键盘加入调度台中
            keyBoardNum.index = keyBoardNum.count + 1;  // 当前所在键盘的序号
            keyBoardNum.count++;                        // 当前键盘的总和
            keyBoardNum.groupNum.count = 0;             // 当前所在键盘组的总数
            keyBoardNum.groupNum.index = 0;             // 当前所在键盘组的序号
            keyBoardNum.hotlineNum.count = 0;           // 当前所在键盘线路的总数
            keyBoardNum.hotlineNum.index = 0;           // 当前所在键盘线路的序号

            // 初始化分组和线路参数
            getDesk.keyboardlist[keyBoardNum.index-1].grouplist = new List<Group>();
            getDesk.keyboardlist[keyBoardNum.index-1].hotlinelist = new List<Hotline>();
            // 新键盘信息为空
            ClearKeyBoardShow();
            // 跳到新键盘详细信息界面
            tabControl_mgt.SelectedIndex = 3;
        }

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

        /// <summary>
        /// 新调度键盘基础信息，确认键
        /// </summary>
        
        private void MakeSure(object sender, RoutedEventArgs e)
        {
            queding.Background = Brushes.Red;
            getDesk.keyboardlist[keyBoardNum.index - 1].name = mingchengtext.Text;
            getDesk.keyboardlist[keyBoardNum.index - 1].sequence = biaoshitext.Text;
            getDesk.keyboardlist[keyBoardNum.index - 1].index = xulietext.Text;
            getDesk.keyboardlist[keyBoardNum.index - 1].ip = IPtext.Text;
            getDesk.keyboardlist[keyBoardNum.index - 1].mac = MACtext.Text;
        }

        /// <summary>
        /// 分组添加键
        /// </summary>
        public SearcheAllRegister searchAllRegest = new SearcheAllRegister();
        private void btn_group_add_Click(object sender, RoutedEventArgs e)
        {                      
            // 加新组
            Group newGroup = new Group();
            getDesk.keyboardlist[keyBoardNum.index - 1].grouplist.Add(newGroup);
            keyBoardNum.groupNum.index = keyBoardNum.groupNum.count + 1;
            keyBoardNum.groupNum.count++;

            // 添加新组前添加界面信息清零
            NewAddGroupShow();
            // 添加界面显示所有可添加号码
            ShowGroupMember(showGroupDate);
            // 跳转到添加界面
            GroupTabCtrl.SelectedIndex = 1;
        }

        /// <summary>
        /// 添加分组左侧页面
        /// </summary>
        //public List<DevList> resAddDevList = new List<DevList>();
        public GetAllRegister getAllRegister = new GetAllRegister();
        private void ShowGroupMember(String date)
        {
            try
            {
                /* 比较临时存储的obj的sequence是否一致 */
                getAllRegister = JsonConvert.DeserializeObject<GetAllRegister>(date);   // 查到的所有注册电话
                if (getAllRegister.sequence == searchAllRegest.sequence)
                {
                    RegesterStack.Items.Clear();
                    // 将查到的注册电话添加到界面中
                    List<DevList> resDevList = new List<DevList>();
                    resDevList = getAllRegister.devlist;
                    int devNum = resDevList.Count;
                    for (int idex = 0; idex < devNum; idex++)
                    {
                        CheckBox checkBox = new CheckBox();
                        checkBox.Style = this.FindResource("MyCheckBox") as Style;
                        checkBox.Content = resDevList[idex].callno;
                        RegesterStack.Items.Add(checkBox);
                    } 
                }
            }
            catch (System.Exception e)
            {
                //Trace.WriteLine(e);
                MessageBox.Show(e.Message + "c");
            }
        }

        private void ShowLineMember(String date)
        {
            try
            {
                /* 比较临时存储的obj的sequence是否一致 */
                getAllRegister = JsonConvert.DeserializeObject<GetAllRegister>(date);   // 查到的所有注册电话
                if (getAllRegister.sequence == searchAllRegest.sequence)
                {
                    CallLine.Items.Clear();
                    // 将查到的注册电话添加到界面中
                    List<DevList> resDevList = new List<DevList>();
                    resDevList = getAllRegister.devlist;
                    int devNum = resDevList.Count;
                    for (int idex = 0; idex < devNum; idex++)
                    {
                        CheckBox checkBox = new CheckBox();
                        checkBox.Style = this.FindResource("MyCheckBox") as Style;
                        checkBox.Content = resDevList[idex].callno;
                        CallLine.Items.Add(checkBox);
                    } 
                }
            }
            catch (System.Exception e)
            {
                //Trace.WriteLine(e);
                MessageBox.Show(e.Message + "d");
            }
        }

        

        /// <summary>
        /// 添加注册设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TurnRight(object sender, RoutedEventArgs e)
        {
            try
            {
                if (getAllRegister.sequence == searchAllRegest.sequence)
                {
                    int itemNum = RegesterStack.Items.Count;
                    for (int idex = 0; idex < itemNum; )
                    {
                        CheckBox item = RegesterStack.Items[idex] as CheckBox;
                        item.Style = this.FindResource("MyCheckBox") as Style;
                        if (item.IsChecked == true)
                        {
                            //RegesterStack.Items.Remove(item);
                            CheckBox itemNow = new CheckBox();
                            itemNow.Style = this.FindResource("MyCheckBox") as Style;
                            itemNow.Content = item.Content;
                            AddCallStack.Items.Add(itemNow);
                            //DevList devIrem = getAllRegister.devlist.Find((DevList s) => s.callno == item.Content.ToString());
                            //resAddDevList.Add(devIrem);
                            //itemNum--;
                            idex++;
                        }
                        else 
                        {
                            idex++;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + "e");
            }

        }

        /// <summary>
        /// 删除选定的注册设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TurnLeft(object sender, RoutedEventArgs e)
        {
            try
            {
                if (getAllRegister.sequence == searchAllRegest.sequence)
                {
                    int itemNum = AddCallStack.Items.Count;
                    for (int idex = 0; idex < itemNum; )
                    {
                        CheckBox item = AddCallStack.Items[idex] as CheckBox;
                        item.Style = this.FindResource("MyCheckBox") as Style;
                        if (item.IsChecked == true)
                        {
                            AddCallStack.Items.Remove(item);
                            foreach (CheckBox itemBox in RegesterStack.Items)
                            {
                                if (itemBox.Content == item.Content)
                                {
                                    itemBox.IsChecked = false;
                                }
                            }
                            string checkName = "name" + item.Content.ToString();
                            //DevList devIrem = getAllRegister.devlist.Find((DevList s) => s.callno == item.Content.ToString());
                            //resAddDevList.Remove(devIrem);
                            itemNum--;
                        }
                        else
                        {
                            idex++;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + "f");
            }

        }

        //public List<Group> newGroupList = new List<Group>();
        //public List<Member> newMemberList = new List<Member>();
        private void GroupSure(object sender, RoutedEventArgs e)
        {
            try
            {
                if (getAllRegister.sequence == searchAllRegest.sequence)
                {
                    // 组基本信息
                    getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[keyBoardNum.groupNum.index-1].groupname = newGroupNameText.Text;
                    getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[keyBoardNum.groupNum.index - 1].column = newGroupIdexText.Text;
                    getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[keyBoardNum.groupNum.index - 1].description = newGroupDesText.Text;
                    getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[keyBoardNum.groupNum.index - 1].memberlist = new List<Member>();
                    
                    // 组含有的电话号码个数
                    keyBoardNum.groupNum.memberNum.count = AddCallStack.Items.Count;

                    // 将分组号码加入组中
                    foreach (DevList item in getAllRegister.devlist)
                    {
                        foreach (CheckBox chek in AddCallStack.Items)
                        {
                            if (item.callno == chek.Content.ToString())
                            {
                                Member newMember = new Member();
                                newMember.callno = item.callno;
                                newMember.description = item.callno;
                                newMember.level = item.level;
                                newMember.name = item.name;
                                newMember.type = item.type;
                                getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[keyBoardNum.groupNum.index - 1].memberlist.Add(newMember);
                            }
                        }
                    }

                    // 该键盘组内信息
                    List<DeskDevice> deskList = new List<DeskDevice>();
                    for (int i = 0; i < keyBoardNum.groupNum.count; i++)
                    {
                        DeskDevice tmp = new DeskDevice();
                        tmp.index = i + 1;
                        tmp.groupname = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[i].groupname;
                        tmp.memberlist = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[i].memberlist;
                        tmp.description = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[i].description;
                        deskList.Add(tmp);
                    }
                    GroupShowGrid.ItemsSource = deskList;


                    // 跳转回之前的页面
                    GroupTabCtrl.SelectedIndex = 0;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + "g");
            }
        }

        private void Btn_Bianji_Click(object sender, RoutedEventArgs e)
        {
            // 跳转到编辑页面
            Button btn = sender as Button;
            if (btn != null)
            {
                Int16 index = Convert.ToInt16(btn.Tag); // 查找到该组的序号
                keyBoardNum.groupNum.index = index;
               
                // 布置编辑界面
                newGroupNameText.Text = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[keyBoardNum.groupNum.index - 1].groupname;
                newGroupIdexText.Text = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[keyBoardNum.groupNum.index - 1].column;
                newGroupDesText.Text = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[keyBoardNum.groupNum.index - 1].description;
                List<Member> memberList = new List<Member>();
                memberList = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[keyBoardNum.groupNum.index - 1].memberlist;// 选中的成员

                int mumberNum = memberList.Count;                           
                AddCallStack.Items.Clear();
                // 选中的成员加入右侧栏中
                for (int idex = 0; idex < mumberNum; idex++)
                {
                    CheckBox itemNow = new CheckBox();
                    itemNow.Style = this.FindResource("MyCheckBox") as Style;
                    itemNow.Content = memberList[idex].callno;
                    itemNow.IsChecked = true;
                    AddCallStack.Items.Add(itemNow);
                }
                
                // 左侧栏中原始查询的数据
                foreach (CheckBox item in RegesterStack.Items)              
                {
                    foreach (CheckBox itemAdd in AddCallStack.Items)
                    {
                        if (item.Content == itemAdd.Content)
                        {
                            if (item.IsChecked != true)
                            {
                                item.IsChecked = true;
                            }
                        }
                        else
                        {
                            if (item.IsChecked == true)
                            {
                                item.IsChecked = false;
                            }
                        }
                    }
                }
            }
            // 跳转到修改页面
            GroupTabCtrl.SelectedIndex = 1;
        }

        private void btn_group_delete_Click(object sender, RoutedEventArgs e)
        {
            if (Brushes.Red == queding.Background)
            {
                
            }

        }




        private void LineAdd(object sender, RoutedEventArgs e)
        {
            // 跳转到编辑页面
            Button btn = sender as Button;
            if (btn != null)
            {
                getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist = new List<Hotline>();
                keyBoardNum.hotlineNum.index = 0;
                keyBoardNum.hotlineNum.count = 0;
                //keyBoardNum.hotlineNum.index = keyBoardNum.hotlineNum.count + 1;
                //keyBoardNum.hotlineNum.count++;
                //Int16 index = Convert.ToInt16(btn.Tag); // 查找到该组的序号
                //keyBoardNum.hotlineNum.index = index;

                // 页面显示
                int idex = 0;
                List<DeskDevice> deskList = new List<DeskDevice>();
                foreach (CheckBox item in CallLine.Items)
                {
                    if (item.IsChecked == true)
                    {
                        DeskDevice tmp = new DeskDevice();
                        tmp.index = idex + 1;
                        tmp.name = getAllRegister.devlist[idex].name;
                        tmp.callno = getAllRegister.devlist[idex].callno;
                        tmp.description = getAllRegister.devlist[idex].description;
                        deskList.Add(tmp);
                    }
                    idex++;
                }
                LineShowGrid.ItemsSource = deskList;

                // 键盘信息改变
                foreach (DevList item in getAllRegister.devlist)
                {
                    foreach (CheckBox chek in CallLine.Items)
                    {
                        if (item.callno == chek.Content.ToString())
                        {
                            if (chek.IsChecked == true)
                            { 
                                keyBoardNum.hotlineNum.index = keyBoardNum.hotlineNum.count + 1;
                                keyBoardNum.hotlineNum.count++;
                                Hotline newMember = new Hotline();
                                newMember.callno = item.callno;
                                newMember.description = item.callno;
                                newMember.level = item.level;
                                newMember.name = item.name;
                                newMember.type = item.type;
                                getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist.Add(newMember);
                                //getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist[keyBoardNum.hotlineNum.index - 1] = newMember;
                            }
                        }
                    }
                }
            }
        }

        private void FinishDeskChange(object sender, RoutedEventArgs e)
        {
            string sendMsg = JsonConvert.SerializeObject(getDesk);
            DeskImage(sendMsg);
            Console.WriteLine("完成新界面" + sendMsg);

            string strMsg = "MAN#ADDKEYBOARD#" + sendMsg;
            mainWindow.ws.Send(strMsg);
            //tabControl1.SelectedIndex = 2;  // 跳转到键盘的列表
            tabControl_mgt.SelectedIndex = 2;  // 跳转到键盘的树
        }

        private void btn_hotline_delete_Click(object sender, RoutedEventArgs e)
        {
            

        }

        //==================================================================




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

                    SwitchDevice item = new SwitchDevice();
                    item.idstr = swdevobj.name;
                    item.name = swdevobj.name;
                    item.index = Convert.ToInt16(swdevobj.index);
                    item.type = Convert.ToInt16(swdevobj.type);

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

                SwitchDevice item = new SwitchDevice();
                item.idstr = swdevobj.name;
                item.name = swdevobj.name;
                item.index = Convert.ToInt16(swdevobj.index);
                item.type = Convert.ToInt16(swdevobj.type);

                //swList.Add(item);
                // 在swList中查询对应的item
                for (int i = 0; i < swList.Count; i++)
                {
                    if (swList[i].index == item.index)
                    {
                        // 保留之前的展开状态
                        DataGridRow row; // (DataGridRow)switchGrid.ItemContainerGenerator.ContainerFromIndex(switchGrid.SelectedIndex);
                        //DataRowView rowview = switchGrid.SelectedItem as DataRowView;
                        Visibility visib = System.Windows.Visibility.Visible;
                        if (swList[i].iscp == false)
                        {
                            visib = System.Windows.Visibility.Collapsed;
                        }
                        else
                        {
                            visib = System.Windows.Visibility.Hidden;
                        }
                        item.IsDetailsExpanded = (visib == System.Windows.Visibility.Collapsed) ? true : false;

                        swList.RemoveAt(i);
                        if (i == 0)
                        {
                            swList.Insert(0, item);
                        }
                        else
                        {
                            swList.Insert(i - 1, item);
                        }

                        // using modified version will not notify the RowDetailsTemplate

                        Trace.WriteLine("current selected index: " + i);
                        //row = (DataGridRow)switchGrid.ItemContainerGenerator.ContainerFromIndex(i);

                        //switchGrid.Focus();
                        //switchGrid.CurrentCell = 
                        switchGrid.SelectedIndex = i;

                    }
                }
            }

        }

        /* 刷新软交换设备列表 */
        private void freshSwitchDevice(string data)
        {
            swList.Clear();
            SW_QUERYRESULT res = JsonConvert.DeserializeObject<SW_QUERYRESULT>(data);
            if (res != null)
            {
                ObservableCollection<SwitchDevice> list = new ObservableCollection<SwitchDevice>();
                foreach (SWDEVITEM member in res.switchlist)
                {
                    SwitchDevice item = new SwitchDevice();
                    item.idstr = member.name;
                    item.name = member.name;
                    item.index = Convert.ToInt16(member.index);
                    item.type = Convert.ToInt16("0" + member.type);
                    item.iscp = false;
                    //item.IsDetailsExpanded = false;

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
                DataGridRow dgr = (DataGridRow)switchGrid.ItemContainerGenerator.ContainerFromIndex(switchGrid.SelectedIndex);
                TextBox temp = FindVisualChildByName<TextBox>(dgr, "detailidstr") as TextBox;
                BindingExpression be = temp.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
                //MainWindow.swList[index].name = switchGrid.
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

        private void DataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var se = switchGrid.SelectedItem;

                FrameworkElement el = switchGrid.Columns[0].GetCellContent(se);
                DataGridRow row = DataGridRow.GetRowContainingElement(el.Parent as FrameworkElement);
                row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                //if (objElement != null)
                //{
                //    TextBlock objChk = (TextBlock)objElement;
                //    //objChk.IsChecked = !objChk.IsChecked;
                //    Console.WriteLine("MouseDown");
                //}
            }
        }

        private void dgCompletedJobs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _rowSelectionChanged = true;
        }

        private void dgCompletedJobsMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            //navigate up the tree
            while (dep != null &&
                !(dep is DataGridCell) &&
                !(dep is DataGridColumnHeader))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
            {
                return;
            }

            DataGridCell dgc = dep as DataGridCell;
            if (dgc != null)
            {
                //navigate further up the tree
                while (dep != null && !(dep is DataGridRow))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                DataGridRow dgr = dep as DataGridRow;
                DataGrid dg = sender as DataGrid;
                if (dg != null && dgr != null)
                {
                    if (dgr.IsSelected && !_rowSelectionChanged)
                    {
                        dg.RowDetailsVisibilityMode =
                            (dg.RowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.VisibleWhenSelected)
                                ? DataGridRowDetailsVisibilityMode.Collapsed
                                : DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
                    }
                    else
                    {
                        dg.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
                    }
                }
            }
            _rowSelectionChanged = false;
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
