using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Engine.FEMap;
using Engine.Models;
using WpfUI.ViewModels;

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
            private set => SetField(ref _enemyDamage, value);
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

            AllayUnit = ((Tile)attackingAllay.Tag).UnitOn;
            EnemyUnit = ((Tile)EnemyButton.Tag).UnitOn;

            if (chooseAttack._mapBuilder.MapLogic.CurrentTurnState.CurrentActionState.GetType() != typeof(ChooseAttack)) return;

            AllayButton = attackingAllay;
            OnPropertyChanged(nameof(EnemyPolygon));
            OnPropertyChanged(nameof(AllayPolygon));

            EnemyDamage = EnemyUnit.EquipedWeapon.WeaponType is WeaponType.Tome
                ? AllayUnit.Get_Attack() - AllayUnit.Statistics.Resistance
                : AllayUnit.Get_Attack() - AllayUnit.Statistics.Defense;

            EnemyHit = EnemyUnit.Get_Hit() - AllayUnit.Dodge;
            EnemyCrit = EnemyUnit.Get_Crit();

            AllayDamage = AllayUnit.EquipedWeapon.WeaponType is WeaponType.Tome
                ? AllayUnit.Get_Attack() - EnemyUnit.Statistics.Resistance
                : AllayUnit.Get_Attack() - EnemyUnit.Statistics.Defense;

            AllayHit = AllayUnit.Get_Hit() - EnemyUnit.Dodge;
            AllayCrit = AllayUnit.Get_Crit();

            PreviewAttackGrid.Visibility = Visibility.Visible;
            LoadHpBars();
        }

        public void LoadHpBars()
        {
            // allay hp bar
            var newTopMargin = 3.8 * (AllayUnit!.Statistics.Hp / (double)AllayUnit.Statistics.HpMax * 100);
            newTopMargin = newTopMargin <= 50 ? 50 : newTopMargin;
            AllayHp.Margin = new Thickness(AllayHp.Margin.Left, 380 - newTopMargin, AllayHp.Margin.Right, AllayHp.Margin.Bottom);

            // enemy hp bar
            var newTopMarginEnemy = 3.8 * (EnemyUnit!.Statistics.Hp / (double)EnemyUnit.Statistics.HpMax * 100);
            newTopMarginEnemy = newTopMarginEnemy <= 50 ? 50 : newTopMarginEnemy;
            EnemyHp.Margin = new Thickness(EnemyHp.Margin.Left, 380 - newTopMarginEnemy, EnemyHp.Margin.Right, EnemyHp.Margin.Bottom);
        }

        private void StartAttack(object sender, RoutedEventArgs e)
        {
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
