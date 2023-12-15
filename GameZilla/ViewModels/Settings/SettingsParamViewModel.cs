using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Contracts.ViewModels;
using GameZilla.Services;

namespace GameZilla.ViewModels.Settings;
public partial class SettingsParamViewModel : ObservableRecipient, INavigationAware
{
    private readonly IAssetService _assetService;
    private ICommand _SaveParamCommand;
    public ICommand SaveParamCommand => _SaveParamCommand ?? (_SaveParamCommand = new RelayCommand(SaveParam));
    private String _splashscreenfolder;
    public String Splashscreenfolder
    {
        get => _splashscreenfolder;
        set
        {
            SetProperty(ref _splashscreenfolder, value);
        }
    }
    private String _splashvideofolder;
    public String Splashvideofolder
    {
        get => _splashvideofolder;
        set
        {
            SetProperty(ref _splashvideofolder, value);
        }
    }
    private String _videowaitfolder;
    public String Videowaitfolder
    {
        get => _videowaitfolder;
        set
        {
            SetProperty(ref _videowaitfolder, value);
        }
    }
    private String _backgroundfolder;
    public String Backgroundfolder
    {
        get => _backgroundfolder;
        set
        {
            SetProperty(ref _backgroundfolder, value);
        }
    }
    public SettingsParamViewModel(IAssetService assetService)
    {
        _assetService = assetService;   
    }
    public void OnNavigatedFrom() => throw new NotImplementedException();
    public async void OnNavigatedTo(object parameter)
    {
        Splashscreenfolder = await _assetService.GetSplashscreenFolder();
        Splashvideofolder = await _assetService.GetSplashvideoFolder();
        Videowaitfolder = await _assetService.GetVideoWaitFolder();
        Backgroundfolder = await _assetService.GetBackgroundFolder();
    }
    private async void SaveParam()
    {
        _assetService.SetSplashscreenFolder(Splashscreenfolder);
        _assetService.SetSplashvideoFolder(Splashvideofolder);
        _assetService.SetVideoWaitFolder(Videowaitfolder);
        _assetService.SetBackgroundFolder(Backgroundfolder);
    }
    public async void OpenFolderPicker(string obj, string folder)
    {
        if (folder != null)
        {
            switch (obj)
            {
                case "0":
                    Splashscreenfolder = folder; break;
                case "1":
                    Splashvideofolder = folder; break;
                case "2":
                    Videowaitfolder = folder; break;
                case "3":
                    Backgroundfolder = folder; break;
            }
        }
    }
}
