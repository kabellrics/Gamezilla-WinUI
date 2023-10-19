﻿using CommunityToolkit.WinUI.UI.Animations;

using GameZilla.Contracts.Services;
using GameZilla.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace GameZilla.Views;

public sealed partial class ItemListDetailPage : Page
{
    public ItemListDetailViewModel ViewModel
    {
        get;
    }

    public ItemListDetailPage()
    {
        ViewModel = App.GetService<ItemListDetailViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.NavigationMode == NavigationMode.Back)
        {
            var navigationService = App.GetService<INavigationService>();

            if (ViewModel.Item != null)
            {
                navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
