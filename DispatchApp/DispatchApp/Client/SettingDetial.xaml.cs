﻿using System;
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

namespace DispatchApp
{
    /// <summary>
    /// SettingDetail.xaml 的交互逻辑
    /// </summary>
    public partial class SettingDetail : UserControl
    {
        public MainWindow mainWindow;
        public SettingDetailViewModel settingDetailViewModel; 
        public SettingDetail(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            settingDetailViewModel = new SettingDetailViewModel();
            this.DataContext = settingDetailViewModel;
        }
    }
}
