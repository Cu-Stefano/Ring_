using Engine.FEMap;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Engine.Models;

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
            previewAttack.attackingAllay.AttackCalculations(enemy, previewAttack.AllayDamage, previewAttack.EnemyDamage, _mapCosmetics);

            State.SetState(new TileToBeSelected(State));
        }
         
        public override async void OnExit()
        {

            foreach (var e in enemyNear)
            {
                _mapCosmetics.SetButtonAsDeselected(e);
            }

            previewAttack.AnimateEnemyHPbar();
            await Task.Delay(700);
            previewAttack.AnimateAllayHPbar();
            await Task.Delay(1200);

            previewAttack.PreviewAttackGrid.Visibility = Visibility.Hidden;
            previewAttack.EnemyButton = null;
            previewAttack.AllayButton = null;
            previewAttack.EnemyNear?.Clear();

            var unitOn = ((Tile)attackingAllay.Tag).UnitOn;
            if (unitOn != null)
            {
                _mapBuilder.UnitCantMoveNoMore(attackingAllay);
            }

            _startinPosition = (0, 0);
            _currentPosition = (0, 0);
            //if alla units moved change state to enemy turn
            if (MapBuilder.AllayButtonList.All(allay => !((Tile)allay.Tag).UnitOn!.CanMove))
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