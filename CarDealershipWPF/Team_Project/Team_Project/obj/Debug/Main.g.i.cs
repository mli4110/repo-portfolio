﻿#pragma checksum "..\..\Main.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6A33FB369AEF0266B933E0E865D81FBF87F83B8F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using Team_Project;


namespace Team_Project {
    
    
    /// <summary>
    /// Main
    /// </summary>
    public partial class Main : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnLoginLogoutButton;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdSalesStats;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblEmployeeInfo;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button main_btnSales;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button main_btnStats;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button main_btnOrders;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\Main.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button main_btnInventory;
        
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
            System.Uri resourceLocater = new System.Uri("/Team_Project;component/main.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Main.xaml"
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
            this.btnLoginLogoutButton = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\Main.xaml"
            this.btnLoginLogoutButton.Click += new System.Windows.RoutedEventHandler(this.LoginLogoutButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grdSalesStats = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.lblEmployeeInfo = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.main_btnSales = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\Main.xaml"
            this.main_btnSales.Click += new System.Windows.RoutedEventHandler(this.main_btnSales_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.main_btnStats = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\Main.xaml"
            this.main_btnStats.Click += new System.Windows.RoutedEventHandler(this.main_btnStats_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.main_btnOrders = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\Main.xaml"
            this.main_btnOrders.Click += new System.Windows.RoutedEventHandler(this.main_btnOrders_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.main_btnInventory = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\Main.xaml"
            this.main_btnInventory.Click += new System.Windows.RoutedEventHandler(this.main_btnInventory_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

