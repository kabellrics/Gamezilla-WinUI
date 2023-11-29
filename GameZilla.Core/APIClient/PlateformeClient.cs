using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.Models;
using Newtonsoft.Json;
using RestSharp;

namespace GameZilla.Core.APIClient;
public class PlateformeClient
{
    private readonly RestClientOptions restClientOptions;
    public PlateformeClient()
    {
        restClientOptions = new RestClientOptions("http://192.168.1.17:900")
        {
            MaxTimeout = -1,
        };
    }
    public async Task<IEnumerable<Plateforme>> GetPlateformes()
    {
        var client = new RestClient(restClientOptions);
        var request = new RestRequest("api/plateforme/read.php", Method.Get);
        var response = await client.GetAsync<PlateformeResponseList>(request);
        return response.body;
    }
    public async Task CreatePlateforme(Plateforme item)
    {
        var jsonItem = JsonConvert.SerializeObject(item);
        var client = new RestClient(restClientOptions);
        var request = new RestRequest("/api/plateforme/create.php", Method.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddParameter("text/plain", jsonItem, ParameterType.RequestBody);
        var response = await client.ExecuteAsync(request);
    }
}
