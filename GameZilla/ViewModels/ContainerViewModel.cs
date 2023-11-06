using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Contracts.Services;
using GameZilla.Contracts.ViewModels;
using GameZilla.Core.Contracts.Services;
using GameZilla.Services;
using GameZilla.ViewModels.Object;
using Windows.Storage;

namespace GameZilla.ViewModels;

public partial class ContainerViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IPageSkinService _pageSkinService;
    private readonly IContainerBuilder _containerBuilder;
    private readonly IPlateformeService _plateformeService;
    private readonly IAssetService _assetService;
    private ICommand _GoBackCommand;
    public ICommand GoBackCommand => _GoBackCommand ?? (_GoBackCommand = new RelayCommand(GoBack));

    private void GoBack()
    {
        _navigationService.GoBack();
    }
    private ICommand _GoHomeCommand;
    public ICommand GoHomeCommand => _GoHomeCommand ?? (_GoHomeCommand = new RelayCommand(GoHome));

    private void GoHome()
    {
        _navigationService.NavigateTo(typeof(HomeViewModel).FullName!);
    }
    private String _display;
    public String Display
    {
        get => _display;
        set => SetProperty(ref _display, value);
    }
    private StorageFile _bck;
    public StorageFile Bck
    {
        get => _bck;
        set => SetProperty(ref _bck, value);
    }
    public ObservableCollection<ObsContainer> CurrentDisplayList;
    public ContainerViewModel(INavigationService navigationService, IPageSkinService pageSkinService, IContainerBuilder containerBuilder, IPlateformeService plateformeService,IAssetService assetService)
    {
        _navigationService = navigationService;
        _pageSkinService = pageSkinService;
        _containerBuilder = containerBuilder;
        CurrentDisplayList = new ObservableCollection<ObsContainer>();
        _plateformeService = plateformeService;
        _assetService = assetService;
    }
    public void OnNavigatedTo(object parameter)
    {
        //InitializeData();
    }

    public async void InitializeData()
    {
        Display = await _pageSkinService.GetCurrentDisplaySystems();
        Bck = await _assetService.GetRandomBackground();
        CurrentDisplayList.Clear();
        var sys = await _plateformeService.GetPlateformes();
        foreach (var item in sys.OrderBy(x => x.ShowOrder))
        {
            var container = await _containerBuilder.FromPlateforme(item);
            if (container.IsActif == "1")
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
