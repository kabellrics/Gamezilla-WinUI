﻿using System.Collections.ObjectModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using GameZilla.Contracts.Services;
using GameZilla.Contracts.ViewModels;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.Services;
using GameZilla.ViewModels.Object;
using Windows.Storage;

namespace GameZilla.ViewModels;

public partial class ItemListViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly IPageSkinService _pageSkinService;
    private readonly IItemBuilder _itemBuilder;
    private readonly IExecutableService _executableService;
    private readonly INonExecutableService _nonexecutableService;
    private readonly IAssetService _assetService;
    private readonly IPlateformeService _plateformeService;
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
    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetProperty(ref _selectedIndex, value);
    }
    private StorageFile _bck;
    public StorageFile Bck
    {
        get => _bck;
        set => SetProperty(ref _bck, value);
    }
    public ObservableCollection<ObsItem> CurrentDisplayList;

    public ItemListViewModel(INavigationService navigationService, IPageSkinService pageSkinService, INonExecutableService nonexecutableService,
        IItemBuilder itemBuilder, IExecutableService executableService, IPlateformeService plateformeService, IAssetService assetService)
    {
        _navigationService = navigationService;
        _pageSkinService = pageSkinService;
        _itemBuilder = itemBuilder;
        _executableService = executableService;
        _plateformeService = plateformeService;
        _assetService = assetService;
        CurrentDisplayList = new ObservableCollection<ObsItem>();
        _nonexecutableService = nonexecutableService;
    }

    public void OnNavigatedTo(object parameter)
    {
        InitializeData(parameter);
    }

    public async void InitializeData(object parameter)
    {
        Display = await _pageSkinService.GetCurrentDisplayGames();
        Bck = await _assetService.GetRandomBackground();
        CurrentDisplayList.Clear();
        if (parameter != null)
        {
            var param = parameter as NavigateToListGameParameter;
            switch (param.typeListGame)
            {
                case TypeListGame.AllGames: LoadAllGames(); break;
                case TypeListGame.Favorite: LoadFavGames(); break;
                case TypeListGame.LastPlayed: LoadLastGames(); break;
                case TypeListGame.NeverPlayed: LoadNevaGames(); break;
                default: LoadPlateformeGames(param.Id); break;
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
        var nonexefavlist = await _nonexecutableService.GetNonExecutables();
        foreach (var nonexe in nonexefavlist)
        {
            CurrentDisplayList.Add(new ObsItem(await _itemBuilder.FromNonExecutable(nonexe)));
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
        var nonexefavlist = await _nonexecutableService.GetNonExecutablesFavorite();
        foreach (var nonexe in nonexefavlist)
        {
            CurrentDisplayList.Add(new ObsItem(await _itemBuilder.FromNonExecutable(nonexe)));
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
        var nonexefavlist = await _nonexecutableService.GetNonExecutablesLastStarted();
        foreach (var nonexe in nonexefavlist)
        {
            CurrentDisplayList.Add(new ObsItem(await _itemBuilder.FromNonExecutable(nonexe)));
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
        var nonexefavlist = await _nonexecutableService.GetNonExecutablesNeverStarted();
        foreach (var nonexe in nonexefavlist)
        {
            CurrentDisplayList.Add(new ObsItem(await _itemBuilder.FromNonExecutable(nonexe)));
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
        var nonexefavlist = await _nonexecutableService.GetNonExecutablesByplatform(plateformeId);
        foreach (var nonexe in nonexefavlist)
        {
            CurrentDisplayList.Add(new ObsItem(await _itemBuilder.FromNonExecutable(nonexe)));
        }
    }
    public void OnNavigatedFrom()
    {
    }
    public async void GoToDetail(string Id)
    {
        try
        {

            _navigationService.NavigateTo(typeof(ItemDetailViewModel).FullName!, Id);
        }
        catch (Exception ex)
        {
            //throw;
        }
    }
}
