using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Contracts.Services;
using GameZilla.Contracts.ViewModels;
using GameZilla.Core.Contracts.Services;
using GameZilla.ViewModels.Object;

namespace GameZilla.ViewModels;

public partial class ItemDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IPageSkinService _pageSkinService;
    private readonly IItemBuilder _itemBuilder;
    private readonly IExecutableService _executableService;
    private ICommand _ToggleFavoriteCommand;
    public ICommand ToggleFavoriteCommand => _ToggleFavoriteCommand ?? (_ToggleFavoriteCommand = new RelayCommand(ToggleFavorite));

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
    private ObsItem _item;
    public ObsItem Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }
    public ItemDetailViewModel(INavigationService navigationService, IPageSkinService pageSkinService, IItemBuilder itemBuilder, IExecutableService executableService)
    {
        _navigationService = navigationService;
        _pageSkinService = pageSkinService;
        _itemBuilder = itemBuilder;
        _executableService = executableService;
    }
    private void ToggleFavorite()
    {
        Item.Favori = Item.Favori  ? false : true;
    }
    public async void OnNavigatedTo(object parameter)
    {
        Display = await _pageSkinService.GetCurrentDisplayGameDetail();
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
