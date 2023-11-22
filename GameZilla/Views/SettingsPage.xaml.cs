﻿using GameZilla.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.Storage;
using GameZilla.Core.Models.Emulateur;

namespace GameZilla.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    private void Page_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Back || e.Key == Windows.System.VirtualKey.Escape || e.Key == Windows.System.VirtualKey.GamepadB)
        {
            ViewModel.GoBackCommand.Execute(null);
        }
        else if (e.Key == Windows.System.VirtualKey.GamepadMenu) { ViewModel.GoHomeCommand.Execute(null); }
    }

    private async Task OpenFolderPicker(string callercode)
    {
        FolderPicker openPicker = new Windows.Storage.Pickers.FolderPicker();
        //var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
        //var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
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

    private async void Button_Click_4(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        FileOpenPicker fileOpenPicker = new FileOpenPicker();
        var hwnd = App.MainWindow.GetWindowHandle();

        // Associate the HWND with the file picker
        WinRT.Interop.InitializeWithWindow.Initialize(fileOpenPicker, hwnd);
        fileOpenPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        fileOpenPicker.FileTypeFilter.Add(".exe");
        StorageFile file = await fileOpenPicker.PickSingleFileAsync();
        if(file != null)
        {
            ViewModel.AddApplication(file.Path);
        }
    }

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
        ViewModel.PickProfiles(emu);
    }
}
