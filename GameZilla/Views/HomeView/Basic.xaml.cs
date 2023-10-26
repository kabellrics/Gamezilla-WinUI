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
using Windows.System;
using WindowsInput.Native;
using WindowsInput;
using GameZilla.FrontModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GameZilla.Views.HomeView
{
    public sealed partial class Basic : UserControl
    {
        public HomeViewModel ViewModel
        {
            get;
        }
        public Basic()
        {
            this.ViewModel = App.GetService<HomeViewModel>();
            this.DataContext = ViewModel;
            this.InitializeComponent();
            gridview.Focus(FocusState.Programmatic);
            this.gridview.SelectedIndex = 0;
            var simulator = new InputSimulator();
            simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
        }

        private void MaxItemsWrapGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var wrapgrid = (ItemsWrapGrid)sender;
            wrapgrid.ItemHeight = this.ActualHeight / 2.5;
            wrapgrid.ItemWidth = this.ActualWidth / 5.5;
        }

        private void Button_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                var bt = (Button)sender;
                switch (bt.Tag.ToString())
                {
                    case "Favoris":ViewModel.GotoFav("true");
                        break;
                    case "Derniers jeux lancés":ViewModel.GotoLast("true");
                        break;
                    case "Et si vous essayez ces jeux":ViewModel.GotoNoPlay("true");
                        break;
                    case "Tous les Systemes":ViewModel.GoSystems();
                        break;
                    case "Tous les Jeux":ViewModel.GoAllGames();
                        break;
                    case "Settings":ViewModel.GoSettings();
                        break;
                    case "Quitter":ViewModel.Quit();
                        break;
                    case "Mettre en Veille":ViewModel.Sleep();
                        break;
                    case "Redémarrage":ViewModel.ReStart();
                        break;
                    case "Eteindre":ViewModel.Shutdown();
                        break;
                    default:
                        break;
                }
            }
        }

        private void gridview_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (GamezillaMenuItem)this.gridview.SelectedItem;
            item.Command.Execute("true");
        }
    }
}
