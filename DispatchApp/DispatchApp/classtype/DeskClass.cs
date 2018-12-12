using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DispatchApp
{
    public struct Reply
    {
        public string result;
        public string reason;
    }

    // =======================查询调度台信息指令======================
    /// 20181024 xiaozi Add
    /// <summary>
    /// 查询调度键盘
    /// </summary>
    public struct SearchDesk
    {
        public string sequence;
    }
    //==================================================================

    // =======================添加调度台信息指令======================
    /// 20181103 xiaozi Add
    /// <summary>
    /// 添加调度键盘的应答
    /// </summary>
    public struct AnsAddKeyBoard
    {
        public string sequence;
        public string index;
        public string result;
        public string reason;
    }
    //==================================================================

    // =======================删除调度台信息指令======================
    /// 20181103 xiaozi Add
    /// <summary>
    /// 删除调度键盘的应答
    /// </summary>
    public struct DelKeyBoard
    {
        public string sequence;
        public string index;
    }
    public struct AnsDelKeyBoard
    {
        public string sequence;
        public string result;
        public string reason;
    }
    //==================================================================

    /// =======================获取查询的调度台信息======================
    /// 20181024 xiaozi Add
    /// <summary>
    /// 每个页面的成员电话
    /// </summary>
    public struct Member
    {
        public string callno;
        public string name;
        public string type;
        public string level;
        public string description;
    }

    /// <summary>
    /// 多个页面
    /// </summary>
    public class Group
    {
        public string index;
        public string groupname;
        public string column;
        public string description;
        public List<Member> memberlist;
    }

    /// <summary>
    /// 线路（键权电话）
    /// </summary>
    public class Hotline
    {
        public string callno;
        public string name;
        public string type;
        public string level;
        public string description;
    }

    /// <summary>
    /// 每个调度键盘（调度用户处理/调度台包含）的内容
    /// </summary>
    public class KeyBoard
    {
        public string sequence;
        public string index;
        public string name;
        public string mac;
        public string ip;
        public List<Group> grouplist;
        public List<Hotline> hotlinelist;
    }

    /// <summary>
    /// 获取查询的调度台信息
    /// </summary>
    public struct GetDesk
    {
        public string sequence;
        public List<KeyBoard> keyboardlist;
    }
    //============================================================

    /// ==========用来记录当前所在调度键盘信息的位置===============
    public struct MemberNum
    {
        public int count;
        public int index;
    }

    public struct GroupNum
    {
        public int count;
        public int index;
        public MemberNum memberNum;
    }
    public struct HotLineNum
    {
        public int count;
        public int index;
    }
    public struct KeyBoardNum
    {
        public int count;
        public int index;
        public GroupNum groupNum;
        public HotLineNum hotlineNum;
    }
    ///============================================================

    /// =======================发送添加的调度台信息===============
    public struct SendAddDesk
    {
        public string sequence;
        public string index;
        public string name;
        public string mac;
        public string ip;
        public List<Group> grouplist;
        public List<Hotline> hotlinelist;
    }
    //============================================================

    /// =======================发送添加的调度台信息===============
    public struct ReceiveAddDesk
    {
        public string sequence;
        public string result;
        public string reason;
    }
    //============================================================

    /// =======================查找所有电话=======================
    public struct SearcheAllRegister
    {
        public string sequence;
    }
    //============================================================

    /// =======================获取所有电话=======================
    public struct DevList
    {
        public string callno;
        public string type;
        public string name;
        public string level;
        public string description;
    }

    public struct GetAllRegister
    {
        public string sequence;
        public List<DevList> devlist;
    }
    //============================================================

    /// =======================直呼键权电话=======================
    public class GroupTrunk : NotifyObject
    {
        private string _trunkid;
        public string trunkid
        {
            get { return _trunkid ; }
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
            get { return _name ; }
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
            get { return _bindingnumber ; }
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
    //============================================================ 

    /// =======================广播分组电话=======================
    public struct GroupBroadcast
    {
        public string name;
        public List<GroupMember> bmemberlist;
    }

    public struct GroupMember
    {
        public string callno;
        public string name;
    }
    //============================================================ 

    /// ==========================查询日志========================
    class DateCDR
    {
        public string Cdrid { set; get; }//话单id
        public string callid { set; get; }//通话的相对唯一标识
        public string type { set; get; }//通话类型 IN(打入)/OU(打出)/FI(呼叫转移入)/FW(呼叫转 移出)/LO(内部通话)/CB(双向外呼)
        public string TimeStart { set; get; }//呼叫起始时间，即发送或收到呼叫请求的时间
        public string TimeEnd { set; get; }//呼叫结束时间，即通话的一方挂断的时间
        public string CPN { set; get; }//主叫号码
        public string CDPN { set; get; }//被叫号码
        public string Duration { set; get; }  //通话时长，值为 0 说明未接通。
    }
    //============================================================

    /// ========================外线呼叫参数======================
    public class OutLineCall : NotifyObject
    {
        public OutLineCall()
        {
            relayNumList = new ObservableCollection<RelayNum>();
        }

        /// <summary>
        /// 键权电话号码
        /// </summary>
        private string _serverNum;
        public string serverNum
        {
            get { return _serverNum ; }
            set
            {
                if (_serverNum != value)
                {
                    _serverNum = value;
                    OnPropertyChanged("serverNum");
                }
            }
        }

        /// <summary>
        /// 外线号码
        /// </summary>
        private string _outLineNum;
        public string outLineNum
        {
            get { return _outLineNum; }
            set
            {
                if (_outLineNum != value)
                {
                    _outLineNum = value;
                    OnPropertyChanged("outLineNum");
                }
            }
        }

        private ObservableCollection<RelayNum> _relayNumList;
        public ObservableCollection<RelayNum> relayNumList
        {

            get { return _relayNumList; }
            set
            {
                SetAndNotifyIfChanged("relayNumList", ref _relayNumList, value);
            }
        }
    }


    public class RelayNum : NotifyObject
    {
        /// <summary>
        /// 中继号码
        /// </summary>
        private string _relayNum;
        public string relayNum
        {
            get { return _relayNum; }
            set
            {
                if (_relayNum != value)
                {
                    _relayNum = value;
                    OnPropertyChanged("relayNum");
                }
            }
        }

        /// <summary>
        /// 选中标志
        /// </summary>
        private bool _ischecked = false;
        public bool isSelected
        {
            get { return _ischecked; }
            set
            {
                if (_ischecked != value)
                {
                    _ischecked = value;
                    OnPropertyChanged("isSelected");
                }
            }
        }

        /// <summary>
        /// 选中button的背景所对应的状态
        /// </summary>
        private string _relayState;
        public string strRelayState
        {
            get { return _relayState; }
            set
            {
                if (_relayState != value)
                {
                    _relayState = value;
                    OnPropertyChanged("strRelayState");
                }
            }

        }
    }



    /// <summary>
    /// 外线拨号记录
    /// </summary>
    public class CallLog : NotifyObject
    {
        /// <summary>
        /// 姓名
        /// </summary>
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

        /// <summary>
        /// 号码
        /// </summary>
        private string _num;
        public string num
        {
            get { return _num; }
            set
            {
                if (_num != value)
                {
                    _num = value;
                    OnPropertyChanged("num");
                }
            }
        }
    }
    /// ==========================查询日志========================

   
}
