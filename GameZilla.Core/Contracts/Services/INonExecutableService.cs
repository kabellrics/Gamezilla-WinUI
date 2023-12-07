using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services
{
    public interface INonExecutableService
    {
        Task CreateNonExecutable(NonExecutable item);
        Task<string> DownloadUrlasset(string url, string type, string namefilewithoutextension);
        Task<bool> ExistinDatabase(string path);
        Task<bool> ExistinDatabaseByPath(string path);
        Task<IEnumerable<NonExecutable>> GetNonExecutablesFavorite();
        Task<IEnumerable<NonExecutable>> GetNonExecutables();
        Task<NonExecutable> GetNonExecutablesByID(string Id);
        Task<IEnumerable<NonExecutable>> GetNonExecutablesByplatform(string plateformeId);
        Task<IEnumerable<NonExecutable>> GetNonExecutablesLastStarted();
        Task<IEnumerable<NonExecutable>> GetNonExecutablesMostStarted();
        Task<IEnumerable<NonExecutable>> GetNonExecutablesNeverStarted();
        void Reinit();
        Task UpdateNonExecutable(NonExecutable item);
    }
}