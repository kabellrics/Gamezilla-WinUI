using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services;
public interface IApplicationFinderService
{
    List<InstalledProgram> GetInstalledPrograms();
    List<InstalledProgram> GetInstalledProgramsV2();
    List<InstalledProgram> GetFullListInstalledApplication();
    void ListInstalledPrograms();
}