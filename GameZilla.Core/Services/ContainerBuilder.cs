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
    private readonly INonExecutableService _nonexecutableService;
    private readonly IItemBuilder _itemBuilder;
    public ContainerBuilder(IExecutableService executableService,INonExecutableService nonExecutableService, IItemBuilder itemBuilder)
    {
        _executableService = executableService;
        _nonexecutableService = nonExecutableService;
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
        var nonexeitems = await _nonexecutableService.GetNonExecutablesByplatform(container.Id);
        foreach (var exeitem in exeitems)
        {
            container.Items.Add(_itemBuilder.FromExecutable(exeitem));
        }
        foreach (var nonexeitem in nonexeitems)
        {
            container.Items.Add(await _itemBuilder.FromNonExecutable(nonexeitem));
        }
        container.IsActif = exeitems.Count() == 0 ? "0" : "1";
        if(container.IsActif == "0")
            container.IsActif = nonexeitems.Count() == 0 ? "0" : "1";
        return container;
    }
}
