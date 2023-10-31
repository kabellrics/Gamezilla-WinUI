using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameZilla.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GameZilla.PageSkinSelector;
public class DisplayPlateformeTemplateSelector : DataTemplateSelector
{
    public DataTemplate FlipTemplate
    {
        get; set;
    }
    public DataTemplate GridTemplate
    {
        get; set;
    }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is HomeViewModel viewModel)
        {
            if (viewModel.Display == "Flip")
            {
                return FlipTemplate;
            }
            else if (viewModel.Display == "Grid")
            {
                return GridTemplate;
            }
        }
        return base.SelectTemplateCore(item);
    }
}
