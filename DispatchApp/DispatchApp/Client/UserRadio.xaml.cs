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

namespace DispatchApp
{
    /// <summary>
    /// UserRadio.xaml 的交互逻辑
    /// </summary>
    public partial class UserRadio : UserControl
    {
        public delegate void ImageEventHandler(string ImageSoures);
        public event ImageEventHandler ImageSouresHandle;
        public event ImageEventHandler ImageSouresDoubleHandle;

        public UserRadio()
        {
            InitializeComponent();
        }

        private void MouseDouble_Click(object sender, MouseButtonEventArgs e)
        {
            if (ImageSouresDoubleHandle != null)
            {
                ImageSouresDoubleHandle(name.Text);
            }
        }

        private void Style_click(object sender, RoutedEventArgs e)
        {
            if (ImageSouresHandle != null)
            {
                ImageSouresHandle(name.Text);
            }
        }
    }
}
