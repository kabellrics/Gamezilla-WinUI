using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZilla.ViewModels.Object;
public class NavigateToListGameParameter
{
    public string Id
    {
        get;set;
    }
    public TypeListGame typeListGame
    {
    get; set; }

}

public enum TypeListGame
{
    Plateforme,
    AllGames,
    Favorite,
    LastPlayed,
    NeverPlayed
}