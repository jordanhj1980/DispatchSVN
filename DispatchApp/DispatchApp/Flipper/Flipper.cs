using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace DispatchApp.wpf
{
    /// <summary>
    /// Flipper.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = Plane3DPartName, Type = typeof(Plane3D))]
    [TemplateVisualState(GroupName = TemplateFlipGroupName, Name = TemplateFlippedStateName)]
    [TemplateVisualState(GroupName = TemplateFlipGroupName, Name = TemplateUnflippedStateName)]
    public partial class Flipper : Control
    {
        public static RoutedCommand FlipCommand = new RoutedCommand();

        public const string Plane3DPartName = "PART_Plane3D";
        public const string TemplateFlipGroupName = "FlipStates";
        public const string TemplateFlippedStateName = "Flipped";
        public const string TemplateUnflippedStateName = "Unflipped";

        private Plane3D _plane3D;

        static Flipper()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Flipper), new FrameworkPropertyMetadata(typeof(Flipper)));
        }

        public Flipper()
        {
            CommandBindings.Add(new CommandBinding(FlipCommand, FlipHandler));
        }

        private void FlipHandler(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            SetCurrentValue(IsFlippedProperty, !IsFlipped);
        }

        public static readonly DependencyProperty FrontContentProperty =
            DependencyProperty.Register("FrontContent", typeof(UIElement), typeof(Flipper), new UIPropertyMetadata(null));
        public UIElement FrontContent
        {
            get { return (UIElement)GetValue(FrontContentProperty); }
            set { SetValue(FrontContentProperty, value); }
        }

        public static readonly DependencyProperty BackContentProperty =
            DependencyProperty.Register("BackContent", typeof(UIElement), typeof(Flipper), new UIPropertyMetadata(null));
        public UIElement BackContent
        {
            get { return (UIElement)GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }

        public static readonly DependencyProperty BackContentTemplateProperty = DependencyProperty.Register(
            "BackContentTemplate", typeof(DataTemplate), typeof(Flipper), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate BackContentTemplate
        {
            get { return (DataTemplate)GetValue(BackContentTemplateProperty); }
            set { SetValue(BackContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty BackContentTemplateSelectorProperty = DependencyProperty.Register(
            "BackContentTemplateSelector", typeof(DataTemplateSelector), typeof(Flipper), new PropertyMetadata(default(DataTemplateSelector)));

        public DataTemplateSelector BackContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(BackContentTemplateSelectorProperty); }
            set { SetValue(BackContentTemplateSelectorProperty, value); }
        }

        public static readonly DependencyProperty BackContentStringFormatProperty = DependencyProperty.Register(
            "BackContentStringFormat", typeof(string), typeof(Flipper), new PropertyMetadata(default(string)));

        public string BackContentStringFormat
        {
            get { return (string)GetValue(BackContentStringFormatProperty); }
            set { SetValue(BackContentStringFormatProperty, value); }
        }

        public static readonly DependencyProperty IsFlippedProperty = DependencyProperty.Register(
            "IsFlipped", typeof(bool), typeof(Flipper), new PropertyMetadata(default(bool), IsFlippedPropertyChangedCallback));
        public bool IsFlipped
        {
            get { return (bool)GetValue(IsFlippedProperty); }
            set { SetValue(IsFlippedProperty, value); }
        }
        private static void IsFlippedPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var flipper = (Flipper)dependencyObject;
            flipper.UpdateVisualStates(true);
            flipper.RemeasureDuringFlip();
            // 执行Flipper的事件函数
            OnIsFlippedChanged(flipper, dependencyPropertyChangedEventArgs);
        }
        private static void OnIsFlippedChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Flipper)d;
            var args = new RoutedPropertyChangedEventArgs<bool>(
                    (bool)e.OldValue,
                    (bool)e.NewValue) { RoutedEvent = IsFlippedChangedEvent };
            instance.RaiseEvent(args);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateVisualStates(false);

            _plane3D = GetTemplateChild(Plane3DPartName) as Plane3D;
        } 
        public static readonly RoutedEvent IsFlippedChangedEvent =
            EventManager.RegisterRoutedEvent(
                "IsFlipped",
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<bool>),
                typeof(Flipper));
        public event RoutedPropertyChangedEventHandler<bool> IsFlippedChanged
        {
            add { AddHandler(IsFlippedChangedEvent, value); }
            remove { RemoveHandler(IsFlippedChangedEvent, value); }
        }
        private void RemeasureDuringFlip()
        {
            //not entirely happy hardcoding this, but I have explored other options I am not happy with, and this will do for now
            const int storyboardMs = 400;
            const int granularity = 6;

            var remeasureInterval = new TimeSpan(0, 0, 0, 0, storyboardMs / granularity);
            var refreshCount = 0;
            var plane3D = _plane3D;
            if (plane3D == null) return;

            DispatcherTimer dt = null;
            dt = new DispatcherTimer(remeasureInterval, DispatcherPriority.Normal,
                (sender, args) =>
                {
                    plane3D.InvalidateMeasure();
                    if (refreshCount++ == granularity)
                        dt.Stop();
                }, Dispatcher);
            dt.Start();
        }
        public void UpdateVisualStates(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsFlipped ? TemplateFlippedStateName : TemplateUnflippedStateName,
                useTransitions);
        }



        public static readonly DependencyProperty FrontContentTemplateProperty = DependencyProperty.Register(
            "FrontContentTemplate", typeof(DataTemplate), typeof(Flipper), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate FrontContentTemplate
        {
            get { return (DataTemplate)GetValue(FrontContentTemplateProperty); }
            set { SetValue(FrontContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty FrontContentTemplateSelectorProperty = DependencyProperty.Register(
            "FrontContentTemplateSelector", typeof(DataTemplateSelector), typeof(Flipper), new PropertyMetadata(default(DataTemplateSelector)));

        public DataTemplateSelector FrontContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(FrontContentTemplateSelectorProperty); }
            set { SetValue(FrontContentTemplateSelectorProperty, value); }
        }

        public static readonly DependencyProperty FrontContentStringFormatProperty = DependencyProperty.Register(
            "FrontContentStringFormat", typeof(string), typeof(Flipper), new PropertyMetadata(default(string)));

        public string FrontContentStringFormat
        {
            get { return (string)GetValue(FrontContentStringFormatProperty); }
            set { SetValue(FrontContentStringFormatProperty, value); }
        }
    }

    public class LessThanXToTrueConverter : IValueConverter
    {
        public double X { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value < X;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}