﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;


namespace DispatchApp
{
    public class OutLineViewModel : NotifyObject
    {
        public OutLine outLine;

        /// <summary>
        /// 外线数据绑定参数，包括键权电话，外线电话和中继电话
        /// </summary>
        private OutLineCall _outLineCall;
        public OutLineCall outLineCall
        {
            get { return _outLineCall; }
            set
            {
                if (_outLineCall != value)
                {
                    _outLineCall = value;
                    OnPropertyChanged("outLineCall");
                }
            }
        }

        /// <summary>
        /// 呼叫按钮
        /// </summary>
        private string _callBtnContent = "呼叫";
        public string callBtnContent
        {
            get { return _callBtnContent; }
            set
            {
                if (_callBtnContent != value)
                {
                    _callBtnContent = value;
                    OnPropertyChanged("callBtnContent");
                }
            }
        }

        /// <summary>
        /// 通话记录全部信息
        /// </summary>
        private ObservableCollection<CallLog> _callLogList;
        public ObservableCollection<CallLog> callLogList
        {
            get { return _callLogList; }
            set
            {
                if (_callLogList != value)
                {
                    _callLogList = value;
                    OnPropertyChanged("callLogList");
                }
            }
        }

        /// <summary>
        /// 选中的通话记录
        /// </summary>
        private CallLog _callLogSelect;
        public CallLog callLogSelect
        {
            get { return _callLogSelect; }
            set
            {
                if (_callLogSelect != value)
                {
                    _callLogSelect = value;
                    OnPropertyChanged("callLogSelect");
                }
            }
        }


        public OutLineViewModel(OutLine outline)
        {
            outLineCall = new OutLineCall();
            callLogList = new ObservableCollection<CallLog>();
            callLogSelect = new CallLog();
            this.outLine = outline;
        }

        /// <summary>
        /// 呼叫按键的绑定命令
        /// </summary>
        private MyCommand _paramCommand;
        public MyCommand ParamCommand
        {
            get
            {
                if (_paramCommand == null)
                    _paramCommand = new MyCommand(
                        new Action<object>(
                            o => BtnCallCommand(o)),//o => MessageBox.Show(o.ToString())),
                        new Func<object, bool>(
                            o => !string.IsNullOrEmpty(o.ToString())));
                return _paramCommand;
            }
        }
        private void BtnCallCommand(object o)
        {
            switch (callBtnContent)
            {
                case "呼叫":
                    outLine.deskTabControl.SelectedIndex = 1;           // 跳转到中继电话界面
                    //((TabItem)(outLine.deskTabControl.Items[0])).Visibility = Visibility.Collapsed;
                    //((TabItem)(outLine.deskTabControl.Items[2])).Visibility = Visibility.Collapsed;
                    callBtnContent = "结束";
                    CallLog callLogNew = new CallLog();
                    callLogNew.num = outLineCall.outLineNum;
                    callLogList.Add(callLogNew);    // 新加拨号记录
                    break;
                case "结束":
                    callBtnContent = "呼叫";
                    outLineCall.outLineNum = "";
                    outLine.BtnCall.Content = callBtnContent;
                    break;
                default:
                    break;
            }
            


        }

    }

    
}
