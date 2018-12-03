using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DispatchApp
{
    public enum SwitchDeviceType
    {
        HuaWei,
        FengHuo,
        ZhongXing
    }

    public class SwitchDeviceWrapper
    {
        public Int16 swt { set; get; }
        public string description { set; get; }

        public SwitchDeviceWrapper(Int16 swtparam, string des)
        {
            swt = swtparam;
            description = des;
        }

    }

    public class UserStatus
    {
        public Int16 id { set; get; }
        public string description { set; get; }

        public UserStatus(Int16 idparam, string des)
        {
            id = idparam;
            description = des;
        }
    }

    public class UserRole
    {
        public Int16 id { set; get; }
        public string description { set; get; }

        public UserRole(Int16 idparam, string des)
        {
            id = idparam;
            description = des;
        }
    }
}
