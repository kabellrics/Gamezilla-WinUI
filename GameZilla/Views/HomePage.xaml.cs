using GameZilla.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
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
       this.ViewModel = App.GetService<HomeViewModel>();
        this.DataContext = ViewModel;
        this.Loaded += HomePage_Loaded;
        InitializeComponent();
        UpdateVisulaState();
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
        var transit = VisualStateManager.GoToState(this, ViewModel.Display, false);
    }
}
