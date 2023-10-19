using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services;
public interface IPlateformeService
{
    IEnumerable<Plateforme> GetPlateformes();
}