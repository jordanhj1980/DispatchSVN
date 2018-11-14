using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

// add by twinkle 20181113
namespace DispatchApp
{
    /* 用于数据源修改后，通知界面同步变化 */
    public class NotifyObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /* propertyName为属性的名称 */
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                //假设属性发生了改变，则触发这个事件
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void SetAndNotifyIfChanged<T>(string propertyName, ref T oldValue, T newValue)
        {
            if (oldValue == null && newValue == null) return;
            if (oldValue != null && oldValue.Equals(newValue)) return;
            if (newValue != null && newValue.Equals(oldValue)) return;
            oldValue = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}
