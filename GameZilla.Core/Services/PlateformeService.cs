using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.APIClient;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;

namespace GameZilla.Core.Services;
public class PlateformeService : IPlateformeService
{
    private PlateformeClient plateformeClient;
    private IEnumerable<Plateforme> plateformes;
    public PlateformeService()
    {
        plateformeClient = new PlateformeClient();
        InitValue();
    }

    private async Task InitValue()
    {
        if (plateformes == null) { plateformes = await plateformeClient.GetPlateformes(); }
    }
    public async Task<IEnumerable<Plateforme>> GetPlateformes()
    {
        if (plateformes == null)
        {
            await InitValue();
        }
        return plateformes.Where(x=>x.IsActif == "1");
    }
}
