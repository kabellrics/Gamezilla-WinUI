namespace GameZilla.Core.Contracts.Services;

public interface IStartProcessService
{
    void Init(string programPath, string plateformeId);
    void StartExe();
}