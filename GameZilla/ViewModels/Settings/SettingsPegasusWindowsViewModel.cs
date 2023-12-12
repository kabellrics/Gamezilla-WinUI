using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GameZilla.Contracts.ViewModels;

namespace GameZilla.ViewModels.Settings;
public partial class SettingsPegasusWindowsViewModel : ObservableRecipient, INavigationAware
{
    public void OnNavigatedFrom() => throw new NotImplementedException();
    public void OnNavigatedTo(object parameter) => throw new NotImplementedException();
}
