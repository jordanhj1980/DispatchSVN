using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
namespace DispatchApp
{
    public class KeyBoardManageViewModel : NotifyObject
    {
        public KeyBoardManageViewModel()
        {
            SelectedKey = new KeyBoardNew();
            SelectedGroup = new GroupNew();
            SelectedBroadcast = new Broadcast();
            KeyboardList = new ObservableCollection<KeyBoardNew>();
            AllDevList = new ObservableCollection<ExtDevice>();
            AllPhoneList = new ObservableCollection<ExtDevice>();
            AllTrunkList = new ObservableCollection<ExtDevice>();
        }


        private MyCommand _RunGroupMemberDialogCommand;

        public MyCommand RunGroupMemberDialogCommand
        {
            get
            {
                if (_RunGroupMemberDialogCommand == null)
                    _RunGroupMemberDialogCommand = new MyCommand(new Action<object>
                    (
                        o =>
                        {
                            ExecuteGroupMemberDialog(o);
                        }
                    ),
                    new Func<object, bool>(o => this.SelectedGroup != null));
                return _RunGroupMemberDialogCommand;
            }
        }
        /// <summary>
        /// 分组成员选择窗口
        /// </summary>
        /// <param name="o"></param>
        private async void ExecuteGroupMemberDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            ListViewDialogViewModel data = new ListViewDialogViewModel();
            List<ExtDevice> selectedgroupmemberlist = new List<ExtDevice>(this.SelectedGroup.memberlist.ToList());
            List<ExtDevice> alldevlist = new List<ExtDevice>(this.AllDevList.ToList());
            //选中已有成员
            foreach(ExtDevice e in alldevlist)
            {
                ExtDevice temp = selectedgroupmemberlist.Find(c => c.callno.Equals(e.callno));
                if(temp==null)
                {
                    e.DevSelected = false;
                }
                else
                {
                    e.DevSelected = true;
                }                
            }
            //成员排序
            var queryresults =
                from n in alldevlist
                orderby n.DevSelected descending, n.callno
                select n;
            alldevlist = queryresults.ToList();
            this.AllDevList = new ObservableCollection<ExtDevice>(alldevlist);
            data.AllDevList = this.AllDevList;

            var view = new ListViewDialog();
            view.DataContext = data;


            //show the dialog
            var result = await DialogHost.Show(view, "RootDialogGroupMember", ListViewClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void ListViewClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        /// <summary>
        /// 分组成员编辑窗口 20181203 xiaozi
        /// </summary>
        //private MyCommand _RunGroupDetailDialogCommand;
        //public MyCommand RunGroupDetailDialogCommand
        //{
        //    get
        //    {
        //        if (_RunGroupDetailDialogCommand == null)
        //            _RunGroupDetailDialogCommand = new MyCommand(new Action<object>
        //            (
        //                o =>
        //                {
        //                    EditGroupDetialDialog();
        //                }
        //            ),
        //            new Func<object, bool>(o => this.SelectedGroup != null));
        //        return _RunGroupDetailDialogCommand;
        //    }
        //}
       

        public GroupDetialViewModel groupData;
        public async void EditGroupDetialDialog()
        {
            GroupDetialViewModel groupData = new GroupDetialViewModel();
            groupData.groupDetail = SelectedGroup;      // 初始化编辑界面参数
            Console.WriteLine("SelectedGroupzzzzzzzz" + SelectedGroup.groupname);

            var view = new GroupDetial();
            view.DataContext = groupData;

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialogGroupDetial", ListViewClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
















        private MyCommand _HotlineListViewDialogCommand;

        public MyCommand HotlineListViewDialogCommand
        {
            get
            {
                if (_HotlineListViewDialogCommand == null)
                    _HotlineListViewDialogCommand = new MyCommand(new Action<object>
                    (
                        o =>
                        {
                            ExecuteHotlineListViewDialog(o);
                        }
                    ));
                return _HotlineListViewDialogCommand;
            }
        }
        /// <summary>
        /// 键权电话选择窗口
        /// </summary>
        /// <param name="o"></param>
        private async void ExecuteHotlineListViewDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            ListViewDialogViewModel data = new ListViewDialogViewModel();
            List<ExtDevice> selectedkeyboardhotlist = new List<ExtDevice>(this.SelectedKey.hotlinelist.ToList());
            List<ExtDevice> alldevlist = new List<ExtDevice>(this.AllPhoneList.ToList());
            this.SelectedGroup = null;
            //选中已有成员
            foreach (ExtDevice e in alldevlist)
            {
                ExtDevice temp = selectedkeyboardhotlist.Find(c => c.callno.Equals(e.callno));
                if (temp == null)
                {
                    e.DevSelected = false;
                }
                else
                {
                    e.DevSelected = true;
                }
            }
            //成员排序
            var queryresults =
                from n in alldevlist
                orderby n.DevSelected descending, n.callno
                select n;
            alldevlist = queryresults.ToList();
            this.AllPhoneList = new ObservableCollection<ExtDevice>(alldevlist);
            data.AllDevList = this.AllPhoneList;

            var view = new ListViewDialog();
            view.DataContext = data;


            //show the dialog
            var result = await DialogHost.Show(view, "RootDialogHotline", HotlineListViewClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void HotlineListViewClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private MyCommand _TrunkListViewDialogCommand;

        public MyCommand TrunklistViewDialogCommand
        {
            get
            {
                if (_TrunkListViewDialogCommand == null)
                    _TrunkListViewDialogCommand = new MyCommand(new Action<object>
                    (
                        o =>
                        {
                            ExecuteTrunkListViewDialog(o);
                        }
                    ));
                return _TrunkListViewDialogCommand;
            }
        }
        /// <summary>
        /// 中继电话选择窗口
        /// </summary>
        /// <param name="o"></param>
        private async void ExecuteTrunkListViewDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            ListViewDialogViewModel data = new ListViewDialogViewModel();
            List<TrunkDev> selectedkeyboardtrunklist = new List<TrunkDev>(this.SelectedKey.trunklist.ToList());
            List<ExtDevice> alltrunklist = new List<ExtDevice>(this.AllTrunkList.ToList());

            //选中已有成员
            foreach (ExtDevice e in alltrunklist)
            {
                TrunkDev temp = selectedkeyboardtrunklist.Find(c => c.name.Equals(e.callno));
                if (temp == null)
                {
                    e.DevSelected = false;
                }
                else
                {
                    e.DevSelected = true;
                }
            }
            //成员排序
            var queryresults =
                from n in alltrunklist
                orderby n.DevSelected descending, n.callno
                select n;
            alltrunklist = queryresults.ToList();
            this.AllTrunkList = new ObservableCollection<ExtDevice>(alltrunklist);
            data.AllDevList = this.AllTrunkList;

            var view = new ListViewDialog();
            view.DataContext = data;


            //show the dialog
            var result = await DialogHost.Show(view, "RootDialogTrunk", TrunkListViewClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void TrunkListViewClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private MyCommand _BroadcastViewDialogCommand;

        public MyCommand BroadcastViewDialogCommand
        {
            get
            {
                if (_BroadcastViewDialogCommand == null)
                    _BroadcastViewDialogCommand = new MyCommand(new Action<object>
                    (
                        o =>
                        {
                            ExecuteBroadcastViewDialog(o);
                        }
                    ),
                    new Func<object, bool>(o => this.SelectedBroadcast != null));
                return _BroadcastViewDialogCommand;
            }
        }
        /// <summary>
        /// 中继电话选择窗口
        /// </summary>
        /// <param name="o"></param>
        private async void ExecuteBroadcastViewDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            ListViewDialogViewModel data = new ListViewDialogViewModel();
            List<BroadcastMember> selectedbroadcastmemberlist = new List<BroadcastMember>(this.SelectedBroadcast.bmemberlist.ToList());
            List<ExtDevice> allphonelist = new List<ExtDevice>(this.AllPhoneList.ToList());

            //选中已有成员
            foreach (ExtDevice e in allphonelist)
            {
                BroadcastMember temp = selectedbroadcastmemberlist.Find(c => c.callno.Equals(e.callno));
                if (temp == null)
                {
                    e.DevSelected = false;
                }
                else
                {
                    e.DevSelected = true;
                }
            }
            //成员排序
            var queryresults =
                from n in allphonelist
                orderby n.DevSelected descending, n.callno
                select n;
            allphonelist = queryresults.ToList();
            this.AllPhoneList = new ObservableCollection<ExtDevice>(allphonelist);
            data.AllDevList = this.AllPhoneList;

            var view = new ListViewDialog();
            view.DataContext = data;


            //show the dialog
            var result = await DialogHost.Show(view, "RootDialogBroadcast", BroadcastViewClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void BroadcastViewClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }


        private KeyBoardNew _selectedkey;
        public KeyBoardNew SelectedKey
        {
            get { return _selectedkey; }
            set
            {
                if (_selectedkey != value)
                {
                    _selectedkey = value;
                    OnPropertyChanged("SelectedKey");
                }
            }
        }
        private GroupNew _selectedgroup;
        public GroupNew SelectedGroup
        {
            get { return _selectedgroup; }
            set
            {
                if (_selectedgroup != value)
                {
                    _selectedgroup = value;
                    OnPropertyChanged("SelectedGroup");
                }
            }
        }
        private Broadcast _selectedbroadcast;
        public Broadcast SelectedBroadcast
        {
            get { return _selectedbroadcast; }
            set
            {
                if (_selectedbroadcast != value)
                {
                    _selectedbroadcast = value;
                    OnPropertyChanged("SelectedBroadcast");
                }
            }
        }
        private string _username;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged("UserName");
                }
            }
        }
        private string _password;
        public string PassWord
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged("PassWord");
                }
            }
        }

        private string _inputstr;
        public string InputStr
        {
            get { return _inputstr; }
            set
            {
                if (_inputstr != value)
                {
                    _inputstr = value;
                    OnPropertyChanged("InputStr");
                }
            }
        }

        private string _outputstr;
        public string OutputStr
        {
            get { return _outputstr; }
            set
            {
                if (_outputstr != value)
                {
                    _outputstr = value;
                    OnPropertyChanged("OutputStr");
                }
            }
        }
        private string _ipaddr;
        public string IpAddr
        {
            get { return _ipaddr; }
            set
            {
                if (_ipaddr != value)
                {
                    _ipaddr = value;
                    OnPropertyChanged("IpAddr");
                }
            }
        }
        private bool _logstate;
        public bool LogState
        {
            get { return _logstate; }
            set
            {
                if (_logstate != value)
                {
                    _logstate = value;
                    OnPropertyChanged("LogState");
                }
            }
        }
        private string _logbuttontext;
        public string LogButtonText
        {
            get { return _logbuttontext; }
            set
            {
                if (_logbuttontext != value)
                {
                    _logbuttontext = value;
                    OnPropertyChanged("LogButtonText");
                }
            }
        }
        private ObservableCollection<KeyBoardNew> _keyboardlist;
        public ObservableCollection<KeyBoardNew> KeyboardList
        {
            get { return _keyboardlist; }
            set
            {
                SetAndNotifyIfChanged("KeyboardList", ref _keyboardlist, value);
            }
        }
        private ObservableCollection<ExtDevice> _alldevlist;
        public ObservableCollection<ExtDevice> AllDevList
        {
            get { return _alldevlist; }
            set
            {
                SetAndNotifyIfChanged("AllDevList", ref _alldevlist, value);
            }
        }
        private ObservableCollection<ExtDevice> _allphonelist;
        public ObservableCollection<ExtDevice> AllPhoneList
        {
            get { return _allphonelist; }
            set
            {
                SetAndNotifyIfChanged("AllPhoneList", ref _allphonelist, value);
            }
        }

        private ObservableCollection<ExtDevice> _alltrunklist;
        public ObservableCollection<ExtDevice> AllTrunkList
        {
            get { return _alltrunklist; }
            set
            {
                SetAndNotifyIfChanged("AllTrunkList", ref _alltrunklist, value);
            }
        }
    }
}
