using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using DamageMeter.Database;
using DamageMeter.Database.Structures;
using Data;
using Lang;
using Tera.Game;
using Tera.Game.Abnormality;

namespace DamageMeter.UI
{
    //copied here for binding since XAML doesen't support nested types
    public enum ViewType
    {
        Damage = 1,
        Heal = 2,
        Mana = 3,
        Counter = 4
    }
    public class Skills2VM : TSPropertyChanged
    {
        private string _deathCounterText;
        private string _deathDurationText;
        private string _aggroCounterText;
        private string _aggroDurationText;
        private object _currentView;

        public string DeathCounterText
        {
            get => _deathCounterText; set
            {
                if (_deathCounterText == value) return;
                _deathCounterText = value;
                NotifyPropertyChanged();
            }
        }
        public string DeathDurationText
        {
            get => _deathDurationText; set
            {
                if (_deathDurationText == value) return;
                _deathDurationText = value;
                NotifyPropertyChanged();

            }
        }
        public string AggroCounterText
        {
            get => _aggroCounterText; set
            {
                if (_aggroCounterText == value) return;
                _aggroCounterText = value;
                NotifyPropertyChanged();

            }
        }
        public string AggroDurationText
        {
            get => _aggroDurationText; set
            {
                if (_aggroDurationText == value) return;
                _aggroDurationText = value;
                NotifyPropertyChanged();

            }
        }
        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView == value) return;
                _currentView = value;
                NotifyPropertyChanged();
            }
        }

        public SkillsDetailVM DpsDetails { get; }
        public SkillsDetailVM HealDetails { get; }
        public SkillsDetailVM ManaDetails { get; }
        public SkillsDetailVM CounterDetails { get; }
        public BuffDetailVM BuffDetails { get; }
        public SkillsLogVM DealtLog { get; }
        public SkillsLogVM ReceivedLog { get; }

        public ICommand SwitchView { get; }

        public Skills2VM()
        {
            DealtLog = new SkillsLogVM();
            ReceivedLog = new SkillsLogVM(true);
            DpsDetails = new SkillsDetailVM(Database.Database.Type.Damage);
            HealDetails = new SkillsDetailVM(Database.Database.Type.Heal);
            ManaDetails = new SkillsDetailVM(Database.Database.Type.Mana);
            CounterDetails = new SkillsDetailVM(Database.Database.Type.Counter);
            BuffDetails = new BuffDetailVM();

            CurrentView = DpsDetails;

            SwitchView = new RelayCommand((o) =>
            {
                switch ((ViewType)o)
                {
                    case ViewType.Damage: CurrentView = DpsDetails; break;
                    case ViewType.Heal: CurrentView = HealDetails; break;
                    case ViewType.Mana: CurrentView = ManaDetails; break;
                    case ViewType.Counter: CurrentView = CounterDetails; break;
                    default: break;
                }
            }, b => true);
        }

        internal void SetAggroAndDeath(PlayerAbnormals buffs, EntityInformation eInfo)
        {
            var death = buffs.Death;
            if (death == null)
            {
                DeathCounterText = "0";
                DeathDurationText = $"0{LP.Seconds}";
            }
            else
            {

                DeathCounterText = death.Count(eInfo.BeginTime, eInfo.EndTime).ToString();
                DeathDurationText = TimeSpan.FromTicks(death.Duration(eInfo.BeginTime, eInfo.EndTime)).ToString(@"mm\:ss");
            }
            var aggro = buffs.Aggro(eInfo.Entity);
            if (aggro == null)
            {
                AggroCounterText = "0";
                AggroDurationText = $"0{LP.Seconds}";
            }
            else
            {
                AggroCounterText = aggro.Count(eInfo.BeginTime, eInfo.EndTime).ToString();
                AggroDurationText = TimeSpan.FromTicks(aggro.Duration(eInfo.BeginTime, eInfo.EndTime)).ToString(@"mm\:ss");
            }
        }
    }
    public abstract class HeaderVMBase : TSPropertyChanged
    {
        private SkillsDetailVM _view;
        public RelayCommand SortCommand { get; }
        public string NameText { get; } = LP.SkillName;
        public virtual void Sort(SortBy c)
        {
            _view.RefreshSorting(c);
        }
        public HeaderVMBase(SkillsDetailVM view)
        {
            _view = view;
            SortCommand = new RelayCommand(p => Sort((SortBy)p), b => true);
        }
    }
    public class DpsHeaderVM : HeaderVMBase
    {
        private string _totalDamageText = LP.Damage;
        private string _damagePercText = LP.DamagePercent;
        private string _critRateDmgText = LP.CritPercent;
        private string _biggestCritText = LP.MaxCrit;
        private string _avgCritText = LP.AverageCrit;
        private string _avgHitText = LP.AvgWhite;
        private string _avgTotalText = LP.Average;
        private string _hitNumText = LP.Hits;
        private string _critNumDmgText = LP.Crits;
        private string _hpmText = LP.HPM;

        public string TotalDamageText
        {
            get => _totalDamageText;
            set
            {
                if (_totalDamageText == value) return;
                _totalDamageText = value;
                NotifyPropertyChanged();
            }
        }
        public string DamagePercText
        {
            get => _damagePercText;
            set
            {
                if (_damagePercText == value) return;
                _damagePercText = value;
                NotifyPropertyChanged();
            }
        }
        public string CritRateDmgText
        {
            get => _critRateDmgText;
            set
            {
                if (_critRateDmgText == value) return;
                _critRateDmgText = value;
                NotifyPropertyChanged();
            }
        }
        public string BiggestCritText
        {
            get => _biggestCritText;
            set
            {
                if (_biggestCritText == value) return;
                _biggestCritText = value;
                NotifyPropertyChanged();
            }
        }
        public string AvgCritText
        {
            get => _avgCritText;
            set
            {
                if (_avgCritText == value) return;
                _avgCritText = value;
                NotifyPropertyChanged();
            }
        }
        public string AvgHitText
        {
            get => _avgHitText;
            set
            {
                if (_avgHitText == value) return;
                _avgHitText = value;
                NotifyPropertyChanged();
            }
        }
        public string AvgTotalText
        {
            get => _avgTotalText;
            set
            {
                if (_avgTotalText == value) return;
                _avgTotalText = value;
                NotifyPropertyChanged();
            }
        }
        public string HitNumText
        {
            get => _hitNumText;
            set
            {
                if (_hitNumText == value) return;
                _hitNumText = value;
                NotifyPropertyChanged();
            }
        }
        public string CritNumDmgText
        {
            get => _critNumDmgText;
            set
            {
                if (_critNumDmgText == value) return;
                _critNumDmgText = value;
                NotifyPropertyChanged();
            }
        }
        public string HpmText
        {
            get => _hpmText;
            set
            {
                if (_hpmText == value) return;
                _hpmText = value;
                NotifyPropertyChanged();
            }
        }

        public DpsHeaderVM(SkillsDetailVM view) : base(view) { }

    }
    public class HealHeaderVM : HeaderVMBase
    {
        private string _critRateHealText = LP.CritPercent;
        private string _averageCritText = LP.AverageCrit;
        private string _maxCritText = LP.MaxCrit;
        private string _maxWhiteText = LP.MaxWhite;
        private string _avgWhiteText = LP.AvgWhite;
        private string _avgTotalText = LP.Average;
        private string _hitsText = LP.Hits;
        private string _critsText = LP.Crits;
        private string _healText = LP.Heal;

        public string CritRateHealText
        {
            get => _critRateHealText;
            set
            {
                if (_critRateHealText == value) return;
                _critRateHealText = value;
                NotifyPropertyChanged();
            }
        }
        public string AverageCritText
        {
            get => _averageCritText;
            set
            {
                if (_averageCritText == value) return;
                _averageCritText = value;
                NotifyPropertyChanged();
            }
        }
        public string MaxCritText
        {
            get => _maxCritText;
            set
            {
                if (_maxCritText == value) return;
                _maxCritText = value;
                NotifyPropertyChanged();
            }
        }
        public string MaxWhiteText
        {
            get => _maxWhiteText;
            set
            {
                if (_maxWhiteText == value) return;
                _maxWhiteText = value;
                NotifyPropertyChanged();
            }
        }
        public string AvgWhiteText
        {
            get => _avgWhiteText;
            set
            {
                if (_avgWhiteText == value) return;
                _avgWhiteText = value;
                NotifyPropertyChanged();
            }
        }
        public string AvgTotalText
        {
            get => _avgTotalText;
            set
            {
                if (_avgTotalText == value) return;
                _avgTotalText = value;
                NotifyPropertyChanged();
            }
        }
        public string HitsText
        {
            get => _hitsText;
            set
            {
                if (_hitsText == value) return;
                _hitsText = value;
                NotifyPropertyChanged();
            }
        }
        public string CritsText
        {
            get => _critsText;
            set
            {
                if (_critsText == value) return;
                _critsText = value;
                NotifyPropertyChanged();
            }
        }
        public string HealText
        {
            get => _healText;
            set
            {
                if (_healText == value) return;
                _healText = value;
                NotifyPropertyChanged();
            }
        }

        public HealHeaderVM(SkillsDetailVM view) : base(view) { }
    }
    public class ManaHeaderVM : HeaderVMBase
    {
        private string _manaHitsText = LP.Hits;
        private string _manaText = LP.Mana;

        public string ManaHitsText
        {
            get => _manaHitsText;
            set
            {
                if (_manaHitsText == value) return;
                _manaHitsText = value;
                NotifyPropertyChanged();
            }
        }
        public string ManaText
        {
            get => _manaText;
            set
            {
                if (_manaText == value) return;
                _manaText = value;
                NotifyPropertyChanged();
            }
        }

        public ManaHeaderVM(SkillsDetailVM view) : base(view) { }
    }
    public class CounterHeaderVM : HeaderVMBase
    {
        private string _hitsText;

        public string HitsText
        {
            get => _hitsText;
            set
            {
                if (_hitsText == value) return;
                _hitsText = value;
                NotifyPropertyChanged();
            }
        }

        public CounterHeaderVM(SkillsDetailVM view) : base(view) { }
    }
    public class DetailsVM : TSPropertyChanged
    {
        public string Name { get; }
        public string Tooltip { get; }

        public string CritRateDmgText { get; }
        public string DamagePercText { get; }
        public string TotalDmgText { get; }
        public string HitsText { get; }
        public string CritsText { get; }
        public string AvgCritText { get; }
        public string MaxCritText { get; }
        public string AvgWhiteText { get; }
        public string AvgTotalText { get; }
        public string HpmText { get; }


        public DetailsVM(Tera.Game.Skill skill, SkillAggregate aggregate)
        {
            Name = skill.Detail ?? "";
            if (skill.IsHotDot) Name = LP.Dot;
            if (skill.IsChained == true) Name += $" {LP.Chained}";

            Tooltip = skill.Id.ToString();

            CritRateDmgText = $"{aggregate.CritRateOf(skill.Id)}%";
            DamagePercText = $"{aggregate.DamagePercentOf(skill.Id)}%";
            TotalDmgText = FormatHelpers.Instance.FormatValue(aggregate.AmountOf(skill.Id));

            HitsText = aggregate.HitsOf(skill.Id).ToString();
            CritsText = aggregate.CritsOf(skill.Id).ToString();
            AvgCritText = FormatHelpers.Instance.FormatValue((long) aggregate.AvgCritOf(skill.Id));
            MaxCritText =  FormatHelpers.Instance.FormatValue(aggregate.BiggestCritOf(skill.Id));
            AvgWhiteText = FormatHelpers.Instance.FormatValue((long)aggregate.AvgWhiteOf(skill.Id));
            AvgTotalText =  FormatHelpers.Instance.FormatValue((long)aggregate.AvgOf(skill.Id));
            HpmText = FormatHelpers.Instance.FormatDouble(aggregate.HPM);
        }

    }
    public class AggregateVM : TSPropertyChanged
    {
        public SkillAggregate Aggregate { get; }
        private SynchronizedObservableCollection<DetailsVM> _details;
        public SynchronizedObservableCollection<DetailsVM> Details
        {
            get
            {
                _details.Clear();
                foreach (var item in Aggregate.Skills)
                {
                    var skill = item.Key;
                    _details.Add(new DetailsVM(skill, Aggregate));
                }
                return _details;
            }
        }

        public ImageSource SkillIcon
        {
            get
            {
                foreach (var skillInfo in Aggregate.Skills)
                {
                    if (string.IsNullOrEmpty(skillInfo.Key.IconName)) { continue; }
                    return BasicTeraData.Instance.Icons.GetImage(skillInfo.Key.IconName);
                }
                return null;
            }
        }
        public AggregateVM(SkillAggregate x)
        {
            Aggregate = x;
            _details = new SynchronizedObservableCollection<DetailsVM>();
        }
    }
    public class SkillsDetailVM : TSPropertyChanged
    {
        private SynchronizedObservableCollection<AggregateVM> _data { get; }
        private ListSortDirection _sortDir = ListSortDirection.Descending;
        private SortBy _sortCrit = SortBy.Amount;
        private string _stringCrit => PropertNameFromSortBy();

        public Database.Database.Type DetailType { get; }
        public ICollectionViewLiveShaping DataView { get; }
        public HeaderVMBase HeaderVM { get; }
        public SkillsDetailVM(Database.Database.Type t)
        {
            _data = new SynchronizedObservableCollection<AggregateVM>();
            DetailType = t;
            DataView = GuiUtils.InitLiveView(null, _data, new string[] { }, new SortDescription[] { });
            ((ICollectionView)DataView).CollectionChanged += JustInCase;
            switch (DetailType)
            {
                case Database.Database.Type.Damage:
                    HeaderVM = new DpsHeaderVM(this);
                    break;
                case Database.Database.Type.Heal:
                    HeaderVM = new HealHeaderVM(this);
                    break;
                case Database.Database.Type.Mana:
                    HeaderVM = new ManaHeaderVM(this);
                    break;
                case Database.Database.Type.Counter:
                    HeaderVM = new CounterHeaderVM(this);
                    break;
                default:
                    break;
            }
        }

        public void RefreshSorting(SortBy criteria)
        {
            _sortCrit = criteria;
            _sortDir = _sortDir == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

            ((CollectionView)DataView).SortDescriptions.Clear();
            ((CollectionView)DataView).SortDescriptions.Add(new SortDescription(_stringCrit, _sortDir));
        }

        internal void Update(IEnumerable<SkillAggregate> dpsAgg)
        {
            _data.Clear();
            dpsAgg.ToList().ForEach(x => _data.Add(new AggregateVM(x)));
        }

        private void JustInCase(object _, NotifyCollectionChangedEventArgs __) { }

        private string PropertNameFromSortBy()
        {
            switch (_sortCrit)
            {
                case SortBy.Amount: return nameof(SkillAggregate.Amount);
                case SortBy.Name: return nameof(SkillAggregate.Name);
                case SortBy.AvgCrit: return nameof(SkillAggregate.AvgCrit);
                case SortBy.Avg: return nameof(SkillAggregate.Avg);
                case SortBy.BigCrit: return nameof(SkillAggregate.BiggestCrit);
                case SortBy.DamagePercent: return nameof(SkillAggregate.DamagePercent);
                case SortBy.NumberHits: return nameof(SkillAggregate.Hits);
                case SortBy.NumberCrits: return nameof(SkillAggregate.Crits);
                case SortBy.AvgHit: return nameof(SkillAggregate.AvgWhite);
                case SortBy.BigHit: return nameof(SkillAggregate.BiggestHit);
                case SortBy.CritRate: return nameof(SkillAggregate.CritRate);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
    public enum SortBy
    {
        Amount = 1,
        Name = 2,
        AvgCrit = 3,
        Avg = 4,
        BigCrit = 5,
        DamagePercent = 6,
        NumberHits = 7,
        NumberCrits = 8,
        AvgHit = 9,
        BigHit = 10,
        CritRate = 11
    }
    public class SkillsLogVM : TSPropertyChanged
    {
        public SynchronizedObservableCollection<Database.Structures.Skill> Log { get; }
        public bool Received; // <-- maybe remove this and make a different class if more specific things are needed in UI
        public SkillsLogVM(bool received = false)
        {
            Log = new SynchronizedObservableCollection<Database.Structures.Skill>();
            Received = received;
        }

        internal void Update(List<Database.Structures.Skill> list)
        {
            Log.Clear();
            list.ForEach(x => Log.Add(x));
        }
    }
    public class BuffDetailVM : TSPropertyChanged
    {
        public PlayerDamageDealt PlayerDamageDealt { get; private set; } //inpc
        public PlayerAbnormals Buffs { get; private set; } // inpc

        public BuffDetailVM()
        {
        }

        internal void Update(PlayerDamageDealt playerDamageDealt, PlayerAbnormals buffs)
        {
            PlayerDamageDealt = playerDamageDealt;
            Buffs = buffs;
        }
    }


    public partial class Skills2
    {
        private readonly PlayerStats _parent;

        private Skills2VM _vm;

        public Skills2(PlayerStats parent, PlayerDamageDealt playerDamageDealt, EntityInformation entityInformation,
                       Database.Structures.Skills skills, PlayerAbnormals buffs, bool timedEncounter)
        {
            _parent = parent;
            Owner = GetWindow(_parent);
            _vm = new Skills2VM();
            DataContext = _vm;
            InitializeComponent();
            ClassImage.Source = ClassIcons.Instance.GetImage(playerDamageDealt.Source.Class).Source; //BasicTeraData.Instance.ImageDatabase.Close.Source;
            Update(playerDamageDealt, entityInformation, skills, buffs, timedEncounter);
        }

        public void Update(PlayerDamageDealt playerDamageDealt,
            EntityInformation entityInformation,
            Database.Structures.Skills skills,
            PlayerAbnormals buffs,
            bool timedEncounter)
        {
            if (skills != null)
            {
                _vm.SetAggroAndDeath(buffs, entityInformation);

                _vm.DpsDetails.Update(SkillAggregate.GetAggregate(playerDamageDealt, entityInformation.Entity, skills, timedEncounter, Database.Database.Type.Damage));
                _vm.HealDetails.Update(SkillAggregate.GetAggregate(playerDamageDealt, entityInformation.Entity, skills, timedEncounter, Database.Database.Type.Heal));
                _vm.ManaDetails.Update(SkillAggregate.GetAggregate(playerDamageDealt, entityInformation.Entity, skills, timedEncounter, Database.Database.Type.Mana));
                _vm.CounterDetails.Update(SkillAggregate.GetAggregate(playerDamageDealt, entityInformation.Entity, skills, timedEncounter, Database.Database.Type.Counter));

                _vm.BuffDetails.Update(playerDamageDealt, buffs);

                _vm.DealtLog.Update(skills?.GetSkillsDealt(playerDamageDealt.Source.User, entityInformation.Entity, timedEncounter));
                _vm.ReceivedLog.Update(skills?.GetSkillsReceived(playerDamageDealt.Source.User, timedEncounter));
            }
        }


        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.CloseSkills();
        }

        private void ClickThrouWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CloseWindow.Source = BasicTeraData.Instance.ImageDatabase.Close.Source;
            DeathIcon.Source = BasicTeraData.Instance.ImageDatabase.Skull.Source;
            DeathTimeIcon.Source = BasicTeraData.Instance.ImageDatabase.SkullTime.Source;
            AggroIcon.Source = BasicTeraData.Instance.ImageDatabase.BossGage.Source;
            AggroTimeIcon.Source = BasicTeraData.Instance.ImageDatabase.AggroTime.Source;

        }

        private void DragWindow(object sender, MouseButtonEventArgs e) { ((ClickThrouWindow)Window.GetWindow(this))?.Move(sender, e); }
    }
}