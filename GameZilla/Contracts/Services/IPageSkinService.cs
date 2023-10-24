namespace GameZilla.Contracts.Services
{
    public interface IPageSkinService
    {
        IEnumerable<string> GetDisplayForGameDetail();
        IEnumerable<string> GetDisplayForGames();
        IEnumerable<string> GetDisplayForHome();
        IEnumerable<string> GetDisplayForSystems();
    }
}