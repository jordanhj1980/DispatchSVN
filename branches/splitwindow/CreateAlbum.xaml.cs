using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for CreateAlbum.xaml
    /// </summary>
    /// <summary>
    /// CreateAlbum.xaml 的交互逻辑
    /// </summary>
    public partial class CreateAlbum : Window
    {
        public CreateAlbum()
        {
            InitializeComponent();
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

            CreateAlbum ca = new CreateAlbum() { Owner = owner };
            ca.ShowDialog();
        }

        private void CreateAlbumWindow_Closed(object sender, EventArgs e)
        {
            //容器Grid
            Grid grid = this.Owner.Content as Grid;
            //父级窗体原来的内容
            UIElement original = VisualTreeHelper.GetChild(grid, 0) as UIElement;
            //将父级窗体原来的内容在容器Grid中移除
            grid.Children.Remove(original);
            //赋给父级窗体
            this.Owner.Content = original;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Close();

        }
        /// <summary>
        /// 添加歌单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (CreateAlbumTitle.Text.Length > 20 || CreateAlbumTitle.Text.Length <= 0) return;
            //CommonEvent._CreateAlbum(CreateAlbumTitle.Text);
            this.Close();
        }

        private void CreateAlbumTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            CreateAlbumTitle.Tag = (20 - CreateAlbumTitle.Text.Length).ToString();
        }
    }
}
