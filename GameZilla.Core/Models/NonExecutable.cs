using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZilla.Core.Models;
public class NonExecutable
{
    public string Id
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public string Path
    {
        get; set;
    }
    public string Cover
    {
        get; set;
    }
    public string Logo
    {
        get; set;
    }
    public string Heroe
    {
        get; set;
    }
    public string Video
    {
        get; set;
    }
    public string Favorite
    {
        get; set;
    }
    public string IsActif
    {
        get; set;
    }
    public string NbStart
    {
        get; set;
    }
    public string LastStartDate
    {
        get; set;
    }
    public string PlateformeId
    {
        get; set;
    }
    public string ExecutableId
    {
        get; set;
    }
}
public class NonExecutableResponseList
{
    public List<NonExecutable> body
    {
        get; set;
    }
    public int itemCount
    {
        get; set;
    }
}
