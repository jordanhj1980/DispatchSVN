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
using System.Diagnostics;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace DispatchApp
{
    /// <summary>
    /// SwitchManage.xaml 的交互逻辑
    /// </summary>
    public partial class SwitchManage : UserControl
    {
        public SWDEV tempobj;      // /* 保存临时的用户item */

        private MainWindow mainWindow;
        private SwitchViewModel switchDataModel;
        public SwitchManage(MainWindow mainwindow)
        {
            InitializeComponent();
            this.mainWindow = mainwindow;
            switchDataModel = new SwitchViewModel();
            this.DataContext = switchDataModel;

            tempobj = new SWDEV();   // 临时变量
        }

        #region recv_operation

        public void recv(string state, string data)
        {
            switch (state)
            {
                case "ADDSW":
                    addSwitchDevice(data);
                    break;
                case "QUERYSW":
                    freshSwitchDevice(data);
                    break;
                case "DELSW":
                    delSwitchDevice(data);
                    break;
                case "EDITSW":
                    editSwitchDevice(data);
                    break;
                default:
                    break;
            }
        }

        /* 添加软交换设备 */
        /// <summary>
        /// 20181025 xf Add
        /// </summary>
        /// <param name="data"></param>
        private void addSwitchDevice(string data)
        {
            try
            {
                /* 比较临时存储的obj的sequence是否一致 */
                SW_ADDRESULT res = JsonConvert.DeserializeObject<SW_ADDRESULT>(data);
                if (res != null && tempobj.sequence == res.sequence)
                {
                    if (res.result == "Fail")
                    {
                        Debug.WriteLine(res.reason);
                        this.result.Content = "添加失败";
                        return;
                    }
                    this.result.Content = "添加成功";

                    // 收到添加成功消息，通知服务器添加调度电话
                    SWQUERY tempDev = new SWQUERY();
                    tempDev.sequence = GlobalFunAndVar.sequenceGenerator();
                    tempDev.index = res.index;

                    StringBuilder sb = new StringBuilder(100);
                    sb.Append("MAN#QUERYALLDEV#");
                    sb.Append(JsonConvert.SerializeObject(tempDev));

                    Debug.WriteLine("SEND: " + sb.ToString());
                    mainWindow.ws.Send(sb.ToString());

                    SWDEV item = new SWDEV();
                    //item.idstr = swdevobj.name;
                    item.name = tempobj.name;
                    item.ip = tempobj.ip;
                    item.port = tempobj.port;
                    /* 这里的index为返回的索引 */
                    item.index = res.index;
                    item.type = tempobj.type;

                    switchDataModel.SwitchList.Add(item);
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private void delSwitchDevice(string data)
        {
            /* 比较临时存储的obj的sequence是否一致 */
            SW_ADDRESULT res = JsonConvert.DeserializeObject<SW_ADDRESULT>(data);
            if (res != null /* && swdevobj.sequence == res.sequence */)
            {
                if (res.result == "Fail")
                {
                    Debug.WriteLine(res.reason);
                    this.result.Content = "删除失败";
                    return;
                }

                this.result.Content = "删除成功";
                /* 查询index */
                for (int i = 0; i < switchDataModel.SwitchList.Count; i++)
                {
                    if ((switchDataModel.SwitchList[i].index).ToString() == res.index)
                    {
                        switchDataModel.SwitchList.RemoveAt(i);
                    }
                }
            }
        }

        /* 接收到服务器的回应消息，更新软交换设备列表 */
        private void editSwitchDevice(string data)
        {
            /* 比较临时存储的obj的sequence是否一致 */
            SW_ADDRESULT res = JsonConvert.DeserializeObject<SW_ADDRESULT>(data);
            if (res != null && tempobj.sequence == res.sequence)
            {
                if (res.result == "Fail")
                {
                    Debug.WriteLine(res.reason);
                    this.result.Content = "更新失败";
                    return;
                }

                this.result.Content = "更新成功";
                // 在swList中查询对应的item
                for (int i = 0; i < switchDataModel.SwitchList.Count; i++)
                {
                    // 根据index来判断
                    if (switchDataModel.SwitchList[i].index == tempobj.index)
                    {
                        SWDEV dev = switchDataModel.SwitchList[i];
                        // index 不允许更新
                        dev.name = tempobj.name;
                        dev.ip = tempobj.ip;
                        dev.port = tempobj.port;
                        dev.type = tempobj.type;
                    }
                }
            }

        }

        /* 刷新软交换设备列表 */
        private void freshSwitchDevice(string data)
        {
            /* 一开始隐藏软交换的基本信息 */
            switchview.Visibility = Visibility.Hidden;

            switchDataModel.SwitchList.Clear();
            SW_QUERYRESULT res = JsonConvert.DeserializeObject<SW_QUERYRESULT>(data);
            if (res != null)
            {
                ObservableCollection<SWDEV> list = new ObservableCollection<SWDEV>();
                foreach (SWDEVITEM member in res.switchlist)
                {
                    SWDEV item = new SWDEV();
                    //item.idstr = member.name;
                    item.name = member.name;
                    item.index = member.index;
                    item.type = member.type;
                    item.ip = member.ip;
                    item.port = member.port;

                    switchDataModel.SwitchList.Add(item);
                }
            }
        }

        #endregion

        public void delSWReq(string index)
        {
            /* 向服务器请求列表 */
            SWQUERY swquery = new SWQUERY();
            swquery.sequence = GlobalFunAndVar.sequenceGenerator();
            swquery.index = index;
            StringBuilder sb = new StringBuilder(100);
            sb.Append("MAN#DELSW#");
            sb.Append(JsonConvert.SerializeObject(swquery));

            Debug.WriteLine("SEND: " + sb.ToString());
            mainWindow.ws.Send(sb.ToString());
        }

        /* 添加用户 */
        private void add_Click(object sender, RoutedEventArgs e)
        {
            switchDataModel.SelectedSwitch = new SWDEV();
            switchDataModel.SelectedSwitch.name = "新设备";
            switchDataModel.NewSwitch = true;

            switchview.Visibility = Visibility.Visible;
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            SWDEV swit = switchDataModel.SelectedSwitch;
            if (MessageBox.Show("确定是否要删除设备 " + swit.name, "提示消息",
                MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Debug.WriteLine("delete switch: " + swit.name);
                delSWReq(swit.index);
            }
        }

        private void switch_Click_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            if (tvi.Header is SWDEV)
            {
                var modelkey = tvi.Header as SWDEV;
                // 赋值给UI的临时对象，用于显示
                switchDataModel.SelectedSwitch = modelkey;
                /* 表示是否是新建用户 */
                switchDataModel.NewSwitch = false;
            }

            if (switchDataModel.SelectedSwitch != null)
                switchview.Visibility = Visibility.Visible;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                /* 用于数据库协议交互 */
                Int16 index = Convert.ToInt16(btn.Tag);
                Debug.WriteLine("update index: " + btn.Tag);

                SWDEV item = new SWDEV();
                item.sequence = GlobalFunAndVar.sequenceGenerator();                
                item.name = swName.Text.Trim();
                item.ip = swIP.Text.Trim();
                item.port = swPort.Text.Trim();
                item.type = swType.Text.Trim();

                // 保存临时User变量
                tempobj = item;

                /* 向服务器发送更新消息 */
                StringBuilder sb = new StringBuilder(100);
                if (true == switchDataModel.NewSwitch)
                {
                    sb.Append("MAN#ADDSW#");
                }
                else
                {
                    item.index = swIndex.Text.Trim();
                    sb.Append("MAN#EDITSW#");
                }
                sb.Append(JsonConvert.SerializeObject(item));
                Debug.WriteLine("SEND: " + sb.ToString());
                mainWindow.ws.Send(sb.ToString());
            }
        }
    }
}
