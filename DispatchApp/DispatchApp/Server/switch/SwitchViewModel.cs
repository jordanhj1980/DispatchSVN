﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DispatchApp
{
    class SwitchViewModel : NotifyObject
    {
        private bool _newSwitch;
        public bool NewSwitch
        {
            get { return _newSwitch; }
            set
            {
                _newSwitch = value;
                OnPropertyChanged("NewSwitch");
            }
        }

        /* 管理软交换设备列表 */
        private ObservableCollection<SWDEV> _switchlist;
        public ObservableCollection<SWDEV> SwitchList
        {
            get { return _switchlist; }
            set
            {
                SetAndNotifyIfChanged("SwitchList", ref _switchlist, value);
            }
        }

        public SwitchViewModel() 
        {
            _switchlist = new ObservableCollection<SWDEV>();
        }

        /// <summary>
        /// function 列表中当前选中用户
        /// </summary>
        // 当对话框打开添加软交换时，临时存储软交换的信息

        private SWDEV _selectedSwitch;
        public SWDEV SelectedSwitch
        {
            get { return _selectedSwitch; }
            set
            {
                if (_selectedSwitch != value)
                {
                    _selectedSwitch = value;
                    OnPropertyChanged("SelectedSwitch");
                }
            }
        }      


    }
}
