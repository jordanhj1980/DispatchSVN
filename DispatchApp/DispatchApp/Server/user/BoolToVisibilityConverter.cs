﻿using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DispatchApp
{
    public class NegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is bool ? !(bool)value : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }


    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public BoolToVisibilityConverter()
            : this(true)
        {

        }
        public BoolToVisibilityConverter(bool collapsewhenInvisible)
            : base()
        {
            CollapseWhenInvisible = collapsewhenInvisible;
        }
        public bool CollapseWhenInvisible { get; set; }

        public Visibility FalseVisible
        {
            get
            {
                if (CollapseWhenInvisible)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }

        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Visible;
            return (bool)value ? Visibility.Visible : FalseVisible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return true;
            return ((Visibility)value == Visibility.Visible);
        }
    }
}
