using GameZilla.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZilla.PageSkinSelector
{
    public class DisplayHomeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BasicTemplate { get; set; }
        public DataTemplate HeroTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is HomeViewModel viewModel)
            {
                if (viewModel.Display == "Basic")
                {
                    return BasicTemplate;
                }
                else if (viewModel.Display == "Hero")
                {
                    return HeroTemplate;
                }
            }
            return base.SelectTemplateCore(item);
        }
    }
}
