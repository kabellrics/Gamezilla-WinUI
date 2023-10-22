using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Contracts.Services;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using Microsoft.UI.Dispatching;
using Windows.Storage;

namespace GameZilla.ViewModels;

public partial class SplashViewModel : ObservableRecipient
{

    private readonly IExecutableService executableService;
    private readonly ISteamGameFinderService steamGameFinderService;
    private readonly IOriginGameFinderService originGameFinderService;
    private readonly IEpicGameFinderService epicGameFinderService;
    private readonly INavigationService _navigationService;
    private ICommand _LoadedCommand;

    private Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;
    public List<StorageFile> playlist = new List<StorageFile>();
    private String _labelTraitement;
    public String LabelTraitement
    {
        get => _labelTraitement;
        set => SetProperty(ref _labelTraitement, value);
    }
    private bool _isLoadingFinish;
    public bool IsLoadingFinish
    {
        get => _isLoadingFinish;
        set => SetProperty(ref _isLoadingFinish, value);
    }
    private bool _isFirstVideoFinish;
    public bool IsFirstVideoFinish
    {
        get => _isFirstVideoFinish;
        set => SetProperty(ref _isFirstVideoFinish, value);
    }
    private bool _showContinuMessage;
    public bool ShowContinuMessage
    {
        get => _showContinuMessage;
        set => SetProperty(ref _showContinuMessage, value);
    }
    public ICommand LoadedCommand => _LoadedCommand ?? (_LoadedCommand = new RelayCommand(Loaded));

    public SplashViewModel(INavigationService navigationService,IExecutableService executableService, ISteamGameFinderService steamGameFinderService, IOriginGameFinderService originGameFinderService, IEpicGameFinderService epicGameFinderService)
    {
        LabelTraitement = "Traitement en cours";
        _navigationService = navigationService;
        this.executableService = executableService;
        this.steamGameFinderService = steamGameFinderService;
        this.originGameFinderService = originGameFinderService;
        this.epicGameFinderService = epicGameFinderService;
        IsFirstVideoFinish = false;
        IsLoadingFinish = false;
        ShowContinuMessage = false;
        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
    }

    public void Loaded()
    {
        InitStoreData();
    }
    public void NavigateToHome()
    {
        if(ShowContinuMessage)
            //_navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
            _navigationService.NavigateTo(typeof(HomeViewModel).FullName!);
    }
    public async IAsyncEnumerable<StorageFile> GetVideoIntro()
    {
        string fullPath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledPath,"Assets");
        string introPath = Path.Combine(fullPath, "Intro");
        string waitingPath = Path.Combine(introPath, "Waiting");
        StorageFolder introFolder = await StorageFolder.GetFolderFromPathAsync(introPath);
        StorageFolder waitingFolder = await StorageFolder.GetFolderFromPathAsync(waitingPath);
        Random random = new Random();

        // Récupérer tous les fichiers vidéos du dossier "Intro"
        IReadOnlyList<StorageFile> introVideos = await introFolder.GetFilesAsync();

        // Sélectionner une vidéo aléatoire du dossier "Intro" (s'il y en a)
        if (introVideos.Count > 0)
        {
            int randomIntroVideoIndex = random.Next(introVideos.Count);
            StorageFile randomIntroVideo = introVideos[randomIntroVideoIndex];
            yield return randomIntroVideo;
        }

        // Récupérer tous les fichiers vidéos du dossier "Waiting"
        IReadOnlyList<StorageFile> waitingVideos = await waitingFolder.GetFilesAsync();

        // Mélanger les fichiers vidéos du dossier "Waiting" dans un ordre aléatoire
        waitingVideos = waitingVideos.OrderBy(v => random.Next()).ToList();

        // Ajouter les fichiers vidéos du dossier "Waiting" à la séquence
        foreach (var video in waitingVideos)
        {
            yield return video;
        }
    }
    public async Task<IEnumerable<StorageFile>> GetVideoIntroAsync()
    {
        var result = new List<StorageFile>();
        string fullPath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledPath,"Assets");
        string introPath = Path.Combine(fullPath, "Intro");
        string waitingPath = Path.Combine(introPath, "Waiting");
        StorageFolder introFolder = await StorageFolder.GetFolderFromPathAsync(introPath);
        StorageFolder waitingFolder = await StorageFolder.GetFolderFromPathAsync(waitingPath);
        Random random = new Random();

        // Récupérer tous les fichiers vidéos du dossier "Intro"
        IReadOnlyList<StorageFile> introVideos = await introFolder.GetFilesAsync();

        // Sélectionner une vidéo aléatoire du dossier "Intro" (s'il y en a)
        if (introVideos.Count > 0)
        {
            int randomIntroVideoIndex = random.Next(introVideos.Count);
            StorageFile randomIntroVideo = introVideos[randomIntroVideoIndex];
            result.Add(randomIntroVideo);
        }

        // Récupérer tous les fichiers vidéos du dossier "Waiting"
        IReadOnlyList<StorageFile> waitingVideos = await waitingFolder.GetFilesAsync();

        // Mélanger les fichiers vidéos du dossier "Waiting" dans un ordre aléatoire
        waitingVideos = waitingVideos.OrderBy(v => random.Next()).ToList();

        // Ajouter les fichiers vidéos du dossier "Waiting" à la séquence
        foreach (var video in waitingVideos)
        {
            result.Add(video);
        }
        return result;
    }
    public void IntroVideoFinish()
    {
        IsFirstVideoFinish = true;
        ShowContinue();
    }

    private void ShowContinue()
    {
        this.dispatcherQueue.TryEnqueue(() =>
        {
            if (IsFirstVideoFinish && IsLoadingFinish) { ShowContinuMessage = true; }
        });
    }

    private async void InitStoreData()
    {
        var task = LoadStoreGamesAsync();
        await Task.WhenAll(task);
        IsLoadingFinish = true;
        ShowContinue();
        LabelTraitement = "Appuyez sur n'importe quel touche pour continuer";
    }

    private async Task LoadStoreGamesAsync()
    {
        LabelTraitement = "Traitement en cours";
        var steamtask = LoadSteamGamesAsync();
        var origintask = LoadOriginGamesAsync();
        var epictask = LoadEpicGamesAsync();
        await Task.WhenAll(steamtask,origintask,epictask);
    }

    private async Task LoadEpicGamesAsync()
    {
        var epicgames = await epicGameFinderService.GetEpicGameAsync();
        foreach (var epicGame in epicgames)
        {
            if (epicGame != null) await executableService.CreateExecutable(epicGame);
        }
        //await foreach (var item in epicGameFinderService.GetEpicGame())
        //{
        //    if (item != null) await executableService.CreateExecutable(item);
        //}
    }

    private async Task LoadOriginGamesAsync()
    {
        var origingames = await originGameFinderService.GetOriginGameAsync();
        foreach (var originGame in origingames)
        {
            if (originGame != null) await executableService.CreateExecutable(originGame);
        }
        //await foreach (var item in originGameFinderService.GetOriginGame())
        //{
        //    if (item != null) await executableService.CreateExecutable(item);
        //}
    }

    private async Task LoadSteamGamesAsync()
    {
        var steamgames = await steamGameFinderService.GetSteamGameAsync();
        foreach (var steamGame in steamgames)
        {
            if (steamGame != null) await executableService.CreateExecutable(steamGame);
        }
        //await foreach(var item in steamGameFinderService.GetSteamGame())
        //{
        //    if(item != null)await executableService.CreateExecutable(item);
        //}
    }
}
