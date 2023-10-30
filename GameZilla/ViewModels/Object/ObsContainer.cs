using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GameZilla.Core.Models;

namespace GameZilla.ViewModels.Object;
public class ObsContainer : ObservableObject
{
    public Container Container;
    public ObsContainer(Container container)
    {
        this.Container = container;
    }
    public string Id
    {
        get => Container.Id;
        set
        {
            SetProperty(Container.Id, value, Container, (syteme, item) => Container.Id = item);
        }
    }
    public string Name
    {
        get => Container.Name;
        set
        {
            SetProperty(Container.Name, value, Container, (syteme, item) => Container.Name = item);
        }
    }
    public string Logo
    {
        get => $"http://192.168.1.17:900{Container.Logo}";
        set
        {
            SetProperty(Container.Logo, value, Container, (syteme, item) => Container.Logo = item);
        }
    }
    public string Fanart
    {
        get => $"http://192.168.1.17:900{Container.Fanart}";
        set
        {
            SetProperty(Container.Fanart, value, Container, (syteme, item) => Container.Fanart = item);
        }
    }
    public string ShowOrder
    {
        get => Container.ShowOrder;
        set
        {
            SetProperty(Container.ShowOrder, value, Container, (syteme, item) => Container.ShowOrder = item);
        }
    }
    public string IsActif
    {
        get => Container.IsActif;
        set
        {
            SetProperty(Container.IsActif, value, Container, (syteme, item) => Container.IsActif = item);
        }
    }
    public IEnumerable<ObsItem> Items
    {
        get
        {
            return Container.Items.Select(x => new ObsItem(x));
        }
    }
}
