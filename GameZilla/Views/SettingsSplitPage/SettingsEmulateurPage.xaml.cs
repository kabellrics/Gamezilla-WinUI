using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using GameZilla.Core.Models.Emulateur;
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
public sealed partial class SettingsEmulateurPage : Page
{
    public SettingsEmulateurViewModel ViewModel
    {
        get;
    }
    public SettingsEmulateurPage()
    {
        ViewModel = App.GetService<SettingsEmulateurViewModel>();
        this.InitializeComponent();
    }
    protected override void OnNavigatedTo(NavigationEventArgs e) { base.OnNavigatedTo(e);ViewModel.OnNavigatedTo(e.Parameter); }
    private void Button_Click_5(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var bt = sender as Button;
        var plats = bt.Tag as Platforms;
        ViewModel.PickPlatforms(plats);
    }

    private void Button_Click_6(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var bt = sender as Button;
        var emu = bt.Tag as Emulateur;
        ViewModel.PickEmu(emu);
    }

    private void Button_Click_7(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var bt = sender as Button;
        var profile = bt.Tag as Profile;
        ViewModel.PickProfiles(profile);
    }

    private async void Button_Click_8(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        FileOpenPicker fileOpenPicker = new FileOpenPicker();
        var hwnd = App.MainWindow.GetWindowHandle();
        WinRT.Interop.InitializeWithWindow.Initialize(fileOpenPicker, hwnd);
        fileOpenPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
        var exeformat = ViewModel.profile.StartupExecutable.Replace("^", "").Replace("\\", "").Replace("$", "").Replace("*", "").Replace("..", ".");
        fileOpenPicker.FileTypeFilter.Add(Path.GetExtension(exeformat));
        StorageFile file = await fileOpenPicker.PickSingleFileAsync();
        if (file != null && file.Name.ToUpper() == exeformat.ToUpper())
        {
            ViewModel.GetExecutablePath(file.Path);
        }
    }
}
