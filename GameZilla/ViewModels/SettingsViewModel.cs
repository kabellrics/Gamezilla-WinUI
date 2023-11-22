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
using GameZilla.Core.Services;
using GameZilla.Helpers;
using GameZilla.Services;
using GameZilla.ViewModels.Object;
using Microsoft.UI.Xaml;

using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace GameZilla.ViewModels;

public partial class SettingsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly INavigationService _navigationService;
    private readonly IPageSkinService _pageSkinService;
    private readonly IAssetService _assetService;
    private readonly IApplicationFinderService _applicationFinderService;
    private readonly IExecutableService _executableService;
    private readonly IParameterService _parameterService;
    private readonly IEmulateurService _emulateurService;
    private ICommand _GoBackCommand;
    public ICommand GoBackCommand => _GoBackCommand ?? (_GoBackCommand = new RelayCommand(GoBack));

    private void GoBack()
    {
        _navigationService.GoBack();
    }
    
    private ICommand _SaveParamCommand;
    public ICommand SaveParamCommand => _SaveParamCommand ?? (_SaveParamCommand = new RelayCommand(SaveParam));
    private ICommand _PickPlatformsCommand;
    public ICommand PickPlatformsCommand => _PickPlatformsCommand ?? (_PickPlatformsCommand = new RelayCommand<Platforms>(PickPlatforms));

    private ICommand _AddMultipleAppCommand;
    public ICommand AddMultipleAppCommand => _AddMultipleAppCommand ?? (_AddMultipleAppCommand = new RelayCommand(AddMultipleApplication));

    private async void SaveParam()
    {
        _assetService.SetSplashscreenFolder(Splashscreenfolder);
        _assetService.SetSplashvideoFolder(Splashvideofolder);
        _assetService.SetVideoWaitFolder(Videowaitfolder);
        _assetService.SetBackgroundFolder(Backgroundfolder);
    }

    private ICommand _GoHomeCommand;
    public ICommand GoHomeCommand => _GoHomeCommand ?? (_GoHomeCommand = new RelayCommand(GoHome));

    private void GoHome()
    {
        _navigationService.NavigateTo(typeof(HomeViewModel).FullName!);
    }
    public async void OpenFolderPicker(string obj,string folder)
    {        
        if (folder != null)
        {
            switch (obj)
            {
                case "0":
                    Splashscreenfolder = folder;break;
                    case "1":
                    Splashvideofolder = folder;break;
                    case "2":
                    Videowaitfolder = folder;break;
                    case "3":
                    Backgroundfolder = folder;break;
            }
        }
    }

    public async void AddMultipleApplication()
    {
        var selectedprg = installedPrograms.Where(x => x.IsSelected);
        foreach (var exepath in selectedprg)
        {
            var exe = new Executable() { };
            exe.Name = exepath.Name;
            exe.Path = exepath.ExecutablePath;
            exe.PlateformeId = await _parameterService.GetParameterValue(ParamEnum.ApplicationPlateformeId); ;
            await _executableService.CreateExecutable(exe);
        }
        _executableService.Reinit();
        var param = new NavigateToListGameParameter() { Id = await _parameterService.GetParameterValue(ParamEnum.ApplicationPlateformeId), typeListGame = TypeListGame.Plateforme };
        _navigationService.NavigateTo(typeof(ItemListViewModel).FullName!, param);

    }
    public async void AddApplication(string exepath)
    {
        var exe = new Executable() { };
        exe.Name = Path.GetFileNameWithoutExtension(exepath);
        exe.Path = exepath;
        exe.PlateformeId = await _parameterService.GetParameterValue(ParamEnum.ApplicationPlateformeId); ;
        await _executableService.CreateExecutable(exe);
        _executableService.Reinit();
        var param = new NavigateToListGameParameter() { Id = await _parameterService.GetParameterValue(ParamEnum.ApplicationPlateformeId), typeListGame = TypeListGame.Plateforme };
        _navigationService.NavigateTo(typeof(ItemListViewModel).FullName!, param);
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

    private String _splashscreenfolder;
    public String Splashscreenfolder
    {
        get => _splashscreenfolder;
        set
        {
            SetProperty(ref _splashscreenfolder, value);
        }
    }
    private String _splashvideofolder;
    public String Splashvideofolder
    {
        get => _splashvideofolder;
        set
        {
            SetProperty(ref _splashvideofolder, value);
        }
    }
    private String _videowaitfolder;
    public String Videowaitfolder
    {
        get => _videowaitfolder;
        set
        {
            SetProperty(ref _videowaitfolder, value);
        }
    }
    private String _backgroundfolder;
    public String Backgroundfolder
    {
        get => _backgroundfolder;
        set
        {
            SetProperty(ref _backgroundfolder, value);
        }
    }

    private Visibility _showEmulateurList;
    public Visibility ShowEmulateurList
    {
        get => _showEmulateurList;
        set
        {
            SetProperty(ref _showEmulateurList, value);
        }
    }
    private Visibility _showEmuExePicker;
    public Visibility ShowEmuExePicker
    {
        get => _showEmuExePicker;
        set
        {
            SetProperty(ref _showEmuExePicker, value);
        }
    }
    private Visibility _showEmuprofilesPicker;
    public Visibility ShowEmuprofilesPicker
    {
        get => _showEmuprofilesPicker;
        set
        {
            SetProperty(ref _showEmuprofilesPicker, value);
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
    public ObservableCollection<InstalledProgram> installedPrograms;
    public ObservableCollection<Platforms> Platforms;
    public ObservableCollection<Emulateur> Emulateurs;
    public ObservableCollection<Profile> Profiles;
    private string plateformeid;
    public SettingsViewModel(IThemeSelectorService themeSelectorService, INavigationService navigationService, IPageSkinService pageSkinService, IEmulateurService emulateurService,
        IAssetService assetService, IApplicationFinderService applicationFinderService, IExecutableService executableService, IParameterService parameterService)
    {
        _themeSelectorService = themeSelectorService;
        _applicationFinderService = applicationFinderService;
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
        sysdisplays = new ObservableCollection<string>();
        homedisplays = new ObservableCollection<string>();
        gamesdisplays = new ObservableCollection<string>();
        gamedetaildisplays = new ObservableCollection<string>();
        installedPrograms = new ObservableCollection<InstalledProgram>();
        Platforms = new ObservableCollection<Platforms>();
        Emulateurs = new ObservableCollection<Emulateur>();
        Profiles = new ObservableCollection<Profile>();
        _executableService = executableService;
        _parameterService = parameterService;
        _emulateurService = emulateurService;
    }

    public async void PickPlatforms(Platforms plateforme)
    {
        Emulateurs.Clear();
        ShowEmulateurList = Visibility.Visible;
        plateformeid = plateforme.Id;
        foreach (var emu in await _emulateurService.GetEmulateursForPlatformsAsync(plateforme.Emulators))
        {
            Emulateurs.Add(emu);
        }
    }
    public async void PickProfiles(Emulateur emu)
    {
        Profiles.Clear();
        ShowEmuprofilesPicker = Visibility.Visible;
        var profiles = Emulateurs.SelectMany(x => x.Profiles);
        foreach (var profile in profiles.Where(x=> x.Platforms.Contains(plateformeid)))
        {
            Profiles.Add(profile);
        }
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
        try
        {
            ShowEmuExePicker = Visibility.Collapsed;
            ShowEmulateurList = Visibility.Collapsed;
            ShowEmuprofilesPicker = Visibility.Collapsed;
            sysdisplays.Clear();
            homedisplays.Clear();
            gamesdisplays.Clear();
            gamedetaildisplays.Clear();
            installedPrograms.Clear();
            Platforms.Clear();
            Bck = await _assetService.GetRandomBackground();
            foreach (var skin in _pageSkinService.GetDisplaysForHome()) { homedisplays.Add(skin); }
            Home = await _pageSkinService.GetCurrentDisplayHome();
            foreach (var skin in _pageSkinService.GetDisplaysForSystems()) { sysdisplays.Add(skin); }
            Systems = await _pageSkinService.GetCurrentDisplaySystems();
            foreach (var skin in _pageSkinService.GetDisplaysForGames()) { gamesdisplays.Add(skin); }
            Games = await _pageSkinService.GetCurrentDisplayGames();
            foreach (var skin in _pageSkinService.GetDisplaysForGameDetail()) { gamedetaildisplays.Add(skin); }
            GameDetail = await _pageSkinService.GetCurrentDisplayGameDetail();

            Splashscreenfolder = await _assetService.GetSplashscreenFolder();
            Splashvideofolder = await _assetService.GetSplashvideoFolder();
            Videowaitfolder = await _assetService.GetVideoWaitFolder();
            Backgroundfolder = await _assetService.GetBackgroundFolder();
            foreach(var platform in await _emulateurService.GetPlatformsAsync())
            {
                Platforms.Add(platform);
            }
            //_applicationFinderService.ListInstalledPrograms();
            var prgs = _applicationFinderService.GetFullListInstalledApplication();
            if (prgs != null)
            {
                foreach (var item in prgs.Where(x => !x.ExecutablePath.Contains("System32")).OrderBy(x => x.Name).GroupBy(x => x.ExecutablePath).Select(x => x.First()))
                {
                    installedPrograms.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            //throw;
        }
    }
    public void OnNavigatedFrom()
    {
    }
}
