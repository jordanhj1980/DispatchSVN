using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;

using Npgsql;

using System.Windows.Interop;
using System.Runtime.InteropServices;
using WebSocket4Net;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DispatchApp
{
    public partial class MainWindow 
    {
        private void Button_CallKeyBord(object sender, RoutedEventArgs e)
        {
            //CallBoard callBoard = new CallBoard();
            //callBoard.ShowDialog();

            //callBoard.WindowStartupLocation = WindowStartupLocation.Manual;
            //callBoard.Left = 1;
            //callBoard.Top = 3;
            callBoard.ShowDialog();
            callBoard.deskTabControl.SelectedItem = 0;

        }

        //private void CallOne(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "1";
        //    Console.WriteLine("11111");
        //}

        //private void CallTwo(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "2";
        //}

        //private void CallThree(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "3";
        //}

        //private void CallFour(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "4";
        //}

        //private void CallFive(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "5";
        //}

        //private void CallSix(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "6";
        //}

        //private void CallSeven(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "7";
        //}

        //private void CallEight(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "8";
        //}

        //private void CallNine(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "9";
        //}

        //private void CallStar(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "*";
        //}

        //private void CallZero(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "0";
        //}
        //private void CallAlarm(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "#";
        //}

        //private void CallClear(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = "";
        //}

        //private void CallBackspace(object sender, RoutedEventArgs e)
        //{
        //    if (CallText.Text.Length > 0)
        //    {
        //        CallText.Text = CallText.Text.Substring(0, CallText.Text.Length - 1);
        //    }
        //    else
        //    {
        //        CallText.Text = "";
        //    }
        //}

        //private void CallKeyBoard(object sender, RoutedEventArgs e)
        //{
        //    if ("0" == callUserCtrl.serverCall)
        //    {
        //        MessageBox.Show("当前键权电话空\r\n请点击键权电话\r\n或拿起键权电话！", "呼叫信息");
        //    }
        //    else
        //    {
        //        if ((null == callUserCtrl.trunkCall) || ("" == callUserCtrl.trunkCall))
        //        {
        //            MessageBox.Show("当前中继电话空\r\n请输入中继电话！", "呼叫信息");
        //        }
        //        else if ("" == CallText.Text)
        //        {
        //            MessageBox.Show("当前呼叫电话空\r\n请输入呼叫电话！", "呼叫信息");
        //        }
        //        else
        //        {
        //            callRel tellCall = new callRel() { fromid = callUserCtrl.serverCall, toid = CallText.Text, trunkid = callUserCtrl.trunkCall };
        //            string strMsg = "CMD#CallOut#" + JsonConvert.SerializeObject(tellCall);
        //            ws.Send(strMsg);
        //            this.Hide();
        //        }
        //    }


        //}


        //private void CallAdd(object sender, RoutedEventArgs e)
        //{
        //    CallText.Text = CallText.Text + "+";
        //}

        private void ClossBoard(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }


    }
}
