using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DispatchApp
{
    public class ExtDevice : NotifyObject
    {
        private bool _ischecked = false;
        public bool DevSelected
        {
            get { return _ischecked; }
            set
            {
                if (_ischecked != value)
                {
                    _ischecked = value;
                    OnPropertyChanged("DevSelected");
                }

            }
        }
        private string _callno;
        public string callno
        {
            get { return _callno; }
            set
            {
                if (_callno != value)
                {
                    _callno = value;
                    OnPropertyChanged("callno");
                }
            }
        }

        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("name");
                }
            }
        }
        private string _type;
        public string type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged("type");
                }
            }
        }
        private string _level;
        public string level
        {
            get { return _level; }
            set
            {
                if (_level != value)
                {
                    _level = value;
                    OnPropertyChanged("level");
                }
            }
        }
        private string _description;
        public string description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("description");
                }
            }
        }
    }
    public class GroupNew : NotifyObject
    {
        public GroupNew()
        {
            memberlist = new ObservableCollection<ExtDevice>();
        }
        private string _index;
        public string index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    OnPropertyChanged("index");
                }
            }
        }
        private string _groupname;
        public string groupname
        {
            get { return _groupname; }
            set
            {
                if (_groupname != value)
                {
                    _groupname = value;
                    OnPropertyChanged("groupname");
                }
            }
        }
        private string _column;
        public string column
        {
            get { return _column; }
            set
            {
                if (_column != value)
                {
                    _column = value;
                    OnPropertyChanged("column");
                }
            }
        }
        private string _description;
        public string description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("description");
                }
            }
        }
        private ObservableCollection<ExtDevice> _memberlist;
        public ObservableCollection<ExtDevice> memberlist
        {

            get { return _memberlist; }
            set
            {
                SetAndNotifyIfChanged("memberlist", ref _memberlist, value);
            }
        }
    }
    public class BroadcastMember : NotifyObject
    {
        private string _callno;
        public string callno
        {
            get { return _callno; }
            set
            {
                if (_callno != value)
                {
                    _callno = value;
                    OnPropertyChanged("callno");
                }
            }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("name");
                }
            }
        }
    }
    public class Broadcast:NotifyObject
    {
        private string _index;
        public string index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    OnPropertyChanged("index");
                }
            }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("name");
                }
            }
        }
        private ObservableCollection<BroadcastMember> _bmemberlist;
        public ObservableCollection<BroadcastMember> bmemberlist
        {
            get { return _bmemberlist; }
            set
            {
                SetAndNotifyIfChanged("bmemberlist", ref _bmemberlist, value);
            }
        }
        public Broadcast()
        {
            bmemberlist = new ObservableCollection<BroadcastMember>();
        }
    }
    public class TrunkDev : NotifyObject
    {
        private string _trunkid;
        public string trunkid
        {
            get { return _trunkid; }
            set
            {
                if (_trunkid != value)
                {
                    _trunkid = value;
                    OnPropertyChanged("trunkid");
                }
            }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("name");
                }
            }
        }
        private string _bindingnumber;
        public string bindingnumber
        {
            get { return _bindingnumber; }
            set
            {
                if (_bindingnumber != value)
                {
                    _bindingnumber = value;
                    OnPropertyChanged("bindingnumber");
                }
            }
        }
    }

    public class KeyBoardNew : NotifyObject
    {
        public KeyBoardNew()
        {
            grouplist = new ObservableCollection<GroupNew>();
            hotlinelist = new ObservableCollection<ExtDevice>();
            broadcastlist = new ObservableCollection<Broadcast>();
            trunklist = new ObservableCollection<TrunkDev>();
        }
        private string _sequence;
        public string sequence
        {
            get { return _sequence; }
            set
            {
                if (_sequence != value)
                {
                    _sequence = value;
                    OnPropertyChanged("sequence");
                }
            }
        }
        private string _index;
        public string index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    OnPropertyChanged("index");
                }
            }
        }
        private string _name;
        public string name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("name");
                }
            }
        }
        private string _mac;
        public string mac
        {
            get { return _mac; }
            set
            {
                if (_mac != value)
                {
                    _mac = value;
                    OnPropertyChanged("mac");
                }
            }
        }
        private string _ip;
        public string ip
        {
            get { return _ip; }
            set
            {
                if (_ip != value)
                {
                    _ip = value;
                    OnPropertyChanged("ip");
                }
            }
        }
        private ObservableCollection<GroupNew> _grouplist;
        public ObservableCollection<GroupNew> grouplist
        {
            get { return _grouplist; }
            set
            {
                SetAndNotifyIfChanged("grouplist", ref _grouplist, value);
            }
        }
        private ObservableCollection<ExtDevice> _hotlinelist;
        public ObservableCollection<ExtDevice> hotlinelist
        {
            get { return _hotlinelist; }
            set
            {
                SetAndNotifyIfChanged("hotlinelist", ref _hotlinelist, value);
            }
        }
        private ObservableCollection<Broadcast> _broadcastlist;
        public ObservableCollection<Broadcast> broadcastlist
        {
            get { return _broadcastlist; }
            set
            {
                SetAndNotifyIfChanged("broadcastlist", ref _broadcastlist, value);
            }
        }
        private ObservableCollection<TrunkDev> _trunklist;
        public ObservableCollection<TrunkDev> trunklist
        {
            get { return _trunklist; }
            set
            {
                SetAndNotifyIfChanged("trunklist", ref _trunklist, value);
            }
        }
    }
    public class AllKeyBoard
    {
        public string sequence;
        public ObservableCollection<KeyBoardNew> keyboardlist;
        public AllKeyBoard()
        {
            keyboardlist = new ObservableCollection<KeyBoardNew>();
        }
    }
    public class AllDev
    {
        public string sequence;
        public ObservableCollection<ExtDevice> DevList;
        public AllDev()
        {
            DevList = new ObservableCollection<ExtDevice>();
        }
    }
}
