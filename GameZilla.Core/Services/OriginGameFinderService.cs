using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.Core.Models.Origin;
using Newtonsoft.Json;

namespace GameZilla.Core.Services;
public class OriginGameFinderService : IOriginGameFinderService
{
    private readonly IParameterService parameterService;
    private readonly IExecutableService executableService;
    public OriginGameFinderService(IParameterService parameterService, IExecutableService executableService)
    {
        this.parameterService = parameterService;
        this.executableService = executableService;
    }
    public async IAsyncEnumerable<Executable> GetOriginGame()
    {
        var originPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Origin", "LocalContent");
        if (Directory.Exists(originPath))
        {
            var manifests = Directory.GetFiles(originPath, "*.mfst", SearchOption.AllDirectories);
            foreach (var files in manifests)
            {
                //string gameName;
                string gameId = Path.GetFileNameWithoutExtension(files);
                if (!gameId.StartsWith("Origin"))
                {
                    var match = Regex.Match(gameId, @"^(.*?)(\d+)$");
                    if (!match.Success)
                    {
                        continue;
                    }
                    gameId = match.Groups[1].Value + ":" + match.Groups[2].Value;
                }
                if (gameId.Contains("@"))
                {
                    gameId = gameId.Substring(0, gameId.IndexOf("@"));
                }
                if (!await executableService.ExistinDatabase(gameId))
                {
                    var origindata = GetGameLocalData(gameId);
                    if (origindata != null)
                    {
                        Executable game = new Executable();
                        game.PlateformeId = await parameterService.GetParameterValue(ParamEnum.OriginPlateformeId);
                        game.Name = origindata.localizableAttributes.displayName;
                        game.StoreId = gameId;
                        game.Path = $"origin://launchgame/{gameId}";
                        yield return game;
                    }
                }
            }
        }
    }
    public async Task<IEnumerable<Executable>> GetOriginGameAsync()
    {
        var result = new List<Executable>();
        var originPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Origin", "LocalContent");
        if (Directory.Exists(originPath))
        {
            var manifests = Directory.GetFiles(originPath, "*.mfst", SearchOption.AllDirectories);
            foreach (var files in manifests)
            {
                //string gameName;
                string gameId = Path.GetFileNameWithoutExtension(files);
                if (!gameId.StartsWith("Origin"))
                {
                    var match = Regex.Match(gameId, @"^(.*?)(\d+)$");
                    if (!match.Success)
                    {
                        continue;
                    }
                    gameId = match.Groups[1].Value + ":" + match.Groups[2].Value;
                }
                if (gameId.Contains("@"))
                {
                    gameId = gameId.Substring(0, gameId.IndexOf("@"));
                }
                if (!await executableService.ExistinDatabase(gameId))
                {
                    var origindata = GetGameLocalData(gameId);
                    if (origindata != null)
                    {
                        Executable game = new Executable();
                        game.PlateformeId = await parameterService.GetParameterValue(ParamEnum.OriginPlateformeId);
                        game.Name = origindata.localizableAttributes.displayName;
                        game.StoreId = gameId;
                        game.Path = $"origin://launchgame/{gameId}";
                        result.Add(game);
                    }
                }
            }
        }
        return result;
    }
    private OriginGame GetGameLocalData(string gameId)
    {
        try
        {
            var url = $@"https://api1.origin.com/ecommerce2/public/{gameId}/fr_FR";
            var webClient = new WebClient();
            var stringData = Encoding.UTF8.GetString(webClient.DownloadData(url));
            var data = JsonConvert.DeserializeObject<OriginGame>(stringData);
            return data;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
