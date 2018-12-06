﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DispatchApp
{

    public class UserLog : NotifyObject
    {
        private string _time;
        public string time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged("time");
                }
            }
        }
        private string _actiontype;
        public string actiontype
        {
            get { return _actiontype; }
            set
            {
                if (_actiontype != value)
                {
                    _actiontype = value;
                    OnPropertyChanged("actiontype");
                }
            }
        }
    }
}
