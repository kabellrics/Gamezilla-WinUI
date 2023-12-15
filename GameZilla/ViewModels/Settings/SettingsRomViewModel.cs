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
using GameZilla.Core.Services;
using GameZilla.ViewModels.Object;

namespace GameZilla.ViewModels.Settings;
public partial class SettingsRomViewModel : ObservableRecipient, INavigationAware
{
    private readonly IExecutableService _executableService;
    private readonly IEmulateurService _emulateurService;
    private readonly IPlateformeService _plateformeService;
    private readonly INonExecutableService _nonexecutableService;
    private readonly IParameterService _parameterService;
    private readonly IItemBuilder _itemBuilder;
    private ICommand _SelectALLCommand;
    public ICommand SelectALLCommand => _SelectALLCommand ?? (_SelectALLCommand = new RelayCommand(SelectALL));

    private ICommand _ResolveALLCommand;
    public ICommand ResolveALLCommand => _ResolveALLCommand ?? (_ResolveALLCommand = new RelayCommand(ResolveALL));
    private ICommand _ImportNonExecutableCommand;
    public ICommand ImportNonExecutableCommand => _ImportNonExecutableCommand ?? (_ImportNonExecutableCommand = new RelayCommand(ImportNonExecutable));

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
    public ObservableCollection<ObsItem> LocalEmulators;
    public ObservableCollection<ImportedGame> ImportedGames;
    public SettingsRomViewModel()
    {
        _emulateurService = App.GetService<IEmulateurService>();
        _plateformeService = App.GetService<IPlateformeService>();
        _executableService = App.GetService<IExecutableService>();
        _nonexecutableService = App.GetService<INonExecutableService>();
        _parameterService = App.GetService<IParameterService>();
        _itemBuilder = App.GetService<IItemBuilder>();
        ImportedGames = new ObservableCollection<ImportedGame>();
        LocalEmulators = new ObservableCollection<ObsItem>();
    }
    public SettingsRomViewModel(IEmulateurService emulateurService, IPlateformeService plateformeService,
    IExecutableService executableService,
    INonExecutableService nonexecutableService, IParameterService parameterService, IItemBuilder itemBuilder)
    {
        _emulateurService = emulateurService;
        _plateformeService = plateformeService;
        _executableService = executableService;
        ImportedGames = new ObservableCollection<ImportedGame>();
        LocalEmulators = new ObservableCollection<ObsItem>();
        _nonexecutableService = nonexecutableService;
        _parameterService = parameterService;
        _itemBuilder = itemBuilder;
    }
    public void OnNavigatedFrom() => throw new NotImplementedException();
    public async void OnNavigatedTo(object parameter)
    {
        LocalEmulators.Clear();
        ImportedGames.Clear();
        var lclemus = await _executableService.GetExecutablesByplatform(await _parameterService.GetParameterValue(ParamEnum.EmulateurPlateformeId));
        foreach (var emu in lclemus)
        {
            LocalEmulators.Add(new ObsItem(_itemBuilder.FromExecutable(emu)));
        }
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
            ImportedGames.Add(new ImportedGame(file));
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
    private void SelectALL()
    {
        foreach (var imp in ImportedGames) { imp.IsSelected = !imp.IsSelected; }
    }
    private async void ResolveALL()
    {
        foreach (var imp in ImportedGames) { await imp.Init(); }
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
            _nonexecutableService.CreateNonExecutable(nonexeitem);
        }
    }


}
