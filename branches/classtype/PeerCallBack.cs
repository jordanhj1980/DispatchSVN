using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DispatchApp
{
    public class PeerCallBack : IServiceCallback
    {
        public static PeerCallBack Instance;

        private SynchronizationContext m_uiSyncContext = null;
        private MainWindow m_mainWindow;
        public PeerCallBack(SynchronizationContext uiSyncContext, MainWindow mainWindow)
        {
            m_uiSyncContext = uiSyncContext;
            m_mainWindow = mainWindow;
        }

        public void GetLoginFeedBack(int type, string msg)
        {
            SendOrPostCallback callback =
                delegate(object state)
                {
                    m_mainWindow.GetLoginFeedBack(type, state as string);
                };
            m_uiSyncContext.Post(callback, msg);
        }

        public void GetOperationMsg(string msg)
        {
            SendOrPostCallback callback =
                delegate(object state)
                {
                    m_mainWindow.GetOperationMsg(state as string);
                };
            m_uiSyncContext.Post(callback, msg);
        }

        public void ServerDisconnected()
        {

        }

    }
}
