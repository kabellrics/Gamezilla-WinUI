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
using GameZilla.Core.Services;
using GameZilla.ViewModels.Object;

namespace GameZilla.ViewModels.Settings;
public partial class SettingsStoreViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISteamGameFinderService _steamGameFinderService;
    private readonly IOriginGameFinderService _originGameFinderService;
    private readonly IEpicGameFinderService _epicGameFinderService;
    private ICommand _SelectALLStoreCommand;
    public ICommand SelectALLStoreCommand => _SelectALLStoreCommand ?? (_SelectALLStoreCommand = new RelayCommand(SelectALLStore));

    private ICommand _ResolveALLStoreCommand;
    public ICommand ResolveALLStoreCommand => _ResolveALLStoreCommand ?? (_ResolveALLStoreCommand = new RelayCommand(ResolveALLStore));
    private ICommand _ImportStoreGameCommand;
    public ICommand ImportStoreGameCommand => _ImportStoreGameCommand ?? (_ImportStoreGameCommand = new RelayCommand(ImportStoreGame));

    public ObservableCollection<ImportedGame> ImportedStoreGames;
    public SettingsStoreViewModel(ISteamGameFinderService steamGameFinderService, IOriginGameFinderService originGameFinderService, IEpicGameFinderService epicGameFinderService)
    {
        ImportedStoreGames = new ObservableCollection<ImportedGame>();
        _steamGameFinderService = steamGameFinderService;
        _originGameFinderService = originGameFinderService;
        _epicGameFinderService = epicGameFinderService;
    }
    private void ImportStoreGame()
    {
    }
    private void SelectALLStore()
    {
        foreach (var imp in ImportedStoreGames) { imp.IsSelected = !imp.IsSelected; }
    }
    private async void ResolveALLStore()
    {
        foreach (var imp in ImportedStoreGames) { await imp.Init(); }
    }
    public void OnNavigatedFrom()
    {
    }
    public async void OnNavigatedTo(object parameter)
    {
        ImportedStoreGames.Clear();
        var epicgames = await _epicGameFinderService.GetEpicGameAsync();
        foreach (var epicgame in epicgames)
            ImportedStoreGames.Add(new ImportedGame(epicgame));
        var origingames = await _originGameFinderService.GetEADesktopGameAsync();
        foreach (var origgame in origingames)
            ImportedStoreGames.Add(new ImportedGame(origgame));
        var steamgames = await _steamGameFinderService.GetSteamGameAsync();
        foreach (var steamgame in steamgames)
            ImportedStoreGames.Add(new ImportedGame(steamgame));
    }
}
