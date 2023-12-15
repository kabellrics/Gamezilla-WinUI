using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using GameZilla.ViewModels.Settings;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GameZilla.Views.SettingsSplitPage;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsParamPage : Page
{
    public SettingsParamViewModel ViewModel
    {
        get;
    }
    public SettingsParamPage()
    {
        ViewModel = App.GetService<SettingsParamViewModel>();
        this.InitializeComponent();
    }
    protected override void OnNavigatedTo(NavigationEventArgs e) { base.OnNavigatedTo(e);ViewModel.OnNavigatedTo(e.Parameter); }
    private async Task OpenFolderPicker(string callercode)
    {
        FolderPicker openPicker = new Windows.Storage.Pickers.FolderPicker();
        var hwnd = App.MainWindow.GetWindowHandle();

        // Associate the HWND with the file picker
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);
        openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        openPicker.FileTypeFilter.Add("*");

        // Open the picker for the user to pick a folder
        StorageFolder folder = await openPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            ViewModel.OpenFolderPicker(callercode, folder.Path);
        }
    }

    private async void Button_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await OpenFolderPicker("0");
    }

    private async void Button_Click_1(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await OpenFolderPicker("1");
    }

    private async void Button_Click_2(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await OpenFolderPicker("2");
    }

    private async void Button_Click_3(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await OpenFolderPicker("3");
    }
}
