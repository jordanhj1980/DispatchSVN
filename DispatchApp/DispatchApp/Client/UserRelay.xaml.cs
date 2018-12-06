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

using System.ComponentModel;


namespace DispatchApp
{
    /// <summary>
    /// UserRelay.xaml 的交互逻辑
    /// </summary>
    public partial class UserRelay : UserControl , INotifyPropertyChanged
    {
        public delegate void ImageEventHandler(GroupTrunk ImageSoures);
        public event ImageEventHandler ImageSouresHandle;
        public event ImageEventHandler ImageSouresDoubleHandle;

        /// <summary>
        /// 状态绑定
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public string _CurrentState;
        public string CurrentState
        {
            get { return _CurrentState; }
            set
            {
                _CurrentState = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentState"));
            }
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                //假设属性发生了改变，则触发这个事件
                PropertyChanged(this, e);
            }
        }

        /// <summary>
        /// 参数绑定
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged2;
        public GroupTrunk _Soures;
        public GroupTrunk Soures
        {
            get { return _Soures; }
            set
            {
                _Soures = value;
                OnPropertyChanged2(new PropertyChangedEventArgs("Soures"));
            }
        }

        public void OnPropertyChanged2(PropertyChangedEventArgs e)
        {
            if (PropertyChanged2 != null)
            {
                //假设属性发生了改变，则触发这个事件
                PropertyChanged2(this, e);
            }
        }

        /// <summary>
        /// UserRelay
        /// </summary>
        public UserRelay()
        {
            InitializeComponent();
            DataContext = this;
            CurrentState = "OFFLINE";
            Soures = new GroupTrunk();
        }

        private void MouseDouble_Click(object sender, MouseButtonEventArgs e)
        {
            if (ImageSouresDoubleHandle != null)
            {
                //GroupTrunk Soures = new GroupTrunk();
                //Soures.name = name.Text;
                //Soures.trunkid = trunkid.Text;
                //Soures.bindingnumber = bindingnumber.Text;
                ImageSouresDoubleHandle(Soures);
            }
        }

        private void Style_click(object sender, RoutedEventArgs e)
        {
            if (ImageSouresHandle != null)
            {
                //GroupTrunk Soures = new GroupTrunk();
                //Soures.name = name.Text;
                //Soures.trunkid = trunkid.Text;
                //Soures.bindingnumber = bindingnumber.Text;
                ImageSouresHandle(Soures);
            }
        }
    }
}
