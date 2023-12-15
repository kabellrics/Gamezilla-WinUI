using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameZilla.Contracts.ViewModels;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.Core.Services;
using GameZilla.ViewModels.Object;

namespace GameZilla.ViewModels.Settings;
public partial class SettingsApplicationViewModel : ObservableRecipient, INavigationAware
{
    private readonly IExecutableService _executableService;
    private readonly IParameterService _parameterService;
    private readonly IApplicationFinderService _applicationFinderService;
    public ObservableCollection<InstalledProgram> installedPrograms;
    private ICommand _AddMultipleAppCommand;
    public ICommand AddMultipleAppCommand => _AddMultipleAppCommand ?? (_AddMultipleAppCommand = new RelayCommand(AddMultipleApplication));
    public SettingsApplicationViewModel(IParameterService parameterService,IExecutableService executableService, IApplicationFinderService applicationFinderService)
    {
        installedPrograms = new ObservableCollection<InstalledProgram>();
        _parameterService = parameterService;
        _executableService = executableService;
        _applicationFinderService = applicationFinderService;
    }
    public void OnNavigatedFrom() => throw new NotImplementedException();
    public void OnNavigatedTo(object parameter)
    {
        installedPrograms.Clear();
        var prgs = _applicationFinderService.GetFullListInstalledApplication();
        if (prgs != null)
        {
            foreach (var item in prgs.Where(x => !x.ExecutablePath.Contains("System32")).OrderBy(x => x.Name).GroupBy(x => x.ExecutablePath).Select(x => x.First()))
            {
                installedPrograms.Add(item);
            }
        }
    }
    public async void AddMultipleApplication()
    {
        var selectedprg = installedPrograms.Where(x => x.IsSelected);
        foreach (var exepath in selectedprg)
        {
            var exe = new Executable() { };
            exe.Name = exepath.Name;
            exe.Path = exepath.ExecutablePath;
            exe.PlateformeId = await _parameterService.GetParameterValue(ParamEnum.ApplicationPlateformeId); ;
            await _executableService.CreateExecutable(exe);
        }
        _executableService.Reinit();
    }
    public async void AddApplication(string exepath)
    {
        var exe = new Executable() { };
        exe.Name = Path.GetFileNameWithoutExtension(exepath);
        exe.Path = exepath;
        exe.PlateformeId = await _parameterService.GetParameterValue(ParamEnum.ApplicationPlateformeId); ;
        await _executableService.CreateExecutable(exe);
        _executableService.Reinit();
    }
}
