using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchApp
{
    public class GroupDetailViewModel : NotifyObject
    {
        /// <summary>
        /// 分组详细信息编辑
        /// </summary>
        private GroupNew _groupDetail;
        public GroupNew groupDetail
        {
            get { return _groupDetail; }
            set
            {
                if (_groupDetail != value)
                {
                    _groupDetail = value;
                    OnPropertyChanged("groupDetail");
                }
            }
        }


        public GroupDetailViewModel()
        {
            groupDetail = new GroupNew();
        }
    }
}
