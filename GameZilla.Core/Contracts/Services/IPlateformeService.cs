using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services;
public interface IPlateformeService
{
    Task<IEnumerable<Plateforme>> GetPlateformes();
    Task CreatePlateforme(Plateforme item);
}