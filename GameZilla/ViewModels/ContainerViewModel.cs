using CommunityToolkit.Mvvm.ComponentModel;
using GameZilla.Contracts.ViewModels;

namespace GameZilla.ViewModels;

public partial class ContainerViewModel : ObservableRecipient, INavigationAware
{
    public ContainerViewModel()
    {
    }
    public void OnNavigatedTo(object parameter)
    {
    }
    public void OnNavigatedFrom()
    {
    }
}
