using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchApp
{
    public class SettingDetailViewModel : NotifyObject
    {
        private Setting _settingDetail;
        public Setting settingDetail
        {
            get { return _settingDetail; }
            set
            {
                if (_settingDetail != value)
                {
                    _settingDetail = value;
                    OnPropertyChanged("settingDetail");
                }
            }
   
        }

        public SettingDetailViewModel()
        {
            settingDetail = new Setting();
        }

    }
}
