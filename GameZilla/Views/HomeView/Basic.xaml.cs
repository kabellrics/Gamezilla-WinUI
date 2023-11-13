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
            this.Loaded += Basic_Loaded;
            this.btfav.Focus(FocusState.Programmatic);
            this.btfav.Focus(FocusState.Keyboard);
            //gridview.Focus(FocusState.Programmatic);
            //this.gridview.SelectedIndex = 0;
            //SetFocusToFirstGridViewItem();
        }

        private void Basic_Loaded(object sender, RoutedEventArgs e)
        {
            this.btfav.Focus(FocusState.Programmatic);
            this.btfav.Focus(FocusState.Keyboard);
        }

        private void btfav_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                btsettings.Focus(FocusState.Programmatic);
                btsettings.Focus(FocusState.Keyboard);
            }
            else if(e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btlast.Focus(FocusState.Programmatic);
                btlast.Focus(FocusState.Keyboard);
            }
        }

        private void btlast_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                btquit.Focus(FocusState.Programmatic);
                btquit.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btnever.Focus(FocusState.Programmatic);
                btnever.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btfav.Focus(FocusState.Programmatic);
                btfav.Focus(FocusState.Keyboard);
            }
        }

        private void btnever_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                btveille.Focus(FocusState.Programmatic);
                btveille.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btallsys.Focus(FocusState.Programmatic);
                btallsys.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btlast.Focus(FocusState.Programmatic);
                btlast.Focus(FocusState.Keyboard);
            }
        }

        private void btallsys_KeyDown(object sender, KeyRoutedEventArgs e)
        {


            if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                btrestart.Focus(FocusState.Programmatic);
                btrestart.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btgames.Focus(FocusState.Programmatic);
                btgames.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btnever.Focus(FocusState.Programmatic);
                btnever.Focus(FocusState.Keyboard);
            }
        }

        private void btgames_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                btshutdown.Focus(FocusState.Programmatic);
                btshutdown.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btallsys.Focus(FocusState.Programmatic);
                btallsys.Focus(FocusState.Keyboard);
            }
        }

        private void btsettings_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Up || e.Key == VirtualKey.GamepadDPadUp)
            {
                btfav.Focus(FocusState.Programmatic);
                btfav.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btquit.Focus(FocusState.Programmatic);
                btquit.Focus(FocusState.Keyboard);
            }
        }

        private void btquit_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Up || e.Key == VirtualKey.GamepadDPadUp)
            {
                btlast.Focus(FocusState.Programmatic);
                btlast.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btsettings.Focus(FocusState.Programmatic);
                btsettings.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btveille.Focus(FocusState.Programmatic);
                btveille.Focus(FocusState.Keyboard);
            }
        }

        private void btveille_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            if (e.Key == VirtualKey.Up || e.Key == VirtualKey.GamepadDPadUp)
            {
                btnever.Focus(FocusState.Programmatic);
                btnever.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btquit.Focus(FocusState.Programmatic);
                btquit.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btrestart.Focus(FocusState.Programmatic);
                btrestart.Focus(FocusState.Keyboard);
            }
        }

        private void btrestart_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            if (e.Key == VirtualKey.Up || e.Key == VirtualKey.GamepadDPadUp)
            {
                btallsys.Focus(FocusState.Programmatic);
                btallsys.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btveille.Focus(FocusState.Programmatic);
                btveille.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btshutdown.Focus(FocusState.Programmatic);
                btshutdown.Focus(FocusState.Keyboard);
            }
        }

        private void btshutdown_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            if (e.Key == VirtualKey.Up || e.Key == VirtualKey.GamepadDPadUp)
            {
                btgames.Focus(FocusState.Programmatic);
                btgames.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btrestart.Focus(FocusState.Programmatic);
                btrestart.Focus(FocusState.Keyboard);
            }
        }


        //private void SetFocusToFirstGridViewItem()
        //{
        //    GridViewItem gvi = this.gridview.ContainerFromIndex(0) as GridViewItem;
        //    if (gvi != null)
        //    {
        //        gvi.IsSelected = true;
        //        gvi.Focus(FocusState.Programmatic);
        //    }
        //    else
        //    {
        //        var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        //        dispatcherQueue.TryEnqueue((Microsoft.UI.Dispatching.DispatcherQueuePriority)DispatcherQueuePriority.Low,
        //                () => {
        //                    gvi = this.gridview.ContainerFromIndex(0) as GridViewItem;
        //                    if (gvi != null)
        //                    {
        //                        gvi.IsSelected = true;
        //                        gvi.Focus(FocusState.Programmatic);
        //                    }
        //                });
        //    }
        //}

        //private void MaxItemsWrapGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    var wrapgrid = (ItemsWrapGrid)sender;
        //    wrapgrid.ItemHeight = this.ActualHeight / 2.5;
        //    wrapgrid.ItemWidth = this.ActualWidth / 5.5;
        //}


        //private void gridview_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    var item = (GamezillaMenuItem)this.gridview.SelectedItem;
        //    if(item != null)
        //    item.Command.Execute("true");
        //}

        //private void gridview_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        //{
        //    var item = (GamezillaMenuItem)this.gridview.SelectedItem;
        //    if (item != null)
        //        item.Command.Execute("true");
        //}

        //private void gridview_KeyDown(object sender, KeyRoutedEventArgs e)
        //{
        //    if(e.Key == VirtualKey.Enter || e.Key == VirtualKey.GamepadA ||  e.Key == VirtualKey.Space)
        //    {
        //        var item = (GamezillaMenuItem)this.gridview.SelectedItem;
        //        if (item != null)
        //            item.Command.Execute("true");
        //    }
        //}

        //private void gridview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var newItem = e.AddedItems.FirstOrDefault();
        //    if(newItem != null)
        //    gridview.SelectedItem = newItem;
        //}
    }
}
