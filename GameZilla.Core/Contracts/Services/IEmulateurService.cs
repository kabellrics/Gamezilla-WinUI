using GameZilla.Core.Models.Emulateur;

namespace GameZilla.Core.Contracts.Services;
public interface IEmulateurService
{
    Task<IEnumerable<Platforms>> GetPlatformsAsync();
    Task<IEnumerable<Emulateur>> GetEmulateursAsync();
    Task<IEnumerable<Emulateur>> GetEmulateursForPlatformsAsync(string[] emulist);
    Task<IEnumerable<Platforms>> GetPlatformsWithoutRetroarcAsync();
    Task<IEnumerable<Emulateur>> GetEmulateursForPlatformsWithoutRetroarchAsync(string[] emulist);
    Task<string[]> GetImageExtensionFromExeEmuName(string emuName);
}