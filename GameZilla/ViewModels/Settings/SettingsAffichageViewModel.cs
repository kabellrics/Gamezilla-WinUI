using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Contracts.Services;
using GameZilla.Contracts.ViewModels;
using GameZilla.Helpers;
using GameZilla.Services;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;

namespace GameZilla.ViewModels.Settings;
public partial class SettingsAffichageViewModel : ObservableRecipient, INavigationAware
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IPageSkinService _pageSkinService;
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
    public ObservableCollection<string> homedisplays;
    public ObservableCollection<string> sysdisplays;
    public ObservableCollection<string> gamesdisplays;
    public ObservableCollection<string> gamedetaildisplays;
    public SettingsAffichageViewModel(IThemeSelectorService themeSelectorService, IPageSkinService pageSkinService)
    {
        _themeSelectorService = themeSelectorService;
        _pageSkinService = pageSkinService;
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
        sysdisplays = new ObservableCollection<string>();
        homedisplays = new ObservableCollection<string>();
        gamesdisplays = new ObservableCollection<string>();
        gamedetaildisplays = new ObservableCollection<string>();
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
    public void OnNavigatedFrom()
    {
    }
    public async void OnNavigatedTo(object parameter)
    {
        try
        {
            sysdisplays.Clear();
            homedisplays.Clear();
            gamesdisplays.Clear();
            gamedetaildisplays.Clear();
            foreach (var skin in _pageSkinService.GetDisplaysForHome()) { homedisplays.Add(skin); }
            Home = await _pageSkinService.GetCurrentDisplayHome();
            foreach (var skin in _pageSkinService.GetDisplaysForSystems()) { sysdisplays.Add(skin); }
            Systems = await _pageSkinService.GetCurrentDisplaySystems();
            foreach (var skin in _pageSkinService.GetDisplaysForGames()) { gamesdisplays.Add(skin); }
            Games = await _pageSkinService.GetCurrentDisplayGames();
            foreach (var skin in _pageSkinService.GetDisplaysForGameDetail()) { gamedetaildisplays.Add(skin); }
            GameDetail = await _pageSkinService.GetCurrentDisplayGameDetail();
        }
        catch (Exception ex)
        {
            //throw;
        }
    }
}
