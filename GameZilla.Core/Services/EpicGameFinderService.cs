using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using Newtonsoft.Json.Linq;

namespace GameZilla.Core.Services;
public class EpicGameFinderService : IEpicGameFinderService
{
    private readonly IParameterService parameterService;
    private readonly IExecutableService executableService;
    public EpicGameFinderService(IParameterService parameterService, IExecutableService executableService)
    {
        this.parameterService = parameterService;
        this.executableService = executableService;
    }
    public async IAsyncEnumerable<Executable> GetEpicGame()
    {
        var originPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Epic", "EpicGamesLauncher", "Data", "Manifests");
        var manifestsFiles = Directory.GetFiles(originPath, "*.item", SearchOption.TopDirectoryOnly);
        foreach (var manifestsFile in manifestsFiles)
        {
            var manifestObject = JObject.Parse(File.ReadAllText(manifestsFile));
            var name = (string)manifestObject["DisplayName"];
            var appId = (string)manifestObject["AppName"];
            if (! await executableService.ExistinDatabase(appId))
            {
                Executable game = new Executable();
                game.Name = name;
                game.StoreId = appId;
                game.PlateformeId = await parameterService.GetParameterValue(ParamEnum.EpicPlateformeId);
                game.Path = $"com.epicgames.launcher://apps/{appId}?action=launch&silent=true";
                yield return game;
            }
        }
    }
    public async Task<IEnumerable<Executable>> GetEpicGameAsync()
    {
        var originPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Epic", "EpicGamesLauncher", "Data", "Manifests");
        var manifestsFiles = Directory.GetFiles(originPath, "*.item", SearchOption.TopDirectoryOnly);
        var result = new List<Executable>();
        foreach (var manifestsFile in manifestsFiles)
        {
            var manifestObject = JObject.Parse(File.ReadAllText(manifestsFile));
            var name = (string)manifestObject["DisplayName"];
            var appId = (string)manifestObject["AppName"];
            if (! await executableService.ExistinDatabase(appId))
            {
                Executable game = new Executable();
                game.Name = name;
                game.StoreId = appId;
                game.PlateformeId = await parameterService.GetParameterValue(ParamEnum.EpicPlateformeId);
                game.Path = $"com.epicgames.launcher://apps/{appId}?action=launch&silent=true";
                result.Add(game);
            }
        }
        return result;
    }
}
