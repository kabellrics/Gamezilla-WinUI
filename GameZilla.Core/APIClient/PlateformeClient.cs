using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.Models;
using RestSharp;

namespace GameZilla.Core.APIClient;
public class PlateformeClient
{
    public async Task<IEnumerable<Plateforme>> GetPlateformes()
    {

        var options = new RestClientOptions("http://192.168.1.17:900")
        {
            MaxTimeout = -1,
        };
        var client = new RestClient(options);
        var request = new RestRequest("api/plateforme/read.php", Method.Get);
        var response = await client.GetAsync<PlateformeResponseList>(request);
        return response.body;
    }
}
