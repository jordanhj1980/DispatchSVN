using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace DispatchApp
{
    /// <summary>
    /// CreateDeskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LogWindow : UserControl
    {
        //#region 字段
        ///// <summary>
        ///// 总页数
        ///// </summary>
        //private int _pageTote = 1;
        ///// <summary>
        ///// 触发页面改变事件的参数
        ///// </summary>
        //private PagesChangedArgs _pagesChangedArgs;
        ///// <summary>
        ///// 滑动计时器
        ///// </summary>
        //private DispatcherTimer _scrollTimer;
        ///// <summary>
        ///// 滑动方向（True：右；False：左）
        ///// </summary>
        //private bool _scrollRight;
        ///// <summary>
        ///// 移动次数
        ///// </summary>
        //private int _scrollCount = 0;
        ///// <summary>
        ///// 每次滑动的数据
        ///// </summary>
        //private int offset = 0;
        ///// <summary>
        ///// 页码按钮尺寸
        ///// </summary>
        //private int _buttonSize = 20;
        ///// <summary>
        ///// 计时器间隔（毫秒）
        ///// </summary>
        //private int _timerInterval = 20;
        ///// <summary>
        ///// 是否可以触发当前页依赖属性的回调
        ///// </summary>
        //private bool _isCanTriggerCurrentPageDependencyCellback = true;
        //#endregion

        //#region 自定义事件
        ///// <summary>
        ///// 页码改变事件
        ///// </summary>
        //public event EventHandler<PagesChangedArgs> OnPagesChanged;
        //#endregion

        //#region 依赖属性

        //#region 分页大小
        ///// <summary>
        ///// 分页大小
        ///// </summary>
        //public int PageSize
        //{
        //    get { return (int)GetValue(PageSizeProperty); }
        //    set { SetValue(PageSizeProperty, value); }
        //}
        ///// <summary>
        ///// 分页大小
        ///// </summary>
        //public static readonly DependencyProperty PageSizeProperty =
        //    DependencyProperty.Register("PageSize", typeof(int), typeof(LogWindow), new FrameworkPropertyMetadata(1, new PropertyChangedCallback(
        //        (obj, args) =>
        //        {
        //            try
        //            {
        //                if (obj is LogWindow)
        //                {
        //                    //计算总页数
        //                    ((LogWindow)obj).CalculatePageTote();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        })));
        //#endregion

        //#region 数据总数
        ///// <summary>
        ///// 数据总数
        ///// </summary>
        //public int DataTote
        //{
        //    get { return (int)GetValue(DataToteProperty); }
        //    set { SetValue(DataToteProperty, value); }
        //}
        ///// <summary>
        ///// 数据总数
        ///// </summary>
        //public static readonly DependencyProperty DataToteProperty =
        //    DependencyProperty.Register("DataTote", typeof(int), typeof(LogWindow), new FrameworkPropertyMetadata(1, new PropertyChangedCallback(
        //        (obj, args) =>
        //        {
        //            try
        //            {
        //                if (obj is LogWindow)
        //                {
        //                    //计算总页数
        //                    ((LogWindow)obj).CalculatePageTote();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        })));
        //#endregion

        //#region 当前页码
        ///// <summary>
        ///// 当前页码
        ///// </summary>
        //public int CurrentPage
        //{
        //    get { return (int)GetValue(CurrentPageProperty); }
        //    set { SetValue(CurrentPageProperty, value); }
        //}
        ///// <summary>
        ///// 当前页码
        ///// </summary>
        //public static readonly DependencyProperty CurrentPageProperty =
        //    DependencyProperty.Register("CurrentPage", typeof(int), typeof(LogWindow), new FrameworkPropertyMetadata(1, new PropertyChangedCallback(
        //        (obj, args) =>
        //        {
        //            try
        //            {
        //                if (obj is LogWindow && args.NewValue is int)
        //                {
        //                    //判断是否可以执行以下代码
        //                    if (((LogWindow)obj)._isCanTriggerCurrentPageDependencyCellback == true)
        //                    {
        //                        //设置当前页码
        //                        ((LogWindow)obj).SetCurrentPage(Convert.ToInt32(args.NewValue));
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        })));
        //#endregion

        //#endregion


        #region 日志界面关闭事件
        /// <summary>
        /// 日志界面关闭事件
        /// </summary>
        public event CtrlSwitchHandler clossCDR;
        #endregion

        public LogWindow(CallUserControl callUser)
        {
            InitializeComponent();
        }


        ///// <summary>
        ///// 设置当前页码
        ///// </summary>
        ///// <param name="newPage">页码</param>
        //private void SetCurrentPage(int newPage)
        //{
        //    if (newPage > 0 && newPage <= this._pageTote)
        //    {
        //        //记录旧的页码
        //        int oldPage = this.CurrentPage;
        //        //设置新的页码（由于当前页“CurrentPage”是依赖属性，这里给它复制，不需要出发依赖属性的回调，所以加上了以下的状态标识）
        //        this._isCanTriggerCurrentPageDependencyCellback = false;
        //        this.CurrentPage = newPage;
        //        this._isCanTriggerCurrentPageDependencyCellback = true;
        //        //选中的已经是起始页，则“上一页”按钮不可用，否则可用
        //        if (this.CurrentPage == 1)
        //        {
        //            this.btnBackPage.IsEnabled = false;
        //        }
        //        else
        //        {
        //            this.btnBackPage.IsEnabled = true;
        //        }
        //        //选中的已经是末页，则“下一页”按钮不可用，否则可用
        //        if (this.CurrentPage == this._pageTote)
        //        {
        //            this.btnNextPage.IsEnabled = false;
        //        }
        //        else
        //        {
        //            this.btnNextPage.IsEnabled = true;
        //        }
        //        //创建页码改变参数
        //        this._pagesChangedArgs = new PagesChangedArgs()
        //        {
        //            NewPage = newPage,
        //            OldPage = oldPage,
        //        };
        //        //更新显示中的页码按钮
        //        this.UpdatePageButton();
        //        //设置分页按钮的样式
        //        this.SetPageButtonStyle();
        //        //抛出页码改变事件
        //        //this.OnPagesChanged.Invoke(this, this._pagesChangedArgs);
        //    }
        //}

        ///// <summary>
        ///// 页码改变事件参数
        ///// </summary>
        //public class PagesChangedArgs : EventArgs
        //{
        //    /// <summary>
        //    /// 改变前的页码
        //    /// </summary>
        //    public int OldPage { get; set; }
        //    /// <summary>
        //    /// 改变后的页码
        //    /// </summary>
        //    public int NewPage { get; set; }
        //}



        private void Btn_MakeSure(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Clear(object sender, RoutedEventArgs e)
        {
            //DetailContent.Text = "";
        }

        private void Btn_Save(object sender, RoutedEventArgs e)
        {

        }

        private void ClossCDR(object sender, RoutedEventArgs e)
        {
            if (clossCDR != null)
            {
                clossCDR();
            }
        }

    }
}
