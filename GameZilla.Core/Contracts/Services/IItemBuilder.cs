using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services
{
    public interface IItemBuilder
    {
        Item FromExecutable(Executable exe);
        Task<Executable> ExecutableFromItem(Item item);
        Task<Item> FromNonExecutable(NonExecutable nonExecutable);
        Task<NonExecutable> NonExecutableFromItem(Item item);
    }
}