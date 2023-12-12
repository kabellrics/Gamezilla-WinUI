using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using GameZilla.Contracts.Services;
using GameZilla.Contracts.ViewModels;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.Core.Models.Emulateur;
using GameZilla.Core.Models.Origin;
using GameZilla.Core.Services;
using GameZilla.Helpers;
using GameZilla.Services;
using GameZilla.ViewModels.Object;
using Microsoft.UI.Xaml;

using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace GameZilla.ViewModels;

public partial class SettingsViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IAssetService _assetService;
    private ICommand _GoBackCommand;
    private ICommand _GoHomeCommand;
    private StorageFile _bck;
    public StorageFile Bck
    {
        get => _bck;
        set => SetProperty(ref _bck, value);
    }
    public ICommand GoHomeCommand => _GoHomeCommand ?? (_GoHomeCommand = new RelayCommand(GoHome));
    public ICommand GoBackCommand => _GoBackCommand ?? (_GoBackCommand = new RelayCommand(GoBack));
    private void GoBack()
    {
        _navigationService.GoBack();
    }
    private void GoHome()
    {
        _navigationService.NavigateTo(typeof(HomeViewModel).FullName!);
    }
    public SettingsViewModel(INavigationService navigationService, IAssetService assetService)
    {
        _navigationService = navigationService;
        _assetService = assetService;
    }
    public void OnNavigatedFrom()
    {
    }
    public async void OnNavigatedTo(object parameter)
    {
        Bck = await _assetService.GetRandomBackground();
    }
}
