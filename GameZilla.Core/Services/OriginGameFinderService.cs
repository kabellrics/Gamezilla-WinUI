using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameFinder.StoreHandlers.EADesktop.Crypto.Windows;
using GameFinder.StoreHandlers.EADesktop;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.Core.Models.Origin;
using Newtonsoft.Json;
using NexusMods.Paths;
using GameFinder.Common;
using System.Xml.Serialization;

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

    //public async Task<IEnumerable<Executable>> GetEADesktopGameAsync()
    public async Task<IEnumerable<Executable>> GetEADesktopGameAsync()
    {
        var resultlist = new List<Executable>();
        var handler = new EADesktopHandler(FileSystem.Shared, new HardwareInfoProvider());
        var results = handler.FindAllGames();
        var gamesfind = new List<EADesktopGame>();
        foreach (var result in results)
        {
            // using the switch method
            result.Switch(game =>
            {
                Console.WriteLine($"Found {game}");
            }, error =>
            {
                Console.WriteLine(error);
            });

            // using the provided extension functions
            if (result.TryGetGame(out var game))
            {
                Console.WriteLine($"Found {game}");
                gamesfind.Add(game);
            }
            else
            {
                Console.WriteLine(result.AsError());
            }
        }
        if(gamesfind.Count > 0)
        {
            foreach (var item in gamesfind)
            {
                if (!await executableService.ExistinDatabase(item.EADesktopGameId.Value))
                {
                    var ManifestgamePath = Path.Combine(item.BaseInstallPath.GetFullPath(), "__Installer", "installerdata.xml");
                    XmlSerializer serializer = new XmlSerializer(typeof(DiPManifest));
                    try
                    {
                        using (FileStream fs = new FileStream(ManifestgamePath, FileMode.Open))
                        {
                            var ManifestContent = (DiPManifest)serializer.Deserialize(fs);
                            var exe = new Executable();
                            exe.Name = ManifestContent.gameTitles.FirstOrDefault(x => x.locale == "fr_FR").Value;
                            //exe.Name = dipManifest.gameTitles.gameTitle.FirstOrDefault(x => x.Locale == "fr_FR")?.Value;
                            var notrialexe = ManifestContent.runtime.FirstOrDefault(x => x.trial == 0);
                            //var notrialexe = dipManifest.runtime.launcher.FirstOrDefault(x => x.trial == 0);
                            exe.Path = $"{item.BaseInstallPath.GetFullPath()}/{getExeName(notrialexe.filePath)}";
                            exe.StoreId = item.EADesktopGameId.Value;
                            exe.PlateformeId = await parameterService.GetParameterValue(ParamEnum.OriginPlateformeId);
                            if (!await executableService.ExistinDatabaseByPath(exe.Path))
                                resultlist.Add(exe);
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw;
                    } 
                }
            }
        }
        return resultlist;
    }

    private string getExeName(string path)
    {
        int indexOfBracket = path.IndexOf("]");

        if (indexOfBracket >= 0)
        {
            // Extraire le texte après "]"
            string textAfterBracket = path.Substring(indexOfBracket + 1);

            // Maintenant, textAfterBracket contient "bfv.exe"
            return textAfterBracket;
        }
        else
        {
            // La chaîne d'entrée ne contient pas de "]"
            return path;
        }

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
