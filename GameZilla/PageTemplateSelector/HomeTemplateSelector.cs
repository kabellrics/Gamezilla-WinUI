using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.Contracts.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GameZilla.PageTemplateSelector;
public class HomeTemplateSelector : DataTemplateSelector
{
    public DataTemplate Basic
    {
        get; set;
    }
    public DataTemplate Hero
    {
        get; set;
    }
    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        try
        {
            switch (item.ToString())
            {
                case "Hero":
                    return Hero;
                    break;
                default:
                    return Basic;
                    break;
            }
        }
        catch (Exception ex)
        {
            return Basic;
            //throw;
        }
    }
}
