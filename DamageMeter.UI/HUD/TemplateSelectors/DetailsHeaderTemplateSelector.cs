using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DamageMeter.UI.HUD.TemplateSelectors
{
    public class DetailsHeaderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DpsHeader { get; set; }
        public DataTemplate HealHeader { get; set; }
        public DataTemplate ManaHeader { get; set; }
        public DataTemplate CounterHeader { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SkillsDetailVM src)
            {
                switch (src.DetailType)
                {
                    case Database.Database.Type.Damage:  return DpsHeader;
                    case Database.Database.Type.Heal:    return HealHeader;
                    case Database.Database.Type.Mana:    return ManaHeader;
                    case Database.Database.Type.Counter: return CounterHeader;
                    default: break;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
