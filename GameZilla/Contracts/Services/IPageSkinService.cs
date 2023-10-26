namespace GameZilla.Contracts.Services
{
    public interface IPageSkinService
    {
        IEnumerable<string> GetDisplaysForGameDetail();
        IEnumerable<string> GetDisplaysForGames();
        IEnumerable<string> GetDisplaysForHome();
        IEnumerable<string> GetDisplaysForSystems();
        Task<string> GetCurrentDisplayHome();
        Task<string> GetCurrentDisplaySystems();
        Task<string> GetCurrentDisplayGames();
        Task<string> GetCurrentDisplayGameDetail();
        void SetCurrentDisplayHome(string value);
        void SetCurrentDisplaySystems(string value);
        void SetCurrentDisplayGames(string value);
        void SetCurrentDisplayGameDetail(string value);
    }
}