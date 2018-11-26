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

namespace DispatchApp
{
    /// <summary>
    /// LoadingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingWindow : Window
    {
        //private MainWindow m_mainWindow;
        public LoadingWindow()
        {
            InitializeComponent();
            //m_mainWindow = mainWindow;
        }

        public static void ShowDialog(Window owner)
        {
            //蒙板
            Grid layer = new Grid() { Background = new SolidColorBrush(Colors.White), Opacity = 0.3 };
            //父级窗体原来的内容
            UIElement original = owner.Content as UIElement;
            owner.Content = null;
            //容器Grid
            Grid container = new Grid();
            container.Children.Add(original);//放入原来的内容
            container.Children.Add(layer);//在上面放一层蒙板
            //将装有原来内容和蒙板的容器赋给父级窗体
            owner.Content = container;

            LoadingWindow ca = new LoadingWindow() { Owner = owner };
            ca.ShowDialog();
        }

        public void LoadingWindow_Closed(object sender, EventArgs e)
        {
            //容器Grid
            Grid grid = this.Owner.Content as Grid;
            if (grid != null) { 
                //父级窗体原来的内容
                UIElement original = VisualTreeHelper.GetChild(grid, 0) as UIElement;
                //将父级窗体原来的内容在容器Grid中移除
                grid.Children.Remove(original);
                //赋给父级窗体
                this.Owner.Content = original;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
            MainWindow win = Owner as MainWindow;
            if (win != null)
            {
                App.isLogin = false;
                win.showLogWin();
            }
        }

        /* 点击重连按钮,触发重新连接 */
        private void BtnClick_reconnect(object sender, RoutedEventArgs e)
        {
            MainWindow win = Owner as MainWindow;
            if (win != null)
            {
                App.isLogin = false;
                win.reLogin();
            }
        }

        /*
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }*/
    }
}
