 using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Contracts.Services;
using GameZilla.Core.Contracts.Services;
using GameZilla.FrontModel;

namespace GameZilla.ViewModels;

public partial class HomeViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly IExecutableService _executableService;
    private ICommand _LoadedCommand;
    public ICommand LoadedCommand => _LoadedCommand ?? (_LoadedCommand = new RelayCommand(Loaded));

    public IEnumerable<GamezillaMenuItem> Menus;
    public HomeViewModel(INavigationService navigationService, IExecutableService executableService)
    {
        _navigationService = navigationService;
        _executableService = executableService;
    }
    public void Loaded()
    {
        InitData();
    }

    private async void InitData()
    {
    }
}
