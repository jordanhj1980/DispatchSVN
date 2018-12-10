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
using System.Windows.Shapes;
using System.Security.Cryptography;

namespace DispatchApp
{
    /// <summary>
    /// LockScreen.xaml 的交互逻辑
    /// </summary>
    public partial class LockScreen : Window
    {
        public delegate void LockHandler(object sender, string msg);
        public event LockHandler msgevent;

        private MainWindow m_mainwin;

        public LockScreen(MainWindow mainwin)
        {
            InitializeComponent();
            m_mainwin = mainwin;
        }

        private void btn_OK(object sender, RoutedEventArgs e)
        {

            var buffer = Encoding.UTF8.GetBytes(TxPassword.Password);
            var data = SHA1.Create().ComputeHash(buffer);
            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("X2"));
            }
            sb.ToString();

            if (sb.ToString() == m_mainwin.password)
            {
                flipc.IsFlipped = false;
                flipc.OnApplyTemplate();

                this.Close();
                msgevent(this, "close");
            }
        }

        private void btn_Cancel(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void exitProgram(object sender, MouseButtonEventArgs e)
        {
            m_mainwin.Close();
        }

    }
}
