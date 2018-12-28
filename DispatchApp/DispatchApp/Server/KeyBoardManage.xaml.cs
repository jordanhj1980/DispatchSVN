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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;

using System.Diagnostics;
using System.Collections.ObjectModel;


namespace DispatchApp
{
    /// <summary>
    /// KeyBoardManage.xaml 的交互逻辑
    /// </summary>
    public partial class KeyBoardManage : UserControl
    {
        private MainWindow mainWindow;
        public KeyBoardManageViewModel keyboardmanagedata; 
        public KeyBoardManage(MainWindow mainwindow)
        {
            InitializeComponent();
            this.mainWindow = mainwindow;
            keyboardmanagedata = new KeyBoardManageViewModel();
            this.DataContext = keyboardmanagedata;
        }

        public void recv(string state, string data)
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
            }
        }

        public KeyBoardNum keyBoardNum = new KeyBoardNum();
        public AllKeyBoard allkeydata = new AllKeyBoard();//hj 2018.11.26
        private void DeskImage(string date)
        {
            /* 调度台查询到的信息 */
            allkeydata = JsonConvert.DeserializeObject<AllKeyBoard>(date);//hj 2018.11.26获取所有调度键盘
            keyboardmanagedata.KeyboardList = allkeydata.keyboardlist;

            // add by twinkle 20181123
            ObservableCollection<UserStatus> deskList = new ObservableCollection<UserStatus>();
            if (mainWindow.callManagerCtrl.tabControl_mgt.SelectedIndex == 1)
            {
                // 首先清空调度台的列表
                deskList.Clear();
            }

            Debug.WriteLine("********查询的调度台消息:" + date);
            keyBoardNum.count = allkeydata.keyboardlist.Count;         // 调度台里的调度键盘个数
            Debug.WriteLine("********查询的调度键盘个数:" + keyBoardNum.count);
            // 遍历所有的调度键盘
            for (int idex = 0; idex < keyBoardNum.count; idex++)
            {
                KeyBoardNew keyBoard = allkeydata.keyboardlist[idex];
                // 添加调度台信息  add by twinkle 20181122
                string keyidx = "";
                if (keyBoard.index != null)
                {
                    keyidx = keyBoard.index;
                }
                UserStatus us = new UserStatus(keyidx, keyBoard.name);
                deskList.Add(us);
            }

            mainWindow.callManagerCtrl.usermanagetab.freshDeskList(deskList);

            if(Keyboardlist.SelectedItem != null)
            {
                TreeViewItem tvItem = (TreeViewItem)Keyboardlist.ItemContainerGenerator.ContainerFromIndex(indexTreeViewItem);
                tvItem.IsSelected = true;
            }
            

            foreach (KeyBoardNew item in Keyboardlist.Items)
            {
                if (item.name == keyboardmanagedata.SelectedKey.name)
                {
                    Debug.WriteLine("名字" + item.name);
                } 
            }


            //List<TreeViewItem> pageRelayCall = FindChirldHelper.FindVisualChild<TreeViewItem>(Keyboardlist);
            //foreach (var item in pageRelayCall)
            //{
               
                //TextBlock oB = item.Name
                //foreach (TextBlock oB in item.Items)
                //{
                //    if (oB.Text == keyboardmanagedata.SelectedKey.name)
                //    {
                //        item.IsExpanded = true;
                //        item.Focus();
                //        break;
                //    }
                //Debug.WriteLine("名字" + item.Header);
                //}
                
            //}
        }

        /// <summary>
        /// 遍历容器内控件
        /// </summary>
        public static class FindChirldHelper
        {
            public static List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
            {
                try
                {
                    List<T> TList = new List<T> { };
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                        if (child != null && child is T)
                        {
                            TList.Add((T)child);
                            List<T> childOfChildren = FindVisualChild<T>(child);
                            if (childOfChildren != null)
                            {
                                TList.AddRange(childOfChildren);
                            }
                        }
                        else
                        {
                            List<T> childOfChildren = FindVisualChild<T>(child);
                            if (childOfChildren != null)
                            {
                                TList.AddRange(childOfChildren);
                            }
                        }
                    }
                    return TList;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message + "a");
                    return null;
                }
            }
        }
        /// <summary>
        /// 添加调度键盘的应答
        /// </summary>
        private void AnsAddKeyBoard(string data)
        {
            AnsAddKeyBoard ans = JsonConvert.DeserializeObject<AnsAddKeyBoard>(data);
            //getDesk.keyboardlist[keyBoardNum.index - 1].index = ans.index;
            Debug.WriteLine("添加键盘应答：" + data);
            result.Content = ans.result;
            SearchDesk searchDesk = new SearchDesk() { sequence = GlobalFunAndVar.sequenceGenerator() };
            string strMsg = "MAN#GETALLKEYBOARD#" + JsonConvert.SerializeObject(searchDesk);
            mainWindow.ws.Send(strMsg);

            // 添加成功则跳转界面
            if (ans.result == "Success")
            {
                result.Content = "更新/添加成功";
                //tabControl_mgt.SelectedIndex = 5;  // 跳转到键盘的树
            }
            else
            {
                result.Content = "更新/添加失败";
                //MessageBox.Show("添加调度键盘失败", "提示信息");
            }
        }

        private void AnsDelKeyBoard(string data)
        {
            AnsDelKeyBoard ans = JsonConvert.DeserializeObject<AnsDelKeyBoard>(data);
            Debug.WriteLine("删除键盘应答：" + data);

            if (ans.result == "Success")
            {
                result.Content = "删除成功";
            }
            else
            {
                result.Content = "删除失败";
                MessageBox.Show("删除调度键盘失败", "提示信息");
            }

            SearchDesk searchDesk = new SearchDesk() { sequence = GlobalFunAndVar.sequenceGenerator() };
            string strMsg = "MAN#GETALLKEYBOARD#" + JsonConvert.SerializeObject(searchDesk);
            mainWindow.ws.Send(strMsg);
        }

        private void delkey_Click(object sender, RoutedEventArgs e)
        {
            //keyboardmanagedata.KeyboardList.Remove(keyboardmanagedata.SelectedKey);
            DelKeyBoard delkeyboard;
            delkeyboard.sequence = GlobalFunAndVar.sequenceGenerator();
            delkeyboard.index = keyboardmanagedata.SelectedKey.index;
            //string cmdstr = "MAN#DELKEYBOARD#"+JsonConvert.SerializeObject(keyboardmanagedata.SelectedKey);
            string cmdstr = "MAN#DELKEYBOARD#" + JsonConvert.SerializeObject(delkeyboard);
            mainWindow.ws.Send(cmdstr);
            indexTreeViewItem = 0;
        }

        private void addkey_Click(object sender, RoutedEventArgs e)
        {
            keyboardmanagedata.SelectedKey = new KeyBoardNew();
            keyboardmanagedata.SelectedKey.name = "新增键盘";
            
            DeskBaseInfo.Visibility = Visibility.Visible;
        
            //keyboardmanagedata.KeyboardList.Add(keyboardmanagedata.SelectedKey);
            
            //keyboardview.Visibility = System.Windows.Visibility.Visible;
        }

        private void addgroup_Click(object sender, RoutedEventArgs e)
        {
            keyboardmanagedata.SelectedKey.grouplist.Add(new GroupNew());
        }

        private void delgroup_Click(object sender, RoutedEventArgs e)
        {
            keyboardmanagedata.SelectedKey.grouplist.Remove(keyboardmanagedata.SelectedGroup);
        }

        private void Keyboardlist_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;

            if (tvi.Header is KeyBoardNew)
            {
                var modelkey = tvi.Header as KeyBoardNew;
                //testdata.SelectedKey = modelkey;
                keyboardmanagedata.SelectedKey = ObjectCopier.Clone<KeyBoardNew>(modelkey);
                //groupview.Visibility = System.Windows.Visibility.Hidden;
                //keyboardview.Visibility = System.Windows.Visibility.Visible;
            }
            else if (tvi.Header is GroupNew)
            {
                var modelgroup = tvi.Header as GroupNew;
                //testdata.SelectedGroup = modelgroup;
                keyboardmanagedata.SelectedGroup = ObjectCopier.Clone<GroupNew>(modelgroup);
                //keyboardview.Visibility = System.Windows.Visibility.Hidden;
                //groupview.Visibility = System.Windows.Visibility.Visible;

                var tv = VisualTreeHelper.GetParent(tvi);
                System.Windows.Controls.StackPanel tvpanel = tv as StackPanel;
                ItemsPresenter ip = tvpanel.TemplatedParent as ItemsPresenter;
                TreeViewItem tvii = ip.TemplatedParent as TreeViewItem;//这是父节点
                //testdata.SelectedKey = tvii.Header as KeyBoard;
                keyboardmanagedata.SelectedKey = ObjectCopier.Clone<KeyBoardNew>(tvii.Header as KeyBoardNew);
            }
            if (Keyboardlist.SelectedItem != null)
            {
                DeskBaseInfo.Visibility = Visibility.Visible;
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            scrollViewer.RaiseEvent(eventArg);
        }
        private void scrollAlldev_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            scrollAlldev.RaiseEvent(eventArg);
        }
        
        public int indexTreeViewItem;
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int state = 0;
            
            foreach(KeyBoardNew item in keyboardmanagedata.KeyboardList) 
            {
                if (item.name == keyboardmanagedata.SelectedKey.name)
                {
                    state = 1;
                    if (Keyboardlist.SelectedItem != null)
                    {
                        KeyBoardNew keyBoardNew = (KeyBoardNew)Keyboardlist.SelectedItem;
                        if (keyBoardNew.index == keyboardmanagedata.SelectedKey.index)
                        {
                            state = 0;
                        }
                    }
                }
            }
            if (state == 1)
            {
                result.Content = "更新失败,请选中左侧键盘更新，或更改键盘名添加";
            }
            else
            {
                // 确定当前选中的树节点
                indexTreeViewItem = 0;
                for (int i = 0; i < Keyboardlist.Items.Count; i++)
                {
                    TreeViewItem tvItem = (TreeViewItem)Keyboardlist.ItemContainerGenerator.ContainerFromIndex(i);
                    KeyBoardNew item = (KeyBoardNew)tvItem.Header;
                    if (item.name == keyboardmanagedata.SelectedKey.name)
                    {
                        indexTreeViewItem = i;
                    }
                    if ((i == Keyboardlist.Items.Count-1) && (indexTreeViewItem == 0))
                    {
                        indexTreeViewItem = Keyboardlist.Items.Count;
                    }
                }

                //TreeViewItem tvItemNew = (TreeViewItem)Keyboardlist.ItemContainerGenerator.ContainerFromIndex(0);
                //tvItemNew.IsSelected = true;

                keyboardmanagedata.SelectedKey.sequence = GlobalFunAndVar.sequenceGenerator();
                string cmdstr = "MAN#ADDKEYBOARD#" + JsonConvert.SerializeObject(keyboardmanagedata.SelectedKey);
                mainWindow.ws.Send(cmdstr);
            }
        }

        //private void getalldeskbtn_Click(object sender, RoutedEventArgs e)
        //{
        //    mainWindow.ws.Send("MAN#GETALLKEYBOARD#{\"sequence\":\"123\"}");
        //}

        //private void getalldevbtnbtn_Click(object sender, RoutedEventArgs e)
        //{
        //    mainWindow.ws.Send("MAN#GETALLREGISTERDEV#{\"sequence\":\"123\"}");
        //}

        //private void alldevicegrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    ExtDevice data = groupdevicegrid.SelectedItem as ExtDevice;
        //    if (data == null)
        //        return;
        //    if(data.DevSelected==true)
        //    {
        //        data.DevSelected = false;
        //    }
        //    else
        //    {
        //        data.DevSelected = true;
        //    }
        //}

        private void addmember_Click(object sender, RoutedEventArgs e)
        {
            keyboardmanagedata.SelectedGroup.memberlist.Clear();
            foreach (ExtDevice t in keyboardmanagedata.AllDevList)
            {
                if(t.DevSelected==true)
                {
                    keyboardmanagedata.SelectedGroup.memberlist.Add(t);
                }
            }
            keyboardmanagedata.SelectedGroup.OnPropertyChanged("memberlist");
        }

        //private void grouplistgrid_Selected(object sender, RoutedEventArgs e)
        //{
        //    foreach (ExtDevice element in keyboardmanagedata.AllDevList)
        //    {
        //        //element = testdata.AllDevList.Find(c => c.clientsession.SessionID.Equals(clientsession.SessionID));
        //        bool bFind = keyboardmanagedata.SelectedGroup.memberlist.Any<ExtDevice>(p => p.callno == element.callno);
        //        if(bFind)
        //        {
        //            element.DevSelected = true;
        //        }
        //    }
        //}

        //private void addkeydev_Click(object sender, RoutedEventArgs e)
        //{
        //    keyboardmanagedata.SelectedKey.hotlinelist.Clear();
        //    foreach (ExtDevice t in keyboardmanagedata.AllDevList)
        //    {
        //        if (t.DevSelected == true)
        //        {
        //            keyboardmanagedata.SelectedKey.hotlinelist.Add(t);
        //        }
        //    }
        //    keyboardmanagedata.SelectedKey.OnPropertyChanged("hotlinelist");
        //}

        private void RootDialogHotline_OnDialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("hotline选择完毕！！！");
            this.keyboardmanagedata.SelectedKey.hotlinelist.Clear();
            foreach (ExtDevice t in this.keyboardmanagedata.AllPhoneList)
            {
                if (t.DevSelected == true)
                {
                    this.keyboardmanagedata.SelectedKey.hotlinelist.Add(t);
                }
            }
            this.keyboardmanagedata.SelectedKey.OnPropertyChanged("hotlinelist");       
        }
        private void RootDialogGroupMember_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("groupmember选择完毕！！！");
            if (this.keyboardmanagedata.SelectedGroup != null)
            {
                this.keyboardmanagedata.SelectedGroup.memberlist.Clear();
                foreach (ExtDevice t in this.keyboardmanagedata.AllDevList)
                {
                    if (t.DevSelected == true)
                    {
                        this.keyboardmanagedata.SelectedGroup.memberlist.Add(t);
                    }
                }
                this.keyboardmanagedata.SelectedGroup.OnPropertyChanged("memberlist");
            }
        }
        private void RootDialogTrunk_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("trunklist选择完毕！！！");

            //实际操作
            this.keyboardmanagedata.SelectedKey.trunklist.Clear();
            foreach (ExtDevice t in this.keyboardmanagedata.AllTrunkList)
            //foreach (ExtDeviceNew t in this.keyboardmanagedata.AllTrunkListNew)
            {
                if (t.DevSelected == true)
                {
                    TrunkDev member = new TrunkDev();
                    member.trunkid = t.callno;
                    member.name = t.name;
                    member.bindingnumber = t.bindingnumber;
                    this.keyboardmanagedata.SelectedKey.trunklist.Add(member);
                }
            }
            this.keyboardmanagedata.SelectedKey.OnPropertyChanged("trunklist");   
        }

        private void RootDialogBroadcast_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("broadcastmember选择完毕！！！");
            this.keyboardmanagedata.SelectedBroadcast.bmemberlist.Clear();
            foreach (ExtDevice t in this.keyboardmanagedata.AllPhoneList)
            {
                if (t.DevSelected == true)
                {
                    BroadcastMember member = new BroadcastMember ();
                    member.callno = t.callno;
                    member.name = t.name;
                    this.keyboardmanagedata.SelectedBroadcast.bmemberlist.Add(member);
                }
            }
            this.keyboardmanagedata.SelectedBroadcast.OnPropertyChanged("bmemberlist");   
        }
        private void grouplistgrid_Selected(object sender, SelectionChangedEventArgs e)
        {
            //memberlistcard.Visibility = System.Windows.Visibility.Visible;
        }

        private void broadcastlistgrid_Selected(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addbroadcast_Click(object sender, RoutedEventArgs e)
        {
            keyboardmanagedata.SelectedKey.broadcastlist.Add(new Broadcast());
        }

        private void delbroadcast_Click(object sender, RoutedEventArgs e)
        {
            keyboardmanagedata.SelectedKey.broadcastlist.Remove(keyboardmanagedata.SelectedBroadcast);
        }

        private void RootDialogGroupDetail_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("分组信息参数编辑完毕！！！");
            if (!Equals(eventArgs.Parameter, true)) return;

            else
            {
                //this.keyboardmanagedata.SelectedGroup = this.keyboardmanagedata.groupData.groupDetail;
                //this.keyboardmanagedata.OnPropertyChanged("SelectedGroup"); 
                this.keyboardmanagedata.SelectedGroup.groupname = this.keyboardmanagedata.groupData.groupDetail.groupname;
                this.keyboardmanagedata.SelectedGroup.index = this.keyboardmanagedata.groupData.groupDetail.index;
                this.keyboardmanagedata.SelectedGroup.memberlist = this.keyboardmanagedata.groupData.groupDetail.memberlist;
                this.keyboardmanagedata.SelectedGroup.column = this.keyboardmanagedata.groupData.groupDetail.column;
                this.keyboardmanagedata.SelectedGroup.description = this.keyboardmanagedata.groupData.groupDetail.description;
            }
                
           
        }

        private void RootDialogTrunkDetail_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("直呼中继信息参数编辑完毕！！！");
            if (!Equals(eventArgs.Parameter, true)) return;

            else
            {
                this.keyboardmanagedata.SelectedTrunk.bindingnumber = this.keyboardmanagedata.trunkData.trunkDetail.bindingnumber;
                this.keyboardmanagedata.SelectedTrunk.name = this.keyboardmanagedata.trunkData.trunkDetail.name;
                this.keyboardmanagedata.SelectedTrunk.trunkid = this.keyboardmanagedata.trunkData.trunkDetail.trunkid;    
            }
        }

        private void RootDialogBoardDetail_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("广播组信息参数编辑完毕！！！");
            if (!Equals(eventArgs.Parameter, true)) return;

            else
            {
                this.keyboardmanagedata.SelectedBroadcast.index = this.keyboardmanagedata.boardData.boardDetail.index;
                this.keyboardmanagedata.SelectedBroadcast.name = this.keyboardmanagedata.boardData.boardDetail.name;
                this.keyboardmanagedata.SelectedBroadcast.bmemberlist = this.keyboardmanagedata.boardData.boardDetail.bmemberlist;

            }
        }

        /// <summary>
        /// 分组详细信息编辑框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunGroupDetailDialogCommand(object sender, MouseButtonEventArgs e)
        {
            this.keyboardmanagedata.EditGroupDetailDialog();
        }



        /// <summary>
        /// 中继直呼详细信息编辑框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunTrunkDetailDialogCommand(object sender, MouseButtonEventArgs e)
        {
            this.keyboardmanagedata.EditTrunkDetailDialog();
        }

        /// <summary>
        /// 广播组的详细信息编辑框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunBoardDetailDialogCommand(object sender, MouseButtonEventArgs e)
        {
            this.keyboardmanagedata.EditBoardDetailDialog();
        }


        

     

    }
}
