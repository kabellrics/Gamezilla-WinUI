// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using GameZilla.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WindowsInput.Native;
using WindowsInput;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GameZilla.Views.HomeView
{
    public sealed partial class Hero : UserControl
    {
        public HomeViewModel ViewModel
        {
            get;
        }
        public Hero()
        {
            this.ViewModel = App.GetService<HomeViewModel>();
            this.DataContext = ViewModel;
            this.InitializeComponent();
            gridview.Focus(FocusState.Programmatic);
            this.gridview.SelectedIndex = 0;
            this.itemListView.SelectedIndex = 0;
            if (gridview.Items.Count > 0)
            {
                var selected = gridview.SelectedItem as GridViewItem;
                selected.Focus(FocusState.Programmatic);
                selected.Focus(FocusState.Keyboard);
                var simulator = new InputSimulator();
                simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
            }
        }
        
        private void MaxItemsWrapGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var wrapgrid = (ItemsWrapGrid)sender;
            wrapgrid.ItemWidth = this.ActualWidth / 10;
        }

    }
}
