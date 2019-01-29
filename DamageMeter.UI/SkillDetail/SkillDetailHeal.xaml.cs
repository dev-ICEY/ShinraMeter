using System;
using System.Windows;
using System.Windows.Input;
using Lang;

namespace DamageMeter.UI.SkillDetail
{
    /// <summary>
    ///     Logique d'interaction pour SkillContent.xaml
    /// </summary>
    public partial class SkillDetailHeal
    {
        public SkillDetailHeal(Tera.Game.Skill skill, SkillAggregate skillAggregate)
        {
            InitializeComponent();
            Update(skill, skillAggregate);
        }

        public void Update(Tera.Game.Skill skill, SkillAggregate skillAggregate)
        {
            var chained = skill.IsChained;
            var hit = skill.Detail;

            if (skill.IsHotDot) { hit = LP.Hot; }
            if (hit != null) { LabelName.Content = hit; }
            if (chained == true) { LabelName.Content += " " + LP.Chained; }

            LabelName.ToolTip = skill.Id;
            LabelCritRateHeal.Content = skillAggregate.CritRateOf(skill.Id) + "%";
            LabelNumberHitHeal.Content = skillAggregate.HitsOf(skill.Id);
            LabelNumberCritHeal.Content = skillAggregate.CritsOf(skill.Id);
            LabelTotalHeal.Content = FormatHelpers.Instance.FormatValue(skillAggregate.AmountOf(skill.Id));
            LabelBiggestHit.Content = FormatHelpers.Instance.FormatValue(skillAggregate.BiggestHitOf(skill.Id));
            LabelBiggestCrit.Content = FormatHelpers.Instance.FormatValue(skillAggregate.BiggestCritOf(skill.Id));
            LabelAverageCrit.Content = FormatHelpers.Instance.FormatValue((long) skillAggregate.AvgCritOf(skill.Id));
            LabelAverageHit.Content = FormatHelpers.Instance.FormatValue((long) skillAggregate.AvgWhiteOf(skill.Id));
            LabelAverage.Content = FormatHelpers.Instance.FormatValue((long) skillAggregate.AvgOf(skill.Id));
        }

        private void DragWindow(object sender, MouseButtonEventArgs e) { ((ClickThrouWindow)Window.GetWindow(this))?.Move(sender, e); }
    }
}