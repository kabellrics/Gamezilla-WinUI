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
using Windows.System;

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
            btfav.Focus(FocusState.Programmatic);
            btfav.Focus(FocusState.Keyboard);
        }

        private void btsettings_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btquit.Focus(FocusState.Programmatic);
                btquit.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                btnever.Focus(FocusState.Programmatic);
                btnever.Focus(FocusState.Keyboard);
            }
        }

        private void btquit_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btsettings.Focus(FocusState.Programmatic);
                btsettings.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btveille.Focus(FocusState.Programmatic);
                btveille.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                btallsys.Focus(FocusState.Programmatic);
                btallsys.Focus(FocusState.Keyboard);
            }
        }

        private void btveille_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btquit.Focus(FocusState.Programmatic);
                btquit.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btrestart.Focus(FocusState.Programmatic);
                btrestart.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                btallsys.Focus(FocusState.Programmatic);
                btallsys.Focus(FocusState.Keyboard);
            }
        }

        private void btrestart_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btveille.Focus(FocusState.Programmatic);
                btveille.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btshutdown.Focus(FocusState.Programmatic);
                btshutdown.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                btgames.Focus(FocusState.Programmatic);
                btgames.Focus(FocusState.Keyboard);
            }
        }

        private void btshutdown_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btrestart.Focus(FocusState.Programmatic);
                btrestart.Focus(FocusState.Keyboard);
            }
            else if(e.Key == VirtualKey.Down|| e.Key == VirtualKey.GamepadDPadDown)
            {
                btgames.Focus(FocusState.Programmatic);
                btgames.Focus(FocusState.Keyboard);
            }
        }

        private void btfav_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btlast.Focus(FocusState.Programmatic);
                btlast.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                bt2Display.Focus(FocusState.Programmatic);
                bt2Display.Focus(FocusState.Keyboard);
            }
        }

        private void btlast_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btnever.Focus(FocusState.Programmatic);
                btnever.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btfav.Focus(FocusState.Programmatic);
                btfav.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                bt2Display.Focus(FocusState.Programmatic);
                bt2Display.Focus(FocusState.Keyboard);
            }
        }

        private void btnever_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btallsys.Focus(FocusState.Programmatic);
                btallsys.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btlast.Focus(FocusState.Programmatic);
                btlast.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                bt3Display.Focus(FocusState.Programmatic);
                bt3Display.Focus(FocusState.Keyboard);
            }
        }

        private void btallsys_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                btgames.Focus(FocusState.Programmatic);
                btgames.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btnever.Focus(FocusState.Programmatic);
                btnever.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                bt4Display.Focus(FocusState.Programmatic);
                bt4Display.Focus(FocusState.Keyboard);
            }
        }

        private void btgames_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                btallsys.Focus(FocusState.Programmatic);
                btallsys.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Down || e.Key == VirtualKey.GamepadDPadDown)
            {
                bt4Display.Focus(FocusState.Programmatic);
                bt4Display.Focus(FocusState.Keyboard);
            }
        }

        private void bt1Display_KeyDown(object sender, KeyRoutedEventArgs e)
        {
         if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                bt2Display.Focus(FocusState.Programmatic);
                bt2Display.Focus(FocusState.Keyboard);
            }
        }

        private void bt2Display_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                bt1Display.Focus(FocusState.Programmatic);
                bt1Display.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight) 
            {
                bt3Display.Focus(FocusState.Programmatic);
                bt3Display.Focus(FocusState.Keyboard);
            }
        }

        private void bt3Display_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                bt2Display.Focus(FocusState.Programmatic);
                bt2Display.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                bt4Display.Focus(FocusState.Programmatic);
                bt4Display.Focus(FocusState.Keyboard);
            }
        }

        private void bt4Display_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                bt3Display.Focus(FocusState.Programmatic);
                bt3Display.Focus(FocusState.Keyboard);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GamepadDPadRight)
            {
                bt5Display.Focus(FocusState.Programmatic);
                bt5Display.Focus(FocusState.Keyboard);
            }

        }

        private void bt5Display_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GamepadDPadLeft)
            {
                bt4Display.Focus(FocusState.Programmatic);
                bt4Display.Focus(FocusState.Keyboard);
            }
        }
    }
}
