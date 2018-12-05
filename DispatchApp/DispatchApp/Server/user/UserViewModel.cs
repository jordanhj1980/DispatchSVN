using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DispatchApp
{
    class UserViewModel : NotifyObject
    {
        private bool _newUser;
        public bool NewUser
        {
            get { return _newUser; }
            set {
                _newUser = value; 
                OnPropertyChanged("NewUser");
            }
        }

        /* 管理用户的列表 */
        private ObservableCollection<User> _userlist;
        public ObservableCollection<User> UserList
        {
            get { return _userlist; }
            set
            {
                SetAndNotifyIfChanged("UserList", ref _userlist, value);
            }
        }

        public UserViewModel() 
        {
            _userlist = new ObservableCollection<User>();
        }

        /// <summary>
        /// function 列表中当前选中用户
        /// </summary>
        // 当对话框打开添加软交换时，临时存储软交换的信息

        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged("SelectedUser");
                }
            }
        }

        

    }
}
