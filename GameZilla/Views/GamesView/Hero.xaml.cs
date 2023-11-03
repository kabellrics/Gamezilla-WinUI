using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using GameZilla.ViewModels;
using GameZilla.ViewModels.Object;
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

namespace GameZilla.Views.GamesView;
public sealed partial class Hero : UserControl
{
    public ItemListViewModel ViewModel
    {
        get;
    }
    public Hero()
    {
        this.ViewModel = App.GetService<ItemListViewModel>();
        this.DataContext = ViewModel;
        this.InitializeComponent();
        itemListView.SelectedIndex = 0;
    }

    private void itemListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        var item = itemListView.SelectedItem as ObsItem;
        if (item != null)
        {
            ViewModel.GoToDetail(item.Id); 
        }
    }

    private void itemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var newItem = e.AddedItems.FirstOrDefault();
        itemListView.SelectedItem = newItem;
    }
}
