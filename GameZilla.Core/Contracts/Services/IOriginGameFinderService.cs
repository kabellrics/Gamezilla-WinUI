using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services;
public interface IOriginGameFinderService
{
    IAsyncEnumerable<Executable> GetOriginGame();
    Task<IEnumerable<Executable>> GetOriginGameAsync();
    Task<IEnumerable<Executable>> GetEADesktopGameAsync();
}