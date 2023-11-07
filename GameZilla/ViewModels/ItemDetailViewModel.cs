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

public partial class ItemDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IPageSkinService _pageSkinService;
    private readonly IItemBuilder _itemBuilder;
    private readonly IExecutableService _executableService;
    private readonly IAssetService _assetService;
    private ICommand _ToggleFavoriteCommand;
    public ICommand ToggleFavoriteCommand => _ToggleFavoriteCommand ?? (_ToggleFavoriteCommand = new RelayCommand(ToggleFavorite));

    private ICommand _StartCommand;
    public ICommand StartCommand => _StartCommand ?? (_StartCommand = new RelayCommand(Start));

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
    private ObsItem _item;
    public ObsItem Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }
    private StorageFile _bck;
    public StorageFile Bck
    {
        get => _bck;
        set => SetProperty(ref _bck, value);
    }
    public ItemDetailViewModel(INavigationService navigationService, IPageSkinService pageSkinService, IItemBuilder itemBuilder, IExecutableService executableService,IAssetService assetService)
    {
        _navigationService = navigationService;
        _pageSkinService = pageSkinService;
        _itemBuilder = itemBuilder;
        _executableService = executableService;
        _assetService = assetService;
    }
    private async void Start()
    {
        Item.LastStart = DateTime.Now.ToString();
        Item.NbStart += 1;
        
    }
    private void ToggleFavorite()
    {
        Item.Favori = Item.Favori  ? false : true;
    }
    public void OnNavigatedTo(object parameter)
    {
         InitializeData(parameter);
    }

    public async void InitializeData(object parameter)
    {
        Display = await _pageSkinService.GetCurrentDisplayGameDetail();
        Bck = await _assetService.GetRandomBackground();
        if (parameter != null)
        {
            var currentexe = await _executableService.GetExecutablesByID(parameter.ToString());
            Item = new ObsItem(_itemBuilder.FromExecutable(currentexe));
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
