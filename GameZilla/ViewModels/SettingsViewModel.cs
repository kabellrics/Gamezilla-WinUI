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
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace GameZilla.ViewModels;

public partial class SettingsViewModel : ObservableRecipient, INavigationAware
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly INavigationService _navigationService;
    private readonly IPageSkinService _pageSkinService;
    private readonly IAssetService _assetService;
    private readonly IItemBuilder _itemBuilder;
    private readonly IApplicationFinderService _applicationFinderService;
    private readonly IExecutableService _executableService;
    private readonly INonExecutableService _nonexecutableService;
    private readonly IPlateformeService _plateformeService;
    private readonly IParameterService _parameterService;
    private readonly IEmulateurService _emulateurService;
    private readonly ISteamGridDBService _steamGridDBService;
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
    private ICommand _PickEmuCommand;
    public ICommand PickEmuCommand => _PickEmuCommand ?? (_PickEmuCommand = new RelayCommand<Emulateur>(PickEmu));
    private ICommand _PickProfilesCommand;
    public ICommand PickProfilesCommand => _PickProfilesCommand ?? (_PickProfilesCommand = new RelayCommand<Profile>(PickProfiles));
    private ICommand _AddMultipleAppCommand;
    public ICommand AddMultipleAppCommand => _AddMultipleAppCommand ?? (_AddMultipleAppCommand = new RelayCommand(AddMultipleApplication));
    private ICommand _AddEmuCommand;
    public ICommand AddEmuCommand => _AddEmuCommand ?? (_AddEmuCommand = new RelayCommand(AddEmu));
    private async void SaveParam()
    {
        _assetService.SetSplashscreenFolder(Splashscreenfolder);
        _assetService.SetSplashvideoFolder(Splashvideofolder);
        _assetService.SetVideoWaitFolder(Videowaitfolder);
        _assetService.SetBackgroundFolder(Backgroundfolder);
    }
    private ICommand _GoHomeCommand;
    public ICommand GoHomeCommand => _GoHomeCommand ?? (_GoHomeCommand = new RelayCommand(GoHome));
    private ICommand _ImportNonExecutableCommand;
    public ICommand ImportNonExecutableCommand => _ImportNonExecutableCommand ?? (_ImportNonExecutableCommand = new RelayCommand(ImportNonExecutable));

    private ICommand _SelectALLCommand;
    public ICommand SelectALLCommand => _SelectALLCommand ?? (_SelectALLCommand = new RelayCommand(SelectALL));

    private void SelectALL()
    {
        foreach(var imp in ImportedGames) { imp.IsSelected = !imp.IsSelected; }
    }

    private ICommand _ResolveALLCommand;
    public ICommand ResolveALLCommand => _ResolveALLCommand ?? (_ResolveALLCommand = new RelayCommand(ResolveALL));

    private async void ResolveALL()
    {
        foreach (var imp in ImportedGames) { await imp.Init(); }
    }

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
    private String _selectedExe;
    public String SelectedExe
    {
        get => _selectedExe;
        set
        {
            SetProperty(ref _selectedExe, value);
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
    private Visibility _showSelectedData;
    public Visibility ShowSelectedData
    {
        get => _showSelectedData;
        set
        {
            SetProperty(ref _showSelectedData, value);
        }
    }
    private Visibility _showAddingEmu;
    public Visibility ShowAddingEmu
    {
        get => _showAddingEmu;
        set
        {
            SetProperty(ref _showAddingEmu, value);
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
    public ObservableCollection<ObsItem> LocalEmulators;
    public ObservableCollection<ImportedGame> ImportedGames;
    private Platforms _plateforme;
    public Platforms plateforme
    {
        get => _plateforme;
        set
        {
            SetProperty(ref _plateforme, value);
        }
    }
    private Profile _profile;
    public Profile profile
    {
        get => _profile;
        set
        {
            SetProperty(ref _profile, value);
        }
    }
    private Emulateur _emulateur;
    public Emulateur emulateur
    {
        get => _emulateur;
        set
        {
            SetProperty(ref _emulateur, value);
        }
    }
    private Executable _emulateurToAddGame;
    public Executable EmulateurToAddGame
    {
        get => _emulateurToAddGame;
        set
        {
            SetProperty(ref _emulateurToAddGame, value);
        }
    }
    private Plateforme _plateformeToAddGame;
    public Plateforme PlateformeToAddGame
    {
        get => _plateformeToAddGame;
        set
        {
            SetProperty(ref _plateformeToAddGame, value);
        }
    }
    public SettingsViewModel(IThemeSelectorService themeSelectorService, INavigationService navigationService, IPageSkinService pageSkinService,
        IEmulateurService emulateurService, IAssetService assetService, IApplicationFinderService applicationFinderService, IItemBuilder itemBuilder,
        IExecutableService executableService, INonExecutableService nonExecutableService, IParameterService parameterService, IPlateformeService plateformeService, ISteamGridDBService steamGridDBService)
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
        LocalEmulators = new ObservableCollection<ObsItem>();
        ImportedGames = new ObservableCollection<ImportedGame>();
        _executableService = executableService;
        _parameterService = parameterService;
        _emulateurService = emulateurService;
        _plateformeService = plateformeService;
        _itemBuilder = itemBuilder;
        _steamGridDBService = steamGridDBService;
        _nonexecutableService = nonExecutableService;
    }
    public async Task<string[]> getImageExtension(string emuName)
    {
        return await _emulateurService.GetImageExtensionFromExeEmuName(emuName);
    }
    public async Task InitImportedGames(IEnumerable<string> files)
    {
        ImportedGames.Clear();
        foreach (var file in files)
        {
            ImportedGames.Add(new ImportedGame(_steamGridDBService,file));
        }
    }
    public async void PickPlatforms(Platforms plateforme)
    {
        Emulateurs.Clear();
        Profiles.Clear();
        ShowEmulateurList = Visibility.Visible;
        ShowEmuprofilesPicker = Visibility.Collapsed;
        ShowSelectedData = Visibility.Collapsed;
        ShowAddingEmu = Visibility.Collapsed;
        this.plateforme = plateforme;
        foreach (var emu in await _emulateurService.GetEmulateursForPlatformsAsync(plateforme.Emulators))
        {
            Emulateurs.Add(emu);
        }
    }
    public async void PickEmu(Emulateur emu)
    {
        Profiles.Clear();
        ShowEmuprofilesPicker = Visibility.Visible;
        ShowSelectedData = Visibility.Collapsed;
        ShowAddingEmu = Visibility.Collapsed;
        this.emulateur = emu;
        var profiles = emu.Profiles;
        foreach (var profile in profiles.Where(x=> x.Platforms.ToList().Contains(plateforme.Id)))
        {
            Profiles.Add(profile);
        }
    }
    public async void PickProfiles(Profile profile)
    {
        this.profile = profile;
        ShowSelectedData = Visibility.Visible;
    }
    public async void GetExecutablePath(string path)
    {
        SelectedExe = path;
        ShowAddingEmu = Visibility.Visible;
    }
    private async void AddEmu()
    {
        Executable newemu = new Executable();
        newemu.PlateformeId = await _parameterService.GetParameterValue(ParamEnum.EmulateurPlateformeId);
        newemu.Name = $"{emulateur.Name} - {profile.Name}";
        newemu.Path = SelectedExe;
        newemu.StartParam = profile.StartupArguments;
        var plats = await _plateformeService.GetPlateformes();
        if (plats.Count(x=>x.Name == this.plateforme.Name) == 0)
        {
            var platitem = new Plateforme();
            platitem.Name = this.plateforme.Name;
            platitem.PlateformeTypeId = "2";
            platitem.Fanart = @"uploads\console\background\" + this.plateforme.Id + ".jpg";
            platitem.Logo = @"uploads\console\logo\" + this.plateforme.Id + ".svg";
            platitem.IsActif = "1";
            await _plateformeService.CreatePlateforme(platitem);
        }
        await _executableService.CreateExecutable(newemu);
        _executableService.Reinit();
    }
    private void ImportNonExecutable()
    {
        var toimport = ImportedGames.Where(x => x.IsSelected == true);
        foreach (var item in toimport)
        {
            var nonexeitem = new NonExecutable();
            nonexeitem.Name = item.Name;
            nonexeitem.IsActif = "1";
            nonexeitem.Favorite = "0";
            nonexeitem.Path = item.Path;
            nonexeitem.Logo = item.Logo;
            nonexeitem.Cover = item.Cover;
            nonexeitem.Heroe = item.Hero;
            nonexeitem.ExecutableId = EmulateurToAddGame.Id;
            nonexeitem.PlateformeId = PlateformeToAddGame.Id;
            //_nonexecutableService.CreateNonExecutable(nonexeitem);
        }
    }
    public async void SetEmuForAddingGame(string emuname)
    {
        var exes = await _executableService.GetExecutables();
            EmulateurToAddGame = exes.First(x => x.Name == emuname);

        var platformName = await _emulateurService.GetPlatformsNamefromEmulatorName(emuname);
        var platforms = await _plateformeService.GetPlateformes();
        PlateformeToAddGame = platforms.First(x => x.Name == platformName);
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
            ShowSelectedData = Visibility.Collapsed;
            ShowAddingEmu = Visibility.Collapsed;
            sysdisplays.Clear();
            homedisplays.Clear();
            gamesdisplays.Clear();
            gamedetaildisplays.Clear();
            installedPrograms.Clear();
            Platforms.Clear();
            LocalEmulators.Clear();
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
            foreach(var platform in await _emulateurService.GetPlatformsWithoutRetroarcAsync())
            {
                Platforms.Add(platform);
            }
            var lclemus = await _executableService.GetExecutablesByplatform(await _parameterService.GetParameterValue(ParamEnum.EmulateurPlateformeId));
            foreach(var emu in lclemus)
            {
                LocalEmulators.Add(new ObsItem(_itemBuilder.FromExecutable(emu)));
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
