using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DamageMeter.UI.HUD.TemplateSelectors
{
    public class SkillsWindowTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SkillDetails { get; set; }
        public DataTemplate BuffDetails { get; set; }
        public DataTemplate DealtLog { get; set; }
        public DataTemplate ReceivedLog { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is SkillsLogVM log) return log.Received ? ReceivedLog : DealtLog;
            if (item is BuffDetailVM) return BuffDetails;
            if (item is SkillsDetailVM) return SkillDetails;

            return base.SelectTemplate(item, container);
        }
    }
}
