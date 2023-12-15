using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using GameZilla.ViewModels;
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
public sealed partial class SettingsApplicationPage : Page
{
    public SettingsApplicationViewModel ViewModel
    {
        get;
    }
    public SettingsApplicationPage()
    {
        ViewModel = App.GetService<SettingsApplicationViewModel>();
        this.InitializeComponent();
    }
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        ViewModel.OnNavigatedTo(e.Parameter);
    }
    private async void Button_Click_4(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        FileOpenPicker fileOpenPicker = new FileOpenPicker();
        var hwnd = App.MainWindow.GetWindowHandle();

        // Associate the HWND with the file picker
        WinRT.Interop.InitializeWithWindow.Initialize(fileOpenPicker, hwnd);
        fileOpenPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        fileOpenPicker.FileTypeFilter.Add(".exe");
        StorageFile file = await fileOpenPicker.PickSingleFileAsync();
        if (file != null)
        {
            ViewModel.AddApplication(file.Path);
        }
    }
}
