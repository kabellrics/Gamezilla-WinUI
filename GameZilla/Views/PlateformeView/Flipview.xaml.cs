using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using GameZilla.ViewModels;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GameZilla.Views.PlateformeView;
public sealed partial class Flipview : UserControl
{
    public ContainerViewModel ViewModel
    {
        get;
    }
    public Flipview()
    {
        this.ViewModel = App.GetService<ContainerViewModel>();
        this.DataContext = ViewModel;
        Loaded += Flipview_Loaded;
        this.InitializeComponent();
        //SetFocusToFirstGridViewItem();
    }

    private void Flipview_Loaded(object sender, RoutedEventArgs e)
    {

        this.flipview.Focus(FocusState.Programmatic);
        this.flipview.Focus(FocusState.Keyboard);
        //SetFocusToFirstGridViewItem();
    }

    private void SetFocusToFirstGridViewItem()
    {
        FlipViewItem gvi = this.flipview.ContainerFromIndex(0) as FlipViewItem;
        if (gvi != null)
        {
            gvi.IsSelected = true;
            gvi.Focus(FocusState.Programmatic);
        }
        else
        {
            var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
            dispatcherQueue.TryEnqueue((Microsoft.UI.Dispatching.DispatcherQueuePriority)DispatcherQueuePriority.Low,
                    () => {
                        gvi = this.flipview.ContainerFromIndex(0) as FlipViewItem;
                        if (gvi != null)
                        {
                            gvi.IsSelected = true;
                            gvi.Focus(FocusState.Programmatic);
                            gvi.Focus(FocusState.Keyboard);
                        }
                    });
        }
    }

    private void FlipView_Tapped(object sender, TappedRoutedEventArgs e)
    {
        var flip = (FlipView)sender;
        ViewModel.GotoGameList(flip.SelectedIndex);
    }
}
