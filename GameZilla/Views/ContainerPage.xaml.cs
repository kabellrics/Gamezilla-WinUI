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
