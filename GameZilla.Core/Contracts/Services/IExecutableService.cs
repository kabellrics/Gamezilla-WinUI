using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services;
public interface IExecutableService
{
    Task<IEnumerable<Executable>> GetExecutables();
    Task<IEnumerable<Executable>> GetExecutablesByplatform(string plateformeId);
    Task<bool> ExistinDatabase(string storeId);
    Task<String> DownloadUrlasset(string url, string type, string namefilewithoutextension);
    Task CreateExecutable(Executable item);
    Task<IEnumerable<Executable>> GetExecutablesMostStarted();
    Task<IEnumerable<Executable>> GetExecutablesFavorite();
    Task<IEnumerable<Executable>> GetExecutablesNeverStarted();
    Task<IEnumerable<Executable>> GetExecutablesLastStarted();
    Task<Executable> GetExecutablesByID(string plateformeId);
}