using Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DamageMeter.UI.HUD.Converters
{
    public class ValueFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long dmg) return FormatHelpers.Instance.FormatValue(dmg);
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DoubleFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double dmg) return FormatHelpers.Instance.FormatDouble(dmg);
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AggregateToIcon : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SkillAggregate sa)
            {
                foreach (var skillInfo in sa.Skills)
                {
                    if (string.IsNullOrEmpty(skillInfo.Key.IconName)) { continue; }
                    return BasicTeraData.Instance.Icons.GetImage(skillInfo.Key.IconName);
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
