using GameZilla.Core.Models.Emulateur;

namespace GameZilla.Core.Contracts.Services;
public interface IEmulateurService
{
    Task<IEnumerable<Platforms>> GetPlatformsAsync();
    Task<IEnumerable<Emulateur>> GetEmulateursAsync();
    Task<IEnumerable<Emulateur>> GetEmulateursForPlatformsAsync(string[] emulist);
}