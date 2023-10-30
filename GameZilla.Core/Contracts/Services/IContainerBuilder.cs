using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services;
public interface IContainerBuilder
{
    Task<Container> FromPlateforme(Plateforme plateforme);
}