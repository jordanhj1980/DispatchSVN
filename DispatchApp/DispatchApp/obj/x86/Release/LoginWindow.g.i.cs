﻿#pragma checksum "..\..\..\LoginWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "CA1C108ADB8632D4876F4395B2DFBEFA036BF415"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace DispatchApp {
    
    
    /// <summary>
    /// LoginWindow
    /// </summary>
    public partial class LoginWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 38 "..\..\..\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Card loginBox;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxUserName;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox TxPassword;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Snackbar SnackbarOne;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_adv;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image image_adv;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel serialBox;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox serial;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel advBox;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\LoginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox IPAddr;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DispatchApp;component/loginwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\LoginWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\..\LoginWindow.xaml"
            ((DispatchApp.LoginWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Form_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.loginBox = ((MaterialDesignThemes.Wpf.Card)(target));
            return;
            case 3:
            this.TxUserName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.TxPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 61 "..\..\..\LoginWindow.xaml"
            this.TxPassword.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.SelectAllText);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 71 "..\..\..\LoginWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtLogin_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 79 "..\..\..\LoginWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtExit_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.SnackbarOne = ((MaterialDesignThemes.Wpf.Snackbar)(target));
            return;
            case 8:
            this.btn_adv = ((System.Windows.Controls.Button)(target));
            
            #line 90 "..\..\..\LoginWindow.xaml"
            this.btn_adv.Click += new System.Windows.RoutedEventHandler(this.BtAdvance_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.image_adv = ((System.Windows.Controls.Image)(target));
            return;
            case 10:
            this.serialBox = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 11:
            this.serial = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            
            #line 98 "..\..\..\LoginWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.register_click);
            
            #line default
            #line hidden
            return;
            case 13:
            this.advBox = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 14:
            this.IPAddr = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

