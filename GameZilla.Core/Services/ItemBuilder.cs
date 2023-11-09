using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZilla.Core.Services;
public class ItemBuilder : IItemBuilder
{
    private readonly IExecutableService _executableService;
    public ItemBuilder(IExecutableService executableService)
    {
        _executableService = executableService;
    }
    public Item FromExecutable(Executable exe)
    {
        Item item = new Item();
        item.Name = exe.Name;
        item.Id = exe.Id;
        item.Favori = exe.Favorite == "1" ? true : false;
        item.NbStart = exe.NbStart;
        item.LastStart = exe.LastStartDate;
        item.Cover = exe.Cover;
        item.Hero = exe.Heroe;
        item.Vidéo = exe.Video;
        item.Logo = exe.Logo;
        item.IsExecutable = true;
        item.StartingCommand = $"{exe.Path} {exe.StartParam}";
        return item;
    }

    public async Task<Executable> ExecutableFromItem(Item item)
    {
        Executable exe = await _executableService.GetExecutablesByID(item.Id);
        exe.Favorite = item.Favori ? "1":"0";
        exe.LastStartDate = item.LastStart;
        exe.NbStart = item.NbStart;        
        return exe;
    }
}
