using GameZilla.Core.APIClient;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZilla.Core.Services
{
    public class NonExecutableService : INonExecutableService
    {
        private readonly NonExecutableClient nonexecutableClient;
        private IEnumerable<NonExecutable> nonexecutables;
        public NonExecutableService()
        {
            nonexecutableClient = new NonExecutableClient();
        }

        private async Task InitValue()
        {
            if (nonexecutables == null) { nonexecutables = await nonexecutableClient.GetExecutables(); }
        }
        public void Reinit()
        {
            nonexecutables = null;
        }
        public async Task<IEnumerable<NonExecutable>> GetNonExecutables()
        {
            if (nonexecutables == null)
            {
                await InitValue();
            }
            return nonexecutables.Where(x => x.IsActif == "1");
        }

        public async Task<IEnumerable<NonExecutable>> GetNonExecutablesNeverStarted()
        {
            if (nonexecutables == null)
            {
                await InitValue();
            }
            return nonexecutables.Where(x => x.IsActif == "1" && int.Parse(x.NbStart) == 0).OrderByDescending(x => x.NbStart);
        }
        public async Task<IEnumerable<NonExecutable>> GetNonExecutablesMostStarted()
        {
            if (nonexecutables == null)
            {
                await InitValue();
            }
            return nonexecutables.Where(x => x.IsActif == "1" && int.Parse(x.NbStart) > 0).OrderByDescending(x => x.NbStart).Take(10);
        }
        public async Task<IEnumerable<NonExecutable>> GetNonExecutablesLastStarted()
        {
            if (nonexecutables == null)
            {
                await InitValue();
            }
            return nonexecutables.Where(x => x.IsActif == "1").OrderByDescending(x => x.LastStartDate).Take(10);
        }
        public async Task<IEnumerable<NonExecutable>> GetNonExecutablesFavorite()
        {
            if (nonexecutables == null)
            {
                await InitValue();
            }
            return nonexecutables.Where(x => x.IsActif == "1" && x.Favorite == "1");
        }

        public async Task<IEnumerable<NonExecutable>> GetNonExecutablesByplatform(string plateformeId)
        {
            if (nonexecutables == null)
            {
                await InitValue();
            }
            return nonexecutables.Where(x => x.IsActif == "1" && x.PlateformeId == plateformeId);
        }
        public async Task<NonExecutable> GetNonExecutablesByID(string Id)
        {
            if (nonexecutables == null)
            {
                await InitValue();
            }
            return nonexecutables.First(x => x.IsActif == "1" && x.Id == Id);
        }
        public async Task<bool> ExistinDatabase(string path)
        {
            if (nonexecutables == null)
            {
                await InitValue();
            }
            return nonexecutables.Any(x => x.Path == path);
        }
        public async Task<bool> ExistinDatabaseByPath(string path)
        {
            if (nonexecutables == null)
            {
                await InitValue();
            }
            return nonexecutables.Any(x => x.Path == path);
        }
        public async Task<String> DownloadUrlasset(string url, string type, string namefilewithoutextension)
        {
            return await nonexecutableClient.DownloadUrlasset(url, type, namefilewithoutextension);
        }
        public async Task CreateNonExecutable(NonExecutable item)
        {
            await nonexecutableClient.CreateNonExecutable(item);
        }
        public async Task UpdateNonExecutable(NonExecutable item)
        {
            await nonexecutableClient.UpdateNonExecutable(item);
        }
    }
}
