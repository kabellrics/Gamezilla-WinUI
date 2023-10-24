using CommunityToolkit.Mvvm.DependencyInjection;
using GameZilla.Activation;
using GameZilla.Contracts.Services;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Services;
using GameZilla.Helpers;
using GameZilla.Models;
using GameZilla.Services;
using GameZilla.ViewModels;
using GameZilla.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

namespace GameZilla;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar
    {
        get; set;
    }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IExecutableService, ExecutableService>();
            services.AddSingleton<IPlateformeService, PlateformeService>();
            services.AddSingleton<IParameterService, ParameterService>();
            services.AddSingleton<ISteamGameFinderService, SteamGameFinderService>();
            services.AddSingleton<IOriginGameFinderService, OriginGameFinderService>();
            services.AddSingleton<IEpicGameFinderService, EpicGameFinderService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<ItemDetailViewModel>();
            services.AddTransient<ItemDetailPage>();
            services.AddTransient<ItemListDetailViewModel>();
            services.AddTransient<ItemListDetailPage>();
            services.AddTransient<ItemListViewModel>();
            services.AddTransient<ItemListPage>();
            services.AddTransient<ContainerViewModel>();
            services.AddTransient<ContainerPage>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<HomePage>();
            services.AddTransient<SplashViewModel>();
            services.AddTransient<SplashPage>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);
    }

    private async Task LoadSkin()
    {
        var settingsService = App.GetService<ILocalSettingsService>();
        var skin = await settingsService.ReadSettingAsync<String>("Skin");
        if (string.IsNullOrEmpty(skin))
        {
            skin = "Basic";
        }
        await settingsService.SaveSettingAsync("Skin", skin);
        var appResources = Application.Current.Resources;
        var mergedDictionaries = appResources.MergedDictionaries.Where(x => x is ResourceDictionary);
        var basicStyle = mergedDictionaries.FirstOrDefault(x => x.Source.ToString().Contains("Basic"));
        ResourceDictionary desiredStyle = new ResourceDictionary();
        if (skin != "Basic")
            desiredStyle = mergedDictionaries.FirstOrDefault(x => x.Source.ToString().Contains(skin));
        var dictionariesToRemove = Application.Current.Resources.MergedDictionaries
        .OfType<ResourceDictionary>()
        .Where(dictionary => dictionary.Source?.OriginalString.Contains("Skin") == true)
        .ToList();

        foreach (var dictionary in dictionariesToRemove)
        {
            Application.Current.Resources.MergedDictionaries.Remove(dictionary);
        }
        Application.Current.Resources.MergedDictionaries.Add(basicStyle);
        if (skin != "Basic")
            Application.Current.Resources.MergedDictionaries.Add(desiredStyle);
    }
}
