using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using GameZilla.ViewModels;
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
