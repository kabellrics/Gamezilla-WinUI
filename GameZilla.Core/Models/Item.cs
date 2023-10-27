using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZilla.Core.Models;
public class Item
{
    public int Id
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public string StartingCommand
    {
        get; set;
    }
    public string Logo
    {
        get; set;
    }
    public string Hero
    {
        get; set;
    }
    public string Cover
    {
        get; set;
    }
    public string Vidéo
    {
        get; set;
    }
    public bool Favori
    {
        get; set;
    }
    public string NbStart
    {
        get; set;
    }
    public string LastStart
    {
        get; set;
    }
}
