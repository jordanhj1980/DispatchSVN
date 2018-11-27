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
    public partial class CallManagerControl : UserControl
    {
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
            //keyBoardNum.groupNum.index = keyBoardNum.groupNum.count + 1;
            //keyBoardNum.groupNum.count++;
        }
        //==================================================================




        /// =====================
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

        //    // Debug.WriteLine("keyBoard" + getDesk);
        //}
        /// =====================
        /// 

        /// <summary>
        /// 添加调度键盘的应答
        /// </summary>
        private void AnsAddKeyBoard(string data)
        {
            AnsAddKeyBoard ans = JsonConvert.DeserializeObject<AnsAddKeyBoard>(data);
            //getDesk.keyboardlist[keyBoardNum.index - 1].index = ans.index;
            Debug.WriteLine("添加键盘应答：" + data);
            SearchDesk searchDesk = new SearchDesk() { sequence = GlobalFunAndVar.sequenceGenerator() };
            string strMsg = "MAN#GETALLKEYBOARD#" + JsonConvert.SerializeObject(searchDesk);
            mainWindow.ws.Send(strMsg);
           
            // 添加成功则跳转界面
            if (ans.result == "Success")
            {
                tabControl_mgt.SelectedIndex = 2;  // 跳转到键盘的树
            }
            else
            {
                MessageBox.Show("添加调度键盘失败", "提示信息");
            }
            
        }

        private void AnsDelKeyBoard(string data)
        {
            AnsDelKeyBoard ans = JsonConvert.DeserializeObject<AnsDelKeyBoard>(data);
            Debug.WriteLine("删除键盘应答："+data);
            SearchDesk searchDesk = new SearchDesk() { sequence = GlobalFunAndVar.sequenceGenerator() };
            string strMsg = "MAN#GETALLKEYBOARD#" + JsonConvert.SerializeObject(searchDesk);
            mainWindow.ws.Send(strMsg);

            if (ans.result == "Success")
            {
                getDesk.keyboardlist.Remove(getDesk.keyboardlist[keyBoardNum.index - 1]);
                keyBoardNum.count--;
                string sendMsg = JsonConvert.SerializeObject(getDesk);
                DeskImage(sendMsg);
                tabControl_mgt.SelectedIndex = 2;  // 跳转到键盘的树
            }
            else 
            {
                MessageBox.Show("删除调度键盘失败","提示信息");
            }
        }

        /// <summary>
        /// 获取到的调度键盘信息
        /// </summary>
        public GetDesk getDesk = new GetDesk();
        public KeyBoardNum keyBoardNum = new KeyBoardNum();
        public AllKeyBoard allkeydata = new AllKeyBoard();//hj 2018.11.26
        //public KeyBoardManageViewModel keyboardmanagedata = new KeyBoardManageViewModel();
        private void DeskImage(string date)
        {
            // 一棵树
            List<propertyNodeItem> listItem = new List<propertyNodeItem>();

            // add by twinkle 20181123
            if (tabControl_mgt.SelectedIndex == 1)
            {
                // 首先清空调度台的列表
                deskList.Clear();
                // 增加一个空白项
                //deskList.Add(new UserStatus(0, ""));
            }

            /* 调度台查询到的信息 */
            getDesk = JsonConvert.DeserializeObject<GetDesk>(date); // 一个调度台
            allkeydata = JsonConvert.DeserializeObject<AllKeyBoard>(date);//hj 2018.11.26获取所有调度键盘
            keyboardmanagetab.keyboardmanagedata.KeyboardList = allkeydata.keyboardlist;

            Debug.WriteLine("********查询的调度台消息:" + date);
            keyBoardNum.count = getDesk.keyboardlist.Count;         // 调度台里的调度键盘个数
            Debug.WriteLine("********查询的调度键盘个数:" + keyBoardNum.count);
            // 遍历所有的调度键盘
            for (int idex = 0; idex < keyBoardNum.count; idex++)
            {
                // 第一级树枝名称
                KeyBoard keyBoard = getDesk.keyboardlist[idex];
                propertyNodeItem keyBoardNode = new propertyNodeItem() { DisplayName = keyBoard.name, Tag = idex + 1 };
                // 第一级树枝加入树中
                listItem.Add(keyBoardNode);

                // 添加调度台信息  add by twinkle 20181122 
                // 如果当前的界面是添加用户界面，才执行下面操作
                if (tabControl_mgt.SelectedIndex == 1) { 
                    UserStatus us = new UserStatus(Convert.ToInt16(keyBoard.index), keyBoard.name);
                    deskList.Add(us);
                }
            }

            this.tvProperty.ItemsSource = listItem;
            if (deskList.Count > 0)
            {
                cuw.freshDeskCombox(deskList.ToList<UserStatus>());
            }

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
            mingchengtext.Text = getDesk.keyboardlist[keyBoardNum.index - 1].name;
            xulietext.Text = getDesk.keyboardlist[keyBoardNum.index - 1].index;
            IPtext.Text = getDesk.keyboardlist[keyBoardNum.index - 1].ip;
            MACtext.Text = getDesk.keyboardlist[keyBoardNum.index - 1].mac;

            //GroupShowGrid.ItemsSource = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist;
            //LineShowGrid.ItemsSource = getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist;

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
            LineShowGrid.ItemsSource = deskLineList;
            ShowLineMember(showGroupDate);

            tabControl_mgt.SelectedIndex = 3;  // 跳转到键盘的详细信息
        }
        //==================================================================

        /// ============================初始化界面信息======================
        private void NewAddGroupShow()
        {
            RegesterStack.Items.Clear();
            ShowGroupMember(showGroupDate);
            AddCallStack.Items.Clear();
            newGroupNameText.Text = "";
            newGroupIdexText.Text = "";
            newGroupDesText.Text = "";
        }

        private void ClearKeyBoardShow()
        {
            // 调度键盘基本信息
            biaoshitext.Text = "";
            mingchengtext.Text = "";
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
        //==================================================================


        /// ====================增加删除修改的调度键盘信息==================
        /// <summary>
        /// 添加调度键盘的按键，跳转到添加界面
        /// </summary>
        private void desk_add_Click()
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
            getDesk.keyboardlist[keyBoardNum.index - 1].grouplist = new List<Group>();
            getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist = new List<Hotline>();
            // 新键盘信息为空
            ClearKeyBoardShow();
            // 跳到新键盘详细信息界面
            tabControl_mgt.SelectedIndex = 3;
        }

        /// <summary>
        /// 新调度键盘基础信息，确认键
        /// </summary>
        private void MakeSure(object sender, RoutedEventArgs e)
        {
            queding.Background = Brushes.Red;
            getDesk.keyboardlist[keyBoardNum.index - 1].name = mingchengtext.Text;
            // modified by twinkle 20181122
            getDesk.keyboardlist[keyBoardNum.index - 1].sequence = GlobalFunAndVar.sequenceGenerator(); // biaoshitext.Text; 
            getDesk.keyboardlist[keyBoardNum.index - 1].index = xulietext.Text;
            getDesk.keyboardlist[keyBoardNum.index - 1].ip = IPtext.Text;
            getDesk.keyboardlist[keyBoardNum.index - 1].mac = MACtext.Text;
        }
        /// ================================================================

        /// ====================初始化分组的注册号码========================
        /// <summary>
        /// 添加分组左侧页面
        /// </summary>
        public GetAllRegister getAllRegister = new GetAllRegister();
        public AllDev alldev = new AllDev();
        private void ShowGroupMember(String date)
        {
            try
            {
                /* 比较临时存储的obj的sequence是否一致 */
                getAllRegister = JsonConvert.DeserializeObject<GetAllRegister>(date);   // 查到的所有注册电话
                alldev = JsonConvert.DeserializeObject<AllDev>(date);  //hj 2018.11.26获取所有注册电话
                List<ExtDevice> alldevlist = new List<ExtDevice>( alldev.DevList.ToList());
                
                List<ExtDevice> allphonelist = alldevlist.FindAll(delegate(ExtDevice ext) { return ext.type.Equals("ext"); });
                allphonelist = allphonelist.OrderBy(e => e.callno).ToList();
                List<ExtDevice> alltrunklist = alldevlist.FindAll(delegate(ExtDevice ext) { return ext.type.Equals("trunk"); });
                alltrunklist = alltrunklist.OrderBy(e => e.callno).ToList();
                
                //keyboardmanagetab.keyboardmanagedata.AllDevList = alldev.DevList;

                keyboardmanagetab.keyboardmanagedata.AllDevList = new ObservableCollection<ExtDevice>(alldevlist);
                keyboardmanagetab.keyboardmanagedata.AllPhoneList = new ObservableCollection<ExtDevice>(allphonelist);

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
                //Debug.WriteLine(e);
                MessageBox.Show(e.Message + "c");
            }
        }
        /// ================================================================


        /// ====================初始化线路的注册号码========================
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
                //Debug.WriteLine(e);
                MessageBox.Show(e.Message + "d");
            }
        }
        /// ================================================================


        /// ====================增加删除修改分组信息========================
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
            GroupTabCtrl.SelectedIndex = 1;
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
                    foreach (CheckBox item in RegesterStack.Items)
                    {
                        string ans = "true";
                        if (item.IsChecked == true)
                        {
                            // 查看号码是否已添加
                            foreach (CheckBox itemAdd in AddCallStack.Items)
                            {
                                if ((item.Content).ToString() == (itemAdd.Content).ToString())
                                {
                                    ans = "false";
                                    break;
                                }
                            }
                            if (ans == "true")
                            {
                                CheckBox itemNow = new CheckBox();
                                itemNow.Style = this.FindResource("MyCheckBox") as Style;
                                itemNow.Content = item.Content;
                                AddCallStack.Items.Add(itemNow);
                            }
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

        private void GroupSure(object sender, RoutedEventArgs e)
        {
            try
            {
                if (getAllRegister.sequence == searchAllRegest.sequence)
                {
                    // 组基本信息
                    getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[keyBoardNum.groupNum.index - 1].groupname = newGroupNameText.Text;
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

                // 左侧数据将勾选的内容去掉
                RegesterStack.Items.Clear();
                ShowGroupMember(showGroupDate);

                // 选中的成员加入右侧栏中
                for (int idex = 0; idex < mumberNum; idex++)
                {
                    CheckBox itemNow = new CheckBox();
                    itemNow.Style = this.FindResource("MyCheckBox") as Style;
                    itemNow.Content = memberList[idex].callno;
                    //itemNow.IsChecked = true;
                    AddCallStack.Items.Add(itemNow);
                }
            }
            // 跳转到修改页面
            GroupTabCtrl.SelectedIndex = 1;
        }

        private void btn_group_delete_Click(object sender, RoutedEventArgs e)
        {
            // 显示列表去除选中项
            List<DeskDevice> deskList = (List<DeskDevice>)(GroupShowGrid.ItemsSource);
            DeskDevice tmp = (DeskDevice)(GroupShowGrid.SelectedItem);
            deskList.Remove(tmp);
            GroupShowGrid.ItemsSource = null;
            GroupShowGrid.ItemsSource = deskList;

            // 存放调度键盘参数的结构体删除相应的组
            Group item = getDesk.keyboardlist[keyBoardNum.index - 1].grouplist[tmp.index-1];
            getDesk.keyboardlist[keyBoardNum.index - 1].grouplist.Remove(item);
        }
        /// ================================================================

        /// ====================增加删除修改线路信息========================
        private void LineAdd(object sender, RoutedEventArgs e)
        {
            // 跳转到编辑页面
            Button btn = sender as Button;
            if (btn != null)
            {
                //getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist = new List<Hotline>();
                //keyBoardNum.hotlineNum.index = 0;
                //keyBoardNum.hotlineNum.count = 0;
                //string ans = "true";
                // 页面显示
                int idex = 0;
                
                foreach (CheckBox item in CallLine.Items)
                {
                    string ans = "true";
                    idex++;
                    if (item.IsChecked == true)
                    {                       
                        // 查找线路是否已添加
                        foreach (Hotline itemLine in getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist)
                        {
                            if ((itemLine.callno).ToString() == (item.Content).ToString())
                            {
                                ans = "false";
                                break;
                            }
                                
                        }
                    
                        Debug.WriteLine("*********线路已添加ans=" + ans);

                        // 对未添加的线路进行添加
                        if (ans == "true")
                        {
                            Hotline newMember = new Hotline();
                            newMember.callno = getAllRegister.devlist[idex-1].callno;
                            newMember.description = getAllRegister.devlist[idex-1].callno;
                            newMember.level = getAllRegister.devlist[idex-1].level;
                            newMember.name = getAllRegister.devlist[idex-1].name;
                            newMember.type = getAllRegister.devlist[idex-1].type;
                            getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist.Add(newMember);
                        }
                    }
                }

                // 添加显示列表
                keyBoardNum.hotlineNum.count = getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist.Count;
                List<DeskDevice> deskList = new List<DeskDevice>();
                for (int i = 0; i < keyBoardNum.hotlineNum.count; i++)
                {
                    DeskDevice tmp = new DeskDevice();
                    tmp.index = i + 1;
                    tmp.name = getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist[i].name;
                    tmp.callno = getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist[i].callno;
                    tmp.description = getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist[i].description;
                    deskList.Add(tmp);
                }
                LineShowGrid.ItemsSource = deskList;
            }
        }

        private void FinishDeskChange(object sender, RoutedEventArgs e)
        {
            // modified by twinkle 1122
            // 按完成键保存调度台信息
            MakeSure(this, null);

            string sendMsg = JsonConvert.SerializeObject(getDesk);
            DeskImage(sendMsg);

            sendMsg = JsonConvert.SerializeObject(getDesk.keyboardlist[keyBoardNum.index - 1]);
            string strMsg = "MAN#ADDKEYBOARD#" + sendMsg;
            mainWindow.ws.Send(strMsg);
            Debug.WriteLine("完成新界面" + strMsg); // change order by twinkle 20181122
        }

        private void btn_hotline_delete_Click(object sender, RoutedEventArgs e)
        {
            // 显示列表去除选中项
            List<DeskDevice> deskList = (List<DeskDevice>)(LineShowGrid.ItemsSource);
            DeskDevice tmp = (DeskDevice)(LineShowGrid.SelectedItem);
            deskList.Remove(tmp);
            LineShowGrid.ItemsSource = null;
            LineShowGrid.ItemsSource = deskList;

            // 存放调度键盘参数的结构体删除相应的组
            Hotline item = getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist[tmp.index - 1];
            getDesk.keyboardlist[keyBoardNum.index - 1].hotlinelist.Remove(item);
        }

        //==================================================================
    }
}
