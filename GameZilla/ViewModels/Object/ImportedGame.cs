using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.Core.Models.SteamGridDb;

namespace GameZilla.ViewModels.Object;
public class ImportedGame : ObservableObject
{
    private readonly ISteamGridDBService _steamGridDBService;
    public ObservableCollection<SGDBGame> Proposals = new ObservableCollection<SGDBGame>();
    private ICommand _ResolveCommand;
    public ICommand ResolveCommand => _ResolveCommand ?? (_ResolveCommand = new RelayCommand(Resolve));

    public ImportedGame(ISteamGridDBService steamGridDBService, string path)
    {
        _steamGridDBService = steamGridDBService;
        Proposals.Clear();
        Path = path;
        Name = System.IO.Path.GetFileNameWithoutExtension(path);
        ResolveText = "Résoudre";
        //Init(path);
    }

    public async void Resolve()
    {
        await Init();
    }
    public async Task Init()
    {
        Proposals.Clear();
        ResolveText = "Résolution en cours";
        var props = await _steamGridDBService.SearchGamesByName(Name);
        if(props != null)
        {
            foreach (var item in props.Data)
            {
                Proposals.Add(item);
            }
            SteamgridDBID = Proposals.First().Id.ToString();
            Name = Proposals.First().Name;
        }
        ResolveText = "Résolu";
    }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            SetProperty(ref _isSelected, value);
        }
    }

    private string _resolveText;
    public string ResolveText
    {
        get => _resolveText;
        set
        {
            SetProperty(ref _resolveText, value);
        }
    }
    private string _path;
    public string Path
    {
        get => _path;
        set
        {
            SetProperty(ref _path, value);
        }
    }
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            SetProperty(ref _name, value);
        }
    }
    private string _steamgridDBID;
    public string SteamgridDBID
    {
        get => _steamgridDBID;
        set
        {
            SetProperty(ref _steamgridDBID, value);
            ChangeAsset();
        }
    }

    public async void ChangeAsset(SGDBGame game)
    {
        Name = game.Name;
        Cover = await _steamGridDBService.GetDefaultCoverUrl(game.Id.ToString());
        Hero = await _steamGridDBService.GetDefaultHeroUrl(game.Id.ToString());
        Logo = await _steamGridDBService.GetDefaultLogoUrl(game.Id.ToString());
    }
    public async void ChangeAsset()
    {
        Cover = await _steamGridDBService.GetDefaultCoverUrl(SteamgridDBID);
        Hero = await _steamGridDBService.GetDefaultHeroUrl(SteamgridDBID);
        Logo = await _steamGridDBService.GetDefaultLogoUrl(SteamgridDBID);
    }

    private string _cover;
    public string Cover
    {
        get => _cover;
        set
        {
            SetProperty(ref _cover, value);
        }
    }
    private string _hero;
    public string Hero
    {
        get => _hero;
        set
        {
            SetProperty(ref _hero, value);
        }
    }
    private string _logo;
    public string Logo
    {
        get => _logo;
        set
        {
            SetProperty(ref _logo, value);
        }
    }
}
