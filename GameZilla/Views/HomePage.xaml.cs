using GameZilla.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace GameZilla.Views;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        LoadResourceDictionary("Hero");
        InitializeComponent();
    }
    private async void LoadResourceDictionary(string styleKey)
    {
        ResourceDictionary resourceDictionary = GetResourceDictgionnary(styleKey);
        if(resourceDictionary != null)
        {
            // Assurez-vous de vider les anciens styles s'il y en a.
            if (this.Resources.MergedDictionaries.Count > 0)
            {
                this.Resources.MergedDictionaries.Where(x => x is ResourceDictionary).Clear();
            }

            // Ajoutez le nouveau ResourceDictionary à la collection de ressources de la page.
            this.Resources.MergedDictionaries.Add(resourceDictionary);
        }
    }

    private ResourceDictionary GetResourceDictgionnary(string styleKey)
    {
        var appResources = Application.Current.Resources;
        var mergedDictionaries = appResources.MergedDictionaries.Where(x => x is ResourceDictionary);
        return mergedDictionaries.FirstOrDefault(x => x.Source.ToString().Contains(styleKey));
    }
}
