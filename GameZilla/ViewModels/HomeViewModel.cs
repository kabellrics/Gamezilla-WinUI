using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Contracts.Services;
using GameZilla.Core.Contracts.Services;
using GameZilla.FrontModel;
using Microsoft.UI.Xaml.Controls;

namespace GameZilla.ViewModels;

public partial class HomeViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly IExecutableService _executableService;
    private readonly IPageSkinService _pageSkinService;
    private ICommand _LoadedCommand;
    private ICommand _GotoFavCommand;
    private ICommand _GotoLastCommand;
    private ICommand _GotoNoPlayCommand;
    private ICommand _GoSystemsCommand;
    private ICommand _GoAllGamesCommand;
    private ICommand _GoSettingsCommand;
    private ICommand _QuitCommand;
    private ICommand _SleepCommand;
    private ICommand _ShutdownCommand;
    public ICommand LoadedCommand => _LoadedCommand ?? (_LoadedCommand = new RelayCommand(Loaded));
    public ICommand GotoFavCommand => _GotoFavCommand ?? (_GotoFavCommand = new RelayCommand<bool>(GotoFav));
    public ICommand GotoLastCommand => _GotoLastCommand ?? (_GotoLastCommand = new RelayCommand<bool>(GotoLast));
    public ICommand GotoNoPlayCommand => _GotoNoPlayCommand ?? (_GotoNoPlayCommand = new RelayCommand<bool>(GotoNoPlay));
    public ICommand GoSystemsCommand => _GoSystemsCommand ?? (_GoSystemsCommand = new RelayCommand(GoSystems));
    public ICommand GoAllGamesCommand => _GoAllGamesCommand ?? (_GoAllGamesCommand = new RelayCommand(GoAllGames));
    public ICommand GoSettingsCommand => _GoSettingsCommand ?? (_GoSettingsCommand = new RelayCommand(GoSettings));
    public ICommand QuitCommand => _QuitCommand ?? (_QuitCommand = new RelayCommand(Quit));
    public ICommand SleepCommand => _SleepCommand ?? (_SleepCommand = new RelayCommand(Sleep));
    public ICommand ShutdownCommand => _ShutdownCommand ?? (_ShutdownCommand = new RelayCommand(Shutdown));

    private String _display;
    public String Display
    {
        get => _display;
        set => SetProperty(ref _display, value);
    }
    public ObservableCollection<GamezillaMenuItem> Menus;
    public HomeViewModel(INavigationService navigationService, IExecutableService executableService, IPageSkinService pageSkinService)
    {
        _navigationService = navigationService;
        _executableService = executableService;
        _pageSkinService = pageSkinService;
        Menus = new ObservableCollection<GamezillaMenuItem>();
        
    }
    public void Loaded()
    {
        InitData();
    }

    private async void InitData()
    {
        Display = await _pageSkinService.GetCurrentDisplayHome();
        Menus.Add(new GamezillaMenuItem() { Text = "Favoris", IconPath = @"ms-appx:///Assets/specificlogo/auto-favorites.png", Command = GotoFavCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Derniers jeux lancés", IconPath = @"ms-appx:///Assets/specificlogo/auto-lastplayed.png", Command = GotoLastCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Et si vous essayez ces jeux", IconPath = @"ms-appx:///Assets/specificlogo/auto-neverplayed.png", Command = GotoNoPlayCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Tous les Systemes", IconPath = "\uE967", Command = GoSystemsCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Tous les Jeux", IconPath = @"ms-appx:///Assets/specificlogo/auto-allgames.png", Command = GotoFavCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Settings", IconPath = "\uE713", Command = GotoFavCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Quitter", IconPath = "\uEDAE", Command = QuitCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Mettre en Veille", IconPath = "\uE708", Command = SleepCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Eteindre", IconPath = "\uE7E8", Command = ShutdownCommand });
    }
    private void GotoFav(bool obj) => throw new NotImplementedException();
    private void GotoLast(bool obj) => throw new NotImplementedException();
    private void GotoNoPlay(bool obj) => throw new NotImplementedException();
    private void GoSystems() => throw new NotImplementedException();
    private void GoAllGames() => throw new NotImplementedException();
    private void GoSettings() => throw new NotImplementedException();
    private void Quit() => throw new NotImplementedException();
    private void Sleep() => throw new NotImplementedException();
    private void Shutdown() => throw new NotImplementedException();
}
