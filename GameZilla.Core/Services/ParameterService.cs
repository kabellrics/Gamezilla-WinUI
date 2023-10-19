using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.APIClient;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;

namespace GameZilla.Core.Services;
public class ParameterService : IParameterService
{
    private ParameterClient parameterClient;
    private IEnumerable<Parametre> parameters;
    public ParameterService()
    {
        parameterClient = new ParameterClient();
    }

    private async Task InitValue()
    {
        if (parameters == null) { parameters = await parameterClient.GetParametre(); }
    }

    public async Task<String> GetParameterValue(ParamEnum param)
    {
        if(parameters == null) { await InitValue(); }
        return parameters.FirstOrDefault(x => x.Clé == param.ToString())?.Valeur;
    }
}
