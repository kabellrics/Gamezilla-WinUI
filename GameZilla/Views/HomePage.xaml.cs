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
        ResourceDictionary resourceDictionary = new ResourceDictionary();
        if(await FileExistsAsync($"Styles/Skin/{styleKey}.xaml"))
        {
            resourceDictionary.Source = new Uri($"Styles/Skin/{styleKey}.xaml");
            // Assurez-vous de vider les anciens styles s'il y en a.
            if (this.Resources.MergedDictionaries.Count > 0)
            {
                this.Resources.MergedDictionaries.Clear();
            }

            // Ajoutez le nouveau ResourceDictionary à la collection de ressources de la page.
            this.Resources.MergedDictionaries.Add(resourceDictionary);
        }
    }
    private async Task<bool> FileExistsAsync(string filePath)
    {
        try
        {
            // Obtenez le fichier depuis l'emplacement spécifié.
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("/ms-appx:///" + filePath));
            return true; // Le fichier existe.
        }
        catch
        {
            return false; // Le fichier n'existe pas.
        }
    }

}
