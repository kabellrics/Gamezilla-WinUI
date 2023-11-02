using Windows.Storage;

namespace GameZilla.Services;

public interface IAssetService
{
    Task<string> GetBackgroundFolder();
    Task<string> GetSplashscreenFolder();
    Task<string> GetSplashvideoFolder();
    Task<string> GetVideoWaitFolder();
    void SetBackgroundFolder(string value);
    void SetSplashscreenFolder(string value);
    void SetSplashvideoFolder(string value);
    void SetVideoWaitFolder(string value);
    Task<StorageFile> GetRandomIntroVidéo();
    Task<StorageFile> GetRandomSplashscreen();
    Task<StorageFile> GetRandomBackground();
    Task<IEnumerable<StorageFile>> GetWaitingVideo();
}