using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameZilla.Core.Models;
public class Plateforme
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
    public string PlateformeTypeId
    {
        get; set;
    }
    public string IsActif
    {
        get; set;
    }
}

public class PlateformeResponseList
{
    public List<Plateforme> body
    {
        get; set;
    }
    public int itemCount
    {
        get; set;
    }
}

