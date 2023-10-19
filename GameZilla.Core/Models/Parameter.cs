using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZilla.Core.Models;
public class Parametre
{
    public string Id
    {
        get; set;
    }
    public string Clé
    {
        get; set;
    }
    public string Valeur
    {
        get; set;
    }
}

public class ParameterReponseList
{
    public List<Parametre> body
    {
        get; set;
    }
    public int itemCount
    {
        get; set;
    }
}
