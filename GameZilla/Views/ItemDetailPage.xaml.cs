using GameZilla.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace GameZilla.Views;

public sealed partial class ItemDetailPage : Page
{
    public ItemDetailViewModel ViewModel
    {
        get;
    }

    public ItemDetailPage()
    {
        ViewModel = App.GetService<ItemDetailViewModel>();
        InitializeComponent();
    }
}
