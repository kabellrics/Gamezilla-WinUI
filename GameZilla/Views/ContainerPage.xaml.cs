using GameZilla.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GameZilla.Views;

public sealed partial class ContainerPage : Page
{
    public ContainerViewModel ViewModel
    {
        get;
    }

    public ContainerPage()
    {
        ViewModel = App.GetService<ContainerViewModel>();
        NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        this.Loaded += ContainerPage_Loaded;
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

    private void ContainerPage_Loaded(object sender, RoutedEventArgs e)
    {
        //ViewModel.InitializeData();
    }

    private void UpdateVisulaState()
    {
        if(string.IsNullOrEmpty(ViewModel.Display))
            VisualStateManager.GoToState(this, "Flip", false);
        else
            VisualStateManager.GoToState(this, ViewModel.Display, false);
    }

    private void Page_Loading(FrameworkElement sender, object args)
    {
        ViewModel.InitializeData();
    }

    private void Page_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if(e.Key == Windows.System.VirtualKey.Back || e.Key == Windows.System.VirtualKey.Escape|| e.Key == Windows.System.VirtualKey.GamepadB)
        {
            ViewModel.GoBackCommand.Execute(null);
        }
        else if(e.Key == Windows.System.VirtualKey.GamepadMenu) { ViewModel.GoHomeCommand.Execute(null); }
    }
}
