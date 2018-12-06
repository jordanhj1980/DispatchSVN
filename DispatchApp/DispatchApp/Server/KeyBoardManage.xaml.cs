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
        private void delkey_Click(object sender, RoutedEventArgs e)
        {
            //keyboardmanagedata.KeyboardList.Remove(keyboardmanagedata.SelectedKey);
            DelKeyBoard delkeyboard;
            delkeyboard.sequence = GlobalFunAndVar.sequenceGenerator();
            delkeyboard.index = keyboardmanagedata.SelectedKey.index;
            string cmdstr = "MAN#DELKEYBOARD#"+JsonConvert.SerializeObject(keyboardmanagedata.SelectedKey);
            mainWindow.ws.Send(cmdstr);
        }

        private void addkey_Click(object sender, RoutedEventArgs e)
        {
            keyboardmanagedata.SelectedKey = new KeyBoardNew();
            keyboardmanagedata.SelectedKey.name = "新增键盘";
            keyboardmanagedata.KeyboardList.Add(keyboardmanagedata.SelectedKey);
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
        
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            keyboardmanagedata.SelectedKey.sequence = GlobalFunAndVar.sequenceGenerator();
            string cmdstr = "MAN#ADDKEYBOARD#" + JsonConvert.SerializeObject(keyboardmanagedata.SelectedKey);
            mainWindow.ws.Send(cmdstr);
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
            memberlistcard.Visibility = System.Windows.Visibility.Visible;
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
