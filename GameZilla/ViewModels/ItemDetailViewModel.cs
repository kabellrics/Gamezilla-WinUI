using CommunityToolkit.Mvvm.ComponentModel;
using GameZilla.Contracts.ViewModels;

namespace GameZilla.ViewModels;

public partial class ItemDetailViewModel : ObservableRecipient, INavigationAware
{
    public ItemDetailViewModel()
    {
    }
    public void OnNavigatedTo(object parameter)
    {
    }
    public void OnNavigatedFrom()
    {
    }
}
