using GameZilla.ViewModels;
using Microsoft.UI.Xaml;
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
        this.DataContext = ViewModel;
        UpdateVisulaState();
        ViewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == "Display")
            {
                UpdateVisulaState();
            }
        };
    }
    private void UpdateVisulaState()
    {
        var transit = VisualStateManager.GoToState(this, ViewModel.Display, false);
    }
}
