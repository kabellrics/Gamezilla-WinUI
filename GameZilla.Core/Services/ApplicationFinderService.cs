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
    public List<InstalledProgram> GetFullListInstalledApplication()
    {
        IEnumerable<InstalledProgram> finalList = new List<InstalledProgram>();

        string registry_key_32 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        string registry_key_64 = @"SOFTWARE\WoW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

        List<InstalledProgram> win32AppsCU = GetInstalledApplication(Registry.CurrentUser, registry_key_32);
        List<InstalledProgram> win32AppsLM = GetInstalledApplication(Registry.LocalMachine, registry_key_32);
        List<InstalledProgram> win64AppsCU = GetInstalledApplication(Registry.CurrentUser, registry_key_64);
        List<InstalledProgram> win64AppsLM = GetInstalledApplication(Registry.LocalMachine, registry_key_64);

        finalList = win32AppsCU.Concat(win32AppsLM).Concat(win64AppsCU).Concat(win64AppsLM);

        finalList = finalList.GroupBy(d => d.Name).Select(d => d.First());

        return finalList.OrderBy(o => o.Name).ToList();
    }

    private List<InstalledProgram> GetInstalledApplication(RegistryKey regKey, string registryKey)
    {
        List<InstalledProgram> list = new List<InstalledProgram>();
        using (Microsoft.Win32.RegistryKey key = regKey.OpenSubKey(registryKey))
        {
            if (key != null)
            {
                foreach (string name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(name))
                    {
                        string displayName = (string)subkey.GetValue("DisplayName");
                        string installLocation = (string)subkey.GetValue("InstallLocation");

                        if (!string.IsNullOrEmpty(displayName)) // && !string.IsNullOrEmpty(installLocation)
                        {
                            var exefiles = GetExecutablePath(installLocation);
                            if (exefiles != null)
                            {
                                foreach (var exe in GetExecutablePath(installLocation))
                                {
                                    list.Add(new InstalledProgram()
                                    {
                                        Name = displayName.Trim(),
                                        ExecutableName = Path.GetFileName(exe),
                                        ExecutablePath = exe,
                                        Publisher = (string)subkey.GetValue("Publisher")
                                    }); 
                                }
                            }
                        }
                    }
                }
            }
        }

        return list;
    }
    public List<InstalledProgram> GetInstalledProgramsV2()
    {
        List<InstalledProgram> installs = new List<InstalledProgram>();
        List<string> keys = new List<string>() {
  @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
  @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
};

        // The RegistryView.Registry64 forces the application to open the registry as x64 even if the application is compiled as x86 
        FindInstalls(RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64), keys, installs);
        FindInstalls(RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64), keys, installs);
        return installs;
    }
    private void FindInstalls(RegistryKey regKey, List<string> keys, List<InstalledProgram> installed)
    {
        foreach (string key in keys)
        {
            using (RegistryKey rk = regKey.OpenSubKey(key))
            {
                if (rk == null)
                {
                    continue;
                }
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {
                            var installLocation = Convert.ToString(sk.GetValue("InstallLocation"));
                            var exefiles = GetExecutablePath(installLocation);
                            if(exefiles != null)
                            {
                                foreach (var exe in GetExecutablePath(installLocation))
                                {
                                    installed.Add(new InstalledProgram
                                    {
                                        Name = Convert.ToString(sk.GetValue("DisplayName")),
                                        Version = Convert.ToString(sk.GetValue("DisplayVersion")),
                                        //IconPath = iconPath,
                                        ExecutableName = Path.GetFileName(exe),
                                        ExecutablePath = exe,
                                        Publisher = Convert.ToString(sk.GetValue("Publisher"))
                                        //InstallationDate = installDate
                                    });
                                }
                            }
                            //installed.Add(
                            //    new InstalledProgram() 
                            //    {
                            //        Name = Convert.ToString(sk.GetValue("DisplayName")),
                            //        Version = Convert.ToString(sk.GetValue("DisplayVersion")),

                            //    }
                            //    //Convert.ToString(sk.GetValue("DisplayName"))
                            //    );
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
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
