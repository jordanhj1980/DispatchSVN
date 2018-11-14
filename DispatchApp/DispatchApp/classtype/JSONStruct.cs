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

    public class SWDEV
    {
        public string sequence { get; set; }
        public string index { get; set; }
        public string name { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public string type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
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
}
