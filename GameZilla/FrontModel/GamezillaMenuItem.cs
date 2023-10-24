using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameZilla.FrontModel;
public class GamezillaMenuItem
{
    public string Text
    {
        get;set;
    }
    public string IconPath
    {
        get;set;
    }
    public ICommand Command
    {
        get;set;
    }
}
