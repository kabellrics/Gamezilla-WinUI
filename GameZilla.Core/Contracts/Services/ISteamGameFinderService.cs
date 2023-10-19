using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services;
public interface ISteamGameFinderService
{
    IAsyncEnumerable<Executable> GetSteamGame();
    Task<IEnumerable<Executable>> GetSteamGameAsync();
    Task<Executable> GetSteamInfos(Executable game);
}