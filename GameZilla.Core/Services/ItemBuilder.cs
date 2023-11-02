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
}
