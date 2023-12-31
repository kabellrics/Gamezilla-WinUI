﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.APIClient;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using GameZilla.Core.Models.Emulateur;
using YamlDotNet.Serialization;

namespace GameZilla.Core.Services;
public class EmulateurService : IEmulateurService
{
    private List<Platforms> platforms;
    private List<Emulateur> emulateurs;

    private readonly IParameterService _parameterService;
    public EmulateurService(IParameterService parameterService)
    {
        _parameterService = parameterService;
        platforms = new List<Platforms>();
        emulateurs = new List<Emulateur>();
        Initialize();
    }

    private async void Initialize()
    {
        await InitialzePlatforms();
        await InitializeEmulateurs();
    }

    private async Task InitializeEmulateurs()
    {

        try
        {
            var emulatorsFilePath = Path.Combine(AppContext.BaseDirectory, "Models", "Emulateur", "Emulation", "Emulators");
            var yamlfiles = FindYamlFiles(emulatorsFilePath);
            foreach (var yamlfile in yamlfiles)
            {
                try
                {
                    string yamlcontent = File.ReadAllText(yamlfile);
                    var deserializer = new DeserializerBuilder().Build();
                    var emu = deserializer.Deserialize<Emulateur>(yamlcontent);
                    emulateurs.Add(emu);
                }
                catch(Exception ex)
                { }
            }
        }
        catch (Exception ex)
        {
            //throw;
        }
    }
    private List<string> FindYamlFiles(string folder)
    {
        List<string> yamlFiles = new List<string>();

        // Recherche tous les fichiers YAML dans le dossier actuel
        foreach (var file in Directory.GetFiles(folder, "*.yaml"))
        {
            yamlFiles.Add(file);
        }

        // Parcours récursif de tous les sous-dossiers
        foreach (var subfolder in Directory.GetDirectories(folder))
        {
            // Ajoute les fichiers YAML trouvés dans le sous-dossier
            yamlFiles.AddRange(FindYamlFiles(subfolder));
        }

        return yamlFiles;
    }
    private async Task InitialzePlatforms()
    {
        try
        {
            var platformFilePath = Path.Combine(AppContext.BaseDirectory, "Models", "Emulateur", "Emulation", "Platforms.yaml");
            string yamlcontent = File.ReadAllText(platformFilePath);
            var deserializer = new DeserializerBuilder().Build();
            platforms = deserializer.Deserialize<List<Platforms>>(yamlcontent);
            //platforms = Platforms.FromJson(yamlcontent).ToList();
        }
        catch (Exception ex)
        {
            //throw;
        }
    }
    public async Task<string[]> GetImageExtensionFromExeEmuName(string emuName)
    {
        if (emulateurs == null)
        {
            await InitEmulateur();
        }
        var args = emuName.Split(" - ");
        var emu = emulateurs.FirstOrDefault(x=>x.Name == args[0]);
        if (emu != null)
        {
            var prof = emu.Profiles.FirstOrDefault(x => x.Name == args[1]);
            if (prof != null)
            {
                return prof.ImageExtensions;
            }
            return new string[] { };
        }
        return new string[] { };
    }
    private async Task InitEmulateur()
    {
        if (emulateurs == null) {  await InitializeEmulateurs(); }
    }
    public void ReinitEmulateurs()
    {
        emulateurs = null;
    }
    private async Task InitPlatforms()
    {
        if (platforms == null) { await InitialzePlatforms(); }
    }
    public void ReinitPlatforms()
    {
        platforms = null;
    }
    public async Task<String> GetPlatformsNamefromEmulatorName(string emuname)
    {
        if (platforms == null)
        {
            await InitPlatforms();
        }
        if (emulateurs == null)
        {
            await InitEmulateur();
        }
        var args = emuname.Split(" - ");
        var emu = emulateurs.FirstOrDefault(x => x.Name == args[0]);
        if (emu != null)
        {
            var prof = emu.Profiles.FirstOrDefault(x => x.Name == args[1]);
            if (prof != null)
            {
                var platformID = prof.Platforms.FirstOrDefault();
                if (platformID != null)
                {
                    var platform = platforms.FirstOrDefault(x=> x.Id == platformID);
                    return platform.Name;
                }
            }
        }
        return string.Empty;
    }
    public async Task<IEnumerable<Platforms>> GetPlatformsWithoutRetroarcAsync()
    {
        if (platforms == null)
        {
            await InitPlatforms();
        }
        var duplicatelist = platforms;
        duplicatelist = platforms.Select(platform =>
        {
            if (platform.Emulators != null)
            {
                // Supprimer 'retroarch' du tableau "Emulators"
                platform.Emulators = platform.Emulators.Where(emulator => emulator != "retroarch").ToArray();
            }
            return platform;
        }).ToList();
        return duplicatelist.Where(x=>x.Emulators != null && x.Emulators.Count() >0);
    }
    public async Task<IEnumerable<Platforms>> GetPlatformsAsync()
    {
        if (platforms == null)
        {
            await InitPlatforms();
        }
        return platforms;
    }
    public async Task<IEnumerable<Emulateur>> GetEmulateursAsync()
    {
        if (emulateurs == null)
        {
            await InitEmulateur();
        }
        return emulateurs;
    }
    public async Task<IEnumerable<Emulateur>> GetEmulateursForPlatformsWithoutRetroarchAsync(string[] emulist)
    {
        if (emulateurs == null)
        {
            await InitEmulateur();
        }
        return emulateurs.Where(x => x.Name != "RetroArch" && emulist.Contains(x.Id));
    }
    public async Task<IEnumerable<Emulateur>> GetEmulateursForPlatformsAsync(string[] emulist)
    {
        if (emulateurs == null)
        {
            await InitEmulateur();
        }
        return emulateurs.Where(x=> emulist.Contains(x.Id));
    }
}
