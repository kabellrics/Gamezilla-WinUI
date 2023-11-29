using GameZilla.Core.Models.SteamGridDb;

namespace GameZilla.Core.Contracts.Services;
public interface ISteamGridDBService
{
    Task<ImgResult> GetCoverBySteamgriddbId(string steamgriddbId);
    Task<string> GetDefaultCoverUrl(string steamgriddbId);
    Task<string> GetDefaultHeroUrl(string steamgriddbId);
    Task<string> GetDefaultLogoUrl(string steamgriddbId);
    Task<ImgResult> GetHeroBySteamgriddbId(string steamgriddbId);
    Task<ImgResult> GetLogoBySteamgriddbId(string steamgriddbId);
    Task<SGDBGameResult> SearchGamesByName(string gameName);
}