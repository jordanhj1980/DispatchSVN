using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchApp
{
    public class TrunkDetailViewModel : NotifyObject
    {
        private TrunkDev _trunkDetail;
        public TrunkDev trunkDetail
        {
            get { return _trunkDetail;}
            set
            {
                if (_trunkDetail != value)
                {
                    _trunkDetail = value;
                    OnPropertyChanged("trunkDetail");
                }
            }
   
        }

        public TrunkDetailViewModel()
        {
            trunkDetail = new TrunkDev();
        }

    }
}
