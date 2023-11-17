using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services;
public interface IApplicationFinderService
{
    List<InstalledProgram> GetInstalledPrograms();
    void ListInstalledPrograms();
}