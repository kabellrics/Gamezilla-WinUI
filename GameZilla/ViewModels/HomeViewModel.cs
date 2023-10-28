using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Contracts.Services;
using GameZilla.Contracts.ViewModels;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.FrontModel;
using Microsoft.UI.Xaml.Controls;

namespace GameZilla.ViewModels;

public partial class HomeViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IExecutableService _executableService;
    private readonly IPageSkinService _pageSkinService;
    private readonly IItemBuilder _itemBuilder;
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
    private ICommand _ReStartCommand;
    public ICommand LoadedCommand => _LoadedCommand ?? (_LoadedCommand = new RelayCommand(Loaded));
    public ICommand GotoFavCommand => _GotoFavCommand ?? (_GotoFavCommand = new RelayCommand<string>(GotoFav));
    public ICommand GotoLastCommand => _GotoLastCommand ?? (_GotoLastCommand = new RelayCommand<string>(GotoLast));
    public ICommand GotoNoPlayCommand => _GotoNoPlayCommand ?? (_GotoNoPlayCommand = new RelayCommand<string>(GotoNoPlay));
    public ICommand GoSystemsCommand => _GoSystemsCommand ?? (_GoSystemsCommand = new RelayCommand(GoSystems));
    public ICommand GoAllGamesCommand => _GoAllGamesCommand ?? (_GoAllGamesCommand = new RelayCommand(GoAllGames));
    public ICommand GoSettingsCommand => _GoSettingsCommand ?? (_GoSettingsCommand = new RelayCommand(GoSettings));
    public ICommand QuitCommand => _QuitCommand ?? (_QuitCommand = new RelayCommand(Quit));
    public ICommand SleepCommand => _SleepCommand ?? (_SleepCommand = new RelayCommand(Sleep));
    public ICommand ShutdownCommand => _ShutdownCommand ?? (_ShutdownCommand = new RelayCommand(Shutdown));
    public ICommand ReStartCommand => _ReStartCommand ?? (_ReStartCommand = new RelayCommand(ReStart));

    private String _display;
    public String Display
    {
        get => _display;
        set => SetProperty(ref _display, value);
    }
    public ObservableCollection<GamezillaMenuItem> Menus;
    public ObservableCollection<Item> CurrentDisplayList;
    public HomeViewModel(INavigationService navigationService, IExecutableService executableService, IPageSkinService pageSkinService, IItemBuilder itemBuilder)
    {
        _navigationService = navigationService;
        _executableService = executableService;
        _pageSkinService = pageSkinService;
        Menus = new ObservableCollection<GamezillaMenuItem>();
        _itemBuilder = itemBuilder;
    }
    public void Loaded()
    {
        InitData();
    }

    private async void InitData()
    {
        Display = await _pageSkinService.GetCurrentDisplayHome();
        Menus.Clear();
        Menus.Add(new GamezillaMenuItem() { Text = "Favoris", IconPath= "\uE735", ImagePath = @"ms-appx:///Assets/specificlogo/auto-favorites-fr.png", Command = GotoFavCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Derniers jeux lancés", IconPath= "\uE81C", ImagePath = @"ms-appx:///Assets/specificlogo/auto-lastplayed-fr.png", Command = GotoLastCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Et si vous essayez ces jeux", IconPath = "\uE916", ImagePath = @"ms-appx:///Assets/specificlogo/auto-neverplayed-fr.png", Command = GotoNoPlayCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Tous les Systemes", IconPath = "\uE967", ImagePath = @"ms-appx:///Assets/specificlogo/Xbox.png", Command = GoSystemsCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Tous les Jeux",IconPath= "\uE7FC", ImagePath = @"ms-appx:///Assets/specificlogo/auto-allgames-fr.png", Command = GoAllGamesCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Settings", IconPath = "\uE713", ImagePath = @"ms-appx:///Assets/specificlogo/Settings.png", Command = GoSettingsCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Quitter", IconPath = "\uEDAE", ImagePath = @"ms-appx:///Assets/specificlogo/Close.png", Command = QuitCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Mettre en Veille", IconPath = "\uE708", ImagePath = @"ms-appx:///Assets/specificlogo/Veille.png", Command = SleepCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Redémarrage", IconPath = "\uF83E", ImagePath = @"ms-appx:///Assets/specificlogo/Redemarrer.png", Command = ReStartCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Eteindre", IconPath = "\uE7E8", ImagePath = @"ms-appx:///Assets/specificlogo/Shutdown.png", Command = ShutdownCommand });
    }
    public void GotoFav(string obj)
    {
    }
    public void GotoLast(string obj)
    {
    }
    public void GotoNoPlay(string obj)
    {
    }
    public void GoSystems()
    {
    }
    public void GoAllGames()
    {
    }
    public void GoSettings()
    {
        try
        {
            _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
        }
        catch (Exception ex)
        {
            //throw;
        }
    }
    public void Quit()
    {
    }
    public void Sleep()
    {
    }
    public void Shutdown()
    {
    }
    public void ReStart()
    {
    }
    public void OnNavigatedTo(object parameter)
    {
    }
    public void OnNavigatedFrom()
    {
    }
}
