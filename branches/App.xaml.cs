using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace DispatchApp
{
    public delegate void CtrlSwitchHandler();//weituo 20181013

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        /* 标识程序是否第一次启动 */
        private bool bFirst = true;
        public static bool isLogin = false;  //2018101 xf Add

        /* 用于存放系统中的程序变量 */
        public static Dictionary<string, object> Dic = new Dictionary<string, object>();

        protected override void OnStartup(StartupEventArgs e)
        {
            if (bFirst)
            {
                //Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;                
                //Dic["LoginWindow"] = logwin;
                base.OnStartup(e);
                //Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
                //通过Application的ShutdownMode控制进程的运行时间
            }
            
            //Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            //LoginWindow window = new LoginWindow();
            //bool? dialogResult = window.ShowDialog();
            
            //if ((dialogResult.HasValue == true) && (dialogResult.Value == true))
            //{
            //    base.OnStartup(e);
            //    Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            //    //通过Application的ShutdownMode控制进程的运行时间
            //} else
            //{
            //    this.Shutdown();
            //}

            /*
            SplashScreen s = new SplashScreen("Resources/startup_1920x1080.jpg");
            //显示初始屏幕 自动关闭设置false
            s.Show(false);

            //在3秒后关闭
            s.Close(new TimeSpan(0, 0, 3));
            base.OnStartup(e);*/
        }
    }
}
