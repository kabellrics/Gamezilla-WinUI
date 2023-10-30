using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Core.Contracts.Services;
using GameZilla.Core.Models;

namespace GameZilla.Core.Services;
public class ContainerBuilder : IContainerBuilder
{
    private readonly IExecutableService _executableService;
    private readonly IItemBuilder _itemBuilder;
    public ContainerBuilder(IExecutableService executableService, IItemBuilder itemBuilder)
    {
        _executableService = executableService;
        _itemBuilder = itemBuilder;
    }
    public async Task<Container> FromPlateforme(Plateforme plateforme)
    {
        Container container = new Container();
        container.Items = new System.Collections.ObjectModel.ObservableCollection<Item>();
        container.IsPlateforme = true;
        container.Id = plateforme.Id;
        container.ShowOrder = plateforme.ShowOrder;
        container.Name = plateforme.Name;
        container.Fanart = plateforme.Fanart;
        container.Logo = plateforme.Logo;
        container.IsActif = plateforme.IsActif;
        var exeitems = await _executableService.GetExecutablesByplatform(container.Id);
        foreach (var exeitem in exeitems)
        {
            container.Items.Add(_itemBuilder.FromExecutable(exeitem));
        }
        return container;
    }
}
