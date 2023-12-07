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
    private readonly INonExecutableService _nonexecutableService;
    public ItemBuilder(IExecutableService executableService, INonExecutableService nonexecutableService)
    {
        _executableService = executableService;
        _nonexecutableService = nonexecutableService;
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
    public async Task<Item> FromNonExecutable(NonExecutable nonExecutable)
    {
        Item item = new Item();
        item.Name = nonExecutable.Name;
        item.Id = nonExecutable.Id;
        item.Favori = nonExecutable.Favorite == "1" ? true : false;
        item.NbStart = nonExecutable.NbStart;
        item.LastStart = nonExecutable.LastStartDate;
        item.Cover = nonExecutable.Cover;
        item.Hero = nonExecutable.Heroe;
        item.Vidéo = nonExecutable.Video;
        item.Logo = nonExecutable.Logo;
        item.IsExecutable = true;
        var exe = await _executableService.GetExecutablesByID(nonExecutable.ExecutableId);
        var StartPath = $"{exe.Path} {exe.StartParam}";
        StartPath = StartPath.Replace("{ImagePath}",nonExecutable.Path);
        item.StartingCommand = StartPath;
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

    public async Task<NonExecutable> NonExecutableFromItem(Item item)
    {
        NonExecutable nonexe = await _nonexecutableService.GetNonExecutablesByID(item.Id);
        nonexe.Favorite = item.Favori ? "1" : "0";
        nonexe.LastStartDate = item.LastStart;
        nonexe.NbStart = item.NbStart;
        return nonexe;

    }
}
