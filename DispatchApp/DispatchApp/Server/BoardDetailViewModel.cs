using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchApp
{
    public class BoardDetailViewModel : NotifyObject
    {
         /// <summary>
        /// 分组详细信息编辑
        /// </summary>
        private Broadcast _boardDetail;
        public Broadcast boardDetail
        {
            get { return _boardDetail; }
            set
            {
                if (_boardDetail != value)
                {
                    _boardDetail = value;
                    OnPropertyChanged("boardDetail");
                }
            }
        }


        public BoardDetailViewModel()
        {
            boardDetail = new Broadcast();
        }
    }
}
