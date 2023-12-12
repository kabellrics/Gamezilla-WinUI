using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Contracts.Services;
using GameZilla.Contracts.ViewModels;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.FrontModel;
using GameZilla.Services;
using GameZilla.ViewModels.Object;
using GameZilla.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace GameZilla.ViewModels;

public partial class HomeViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IExecutableService _executableService;
    private readonly IPageSkinService _pageSkinService;
    private readonly IAssetService _assetService;
    private readonly IItemBuilder _itemBuilder;
    private DispatcherTimer bg;
    private Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;
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
    private ICommand _GotoGameCommand;
    public ICommand GotoGameCommand => _GotoGameCommand ?? (_GotoGameCommand = new RelayCommand<ObsItem>(GotoGame));
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

    private StorageFile _bck;
    public StorageFile Bck
    {
        get => _bck;
        set => SetProperty(ref _bck, value);
    }
    private String _display;
    public String Display
    {
        get => _display;
        set => SetProperty(ref _display, value);
    }
    private String _displayDateTime;
    public String DisplayDateTime
    {
        get => _displayDateTime;
        set => SetProperty(ref _displayDateTime, value);
    }
    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetProperty(ref _selectedIndex, value);
    }
    private int _selectedMenuIndex;
    public int SelectedMenuIndex
    {
        get => _selectedMenuIndex;
        set => SetProperty(ref _selectedMenuIndex, value);
    }
    public ObservableCollection<GamezillaMenuItem> Menus;
    public ObservableCollection<ObsItem> CurrentDisplayList;
    public ObservableCollection<ObsItem> FavorisDisplayList;
    public ObservableCollection<ObsItem> LastPlayedDisplayList;
    public ObservableCollection<ObsItem> NeverPlayedDisplayList;
    private ObsItem _Display1Item;
    public ObsItem Display1Item
    {
        get => _Display1Item;
        set => SetProperty(ref _Display1Item, value);
    }
    private ObsItem _Display2Item;
    public ObsItem Display2Item
    {
        get => _Display2Item;
        set => SetProperty(ref _Display2Item, value);
    }
    private ObsItem _Display3Item;
    public ObsItem Display3Item
    {
        get => _Display3Item;
        set => SetProperty(ref _Display3Item, value);
    }
    private ObsItem _Display4Item;
    public ObsItem Display4Item
    {
        get => _Display4Item;
        set => SetProperty(ref _Display4Item, value);
    }
    private ObsItem _Display5Item;
    public ObsItem Display5Item
    {
        get => _Display5Item;
        set => SetProperty(ref _Display5Item, value);
    }

    public HomeViewModel(INavigationService navigationService, IExecutableService executableService, IPageSkinService pageSkinService, IItemBuilder itemBuilder,IAssetService assetService)
    {
        _navigationService = navigationService;
        _executableService = executableService;
        _pageSkinService = pageSkinService;
        _assetService = assetService;
        Menus = new ObservableCollection<GamezillaMenuItem>();
        CurrentDisplayList = new ObservableCollection<ObsItem>();
        FavorisDisplayList = new ObservableCollection<ObsItem>();
        LastPlayedDisplayList = new ObservableCollection<ObsItem>();
        NeverPlayedDisplayList = new ObservableCollection<ObsItem>();
        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        _itemBuilder = itemBuilder;
        bg = new DispatcherTimer();
        bg.Tick += (s, e) =>
        {
            this.dispatcherQueue.TryEnqueue(() =>
            {
                DisplayDateTime = DateTime.Now.ToString("F", new CultureInfo("fr-FR"));
            });
        };
        bg.Interval = TimeSpan.FromMilliseconds(333); 
        bg.Start();
        Menus.Clear();
        Menus.Add(new GamezillaMenuItem() { Text = "Favoris", IconPath = "\uE735", ImagePath = @"ms-appx:///Assets/specificlogo/auto-favorites-fr.png", Command = GotoFavCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Derniers jeux lancés", IconPath = "\uE81C", ImagePath = @"ms-appx:///Assets/specificlogo/auto-lastplayed-fr.png", Command = GotoLastCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Et si vous essayez ces jeux", IconPath = "\uE916", ImagePath = @"ms-appx:///Assets/specificlogo/auto-neverplayed-fr.png", Command = GotoNoPlayCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Tous les Systemes", IconPath = "\uE967", ImagePath = @"ms-appx:///Assets/specificlogo/Xbox.png", Command = GoSystemsCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Tous les Jeux", IconPath = "\uE7FC", ImagePath = @"ms-appx:///Assets/specificlogo/auto-allgames-fr.png", Command = GoAllGamesCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Settings", IconPath = "\uE713", ImagePath = @"ms-appx:///Assets/specificlogo/Settings.png", Command = GoSettingsCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Quitter", IconPath = "\uEDAE", ImagePath = @"ms-appx:///Assets/specificlogo/Close.png", Command = QuitCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Mettre en Veille", IconPath = "\uE708", ImagePath = @"ms-appx:///Assets/specificlogo/Veille.png", Command = SleepCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Redémarrage", IconPath = "\uF83E", ImagePath = @"ms-appx:///Assets/specificlogo/Redemarrer.png", Command = ReStartCommand });
        Menus.Add(new GamezillaMenuItem() { Text = "Eteindre", IconPath = "\uE7E8", ImagePath = @"ms-appx:///Assets/specificlogo/Shutdown.png", Command = ShutdownCommand });
        Display1Item = null;
        Display2Item = null;
        Display3Item = null;
        Display4Item = null;
        Display5Item = null;
    }
    public void Loaded()
    {
        InitData();
    }

    private async void InitData()
    {
        Display = await _pageSkinService.GetCurrentDisplayHome();
        Bck = await _assetService.GetRandomBackground();
        SelectedMenuIndex = 0;
    }

    private void GotoGame(ObsItem item)
    {
        try
        {
            _navigationService.NavigateTo(typeof(ItemDetailViewModel).FullName!, item.Id);
        }
        catch (Exception ex)
        {
            //throw;
        }
    }
    public void GotoFav(string obj)
    {
        if(obj == "true") {
            try
            {
                var param = new NavigateToListGameParameter() { Id = "-1", typeListGame = TypeListGame.Favorite };
                _navigationService.NavigateTo(typeof(ItemListViewModel).FullName!, param);
            }
            catch (Exception ex)
            {
                //throw;
            }
        }
        else
        {
            FillCurrentList(FavorisDisplayList);
        }
    }
    public void GotoLast(string obj)
    {
        if (obj == "true")
        {
            try
            {
                var param = new NavigateToListGameParameter() { Id = "-1", typeListGame = TypeListGame.LastPlayed };
                _navigationService.NavigateTo(typeof(ItemListViewModel).FullName!, param);
            }
            catch (Exception ex)
            {
                //throw;
            }
        }
        else
        {
            FillCurrentList(LastPlayedDisplayList);
        }
    }
    public void GotoNoPlay(string obj)
    {
        if (obj == "true")
        {
            try
            {
                var param = new NavigateToListGameParameter() { Id = "-1", typeListGame = TypeListGame.NeverPlayed };
                _navigationService.NavigateTo(typeof(ItemListViewModel).FullName!, param);
            }
            catch (Exception ex)
            {
                //throw;
            }
        }
        else
        {
            FillCurrentList(NeverPlayedDisplayList);
        }
    }
    public void GoSystems()
    {
        try
        {
            _navigationService.NavigateTo(typeof(ContainerViewModel).FullName!);
        }
        catch (Exception ex)
        {
            //throw;
        }
    }
    public void GoAllGames()
    {
        try
        {
            var param = new NavigateToListGameParameter() { Id = "-1", typeListGame = TypeListGame.AllGames };
            _navigationService.NavigateTo(typeof(ItemListViewModel).FullName!, param);
        }
        catch (Exception ex)
        {
            //throw;
        }
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
    public async void OnNavigatedTo(object parameter)
    {
        SelectedMenuIndex = 0;
        CurrentDisplayList.Clear();
        var favlist = await _executableService.GetExecutablesFavorite();
        foreach (var favitem in favlist)
        {
            FavorisDisplayList.Add(new ObsItem(_itemBuilder.FromExecutable(favitem)));
        }
        var lastlist = await _executableService.GetExecutablesLastStarted();
        foreach (var lastitem in lastlist)
        {
            LastPlayedDisplayList.Add(new ObsItem(_itemBuilder.FromExecutable(lastitem)));
        }
        var neverlist = await _executableService.GetExecutablesNeverStarted();
        foreach (var neveritem in neverlist)
        {
            NeverPlayedDisplayList.Add(new ObsItem(_itemBuilder.FromExecutable(neveritem)));
        }
        FillCurrentList(FavorisDisplayList);
    }

    private void FillCurrentList(ObservableCollection<ObsItem> list)
    {
        CurrentDisplayList.Clear();
        foreach (var item in list)
            CurrentDisplayList.Add(item);
        Display1Item = CurrentDisplayList.ElementAtOrDefault(0) ?? null;
        Display2Item = CurrentDisplayList.ElementAtOrDefault(1) ?? null;
        Display3Item = CurrentDisplayList.ElementAtOrDefault(2) ?? null;
        Display4Item = CurrentDisplayList.ElementAtOrDefault(3) ?? null;
        Display5Item = CurrentDisplayList.ElementAtOrDefault(4) ?? null;
    }

    public void OnNavigatedFrom()
    {
    }
}
