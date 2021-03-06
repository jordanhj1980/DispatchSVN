﻿using System;
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

        /* 调度员的列表 */
        private ObservableCollection<User> _userlist;
        public ObservableCollection<User> UserList
        {
            get { return _userlist; }
            set
            {
                SetAndNotifyIfChanged("UserList", ref _userlist, value);
            }
        }

        /* 管理员的列表 */
        private ObservableCollection<User> _adminlist;
        public ObservableCollection<User> AdminList
        {
            get { return _adminlist; }
            set
            {
                SetAndNotifyIfChanged("AdminList", ref _adminlist, value);
            }
        }

        private ObservableCollection<AllUser> _allUser;
        public ObservableCollection<AllUser> allUser
        {
            get { return _allUser; }
            set
            {
                SetAndNotifyIfChanged("allUser", ref _allUser, value);
            }
        }

        public UserViewModel() 
        {
            _userlist = new ObservableCollection<User>();
            _adminlist = new ObservableCollection<User>();
            _deskList = new ObservableCollection<UserStatus>();
            _allUser = new ObservableCollection<AllUser>();


            AllUser item = new AllUser();
            item.Header = "管理员";
            item.UserItem = new ObservableCollection<User>();
            _allUser.Add(item);
            AllUser item2 = new AllUser();
            item2.Header = "调度用户";
            item2.UserItem = new ObservableCollection<User>();
            _allUser.Add(item2);



            _privCandidateList = new List<UserStatus>();
            /* 初始化权限下拉列表 */
            UserStatus stat = new UserStatus("2", "键盘调度员");
            _privCandidateList.Add(stat);
            stat = new UserStatus("1", "管理员");
            _privCandidateList.Add(stat);


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

        private List<UserStatus> _privCandidateList;
        public List<UserStatus> privCandidateList
        {
            get { return _privCandidateList; }
            set
            {
                SetAndNotifyIfChanged("privCandidateList", ref _privCandidateList, value);
            }
        }

        // 添加调度台信息  add by twinkle 20181122 
        private ObservableCollection<UserStatus> _deskList;
        public ObservableCollection<UserStatus> deskList
        {
            get { return _deskList; }
            set
            {
                SetAndNotifyIfChanged("deskList", ref _deskList, value);
            }
        }

    }
}
