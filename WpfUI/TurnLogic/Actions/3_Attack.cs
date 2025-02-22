using Engine.FEMap;
using System.Windows;
using System.Windows.Controls;

namespace WpfUI.TurnLogic.Actions;

public class Attack(TurnState state, Button buttonToThanDeselect, Button enemy, List<Button> enemyNear) : ActionState(state)
{
    public override void OnEnter()
    {
        foreach (var e in enemyNear)
        {
            if (e != enemy)
                _mapCosmetics.SetButtonAsDeselected(e);
        }   
    }

    public override void OnExit()
    {
        _gameSession.PreviewAttack.PreviewAttackGrid.Visibility = Visibility.Hidden;
        _mapCosmetics.SetButtonAsDeselected(enemy);
        //if alla units moved change state to enemy turn
        if (_mapBuilder.AllayButtonList.All(allay => !((Tile)allay.Tag).UnitOn!.CanMove))
            State._turnMapLogic.SetState(new EnemyTurn(State._turnMapLogic));

        ((Tile)buttonToThanDeselect.Tag).UnitOn.CanMove = false;
        _mapBuilder.UnitCantMoveNoMore(buttonToThanDeselect);

        //da spostare in un'altro metodo dopo che l'attaco è avvenuto
        ////CHANGE STATE BACK TO 0
        //State.SetState(new TileToBeSelected(State));
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
