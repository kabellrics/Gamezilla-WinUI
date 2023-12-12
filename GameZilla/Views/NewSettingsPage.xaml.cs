using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using GameZilla.ViewModels;
using GameZilla.Views.SettingsSplitPage;
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

namespace GameZilla.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class NewSettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }
    public NewSettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        this.InitializeComponent();
        ContentFrame.Navigate(typeof(SettingAffichagePage), null);
    }

    private void settingNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        var navOptions = new FrameNavigationOptions
        {
            TransitionInfoOverride = args.RecommendedNavigationTransitionInfo,
            IsNavigationStackEnabled = false,
        };
        
        switch (args.InvokedItemContainer.Name)
        {
            case nameof(itemAffichage):
                ContentFrame.NavigateToType(typeof(SettingAffichagePage), null, navOptions);
                break;

            case nameof(itemParamètres):
                ContentFrame.NavigateToType(typeof(SettingsParamPage), null, navOptions);
                break;

            case nameof(itemApplications):
                ContentFrame.NavigateToType(typeof(SettingsApplicationPage), null, navOptions);
                break;

            case nameof(itemEmulateurs):
                ContentFrame.NavigateToType(typeof(SettingsEmulateurPage), null, navOptions);
                break;

            case nameof(itemRetroarch):
                ContentFrame.NavigateToType(typeof(SettingsRetroarchPage), null, navOptions);
                break;

            case nameof(itemStore):
                ContentFrame.NavigateToType(typeof(SettingsStorePage), null, navOptions);
                break;

            case nameof(itemRoms):
                ContentFrame.NavigateToType(typeof(SettingsRomsPage), null, navOptions);
                break;

            case nameof(itemPegasusWindows):
                ContentFrame.NavigateToType(typeof(SettingsPegasusWindowsPage), null, navOptions);
                break;

            case nameof(itemPegasusAndroid):
                ContentFrame.NavigateToType(typeof(SettingsPegasusAndroidPage), null, navOptions);
                break;
        }
    }

    private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Back || e.Key == Windows.System.VirtualKey.Escape || e.Key == Windows.System.VirtualKey.GamepadB)
        {
            ViewModel.GoBackCommand.Execute(null);
        }
        else if (e.Key == Windows.System.VirtualKey.GamepadMenu) { ViewModel.GoHomeCommand.Execute(null); }

    }
}
