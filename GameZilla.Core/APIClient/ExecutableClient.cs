using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.Models;
using Newtonsoft.Json;
using RestSharp;

namespace GameZilla.Core.APIClient;
public class ExecutableClient
{
    private readonly RestClientOptions restClientOptions;

    public ExecutableClient()
    {
        restClientOptions = new RestClientOptions("http://192.168.1.17:900")
        {
            MaxTimeout = -1,
        };
    }
    public async Task<IEnumerable<Executable>> GetExecutables()
    {
        var client = new RestClient(restClientOptions);
        var request = new RestRequest("api/executable/read.php", Method.Get);
        //var response = await client.GetAsync<ExecutableResponseList>(request);
        RestResponse response = await client.ExecuteAsync(request);
        var resultobj = JsonConvert.DeserializeObject<ExecutableResponseList>(response.Content);
        return resultobj.body;
    }

    public async Task<String> DownloadUrlasset(string url, string type, string namefilewithoutextension)
    {
        var client = new RestClient(restClientOptions);
        var request = new RestRequest("/api/executable/uploadurlasset.php", Method.Get)
            .AddParameter("url", url)
            .AddParameter("type", type)
            .AddParameter("newname", namefilewithoutextension);
        RestResponse response = await client.ExecuteAsync(request);
        return response.Content;
    }

    public async Task CreateExecutable(Executable item)
    {
        var jsonItem = JsonConvert.SerializeObject(item);
        var client = new RestClient(restClientOptions);
        var request = new RestRequest("/api/executable/create.php", Method.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddParameter("text/plain", jsonItem, ParameterType.RequestBody);
        await client.ExecuteAsync(request);
    }
}
