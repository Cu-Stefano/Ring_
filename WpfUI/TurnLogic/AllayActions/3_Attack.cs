using Engine.FEMap;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Engine.Models;
using WpfUI.Utilities;

namespace WpfUI.TurnLogic.Actions
{
    public class Attack(
    TurnState state,
    Button? attackingAllay,
    Button? enemy,
    List<Button?> enemyNear,
    PreviewAttack previewAttack) : ActionState(state)
    {
        public override async void OnEnter()
        {
            await previewAttack.attackingAllay.AttackCalculations(_mapCosmetics, previewAttack, _mapBuilder);
            
            if (state.GetType() != typeof(EnemyTurn))
                State.SetState(new TileToBeSelected(State));
            else OnExit(); // Call OnExit without waiting
        }

        public override void OnExit()
        {
            PerformExitAsync();
        }

        private void PerformExitAsync()
        {
            foreach (var e in enemyNear)
            {
                _mapCosmetics.SetButtonAsDeselected(e);
            }

            previewAttack.PreviewAttackGrid.Visibility = Visibility.Hidden;
            previewAttack.EnemyButton = null;
            previewAttack.AllayButton = null;
            previewAttack.EnemyNear?.Clear();

            var unitOn = attackingAllay.GetTile().UnitOn;
            if (unitOn != null)
            {
                _mapBuilder.UnitCantMoveNoMore(attackingAllay);
            }

            _startinPosition = (0, 0);
            _currentPosition = (0, 0);
            //if alla units moved change state to enemy turn
            if (MapBuilder.AllayButtonList.All(allay => !allay.GetTile().UnitOn!.CanMove))
                State._turnMapLogic.SetState(new EnemyTurn(State._turnMapLogic));
        }

        public override void Mouse_Over(object sender, RoutedEventArgs e)
        {

        }

        public override void Double_Click(object sender, RoutedEventArgs e)
        {

        }

        public override void Single_Click(object sender, RoutedEventArgs e)
        {

        }
    }



}