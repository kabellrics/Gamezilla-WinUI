using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using Microsoft.Win32;

namespace GameZilla.Core.Services;
public class ApplicationFinderService : IApplicationFinderService
{
    public void ListInstalledPrograms()
    {
        try
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
            var collection = searcher.Get();

            foreach (var obj in collection)
            {
                if (!obj["LocalPackage"].ToString().Contains(".msi"))
                {
                    Debug.WriteLine($"Name: {obj["Name"]}");
                    Debug.WriteLine($"Version: {obj["Version"]}");
                    Debug.WriteLine($"InstallLocation: {obj["InstallLocation"]}");
                    Debug.WriteLine($"LocalPackage: {obj["LocalPackage"]}");
                    Debug.WriteLine("------------------------"); 
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
        }
    }
    public List<InstalledProgram> GetInstalledPrograms()
    {
        const string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        List<InstalledProgram> programs = new List<InstalledProgram>();

        using (var uninstallKey32 = Registry.LocalMachine.OpenSubKey(uninstallKey))
        using (var uninstallKey64 = Registry.LocalMachine.OpenSubKey(uninstallKey, false))
        {
            if (uninstallKey32 != null)
                GetInstalledPrograms(uninstallKey32, programs);

            if (uninstallKey64 != null)
                GetInstalledPrograms(uninstallKey64, programs);
        }

        return programs;
    }

    private void GetInstalledPrograms(RegistryKey key, List<InstalledProgram> programs)
    {
        foreach (var subKeyName in key.GetSubKeyNames())
        {
            using (var programKey = key.OpenSubKey(subKeyName))
            {
                var displayName = programKey?.GetValue("DisplayName") as string;
                var displayVersion = programKey?.GetValue("DisplayVersion") as string;
                var installLocation = programKey?.GetValue("InstallLocation") as string;
                var publisher = programKey?.GetValue("Publisher") as string;
                var installDate = programKey?.GetValue("InstallDate") as string;

                if (!string.IsNullOrEmpty(displayName) && !string.IsNullOrEmpty(installLocation))
                {
                    var exefiles = GetExecutablePath(installLocation);
                    if (exefiles != null)
                    {
                        foreach (var exe in GetExecutablePath(installLocation))
                        {
                            programs.Add(new InstalledProgram
                            {
                                Name = displayName,
                                Version = displayVersion,
                                //IconPath = iconPath,
                                ExecutableName = Path.GetFileName(exe),
                                ExecutablePath = exe,
                                Publisher = publisher
                                //InstallationDate = installDate
                            });
                        } 
                    }
                }
            }
        }
    }
    private string[] GetExecutablePath(string installLocation)
    {
        if (string.IsNullOrEmpty(installLocation))
        {
            return null;
        }

        try
        {
            // Recherche de fichiers exécutables dans le dossier d'installation
            string[] executableFiles = Directory.GetFiles(installLocation, "*.exe");

            // Si des fichiers exécutables sont trouvés, on peut utiliser le premier trouvé
            if (executableFiles.Length > 0)
            {
                return executableFiles;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while searching for executable: {ex.Message}");
        }

        return null;
    }
}
