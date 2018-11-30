using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections.ObjectModel;
using System.ComponentModel;

using Npgsql;

using System.Windows.Interop;
using System.Runtime.InteropServices;
using WebSocket4Net;
using Newtonsoft.Json;

using System.Threading;
using System.Diagnostics;
using MaterialDesignThemes.Wpf.Transitions;


namespace DispatchApp
{
    /// <summary>
    /// NightServerCloseBtn.xaml 的交互逻辑
    /// </summary>
    public partial class NightServerCloseBtn : UserControl
    {
        public NightServerCloseBtn()
        {
            InitializeComponent();
        }
    }
}
