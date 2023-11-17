using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZilla.Core.Models;
public class InstalledProgram
{
    public string Name
    {
        get; set;
    }
    public string Version
    {
        get; set;
    }
    //public string IconPath
    //{
    //    get; set;
    //}
    public string ExecutableName
    {
        get; set;
    }
    public string ExecutablePath
    {
        get; set;
    }
    public string Publisher
    {
        get; set;
    }
    //public string InstallationDate
    //{
    //    get; set;
    //}
}
