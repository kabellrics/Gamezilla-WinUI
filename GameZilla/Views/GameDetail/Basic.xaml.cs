using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using GameZilla.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GameZilla.Views.GameDetail;
public sealed partial class Basic : UserControl
{
    public ItemDetailViewModel ViewModel
    {
        get;
    }
    public Basic()
    {
        this.ViewModel = App.GetService<ItemDetailViewModel>();
        this.DataContext = ViewModel;
        this.InitializeComponent();
        PlayBT.Focus(FocusState.Programmatic);
        PlayBT.Focus(FocusState.Keyboard);
    }

    private void PlayBT_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if(e.Key == Windows.System.VirtualKey.Down || e.Key == Windows.System.VirtualKey.GamepadDPadDown) { FavBT.Focus(FocusState.Programmatic); }
    }

    private void FavBT_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Up || e.Key == Windows.System.VirtualKey.GamepadDPadUp) { PlayBT.Focus(FocusState.Programmatic); }
    }
}
