using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using GameZilla.Contracts.Services;
using GameZilla.Contracts.ViewModels;
using GameZilla.Helpers;
using GameZilla.Services;
using Microsoft.UI.Xaml;

using Windows.ApplicationModel;
using Windows.Storage;

namespace GameZilla.ViewModels;

public partial class SettingsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly INavigationService _navigationService;
    private readonly IPageSkinService _pageSkinService;
    private readonly IAssetService _assetService;
    private ICommand _GoBackCommand;
    public ICommand GoBackCommand => _GoBackCommand ?? (_GoBackCommand = new RelayCommand(GoBack));

    private void GoBack()
    {
        _navigationService.GoBack();
    }


    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _versionDescription;

    public ICommand SwitchThemeCommand
    {
        get;
    }
    private String _home;
    public String Home
    {
        get => _home;
        set
        {
            SetProperty(ref _home, value);
            _pageSkinService.SetCurrentDisplayHome(value);
        }
    }
    private String _systems;
    public String Systems
    {
        get => _systems;
        set
        {
            SetProperty(ref _home, value);
            _pageSkinService.SetCurrentDisplaySystems(value);
        }
    }
    private String _games;
    public String Games
    {
        get => _games;
        set
        {
            SetProperty(ref _home, value);
            _pageSkinService.SetCurrentDisplayGames(value);
        }
    }
    private String _gamedetail;
    public String GameDetail
    {
        get => _gamedetail;
        set
        {
            SetProperty(ref _home, value);
            _pageSkinService.SetCurrentDisplayGameDetail(value);
        }
    }
    private StorageFile _bck;
    public StorageFile Bck
    {
        get => _bck;
        set => SetProperty(ref _bck, value);
    }
    public ObservableCollection<string> homedisplays;
    public ObservableCollection<string> sysdisplays;
    public ObservableCollection<string> gamesdisplays;
    public ObservableCollection<string> gamedetaildisplays;
    public SettingsViewModel(IThemeSelectorService themeSelectorService, INavigationService navigationService, IPageSkinService pageSkinService, IAssetService assetService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });
        _navigationService = navigationService;
        _pageSkinService = pageSkinService;
        _assetService = assetService;
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
    public async void OnNavigatedTo(object parameter)
    {
        Bck = await _assetService.GetRandomBackground();
        homedisplays = new ObservableCollection<string>();
        foreach(var skin in _pageSkinService.GetDisplaysForHome()) { homedisplays.Add(skin); }
        Home = await _pageSkinService.GetCurrentDisplayHome();
        sysdisplays = new ObservableCollection<string>();
        foreach (var skin in _pageSkinService.GetDisplaysForSystems()) { sysdisplays.Add(skin); }
        Systems = await _pageSkinService.GetCurrentDisplaySystems();
        gamesdisplays = new ObservableCollection<string>();
        foreach (var skin in _pageSkinService.GetDisplaysForGames()) { gamesdisplays.Add(skin); }
        Games = await _pageSkinService.GetCurrentDisplayGames();
        gamedetaildisplays = new ObservableCollection<string>();
        foreach (var skin in _pageSkinService.GetDisplaysForGameDetail()) { gamedetaildisplays.Add(skin); }
        GameDetail = await _pageSkinService.GetCurrentDisplayGameDetail();
    }
    public void OnNavigatedFrom()
    {
    }
}
