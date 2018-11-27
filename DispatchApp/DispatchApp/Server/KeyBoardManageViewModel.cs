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
            KeyboardList = new ObservableCollection<KeyBoardNew>();
            AllDevList = new ObservableCollection<ExtDevice>();
            AllPhoneList = new ObservableCollection<ExtDevice>();
        }
        private MyCommand _RunListViewDialogCommand;

        public MyCommand RunListViewDialogCommand
        {
            get
            {
                if (_RunListViewDialogCommand == null)
                    _RunListViewDialogCommand = new MyCommand(new Action<object>
                    (
                        o =>
                        {
                            ExecuteRunListViewDialog(o);
                        }
                    ),
                    new Func<object, bool>(o => this.SelectedGroup != null));
                return _RunListViewDialogCommand;
            }
        }
        /// <summary>
        /// 分组成员选择窗口
        /// </summary>
        /// <param name="o"></param>
        private async void ExecuteRunListViewDialog(object o)
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
            var result = await DialogHost.Show(view, "RootDialog", ListViewClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void ListViewClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
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
            var result = await DialogHost.Show(view, "RootDialog", HotlineListViewClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void HotlineListViewClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
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
    }
}
