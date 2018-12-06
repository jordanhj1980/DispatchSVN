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
    /// ContactManage.xaml 的交互逻辑
    /// </summary>
    public partial class ContactManage : UserControl
    {
        private MainWindow mainWindow;
        private ContactViewModel contactDataModel;
        public ContactManage(MainWindow mainwindow)
        {
            InitializeComponent();
            this.mainWindow = mainwindow;
            contactDataModel = new ContactViewModel();
            this.DataContext = contactDataModel;
        }

        #region recv_operation

        public void recv(string state, string data)
        {
            switch (state)
            {
                case "GETPHONEBOOK":    
                    freshContact(data);
                    break;
                case "DELPHONEBOOK":
                    editContact(data, true);
                    
                    break;
                case "ADDPHONEBOOK":
                case "EDITPHONEBOOK":
                    editContact(data);
                    break;
                default:
                    break;
            }
        }

        /* 刷新电话簿列表 */
        private void freshContact(string data)
        {
            contactDataModel.ContactList.Clear();
            PhoneBook pb = JsonConvert.DeserializeObject<PhoneBook>(data);
            if (pb != null)
            {
                foreach (Department member in pb.departmentlist)
                {
                    Department item = new Department();
                    item.department = member.department;
                    item.memberlist = new List<PhoneItem>();
                    foreach (PhoneItem it in member.memberlist) {
                        PhoneItem newItem = new PhoneItem();
                        newItem.callno = it.callno;
                        newItem.name = it.name;
                        item.memberlist.Add(newItem);
                    }

                    contactDataModel.ContactList.Add(item);
                }
            }

        }

        /* 刷新电话簿列表 */
        private void editContact(string data, bool isDel = false)
        {
            //freshContact(data);
            /* 比较临时存储的obj的sequence是否一致 */
            USER_ADDRESULT res = JsonConvert.DeserializeObject<USER_ADDRESULT>(data);
            if (res != null /*&& userobj.sequence == res.sequence*/)
            {
                if (res.result == "Fail")
                {
                    Debug.WriteLine(res.reason);
                    this.result.Content = "保存失败";
                    return;
                }

                /* 显示修改成功 */
                this.result.Content = "保存成功";

                if (isDel)
                {
                    /* 定位到第一个department */
                    TreeViewItem tvi = contactlist.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;
                    tvi.IsExpanded = true;
                    tvi.Focus();
                    //tvi.IsSelected = true;
                }

                /* 发送查询请求 */
                queryContact();
            }
        }

        #endregion

        public void queryContact()
        {
            /* 向服务器请求列表 */
            string sequence = GlobalFunAndVar.sequenceGenerator();
            StringBuilder sb = new StringBuilder(100);

            sb.Append("MAN#GETPHONEBOOK#{\"sequence\":");
            sb.Append(JsonConvert.SerializeObject(sequence));
            sb.Append("}");

            Debug.WriteLine("SEND: " + sb.ToString());
            mainWindow.ws.Send(sb.ToString());
        }  


        /* 添加分组 */
        private void add_Click(object sender, RoutedEventArgs e)
        {
            contactDataModel.SelectedContact = new Department();
            contactDataModel.SelectedContact.department = "新分组";
            contactDataModel.NewContact = true;
            /* 新建一个memlist */
            //contactDataModel.gridMemList = new ObservableCollection<PhoneItem>();
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            Department dep = contactDataModel.SelectedContact;
            if (MessageBox.Show("确定是否要删除分组 " + dep.department, "提示消息",
                MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Debug.WriteLine("delete user: " + dep.department);
                delReq(dep.department);
            }
        }

        public void delReq(string name)
        {
            /* 向服务器请求列表 */
            PhoneBook req = new PhoneBook();
            req.sequence = GlobalFunAndVar.sequenceGenerator();
            req.departmentlist = contactDataModel.ContactList.ToList<Department>();
            /* 从列表中删除指定partment */
            for (int i = 0; i < req.departmentlist.Count; i++)
            {
                if (req.departmentlist[i].department == name)
                {
                    req.departmentlist.RemoveAt(i);
                    break;
                }
            }

            StringBuilder sb = new StringBuilder(1000);
            sb.Append("MAN#DELPHONEBOOK#");
            sb.Append(JsonConvert.SerializeObject(req));

            Debug.WriteLine("SEND: " + sb.ToString());
            //sendMsg(this, "net", sb.ToString());
            mainWindow.ws.Send(sb.ToString());

            /* 临时保存用户名 */
            //userobj.name = name;
        }

        private void contact_Click_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            if (tvi.Header is Department)
            {
                var modelkey = tvi.Header as Department;
                // 赋值给UI的临时对象，用于显示
                contactDataModel.SelectedContact = modelkey;
                /* 表示是否是新建用户 */
                contactDataModel.NewContact = false;

                /* 定位当前的索引号 */
                int i = 0;
                foreach (Department mem in contactDataModel.ContactList) {
                    if (modelkey == mem) {
                        contactDataModel.selectedIndex = i;
                        break;
                    }
                    i++;
                }

                
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string btnName = btn.Name;

                /* 用于数据库协议交互 */
                PhoneBook pb = new PhoneBook();
                pb.sequence = GlobalFunAndVar.sequenceGenerator();
                List<Department> list = contactDataModel.ContactList.ToList<Department>();
                pb.departmentlist = ObjectCopier.Clone<List<Department>>(list);

                if (true == contactDataModel.NewContact)
                {
                    /* 新增分组 当前的SelectedContact不是源*/
                    contactDataModel.SelectedContact.department = deptName.Text.Trim();
                    // 添加gridMemList到SelectedContact
                    List<PhoneItem> tempList = contactDataModel.gridMemList.ToList();
                    List<PhoneItem> memlist = new List<PhoneItem>();
                    foreach (PhoneItem item in tempList)
                    {
                        if (item.callno == null || item.name == null) 
                        {
                            continue;
                        }
                        else if (item.callno.Trim() != "" && item.name.Trim() != "")
                        {
                            memlist.Add(item);
                        }                        
                    }
                    contactDataModel.SelectedContact.memberlist = memlist;
                    pb.departmentlist.Add(contactDataModel.SelectedContact);

                } else {
                    /* 更改当前的分组 */
                    int index = contactDataModel.selectedIndex;
                    if (index >= 0 && index < contactDataModel.ContactList.Count)
                    {
                        pb.departmentlist[contactDataModel.selectedIndex].department = this.deptName.Text;

                        List<PhoneItem> tempList = contactDataModel.gridMemList.ToList();
                        pb.departmentlist[contactDataModel.selectedIndex].memberlist.Clear();

                        foreach (PhoneItem item in tempList) {
                            if (item.callno.Trim() != "" && item.name.Trim() != "" ) 
                            {
                                pb.departmentlist[contactDataModel.selectedIndex].memberlist.Add(item);
                            }
                        }
                    }
                }

                /* 向服务器发送删除消息 */
                StringBuilder sb = new StringBuilder(100);
                if (btnName == "delbtn")
                {
                    sb.Append("MAN#DELPHONEBOOK#");
                }
                else
                {
                    sb.Append("MAN#EDITPHONEBOOK#");
                }
                sb.Append(JsonConvert.SerializeObject(pb));

                Debug.WriteLine("SEND: " + sb.ToString());
                //sendMsg(this, "net", sb.ToString());
                //sendMsg(this, "user", uiUser);
                mainWindow.ws.Send(sb.ToString());

                this.result.Content = "未选中分组";
            }
        }

        /* 添加用户列表 */
        private void addMem_Click(object sender, RoutedEventArgs e)
        {
            PhoneItem item = new PhoneItem();
            contactDataModel.gridMemList.Add(item);
        }

        /* 删除用户列表 */
        private void delMem_Click(object sender, RoutedEventArgs e)
        {
            int index = gridMem.SelectedIndex;
            if (index < 0 || index >= contactDataModel.gridMemList.Count)
            {
                return;
            }
            contactDataModel.gridMemList.RemoveAt(index);


        }

    }

}
