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
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DispatchApp
{
    /// <summary>
    /// LockScreen.xaml 的交互逻辑
    /// </summary>
    public partial class LockScreen : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /* propertyName为属性的名称 */
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                //假设属性发生了改变，则触发这个事件
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void SetAndNotifyIfChanged<T>(string propertyName, ref T oldValue, T newValue)
        {
            if (oldValue == null && newValue == null) return;
            if (oldValue != null && oldValue.Equals(newValue)) return;
            if (newValue != null && newValue.Equals(oldValue)) return;
            oldValue = newValue;
            OnPropertyChanged(propertyName);
        }

        public delegate void LockHandler(object sender, string msg);
        public event LockHandler msgevent;

        private MainWindow m_mainwin;

        private string _account;
        public string account
        {
            get { return _account; }
            set { 
                _account = value;
                OnPropertyChanged("account");
            }
        }

        public LockScreen(MainWindow mainwin)
        {
            InitializeComponent();
            m_mainwin = mainwin;
            DataContext = this;
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
                this.message.Text = "";
                this.TxPassword.Clear();

                flipc.OnApplyTemplate();

                this.Close();
                msgevent(this, "close");
            }
            else
            {
                this.message.Text = "解锁密码错误";
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
        
    }
}
