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

using Newtonsoft.Json;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls.Primitives;

using System.Diagnostics;

namespace DispatchApp
{
    /// <summary>
    /// OutLine.xaml 的交互逻辑
    /// </summary>
    public partial class OutLine : UserControl
    {
        
        public MainWindow mainWindow;
        public OutLineViewModel outLineViewModel;

        public OutLine(MainWindow mainWindow)
        {
            InitializeComponent();
            outLineViewModel = new OutLineViewModel(this);
            this.DataContext = outLineViewModel;
            this.mainWindow = mainWindow;
        }

        /// <summary>
        /// 号码按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CallAddText(object sender, RoutedEventArgs e)
        {
            Button item = sender as Button;
            outLineViewModel.outLineCall.outLineNum += item.Content;
        }


        private void CallClear(object sender, RoutedEventArgs e)
        {
            outLineViewModel.outLineCall.outLineNum = "";
            //CallText.Text = "";
        }

        private void CallBackspace(object sender, RoutedEventArgs e)
        {
            if (CallText.Text.Length > 0)
            {
                int length =  outLineViewModel.outLineCall.outLineNum.Length - 1;
                outLineViewModel.outLineCall.outLineNum = outLineViewModel.outLineCall.outLineNum.Substring(0, length);
                //CallText.Text = CallText.Text.Substring(0, CallText.Text.Length - 1);
            }
            else
            {
                outLineViewModel.outLineCall.outLineNum = "";
            }
        }

        private void CallAdd(object sender, RoutedEventArgs e)
        {
            //CallText.Text = CallText.Text + "+";
            outLineViewModel.outLineCall.outLineNum += "+";
        }

        private void ClossBoard(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 中继呼叫布置电话，查初始状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RelayCallKeyBoard(object sender, RoutedEventArgs e)
        {
            //if ("0" == outLineViewModel.outLineCall.serverNum)
            //{
            //    MessageBox.Show("当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "呼叫信息");
            //}
            //else
            //{
            //    if (("" == outLineViewModel.outLineCall.outLineNum) || (null == outLineViewModel.outLineCall.outLineNum))
            //    {
            //        MessageBox.Show("当前呼叫电话空\r\n请输入呼叫电话！", "呼叫信息");
            //    }
            //    else
            //    {
            //        deskTabControl.SelectedIndex = 1;

            //        ((TabItem)(deskTabControl.Items[0])).Visibility = Visibility.Visible;
            //        ((TabItem)(deskTabControl.Items[1])).Visibility = Visibility.Visible;
            //    }
            //}
        }

        /// <summary>
        /// 单击号码事件
        /// </summary>
        /// 
        private void RelayClick(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleBtn = sender as ToggleButton;
            if (toggleBtn.IsChecked == true)
            {
                callRel tellCall = new callRel() { fromid = outLineViewModel.outLineCall.serverNum, toid = outLineViewModel.outLineCall.outLineNum, trunkid = (toggleBtn.Content).ToString() };
                string strMsg = "CMD#CallOut#" + JsonConvert.SerializeObject(tellCall);
                mainWindow.ws.Send(strMsg);
                deskTabControl.SelectedIndex = 0;
                outLineViewModel.callBtnContent = "结束";
                BtnCall.Content = outLineViewModel.callBtnContent;
            }
            else 
            {
                Debug.WriteLine("此处应该强拆通话");
            }

            // 仅允许选中一个toggleBtn
            foreach (RelayNum item in outLineViewModel.outLineCall.relayNumList)
            {
                if (item.relayNum.ToString() != toggleBtn.Content.ToString())
                {
                    item.isSelected = false;
                }
            }
            
        }

        private void callLogSelected(object sender, SelectionChangedEventArgs e)
        {
            deskTabControl.SelectedIndex = 0;
            outLineViewModel.outLineCall.outLineNum = outLineViewModel.callLogSelect.num;
            outLineViewModel.callBtnContent = "呼叫";
            //BtnCall.Content = outLineViewModel.callBtnContent;
        }

        private void contact_Click_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            outLineViewModel.selectedTreeItem = tvi;
        }

        private void preMouseUp(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem tvi = outLineViewModel.selectedTreeItem;
            if (tvi == null)
            {
                return;
            }

            if (tvi.Header is Department)
            {
                tvi.IsExpanded = !tvi.IsExpanded;
            }
        }

        private void callout(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem tvi = outLineViewModel.selectedTreeItem;
            if (tvi == null)
            {
                return;
            }

            PhoneItem phone = tvi.Header as PhoneItem;
            if (phone != null)
            {
                // 跳转到拨号界面
                deskTabControl.SelectedIndex = 0;
                outLineViewModel.outLineCall.outLineNum = phone.callno;

                callRel tellCall = new callRel() { fromid = outLineViewModel.outLineCall.serverNum, toid = phone.callno, trunkid = "" };
                string strMsg = "CMD#CallOut#" + JsonConvert.SerializeObject(tellCall);
                mainWindow.ws.Send(strMsg);
                outLineViewModel.callBtnContent = "结束";
                //BtnCall.Content = "结束";
            }
        }
    }
}
