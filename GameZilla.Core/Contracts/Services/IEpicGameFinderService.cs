using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services;
public interface IEpicGameFinderService
{
    IAsyncEnumerable<Executable> GetEpicGame();
    Task<IEnumerable<Executable>> GetEpicGameAsync();
}