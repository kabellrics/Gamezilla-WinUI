using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services;
public interface IParameterService
{
    Task<String> GetParameterValue(ParamEnum param);
}