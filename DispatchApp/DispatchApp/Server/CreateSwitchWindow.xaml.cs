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

using Newtonsoft.Json;
using System.Diagnostics;

namespace DispatchApp
{
    /// <summary>
    /// Interaction logic for CreateSwitchWindow.xaml
    /// </summary>
    public partial class CreateSwitchWindow : Window
    {
        public delegate void CWHandler(object sender, string msg, object obj);

        public event CWHandler msgevent;

        public CreateSwitchWindow()
        {
            InitializeComponent();

            /* 初始化软交换设备选择类型列表 */
            IEnumerable<SwitchDeviceWrapper> allType = new List<SwitchDeviceWrapper>
            {
                new SwitchDeviceWrapper(1, "华为终端"),
                new SwitchDeviceWrapper(2, "烽火终端"),
                new SwitchDeviceWrapper(3, "中兴终端"),
            };

            comboBox_type.ItemsSource = allType;
            comboBox_type.DisplayMemberPath = "description";
            comboBox_type.SelectedValuePath = "swt";
            comboBox_type.SelectedIndex = 0;
        }

        private void bt_Click_apply(object sender, RoutedEventArgs e)
        {
            /* 首先校验用户输入 */
            //if (!IsValid(this)) 
            //    return;

            // 保存当前的软交换配置，并发送给服务器

            //联网登陆，有问题
            SWDEV swdevobj = new SWDEV();
            swdevobj.sequence = GlobalFunAndVar.sequenceGenerator();
            swdevobj.name = tb_name.Text.Trim();
            swdevobj.ip = tb_ip.Text.Trim();
            swdevobj.port = tb_port.Text.Trim();

            //SwitchDeviceType testenum = (SwitchDeviceType)Enum.Parse(typeof(SwitchDeviceType) ,comboBox_type.SelectedItem.ToString() ,false)
            swdevobj.type = comboBox_type.SelectedValue.ToString();

            StringBuilder sb = new StringBuilder(100);
            sb.Append("MAN#ADDSW#");
            sb.Append(JsonConvert.SerializeObject(swdevobj));

            Debug.WriteLine(sb.ToString());

            /* 发送网络信息 */
            if (msgevent != null)
            {
                msgevent(this, "net" ,sb.ToString());
                msgevent(this, "swdev", swdevobj);
            }
        }

        private void bt_Click_cancel(object sender, RoutedEventArgs e)
        {
            // 取消配置并且关闭窗口
            Close();
        }

        private void bt_Click_ok(object sender, RoutedEventArgs e)
        {
            // 确定配置并关闭当前窗口
            Close();
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
