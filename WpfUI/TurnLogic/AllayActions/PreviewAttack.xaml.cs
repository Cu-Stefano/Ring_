using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Engine.FEMap;
using Engine.Models;
using WpfUI.ViewModels;
using System.Windows.Media.Animation;
using WpfUI.Utilities;

namespace WpfUI.TurnLogic.Actions
{
    public partial class PreviewAttack : UserControl, INotifyPropertyChanged
    {
        public ChooseAttack chooseAttack { get; set; }
        public GameSession GameSession;
        public MapBuilder MapBuilder { get; set; }

        public Button? EnemyButton { get; set; }
        public Polygon? EnemyPolygon
        {
            get => EnemyButton?.Content as Polygon;
            set
            {
                if (EnemyButton != null)
                {
                    EnemyButton.Content = value;
                    OnPropertyChanged();
                }
            }
        }

        private Unit? _allayUnit;
        public Unit? AllayUnit
        {
            get => _allayUnit;
            internal set => SetField(ref _allayUnit, value);
        }
        public Polygon? AllayPolygon
        {
            get => AllayButton?.Content as Polygon;
            set
            {
                if (AllayButton != null)
                {
                    AllayButton.Content = value;
                    OnPropertyChanged();
                }
            }
        }
        private Unit? _enemyUnit;
        public Unit? EnemyUnit
        {
            get => _enemyUnit;
            internal set => SetField(ref _enemyUnit, value);
        }

        private int _enemyDamage;
        public int EnemyDamage
        {
            get => _enemyDamage;
            internal set => SetField(ref _enemyDamage, value);
        }

        private int _enemyHit;
        public int EnemyHit
        {
            get => _enemyHit;
            private set => SetField(ref _enemyHit, value);
        }

        private int _enemyCrit;
        public int EnemyCrit
        {
            get => _enemyCrit;
            private set => SetField(ref _enemyCrit, value);
        }

        private int _allayDamage;
        public int AllayDamage
        {
            get => _allayDamage;
            private set => SetField(ref _allayDamage, value);
        }

        private int _allayHit;
        public int AllayHit
        {
            get => _allayHit;
            private set => SetField(ref _allayHit, value);
        }

        private int _allayHp;
        public int AllayHp
        {
            get => _allayHp;
            internal set => SetField(ref _allayHp, value);
        }

        private int _allayHpMax;
        public int AllayHpMax
        {
            get => _allayHpMax;
            private set => SetField(ref _allayHpMax, value);
        }

        private int _enemyHp;
        public int EnemyHp
        {
            get => _enemyHp;
            internal set => SetField(ref _enemyHp, value);
        }

        private int _enemyHpMax;
        public int EnemyHpMax
        {
            get => _enemyHpMax;
            private set => SetField(ref _enemyHpMax, value);
        }

        private int _allayCrit;
        public int AllayCrit
        {
            get => _allayCrit;
            private set => SetField(ref _allayCrit, value);
        }

        public Button? attackingAllay { get; set; }
        public List<Button?> EnemyNear { get; set; }
        public Button? AllayButton { get; set; }

        public PreviewAttack()
        {
            InitializeComponent();
            DataContext = this; // Ensure the DataContext is set correctly
        }

        public void Start()
        {
            if (GameSession == null || EnemyButton == null || attackingAllay == null || EnemyNear == null) return;

            AllayUnit = attackingAllay.GetTile().UnitOn;
            EnemyUnit = EnemyButton.GetTile().UnitOn;

            if (chooseAttack._mapBuilder.MapLogic.CurrentTurnState.CurrentActionState.GetType() != typeof(ChooseAttack)) return;

            AllayButton = attackingAllay;
            OnPropertyChanged(nameof(EnemyPolygon));
            OnPropertyChanged(nameof(AllayPolygon));

            if (EnemyUnit.EquipedWeapon == null || EnemyUnit.EquipedWeapon.Range < AllayUnit.EquipedWeapon.Range)
                EnemyDamage = 0;
            else
            {
                EnemyDamage = EnemyUnit.EquipedWeapon.WeaponType is WeaponType.Tome
                    ? EnemyUnit.Get_Attack() - EnemyUnit.Statistics.Resistance
                    : EnemyUnit.Get_Attack() - EnemyUnit.Statistics.Defense;
            }
            
            EnemyHit = EnemyUnit.Get_Hit() - AllayUnit.Dodge;
            EnemyCrit = EnemyUnit.Get_Crit();

            AllayDamage = AllayUnit.EquipedWeapon.WeaponType is WeaponType.Tome
                ? AllayUnit.Get_Attack() - EnemyUnit.Statistics.Resistance
                : AllayUnit.Get_Attack() - EnemyUnit.Statistics.Defense;

            AllayHit = AllayUnit.Get_Hit() - EnemyUnit.Dodge;
            AllayCrit = AllayUnit.Get_Crit();

            AllayHp = AllayUnit.Statistics.Hp;
            AllayHpMax = AllayUnit.Statistics.HpMax;
            EnemyHp = EnemyUnit.Statistics.Hp;
            EnemyHpMax = EnemyUnit.Statistics.HpMax;

            PreviewAttackGrid.Visibility = Visibility.Visible;
            AnimateAllayHPbar();
            AnimateEnemyHPbar();
        }

        public void AnimateAllayHPbar()
        {
            // Animate allay hp bar
            AnimateHpBar(AllayHpBar, AllayUnit!.Statistics.Hp, AllayUnit.Statistics.HpMax);
        }
        public void AnimateEnemyHPbar()
        {
            // Animate enemy hp bar
            AnimateHpBar(EnemyHpBar, EnemyUnit!.Statistics.Hp, EnemyUnit.Statistics.HpMax);
        }

        private void AnimateHpBar(FrameworkElement hpBar, double currentHp, double maxHp)
        {
            var newTopMargin = 3.8 * (currentHp / maxHp * 100);
            newTopMargin = newTopMargin <= 30 ? 30 : newTopMargin;
            if(currentHp <= 0)
                newTopMargin = 0;
            var newMargin = new Thickness(hpBar.Margin.Left, 380 - newTopMargin, hpBar.Margin.Right, hpBar.Margin.Bottom);

            var hpBarAnimation = new ThicknessAnimation
            {
                To = newMargin,
                Duration = TimeSpan.FromSeconds(0.5) // Duration of the animation
            };
            hpBar.BeginAnimation(MarginProperty, hpBarAnimation);
        }

        public void StartAttack(object sender, RoutedEventArgs e)
        {
            PreviewAttackGrid.Visibility = Visibility.Visible;
            chooseAttack.State.SetState(new Attack(chooseAttack.State, attackingAllay, EnemyButton, EnemyNear, this));

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
