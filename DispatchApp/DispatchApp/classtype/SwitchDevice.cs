using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DispatchApp
{
    /* 软交换的UI数据结构 */
    class SwitchDevice
    {
        public int index { set; get; }  // 当前列表的索引号
        public string idstr { set; get; }
        public string name { set; get; }
        public int type { set; get; }
        public bool iscp { set; get; } // 是否展开，隐藏属性
        public bool IsDetailsExpanded { set; get; } // 是否展开，隐藏属性
    }

    /* 用户的UI数据结构 */
    public class User : NotifyObject
    {
        public string sequence { set; get; }
        public int index { set; get; }  // 当前列表的索引号
        public string idstr { set; get; }
        public string name { set; get; }
        public string password { set; get; }
        public string privilege { set; get; }
        public string description { set; get; }
        public int status { set; get; }
        public int role { set; get; }
        public string desk { set; get; }    // 调度台
        public bool IsDetailsExpanded { set; get; } // 是否展开，隐藏属性
    }

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


}
