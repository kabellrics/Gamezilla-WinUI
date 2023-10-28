using GameZilla.Core.Models;

namespace GameZilla.Core.Contracts.Services
{
    public interface IItemBuilder
    {
        Item FromExecutable(Executable exe);
    }
}