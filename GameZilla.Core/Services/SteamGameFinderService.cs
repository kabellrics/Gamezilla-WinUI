using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Gameloop.Vdf;
using System.Text;
using Gameloop.Vdf.Linq;
using System.Threading.Tasks;
using GameZilla.Core.Contracts.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using GameZilla.Core.Models;
using GameZilla.Core.Models.Steam;

namespace GameZilla.Core.Services;
public class SteamGameFinderService : ISteamGameFinderService
{
    private readonly IParameterService parameterService;
    private readonly IExecutableService executableService;
    public SteamGameFinderService(IParameterService parameterService, IExecutableService executableService)
    {
        this.parameterService = parameterService;
        this.executableService = executableService;
    }
    public async IAsyncEnumerable<Executable> GetSteamGame()
    {
        string steamfolder;
        var key64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam";
        var key32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam";
        if (Environment.Is64BitOperatingSystem)
        {
            steamfolder = (string)Microsoft.Win32.Registry.GetValue(key64, "InstallPath", string.Empty);
        }
        else
        {
            steamfolder = (string)Microsoft.Win32.Registry.GetValue(key32, "InstallPath", string.Empty);
        }

        if (steamfolder != null)
        {
            List<string> foldersTosearch = new List<string>();
            foldersTosearch.Add(Path.Combine(steamfolder, "steamapps"));
            VProperty volvo = VdfConvert.Deserialize(File.ReadAllText(Path.Combine(steamfolder, "steamapps", "libraryfolders.vdf")));
            var childs = volvo.Value.Children();
            foreach (var child in childs)
            {
                var childKV = (VProperty)child;
                var childValueKV = childKV.Value;
                var pathchildKV = childValueKV.FirstOrDefault();
                if (pathchildKV != null)
                {
                    //if (Directory.Exists(((VProperty)child).Value.ToString()))
                    if (Directory.Exists(((VProperty)pathchildKV).Value.ToString()))
                    {
                        foldersTosearch.Add(Path.Combine(((VProperty)pathchildKV).Value.ToString(), "steamapps"));
                    }
                }
            }
            List<string> appmanifestfiles = new List<string>();
            foreach (string foldertoSeek in foldersTosearch)
            {
                appmanifestfiles.AddRange(Directory.GetFiles(foldertoSeek, "appmanifest_*.acf").ToList());
            }

            foreach (var file in appmanifestfiles)
            {
                dynamic appfile = VdfConvert.Deserialize(File.ReadAllText(file));
                if (! await executableService.ExistinDatabase(appfile.Value.appid.Value))
                {
                    Executable game = new Executable();
                    game.StoreId = appfile.Value.appid.Value;
                    game.Name = appfile.Value.name.Value;
                    game.PlateformeId = await parameterService.GetParameterValue(ParamEnum.SteamPlateformeId);
                    game = await GetSteamInfos(game);
                    yield return game;
                    //gamesfind.Add(game);
                }
            }
            //return gamesfind;
        }
        //else
        //    return null;
    }
    public async Task<IEnumerable<Executable>> GetSteamGameAsync()
    {
        var result = new List<Executable>();
        string steamfolder;
        var key64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam";
        var key32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam";
        if (Environment.Is64BitOperatingSystem)
        {
            steamfolder = (string)Microsoft.Win32.Registry.GetValue(key64, "InstallPath", string.Empty);
        }
        else
        {
            steamfolder = (string)Microsoft.Win32.Registry.GetValue(key32, "InstallPath", string.Empty);
        }

        if (steamfolder != null)
        {
            List<string> foldersTosearch = new List<string>();
            foldersTosearch.Add(Path.Combine(steamfolder, "steamapps"));
            VProperty volvo = VdfConvert.Deserialize(File.ReadAllText(Path.Combine(steamfolder, "steamapps", "libraryfolders.vdf")));
            var childs = volvo.Value.Children();
            foreach (var child in childs)
            {
                var childKV = (VProperty)child;
                var childValueKV = childKV.Value;
                var pathchildKV = childValueKV.FirstOrDefault();
                if (pathchildKV != null)
                {
                    //if (Directory.Exists(((VProperty)child).Value.ToString()))
                    if (Directory.Exists(((VProperty)pathchildKV).Value.ToString()))
                    {
                        foldersTosearch.Add(Path.Combine(((VProperty)pathchildKV).Value.ToString(), "steamapps"));
                    }
                }
            }
            List<string> appmanifestfiles = new List<string>();
            foreach (string foldertoSeek in foldersTosearch)
            {
                appmanifestfiles.AddRange(Directory.GetFiles(foldertoSeek, "appmanifest_*.acf").ToList());
            }

            foreach (var file in appmanifestfiles)
            {
                dynamic appfile = VdfConvert.Deserialize(File.ReadAllText(file));
                if (! await executableService.ExistinDatabase(appfile.Value.appid.Value))
                {
                    Executable game = new Executable();
                    game.StoreId = appfile.Value.appid.Value;
                    game.Name = appfile.Value.name.Value;
                    game.PlateformeId = await parameterService.GetParameterValue(ParamEnum.SteamPlateformeId);
                    game = await GetSteamInfos(game);
                    result.Add(game);
                }
            }
        }
        return result;
    }
    public async Task<Executable> GetSteamInfos(Executable game/*, Emulator emu*/)
    {
        //var plateforme = dbService.GetSysteme(emu.SystemeID);
        //string imgfolder = Path.Combine(FileJSONService.appSettings.AppSettingsFolder, "media", plateforme.Shortname);
        var urlinfo = await parameterService.GetParameterValue(ParamEnum.SteamUrlInfos);
        var URLInfosPath = urlinfo.Replace("%STEAMID%", game.StoreId);
        try
        {
            string jsoninfos;
            using (WebClient wc = new WebClient())
            {
                jsoninfos = wc.DownloadString(URLInfosPath);
            }
            JObject json = JObject.Parse(jsoninfos);
            var datajson = json[game.StoreId.ToString()]["data"];
            if (datajson != null)
            {
                var data = JsonConvert.DeserializeObject<Data>(datajson.ToString());

                game.Path = $"steam://rungameid/{game.StoreId.ToString()}";
                game.Favorite = "0";
                game.IsActif = "1";
                game = await Task.Run(async () =>
                {
                    var logopath = await parameterService.GetParameterValue(ParamEnum.SteamLogoPath);
                    var logotask = executableService.DownloadUrlasset(logopath.Replace("%STEAMID%", game.StoreId), "logo", game.StoreId);
                    var coverpath = await parameterService.GetParameterValue(ParamEnum.SteamBoxPath);
                    var covertask = executableService.DownloadUrlasset(coverpath.Replace("%STEAMID%", game.StoreId), "cover", game.StoreId);
                    var heropath = await parameterService.GetParameterValue(ParamEnum.SteamHeroPath);
                    var herotask = executableService.DownloadUrlasset(heropath.Replace("%STEAMID%", game.StoreId), "hero", game.StoreId);
                    var videopath = RemoveTextAfterMp4(data.movies.FirstOrDefault().mp4.max);
                    var videotask = executableService.DownloadUrlasset(videopath, "video", game.StoreId);

                    await Task.WhenAll(logotask, covertask, herotask, videotask);

                    game.Logo = await logotask;
                    game.Cover = await covertask;
                    game.Heroe = await herotask;
                    game.Video = await videotask;
                    return game;
                });
            }
        }
        catch (Exception ex)
        {
            throw new Exception();
        }
        return game;
    }
    public string RemoveTextAfterMp4(string input)
    {
        int index = input.IndexOf("mp4", StringComparison.OrdinalIgnoreCase);

        if (index >= 0)
        {
            return input.Substring(0, index + 3); // +3 pour inclure "mp4"
        }
        else
        {
            return input; // Retourne la chaîne d'origine si "mp4" n'est pas trouvé
        }
    }
}
