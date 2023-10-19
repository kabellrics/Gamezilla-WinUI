using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using GameZilla.Core.Models;
using Newtonsoft.Json;
using RestSharp;

namespace GameZilla.Core.APIClient;
public class ParameterClient
{
    public async Task<IEnumerable<Parametre>> GetParametre()
    {

        var options = new RestClientOptions("http://192.168.1.17:900")
        {
            MaxTimeout = -1,
        };
        var client = new RestClient(options);
        var request = new RestRequest("api/parametre/read.php", Method.Get);
        var response = await client.GetAsync<ParameterReponseList>(request);
        return response.body;
    }
}
