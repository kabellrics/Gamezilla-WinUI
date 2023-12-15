using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Contracts.ViewModels;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.Core.Models.Emulateur;
using GameZilla.Core.Services;
using Microsoft.UI.Xaml;

namespace GameZilla.ViewModels.Settings;
public partial class SettingsEmulateurViewModel : ObservableRecipient, INavigationAware
{
    private readonly IParameterService _parameterService;
    private readonly IEmulateurService _emulateurService;
    private readonly IPlateformeService _plateformeService;
    private readonly IExecutableService _executableService;
    private Visibility _showEmulateurList;
    public Visibility ShowEmulateurList
    {
        get => _showEmulateurList;
        set
        {
            SetProperty(ref _showEmulateurList, value);
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
    private Visibility _showEmuprofilesPicker;
    public Visibility ShowEmuprofilesPicker
    {
        get => _showEmuprofilesPicker;
        set
        {
            SetProperty(ref _showEmuprofilesPicker, value);
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
    private String _selectedExe;
    public String SelectedExe
    {
        get => _selectedExe;
        set
        {
            SetProperty(ref _selectedExe, value);
        }
    }
    public ObservableCollection<Platforms> Platforms;
    public ObservableCollection<Emulateur> Emulateurs;
    public ObservableCollection<Profile> Profiles;
    private ICommand _AddEmuCommand;
    public ICommand AddEmuCommand => _AddEmuCommand ?? (_AddEmuCommand = new RelayCommand(AddEmu));
    public SettingsEmulateurViewModel(IEmulateurService emulateurService, IParameterService parameterService, IPlateformeService plateformeService, IExecutableService executableService)
    {
        Platforms = new ObservableCollection<Platforms>();
        Emulateurs = new ObservableCollection<Emulateur>();
        Profiles = new ObservableCollection<Profile>();
        _emulateurService = emulateurService;
        _parameterService = parameterService;
        _plateformeService = plateformeService;
        _executableService = executableService;
        ShowEmulateurList = Visibility.Collapsed;
        ShowEmuprofilesPicker = Visibility.Collapsed;
        ShowSelectedData = Visibility.Collapsed;
        ShowAddingEmu = Visibility.Collapsed;
    }
    public void OnNavigatedFrom() => throw new NotImplementedException();
    public async void OnNavigatedTo(object parameter)
    {
        ShowEmulateurList = Visibility.Visible;
        ShowEmuprofilesPicker = Visibility.Collapsed;
        ShowSelectedData = Visibility.Collapsed;
        ShowAddingEmu = Visibility.Collapsed;
        Platforms.Clear();
        Emulateurs.Clear();
        Profiles.Clear();
        foreach (var platform in await _emulateurService.GetPlatformsWithoutRetroarcAsync())
        {
            Platforms.Add(platform);
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
        foreach (var profile in profiles.Where(x => x.Platforms.ToList().Contains(plateforme.Id)))
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
        if (plats.Count(x => x.Name == this.plateforme.Name) == 0)
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
}
