using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZilla.Core.Models;
public class Container
{
    public string Id
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public string Fanart
    {
        get; set;
    }
    public string Logo
    {
        get; set;
    }
    public string ShowOrder
    {
        get; set;
    }
    public bool IsPlateforme
    {
        get; set;
    }
    public string IsActif
    {
        get; set;
    }
}
