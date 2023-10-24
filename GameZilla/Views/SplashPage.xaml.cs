using GameZilla.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Graphics.Display;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.Playlists;

namespace GameZilla.Views;

public sealed partial class SplashPage : Page
{
    public SplashViewModel ViewModel
    {
        get;
    }
    private bool firstvideoplay;
    private bool secondvideoplay;
    public SplashPage()
    {
        ViewModel = App.GetService<SplashViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
        this.KeyDown += Page_KeyDown;
        this.Tapped += SplashPage_Tapped;
        this.Focus(Microsoft.UI.Xaml.FocusState.Programmatic);
    }

    private void SplashPage_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        ViewModel.NavigateToHome();
    }

    protected async override void OnNavigatedFrom(NavigationEventArgs e)
    {
        player.MediaPlayer.Pause();
    } 
    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        firstvideoplay = false;
        secondvideoplay = false;
        ViewModel.Loaded();
        MediaPlaybackList mPlaybackList = new MediaPlaybackList();
        mPlaybackList.CurrentItemChanged += MPlaybackList_CurrentItemChanged;
        await foreach (var item in ViewModel.GetVideoIntro())
        {
            mPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromStorageFile(item)));
        }
        var _mediaPlayer = new MediaPlayer();
        _mediaPlayer.Source = mPlaybackList;
        player.SetMediaPlayer(_mediaPlayer);

        //var videos = await ViewModel.GetVideoIntroAsync();
        //foreach (var item in videos)
        //{
        //    mPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromStorageFile(item)));
        //}
        //player.Source = mPlaybackList;
    }

    private void MPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
    {
        if(!firstvideoplay&&!secondvideoplay) { firstvideoplay = true; }
        else if(firstvideoplay&&!secondvideoplay) { ViewModel.IntroVideoFinish(); secondvideoplay = true; }
    }

    private void Page_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        ViewModel.NavigateToHome();
    }
}
