using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Contracts.Services;
using GameZilla.Core.Contracts.Services;
using Windows.Storage;

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

    public async Task<StorageFile> GetRandomIntroVidéo()
    {
        var folderpath = await GetSplashvideoFolder();
        StorageFolder introFolder = await StorageFolder.GetFolderFromPathAsync(folderpath);
        Random random = new Random();
        IReadOnlyList<StorageFile> introVideos = await introFolder.GetFilesAsync();
        if (introVideos.Count > 0)
        {
            int randomIntroVideoIndex = random.Next(introVideos.Count);
            StorageFile randomIntroVideo = introVideos[randomIntroVideoIndex];
            return randomIntroVideo;
        }
        return null;
    }
    public async Task<StorageFile> GetRandomSplashscreen()
    {
        var folderpath = await GetSplashscreenFolder();
        StorageFolder introFolder = await StorageFolder.GetFolderFromPathAsync(folderpath);
        Random random = new Random();
        IReadOnlyList<StorageFile> introVideos = await introFolder.GetFilesAsync();
        if (introVideos.Count > 0)
        {
            int randomIntroVideoIndex = random.Next(introVideos.Count);
            StorageFile randomIntroVideo = introVideos[randomIntroVideoIndex];
            return randomIntroVideo;
        }
        return null;
    }
    public async Task<StorageFile> GetRandomBackground()
    {
        var folderpath = await GetBackgroundFolder();
        StorageFolder introFolder = await StorageFolder.GetFolderFromPathAsync(folderpath);
        Random random = new Random();
        IReadOnlyList<StorageFile> introVideos = await introFolder.GetFilesAsync();
        if (introVideos.Count > 0)
        {
            int randomIntroVideoIndex = random.Next(introVideos.Count);
            StorageFile randomIntroVideo = introVideos[randomIntroVideoIndex];
            return randomIntroVideo;
        }
        return null;
    }
    public async Task<IEnumerable<StorageFile>> GetWaitingVideo()
    {
        var result = new List<StorageFile>();
        var folderpath = await GetVideoWaitFolder();
        StorageFolder waitingFolder = await StorageFolder.GetFolderFromPathAsync(folderpath);
        Random random = new Random();
        // Récupérer tous les fichiers vidéos du dossier "Waiting"
        IReadOnlyList<StorageFile> waitingVideos = await waitingFolder.GetFilesAsync();
        if (waitingVideos.Count > 0)
        {
            return waitingVideos.OrderBy(v => random.Next()).ToList();
        }
        return null;
    }
}
