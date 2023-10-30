using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GameZilla.Core.Models;

namespace GameZilla.ViewModels.Object;
public class ObsItem : ObservableObject
{
    public Item Item;
    public ObsItem(Item item)
    {
        this.Item = item;
    }
    public string Id
    {
        get => Item.Id;
        set
        {
            SetProperty(Item.Id, value, Item, (syteme, item) => Item.Id = item);
        }
    }
    public string Name
    {
        get => Item.Name;
        set
        {
            SetProperty(Item.Name, value, Item, (syteme, item) => Item.Name = item);
        }
    }
    public string StartingCommand
    {
        get => Item.StartingCommand;
        set
        {
            SetProperty(Item.StartingCommand, value, Item, (syteme, item) => Item.StartingCommand = item);
        }
    }
    public string Logo
    {
        get => $"http://192.168.1.17:900{Item.Logo}";
        set
        {
            SetProperty(Item.Logo, value, Item, (syteme, item) => Item.Logo = item);
        }
    }
    public string Hero
    {
        get => $"http://192.168.1.17:900{Item.Hero}";
        set
        {
            SetProperty(Item.Hero, value, Item, (syteme, item) => Item.Hero = item);
        }
    }
    public string Cover
{
    get => $"http://192.168.1.17:900{Item.Cover}";
        set
        {
            SetProperty(Item.Cover, value, Item, (syteme, item) => Item.Cover = item);
        }
    }
    public string Vidéo
{
    get => $"http://192.168.1.17:900{Item.Vidéo}";
        set
        {
            SetProperty(Item.Vidéo, value, Item, (syteme, item) => Item.Vidéo = item);
        }
    }
    public string NbStart
    {
        get => Item.NbStart;
        set
        {
            SetProperty(Item.NbStart, value, Item, (syteme, item) => Item.NbStart = item);
        }
    }
    public string LastStart
    {
        get => Item.LastStart;
        set
        {
            SetProperty(Item.LastStart, value, Item, (syteme, item) => Item.LastStart = item);
        }
    }
    public bool Favori
    {
        get => Item.Favori;
        set
        {
            SetProperty(Item.Favori, value, Item, (syteme, item) => Item.Favori = item);
        }
    }
    public bool IsExecutable
    {
        get => Item.IsExecutable;
        set
        {
            SetProperty(Item.IsExecutable, value, Item, (syteme, item) => Item.IsExecutable = item);
        }
    }
}
