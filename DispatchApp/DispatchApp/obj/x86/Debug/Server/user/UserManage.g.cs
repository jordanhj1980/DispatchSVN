﻿#pragma checksum "..\..\..\..\..\Server\user\UserManage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0C1FF158E27563DC4933ACE66EB1A365D9DC7757"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using DispatchApp;
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
    /// UserManage
    /// </summary>
    public partial class UserManage : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 80 "..\..\..\..\..\Server\user\UserManage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Card usertreeview;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\..\..\Server\user\UserManage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button adduser;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\..\Server\user\UserManage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button deluser;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\..\Server\user\UserManage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView userlist;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\..\..\Server\user\UserManage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Card userview;
        
        #line default
        #line hidden
        
        
        #line 131 "..\..\..\..\..\Server\user\UserManage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox userName;
        
        #line default
        #line hidden
        
        
        #line 136 "..\..\..\..\..\Server\user\UserManage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox userPass;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\..\..\..\Server\user\UserManage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox userPriv;
        
        #line default
        #line hidden
        
        
        #line 163 "..\..\..\..\..\Server\user\UserManage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox userDesk;
        
        #line default
        #line hidden
        
        
        #line 189 "..\..\..\..\..\Server\user\UserManage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label result;
        
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
            System.Uri resourceLocater = new System.Uri("/DispatchApp;component/server/user/usermanage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Server\user\UserManage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.usertreeview = ((MaterialDesignThemes.Wpf.Card)(target));
            return;
            case 2:
            this.adduser = ((System.Windows.Controls.Button)(target));
            
            #line 85 "..\..\..\..\..\Server\user\UserManage.xaml"
            this.adduser.Click += new System.Windows.RoutedEventHandler(this.add_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.deluser = ((System.Windows.Controls.Button)(target));
            
            #line 86 "..\..\..\..\..\Server\user\UserManage.xaml"
            this.deluser.Click += new System.Windows.RoutedEventHandler(this.del_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.userlist = ((System.Windows.Controls.TreeView)(target));
            
            #line 101 "..\..\..\..\..\Server\user\UserManage.xaml"
            this.userlist.Loaded += new System.Windows.RoutedEventHandler(this.treelist_loaded);
            
            #line default
            #line hidden
            
            #line 101 "..\..\..\..\..\Server\user\UserManage.xaml"
            this.userlist.AddHandler(System.Windows.Controls.TreeViewItem.SelectedEvent, new System.Windows.RoutedEventHandler(this.user_Click_Selected));
            
            #line default
            #line hidden
            return;
            case 5:
            this.userview = ((MaterialDesignThemes.Wpf.Card)(target));
            return;
            case 6:
            this.userName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.userPass = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.userPriv = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 9:
            this.userDesk = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 10:
            
            #line 188 "..\..\..\..\..\Server\user\UserManage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Update_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.result = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

