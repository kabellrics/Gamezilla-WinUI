using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.Core.Models.SteamGridDb;
using Newtonsoft.Json;
using RestSharp;

namespace GameZilla.Core.Services;
public class SteamGridDBService : ISteamGridDBService
{
    private readonly RestClientOptions restClientOptions;
    private readonly IParameterService _parameterService;
    private readonly string apiKey;
    private static readonly object lockObject = new object();
    public SteamGridDBService(IParameterService parameterService)
    {
        _parameterService = parameterService;
        apiKey = _parameterService.GetParameterValue(Models.ParamEnum.SteamGridDBApiKey).Result;
        restClientOptions = new RestClientOptions("http://192.168.1.17:900")
        {
            MaxTimeout = -1,
        };
    }
    public async Task<SGDBGameResult> SearchGamesByName(string gameName)
    {
        try
        {
            var client = new RestClient(restClientOptions);
            var request = new RestRequest($"api/steamgriddb/searchbyname.php?name={gameName}&token={apiKey}", Method.Get);
            //var response = client.Get(request);
            var response = client.Get<SGDBGameResult>(request);
            //return null;
            return response;
        }
        catch (Exception ex)
        {
            //throw;
            return null;
        }
    }

    public async Task<ImgResult> GetCoverBySteamgriddbId(string steamgriddbId)
    {
        var client = new RestClient(restClientOptions);
        var request = new RestRequest($"api/steamgriddb/coverbygameid.php?id={steamgriddbId}&token={apiKey}", Method.Get);
        var response = client.Get<ImgResult>(request);
        return response;
    }

    public async Task<ImgResult> GetLogoBySteamgriddbId(string steamgriddbId)
    {
        var client = new RestClient(restClientOptions);
        var request = new RestRequest($"api/steamgriddb/logobygameid.php?id={steamgriddbId}&token={apiKey}", Method.Get);
        var response =  client.Get<ImgResult>(request);
        return response;
    }

    public async Task<ImgResult> GetHeroBySteamgriddbId(string steamgriddbId)
    {
        var client = new RestClient(restClientOptions);
        var request = new RestRequest($"api/steamgriddb/herobygameid.php?id={steamgriddbId}&token={apiKey}", Method.Get);
        var response =  client.Get<ImgResult>(request);
        return response;
    }
    public async Task<String> GetDefaultCoverUrl(string steamgriddbId)
    {
        var response = await GetCoverBySteamgriddbId(steamgriddbId);
        return response.Data.First().Url ?? string.Empty;
    }
    public async Task<String> GetDefaultHeroUrl(string steamgriddbId)
    {
        var response = await GetHeroBySteamgriddbId(steamgriddbId);
        return response.Data.First().Url ?? string.Empty;
    }
    public async Task<String> GetDefaultLogoUrl(string steamgriddbId)
    {
        var response = await GetLogoBySteamgriddbId(steamgriddbId);
        return response.Data.First().Url ?? string.Empty;
    }
}
