using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DispatchApp
{

    #region 软交换设备

    public class SWQUERY
    {
        public string sequence { get; set; }
        public string index { get; set; }
    }

    public class SW_QUERYRESULT
    {
        public string sequence { get; set; }
        public List<SWDEVITEM> switchlist { get; set; }
    }

    /* 协议中软交换设备数据结构 */
    public class SWDEVITEM
    {
        public string index { get; set; }
        public string name { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public string type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    /* 协议中软交换设备数据结构 */
    public class SWDEV: NotifyObject
    {
        private string _name;
        public string name {
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

        private string _port;
        public string port
        {
            get { return _port; }
            set
            {
                if (_port != value)
                {
                    _port = value;
                    OnPropertyChanged("port");
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

        private string _username;
        public string username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged("username");
                }
            }
        }

        private string _password;
        public string password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged("password");
                }
            }
        }

        public string sequence { get; set; }
        public string index { get; set; }
        //public string name { get; set; }
        //public string ip { get; set; }
        //public string port { get; set; }
        //public string type { get; set; }
        //public string username { get; set; }
        //public string password { get; set; }
    }

    public class SW_ADDRESULT
    {
        public string sequence { get; set; }
        public string index { get; set; }
        public string result { get; set; }
        public string reason { get; set; }
    }

    #endregion

    #region 用户

    public class USERQUERY
    {
        public string sequence { get; set; }
        public string name { get; set; }
    }

    /* 用户的UI数据结构，仅仅比协议传输的USEREDITITEM多了一个index */
    public class User : NotifyObject
    {
        public string sequence { set; get; }
        public int index { set; get; }  // 当前列表的索引号

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

        private string _password;
        public string password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged("password");
                }
            }
        }

        private string _privilege;
        public string privilege
        {
            get { return _privilege; }
            set
            {
                if (_privilege != value)
                {
                    _privilege = value;
                    OnPropertyChanged("privilege");
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

        private string _status;
        public string status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("status");
                }
            }
        }

        private string _role;
        public string role
        {
            get { return _role; }
            set
            {
                if (_role != value)
                {
                    _role = value;
                    OnPropertyChanged("role");
                }
            }
        }

        private string _desk;   // 调度台
        public string desk
        {
            get { return _desk; }
            set
            {
                if (_desk != value)
                {
                    _desk = value;
                    OnPropertyChanged("desk");
                }
            }
        }
    }


    public class USER_QUERYRESULT
    {
        public string sequence { get; set; }
        public List<USERITEM> userlist { get; set; }
    }

    public class USERITEM
    {
        public string name { get; set; }
        public string password { get; set; }
        public string privilege { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string role { get; set; }
        public string desk { get; set; }
    }

    public class USEREDITITEM
    {
        public string sequence { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string privilege { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string role { get; set; }
        public string desk { get; set; }
    }

    public class USER_ADDRESULT
    {
        public string sequence { get; set; }
        public string result { get; set; }
        public string reason { get; set; }
    }

    #endregion


    #region 调度台

    /* 调度台的UI数据结构 */
    class DeskDevice
    {
        public int index { set; get; }  // 当前列表的索引号
        public string groupname { set; get; }
        public string description { set; get; }
        public List<Member> memberlist { set; get; }

        public string callno { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public string level { set; get; }
    }
    #endregion
}
