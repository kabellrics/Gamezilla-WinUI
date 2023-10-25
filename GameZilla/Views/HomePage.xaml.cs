using GameZilla.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace GameZilla.Views;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        this.Loaded += HomePage_Loaded;
        InitializeComponent();
        ViewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == "Display")
            {
                UpdateVisulaState();
            }
        };
    }

    private void HomePage_Loaded(object sender, RoutedEventArgs e)
    {
        ViewModel.Loaded();
    }

    private void UpdateVisulaState()
    {
       VisualStateManager.GoToState(this, ViewModel.Display, true);
    }
}
