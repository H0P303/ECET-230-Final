﻿#pragma checksum "..\..\..\jsonFileWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7E1D18747ABA848E33A17FCEFD633189E1FBFC64"
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
using System.Windows.Controls.Ribbon;
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


namespace MeadowSolar {
    
    
    /// <summary>
    /// jsonFileWindow
    /// </summary>
    public partial class jsonFileWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\..\jsonFileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MeadowSolar.jsonFileWindow JsonWindowMain;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\jsonFileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox dataDisplay;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\jsonFileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox packetList;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\jsonFileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button packetSelectorBtn;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\jsonFileWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas GraphCanvas;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.12.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MeadowSolar;component/jsonfilewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\jsonFileWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.12.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.JsonWindowMain = ((MeadowSolar.jsonFileWindow)(target));
            
            #line 7 "..\..\..\jsonFileWindow.xaml"
            this.JsonWindowMain.Loaded += new System.Windows.RoutedEventHandler(this.JsonWindowMain_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dataDisplay = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.packetList = ((System.Windows.Controls.ListBox)(target));
            return;
            case 4:
            this.packetSelectorBtn = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\jsonFileWindow.xaml"
            this.packetSelectorBtn.Click += new System.Windows.RoutedEventHandler(this.packetSelectorBtn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.GraphCanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

