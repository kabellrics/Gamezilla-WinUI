using GameZilla.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace GameZilla.Views;

public sealed partial class ItemListPage : Page
{
    public ItemListViewModel ViewModel
    {
        get;
    }

    public ItemListPage()
    {
        ViewModel = App.GetService<ItemListViewModel>();
        InitializeComponent();
    }
}
