﻿#pragma checksum "..\..\loginWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E18305459C2B32113CE80AA99814EC45B4ECBA81"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Bridge;
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


namespace Bridge {
    
    
    /// <summary>
    /// loginWindow
    /// </summary>
    public partial class loginWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\loginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl ttab;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\loginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbLogin;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\loginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbPassword;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\loginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSend;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\loginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRemind;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\loginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbRegLogin;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\loginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbRegPass;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\loginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbRegMail;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\loginWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSignUp;
        
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
            System.Uri resourceLocater = new System.Uri("/Bridge;component/loginwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\loginWindow.xaml"
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
            
            #line 8 "..\..\loginWindow.xaml"
            ((Bridge.loginWindow)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ttab = ((System.Windows.Controls.TabControl)(target));
            return;
            case 3:
            this.tbLogin = ((System.Windows.Controls.TextBox)(target));
            
            #line 14 "..\..\loginWindow.xaml"
            this.tbLogin.KeyUp += new System.Windows.Input.KeyEventHandler(this.tbLogin_KeyUp);
            
            #line default
            #line hidden
            
            #line 14 "..\..\loginWindow.xaml"
            this.tbLogin.GotFocus += new System.Windows.RoutedEventHandler(this.tbLogin_GotFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tbPassword = ((System.Windows.Controls.TextBox)(target));
            
            #line 15 "..\..\loginWindow.xaml"
            this.tbPassword.KeyUp += new System.Windows.Input.KeyEventHandler(this.tbPassword_KeyUp);
            
            #line default
            #line hidden
            
            #line 15 "..\..\loginWindow.xaml"
            this.tbPassword.GotFocus += new System.Windows.RoutedEventHandler(this.tbPassword_GotFocus);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnSend = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\loginWindow.xaml"
            this.btnSend.Click += new System.Windows.RoutedEventHandler(this.btnSend_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnRemind = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.tbRegLogin = ((System.Windows.Controls.TextBox)(target));
            
            #line 23 "..\..\loginWindow.xaml"
            this.tbRegLogin.KeyUp += new System.Windows.Input.KeyEventHandler(this.tbRegLogin_KeyUp);
            
            #line default
            #line hidden
            
            #line 23 "..\..\loginWindow.xaml"
            this.tbRegLogin.GotFocus += new System.Windows.RoutedEventHandler(this.tbRegLogin_GotFocus);
            
            #line default
            #line hidden
            return;
            case 8:
            this.tbRegPass = ((System.Windows.Controls.TextBox)(target));
            
            #line 24 "..\..\loginWindow.xaml"
            this.tbRegPass.KeyUp += new System.Windows.Input.KeyEventHandler(this.tbRegPass_KeyUp);
            
            #line default
            #line hidden
            
            #line 24 "..\..\loginWindow.xaml"
            this.tbRegPass.GotFocus += new System.Windows.RoutedEventHandler(this.tbRegPass_GotFocus);
            
            #line default
            #line hidden
            return;
            case 9:
            this.tbRegMail = ((System.Windows.Controls.TextBox)(target));
            
            #line 25 "..\..\loginWindow.xaml"
            this.tbRegMail.KeyUp += new System.Windows.Input.KeyEventHandler(this.tbRegMail_KeyUp);
            
            #line default
            #line hidden
            
            #line 25 "..\..\loginWindow.xaml"
            this.tbRegMail.GotFocus += new System.Windows.RoutedEventHandler(this.tbRegMail_GotFocus);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnSignUp = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\loginWindow.xaml"
            this.btnSignUp.Click += new System.Windows.RoutedEventHandler(this.btnSignUp_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

