using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace DispatchApp
{
    public class ListViewDialogViewModel : NotifyObject
    {

        private ObservableCollection<ExtDevice> _alldevlist;
        public ObservableCollection<ExtDevice> AllDevList
        {

            get { return _alldevlist; }
            set
            {
                SetAndNotifyIfChanged("AllDevList", ref _alldevlist, value);
            }
        }

        public ListViewDialogViewModel()
        {
            AllDevList = new ObservableCollection<ExtDevice>();
        }
    }
}
