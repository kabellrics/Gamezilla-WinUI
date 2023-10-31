using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using GameZilla.Contracts.Services;
using GameZilla.Contracts.ViewModels;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.ViewModels.Object;

namespace GameZilla.ViewModels;

public partial class ItemListViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IPageSkinService _pageSkinService;
    private readonly IItemBuilder _itemBuilder;
    private readonly IExecutableService _executableService;
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
    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetProperty(ref _selectedIndex, value);
    }
    public ObservableCollection<ObsItem> CurrentDisplayList;

    public ItemListViewModel(INavigationService navigationService, IPageSkinService pageSkinService, IItemBuilder itemBuilder, IExecutableService executableService, IPlateformeService plateformeService)
    {
        _navigationService = navigationService;
        _pageSkinService = pageSkinService;
        _itemBuilder = itemBuilder;
        _executableService = executableService;
        _plateformeService = plateformeService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Display = await _pageSkinService.GetCurrentDisplayGames();
        CurrentDisplayList = new ObservableCollection<ObsItem>();
        if(parameter != null)
        {
            var param = parameter as NavigateToListGameParameter;
            switch (param.typeListGame)
            {
                case TypeListGame.AllGames: LoadAllGames(); break;
                case TypeListGame.Favorite: LoadFavGames(); break;
                case TypeListGame.LastPlayed: LoadLastGames(); break;
                case TypeListGame.NeverPlayed: LoadNevaGames(); break;
                default: LoadPlateformeGames(param.Id);break;
            }
        }
    }
    private async Task LoadAllGames()
    {
        CurrentDisplayList.Clear();
        var favlist = await _executableService.GetExecutables();
        foreach (var favitem in favlist)
        {
            CurrentDisplayList.Add(new ObsItem(_itemBuilder.FromExecutable(favitem)));
        }
    }
    private async Task LoadFavGames()
    {
        CurrentDisplayList.Clear();
        var favlist = await _executableService.GetExecutablesFavorite();
        foreach (var favitem in favlist)
        {
            CurrentDisplayList.Add(new ObsItem(_itemBuilder.FromExecutable(favitem)));
        }
    }
    private async Task LoadLastGames()
    {
        CurrentDisplayList.Clear();
        var lastlist = await _executableService.GetExecutablesLastStarted();
        foreach (var lastitem in lastlist)
        {
            CurrentDisplayList.Add(new ObsItem(_itemBuilder.FromExecutable(lastitem)));
        }
    }
    private async Task LoadNevaGames()
    {
        CurrentDisplayList.Clear();
        var neverlist = await _executableService.GetExecutablesNeverStarted();
        foreach (var neveritem in neverlist)
        {
            CurrentDisplayList.Add(new ObsItem(_itemBuilder.FromExecutable(neveritem)));
        }
    }
    private async Task LoadPlateformeGames(string plateformeId)
    {
        CurrentDisplayList.Clear();
        var list = await _executableService.GetExecutablesByplatform(plateformeId);
        foreach (var item in list)
        {
            CurrentDisplayList.Add(new ObsItem(_itemBuilder.FromExecutable(item)));
        }
    }
    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void OnItemClick(SampleOrder? clickedItem)
    {

    }
}
