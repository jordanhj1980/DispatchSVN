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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;   // animation

using Newtonsoft.Json;
using System.Diagnostics;

namespace DispatchApp
{
    public partial class CallUserControl
    {
        private bool isHolding = false;

        private void btn_holdoff_click(object sender, RoutedEventArgs e)
        {
            /* 如果当前处于呼叫保持状态，按保持按钮后，将回到之前的回话 */
            if (isHolding)
            {
                // 发送解保持命令
                Operation_unhold(sender, e);
                isHolding = false;
            }

        }

        public void Operation_hold(string extid, object sender)
        {
            /* extid为当前选择的hold电话 */
            StringBuilder sb = new StringBuilder(100);

            sb.Append("CMD#Hold#");
            sb.Append(extid);

            string strMsg = sb.ToString();
            /* 发送网络消息 */
            mainWindow.ws.Send(strMsg);

            Debug.WriteLine("send Hold Command");
            Debug.WriteLine(strMsg);

            /* 缩小显示键权按钮 */
            btn_shrink();
            isHolding = true;
        }


        /* 发送当前键权电话的Unhold */
        /* 1、第三方不同意接听；
         * 2、第三方接听超时 */
        public void Operation_unhold(object sender, RoutedEventArgs e)
        {
            /* extid为当前选择的键权电话 */
            string extid = serverCall;
            if (!isHolding)
            {
                return;
            }
            isHolding = false;

            StringBuilder sb = new StringBuilder(100);
            sb.Append("CMD#Unhold#");
            sb.Append(extid);

            string strMsg = sb.ToString();
            /* 发送网络消息 */
            mainWindow.ws.Send(strMsg);

            Debug.WriteLine("send Unhold Command");
            Debug.WriteLine(strMsg);

            btn_expand();
            //btn_holdoff.Visibility = Visibility.Hidden;
            isHolding = false;
        }

        private void btn_shrink()
        {
            /* 当前的挂断按钮缩小动画 */
            DoubleAnimation widthAnimation = new DoubleAnimation()
            {
                To = btn_key.Width - 30,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            btn_key.BeginAnimation(Button.WidthProperty, widthAnimation);
            //btn_holdoff.Visibility = Visibility.Visible;            
        }

        private void btn_expand()
        {
            /* 当前的挂断按钮放大动画 */
            DoubleAnimation widthAnimation = new DoubleAnimation()
            {
                To = btn_key.Width + 30,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            btn_key.BeginAnimation(Button.WidthProperty, widthAnimation);

            /* 隐藏保持按钮 */
            //btn_holdoff.Visibility = Visibility.Hidden;
        }

        private void btn_lbqueue_click(object sender, RoutedEventArgs e)
        {
            string extid = m_keyphone[m_keyIndex].extid;
            if (m_keyphone[m_keyIndex].Status == KeyStatus.ESTABLISHED ||
                m_keyphone[m_keyIndex].Status == KeyStatus.CALLING) 
            {
                MessageBox.Show("当前键权电话正忙", "温馨提示");
                return;
            }

            /* 如果有号码过来，接听后按钮显示为绿色 */
            Button btn = sender as Button;
            btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00FF00"));
            RelativeSource rs = new RelativeSource(RelativeSourceMode.FindAncestor);
            rs.AncestorType = typeof(ListBoxItem);
            string visitorid = btn.Tag as string;

            /* 将当前选择的电话移动到呼叫队列的最上面 */
            //List<CallSession> lst_cs = lbCallQueue.Items;

            for (int j = 0; j < m_callQueue.Count; j++)
            {
                if (m_callQueue[j].visitorid == visitorid)
                {
                    UI_CallSession uics = m_callQueue[j];
                    uics.CurrentState = "ANSWER";
                    m_callQueue.RemoveAt(j);
                    m_callQueue.Insert(0, uics);
                    
                    break;
                }
            }

            /* 显示正在通话 */
            /*string btn_content = btn.Content as string;
            btn_content = "正在通话：" + btn_content;
            btn.Content = btn_content;*/

            /* 发送来电转接命令 */
            /* extid为当前选择的键权电话 */
            call call = new call();
            call.fromid = visitorid;
            call.toid = extid;
            
            StringBuilder sb = new StringBuilder(100);
            sb.Append("CMD#Visitor#");
            sb.Append(JsonConvert.SerializeObject(call));

            string strMsg = sb.ToString();
            /* 发送网络消息 */
            mainWindow.ws.Send(strMsg);

            Debug.WriteLine("send Visitor Command");
            Debug.WriteLine(strMsg);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UI_CallSession cs = new UI_CallSession()
            {
                fromnumber = "18163350377",
                tonumber = "220",
                visitorid = "86",
                CurrentState = "IDLE",
            };
            //lbCallQueue.Items.Add(cs);
            m_callQueue.Add(cs);
        }

    }
}
