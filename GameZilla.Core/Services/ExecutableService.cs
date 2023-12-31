﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.APIClient;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using RestSharp.Serializers.Xml;

namespace GameZilla.Core.Services;
public class ExecutableService : IExecutableService
{
    private readonly ExecutableClient executableClient;
    private IEnumerable<Executable> executables;
    public ExecutableService()
    {
        executableClient = new ExecutableClient();
    }

    private async Task InitValue()
    {
        if (executables == null) { executables = await executableClient.GetExecutables(); }
    }
    public void Reinit()
    {
        executables = null;
    }
    public async Task<IEnumerable<Executable>> GetExecutables()
    {
        if(executables == null)
        {
            await InitValue();
        }
        return executables.Where(x => x.IsActif == "1");
    }

    public async Task<IEnumerable<Executable>> GetExecutablesNeverStarted()
    {
        if (executables == null)
        {
            await InitValue();
        }
        return executables.Where(x => x.IsActif == "1" && int.Parse(x.NbStart) == 0).OrderByDescending(x=>x.NbStart);
    }
    public async Task<IEnumerable<Executable>> GetExecutablesMostStarted()
    {
        if (executables == null)
        {
            await InitValue();
        }
        return executables.Where(x => x.IsActif == "1" && int.Parse(x.NbStart) > 0).OrderByDescending(x => x.NbStart).Take(10);
    }
    public async Task<IEnumerable<Executable>> GetExecutablesLastStarted()
    {
        if (executables == null)
        {
            await InitValue();
        }
        return executables.Where(x => x.IsActif == "1").OrderByDescending(x=>x.LastStartDate).Take(10);
    }
    public async Task<IEnumerable<Executable>> GetExecutablesFavorite()
    {
        if (executables == null)
        {
            await InitValue();
        }
        return executables.Where(x => x.IsActif == "1" && x.Favorite == "1");
    }

    public async Task<IEnumerable<Executable>> GetExecutablesByplatform(string plateformeId)
    {
        if (executables == null)
        {
            await InitValue();
        }
        return executables.Where(x => x.IsActif == "1" && x.PlateformeId == plateformeId);
    }
    public async Task<Executable> GetExecutablesByID(string Id)
    {
        if (executables == null)
        {
            await InitValue();
        }
        return executables.First(x => x.IsActif == "1" && x.Id == Id);
    }
    public async Task<bool> ExistinDatabase(string storeId)
    {
        if (executables == null)
        {
            await InitValue();
        }
        return executables.Any(x => x.StoreId == storeId);
    }
    public async Task<bool> ExistinDatabaseByPath(string path)
    {
        if (executables == null)
        {
            await InitValue();
        }
        return executables.Any(x => x.Path == path);
    }
    public async Task<String> DownloadUrlasset(string url, string type, string namefilewithoutextension)
    {
        return await executableClient.DownloadUrlasset(url, type, namefilewithoutextension);
    }
    public async Task CreateExecutable(Executable item)
    {
        await executableClient.CreateExecutable(item);
    }
    public async Task UpdateExecutable(Executable item)
    {
        await executableClient.UpdateExecutable(item);
    }
}
