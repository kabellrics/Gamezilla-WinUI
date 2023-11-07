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
public sealed partial class GridView : UserControl
{
    public ContainerViewModel ViewModel
    {
        get;
    }
    public GridView()
    {
        this.ViewModel = App.GetService<ContainerViewModel>();
        this.DataContext = ViewModel;
        this.InitializeComponent();
        SetFocusToFirstGridViewItem();
    }
    
    private void SetFocusToFirstGridViewItem()
    {
        GridViewItem gvi = this.StyledGrid.ContainerFromIndex(0) as GridViewItem;
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
                        gvi = this.StyledGrid.ContainerFromIndex(0) as GridViewItem;
                        if (gvi != null)
                        {
                            gvi.IsSelected = true;
                            gvi.Focus(FocusState.Programmatic);
                        }
                    });
        }
    }

    private void MaxItemsWrapGrid_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var wrap = (ItemsWrapGrid)sender;
        wrap.ItemWidth = wrap.ActualWidth / 4.2;
    }

    private void StyledGrid_ItemClick(object sender, ItemClickEventArgs e)
    {
        ViewModel.GotoGameList(StyledGrid.SelectedIndex);
    }
}
