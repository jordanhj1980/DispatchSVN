using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DispatchApp
{
    class ContactViewModel: NotifyObject
    {
        public int selectedIndex;


        private bool _newContact;
        public bool NewContact
        {
            get { return _newContact; }
            set
            {
                _newContact = value;
                OnPropertyChanged("NewContact");
            }
        }

        /* 管理软交换设备列表 */
        private ObservableCollection<Department> _contactList;
        public ObservableCollection<Department> ContactList
        {
            get { return _contactList; }
            set
            {
                SetAndNotifyIfChanged("ContactList", ref _contactList, value);
            }
        }

        public ContactViewModel() 
        {
            _contactList = new ObservableCollection<Department>();
            _gridMemList = new ObservableCollection<PhoneItem>();
            selectedIndex = -1;
        }

        /* 临时保存电话簿分组中的电话列表 */
        private ObservableCollection<PhoneItem> _gridMemList;
        public ObservableCollection<PhoneItem> gridMemList
        {
            get { return _gridMemList; }
            set
            {
                if (_gridMemList != value)
                {
                    _gridMemList = value;
                    OnPropertyChanged("gridMemList");
                }
            }
        }

        /// <summary>
        /// function 列表中当前选中用户
        /// </summary>
        // 当对话框打开添加软交换时，临时存储软交换的信息

        private Department _selectedContact;
        public Department SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                if (_selectedContact != value)
                {
                    _selectedContact = value;
                    OnPropertyChanged("SelectedContact");

                    List<PhoneItem> memlist = ObjectCopier.Clone<List<PhoneItem>>(_selectedContact.memberlist);
                    if (memlist == null)
                    {
                        memlist = new List<PhoneItem>();
                    }
                    gridMemList = new ObservableCollection<PhoneItem>(memlist);
                }
            }
        }


    }
}
