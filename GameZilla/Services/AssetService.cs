using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Contracts.Services;
using GameZilla.Core.Contracts.Services;

namespace GameZilla.Services;
public class AssetService : IAssetService
{
    private readonly ILocalSettingsService _options;
    private readonly string GamezillaDocFolder;
    public AssetService(ILocalSettingsService parameterService)
    {
        _options = parameterService;
        var docpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        GamezillaDocFolder = Path.Combine(docpath, "Gamezilla");
        Directory.CreateDirectory(GamezillaDocFolder);
    }

    public async Task<string> GetSplashscreenFolder()
    {
        var value = await _options.ReadSettingAsync<string>("splashscreenfolder");
        if (string.IsNullOrEmpty(value))
        {
            value = Path.Combine(GamezillaDocFolder, "Splashscreen");
        }
        SetSplashscreenFolder(value);
        Directory.CreateDirectory(value);
        return value;
    }
    public async void SetSplashscreenFolder(string value)
    {
        await _options.SaveSettingAsync<string>("splashscreenfolder", value);
    }
    public async Task<string> GetSplashvideoFolder()
    {
        var value = await _options.ReadSettingAsync<string>("splashvideofolder");
        if (string.IsNullOrEmpty(value))
        {
            value = Path.Combine(GamezillaDocFolder, "Splashvideo");
        }
        SetSplashvideoFolder(value);
        Directory.CreateDirectory(value);
        return value;
    }
    public async void SetSplashvideoFolder(string value)
    {
        await _options.SaveSettingAsync<string>("splashvideofolder", value);
    }
    public async Task<string> GetVideoWaitFolder()
    {
        var value = await _options.ReadSettingAsync<string>("videowaitfolder");
        if (string.IsNullOrEmpty(value))
        {
            value = Path.Combine(GamezillaDocFolder, "VideoWait");
        }
        SetVideoWaitFolder(value);
        Directory.CreateDirectory(value);
        return value;
    }
    public async void SetVideoWaitFolder(string value)
    {
        await _options.SaveSettingAsync<string>("videowaitfolder", value);
    }
    public async Task<string> GetBackgroundFolder()
    {
        var value = await _options.ReadSettingAsync<string>("backgroundfolder");
        if (string.IsNullOrEmpty(value))
        {
            value = Path.Combine(GamezillaDocFolder, "Background");
        }
        SetVideoWaitFolder(value);
        Directory.CreateDirectory(value);
        return value;
    }
    public async void SetBackgroundFolder(string value)
    {
        await _options.SaveSettingAsync<string>("backgroundfolder", value);
    }
}
