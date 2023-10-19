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

    private async void InitValue()
    {
        if (plateformes == null) { plateformes = await plateformeClient.GetPlateformes(); }
    }
    public IEnumerable<Plateforme> GetPlateformes()
    {
        if (plateformes == null)
        {
            InitValue();
        }
        return plateformes;
    }
}
