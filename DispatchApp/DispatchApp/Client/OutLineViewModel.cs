using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Newtonsoft.Json;


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

        public TreeViewItem selectedTreeItem    { get;set; }

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

        /* 电话簿 */
        private ObservableCollection<Department> _contactlist;
        public ObservableCollection<Department> ContactList
        {
            get { return _contactlist; }
            set
            {
                if (_contactlist != value)
                {
                    _contactlist = value;
                    OnPropertyChanged("ContactList");
                }
            }
        }


        public OutLineViewModel(OutLine outline)
        {
            outLineCall = new OutLineCall();
            callLogList = new ObservableCollection<CallLog>();
            callLogSelect = new CallLog();
            this.outLine = outline;


            /* 初始化电话簿 */
            ContactList = new ObservableCollection<Department>();
            Department dept = new Department();
            dept.department = "group1";
            PhoneItem item = new PhoneItem();
            item.callno = "12";
            item.name = "123";

            List<PhoneItem> mlist = new List<PhoneItem>();
            mlist.Add(item);

            dept.memberlist = mlist;
            ContactList.Add(dept);

            // item 2
            dept = new Department();
            dept.department = "group2";
            item = new PhoneItem();
            item.callno = "21";
            item.name = "222";

            mlist = new List<PhoneItem>();
            mlist.Add(item);
            dept.memberlist = mlist;
            ContactList.Add(dept);

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
                    List<ToggleButton> toggleButtonList = FindChirldHelper.FindVisualChild<ToggleButton>(outLine.RelayList);
                    foreach (ToggleButton Item in toggleButtonList)
                    {
                        Item.IsChecked = false;
                    }
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

                    // 挂键权
                    call callNum = new call();
                    callNum.fromid = outLine.mainWindow.callUserCtrl.serverCall;
                    callNum.toid = outLine.mainWindow.callUserCtrl.serverCall;
                    string strMsg = "CMD#Clear#" + JsonConvert.SerializeObject(callNum);
                    outLine.mainWindow.ws.Send(strMsg);
                    outLine.mainWindow.callUserCtrl.operaState = e_OperaState.NULL;
                    outLine.mainWindow.callUserCtrl.FunKeysBorderBrush("");
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 遍历容器内控件
        /// </summary>
        public static class FindChirldHelper
        {
            public static List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
            {
                try
                {
                    List<T> TList = new List<T> { };
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                        if (child != null && child is T)
                        {
                            TList.Add((T)child);
                            List<T> childOfChildren = FindVisualChild<T>(child);
                            if (childOfChildren != null)
                            {
                                TList.AddRange(childOfChildren);
                            }
                        }
                        else
                        {
                            List<T> childOfChildren = FindVisualChild<T>(child);
                            if (childOfChildren != null)
                            {
                                TList.AddRange(childOfChildren);
                            }
                        }
                    }
                    return TList;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message + "a");
                    return null;
                }
            }
        }

    }

    
}
