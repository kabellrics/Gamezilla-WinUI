using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    private ICommand _GoBackCommand;
    public ICommand GoBackCommand => _GoBackCommand ?? (_GoBackCommand = new RelayCommand(GoBack));

    private void GoBack()
    {
        _navigationService.GoBack();
    }
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
        Display = await _pageSkinService.GetCurrentDisplaySystems();
        CurrentDisplayList = new ObservableCollection<ObsContainer>();
        var sys = await _plateformeService.GetPlateformes();
        foreach(var item in sys.OrderBy(x=>x.ShowOrder))
        {
            var container = await _containerBuilder.FromPlateforme(item);
            if(container.IsActif == "1")
            CurrentDisplayList.Add(new ObsContainer(container));
        }
    }
    public void GotoGameList(int selectedIndex)
    {
        var plate = CurrentDisplayList[selectedIndex];
        var param = new NavigateToListGameParameter() { Id = plate.Id, typeListGame = TypeListGame.Plateforme };
        _navigationService.NavigateTo(typeof(ItemListViewModel).FullName!, param);
    }
    public void OnNavigatedFrom()
    {
    }
}
