using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DispatchApp
{
    public class StateBackgroundDependencyProperty : DependencyObject
    {
        public Brush brush = null;

        /// <summary>
        /// 依赖项属性定义和注册
        /// </summary>
        public static readonly DependencyProperty StateBackgroundProperty = DependencyProperty.Register("StateBackground", typeof(Brush), typeof(StateBackgroundDependencyProperty), new PropertyMetadata((Brush)new BrushConverter().ConvertFromString("#FF0078D7"), StateBackgroundChange));//#EEEEF2
        
        /// <summary>
        /// 参数发生变化时回调函数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void StateBackgroundChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StateBackgroundDependencyProperty dependencyPropertyCalss = d as StateBackgroundDependencyProperty;
            if (dependencyPropertyCalss == null)
            {
                return;
            }

            if (dependencyPropertyCalss.brush != null)
            {
                dependencyPropertyCalss.brush = Brushes.Red;
            }

        }

        /// <summary>
        /// 属性包装器
        /// </summary>
        public Brush StateBackground
        { 
            get { return (Brush)GetValue(StateBackgroundProperty);}
            set{SetValue (StateBackgroundProperty, value);}
        }





         #region BgColor-背景颜色
        /// <summary>
        /// 背景颜色
        /// </summary>
        public Brush BgColor
        {
            get { return (Brush)GetValue(BgColorProperty); }
            set { SetValue(BgColorProperty, value); }
        }

        public static readonly DependencyProperty BgColorProperty = DependencyProperty.Register("BgColor",
            typeof(Brush), typeof(StateBackgroundDependencyProperty), new PropertyMetadata((Brush)new BrushConverter().ConvertFromString("#EEEEF2"), (sender, args) =>
            {
                try
                {
                    BrushConverter brushConverter = new BrushConverter();
                    //(sender as StateBackgroundDependencyProperty).border.Background = (Brush)args.NewValue;
                }
                catch (Exception) { }
            }));

        #endregion


     }
    
}
