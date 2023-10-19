using GameZilla.ViewModels;

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
    }
}
