using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DispatchApp
{
    public interface IServiceCallback
    {
        void GetLoginFeedBack(int type, string msg);

        void GetOperationMsg(string msg);

        void ServerDisconnected();
    }
}
