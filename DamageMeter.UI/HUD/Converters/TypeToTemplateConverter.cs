using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DamageMeter.UI.HUD.Converters
{
    public class TypeToTemplateConverter : IValueConverter
    {
        public DataTemplate Dps { get; set; }
        public DataTemplate Heal { get; set; }
        public DataTemplate Mana { get; set; }
        public DataTemplate Count { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Database.Database.Type t)
            {
                switch (t)
                {
                    case Database.Database.Type.Damage: return Dps;
                    case Database.Database.Type.Heal: return Heal;
                    case Database.Database.Type.Mana: return Mana;
                    case Database.Database.Type.Counter: return Count;
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
