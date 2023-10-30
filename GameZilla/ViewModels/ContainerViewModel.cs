using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using GameZilla.Contracts.Services;
using GameZilla.Contracts.ViewModels;
using GameZilla.Core.Contracts.Services;
using GameZilla.ViewModels.Object;

namespace GameZilla.ViewModels;

public partial class ContainerViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IPageSkinService _pageSkinService;
    private readonly IContainerBuilder _containerBuilder;
    private readonly IPlateformeService _plateformeService;
    private String _display;
    public String Display
    {
        get => _display;
        set => SetProperty(ref _display, value);
    }
    public ObservableCollection<ObsContainer> CurrentDisplayList;
    public ContainerViewModel(INavigationService navigationService, IPageSkinService pageSkinService, IContainerBuilder containerBuilder, IPlateformeService plateformeService)
    {
        _navigationService = navigationService;
        _pageSkinService = pageSkinService;
        _containerBuilder = containerBuilder;
        CurrentDisplayList = new ObservableCollection<ObsContainer>();
        _plateformeService = plateformeService;
    }
    public async void OnNavigatedTo(object parameter)
    {
        Display = await _pageSkinService.GetCurrentDisplayHome();
        CurrentDisplayList = new ObservableCollection<ObsContainer>();
        var sys = _plateformeService.GetPlateformes();
        foreach(var item in sys)
        {
            CurrentDisplayList.Add(new ObsContainer(await _containerBuilder.FromPlateforme(item)));
        }
    }
    public void OnNavigatedFrom()
    {
    }
}
