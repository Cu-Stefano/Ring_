using Engine.FEMap;
using Engine.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfUI.TurnLogic.Actions;

public class TileToBeSelected(TurnState state) : ActionState(state)
{
    public override void OnEnter()
    {

    }

    public override void OnExit()
    {

    }

    public override void Mouse_Over(object sender, RoutedEventArgs e)
    {
    }

    //SELECT_UNIT
    public override void Double_Click(object sender, RoutedEventArgs e)
    {
        EnemyPathAlgorithm?.ResetAll();
        if (sender is not Button { Tag: Tile { UnitOn: not null } tile } button) return;

        if (tile.UnitOn!.Type == UnitType.Allay)
        {
            if (_mapBuilder.CurrentSelectedTile != null)
            {
                var selectedButton = _mapBuilder.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile);
                _mapCosmetics.SetButtonAsDeselected(selectedButton!);
                _mapBuilder.CurrentSelectedTile = null;
            }
            _mapBuilder.CurrentSelectedTile = tile;
            _mapBuilder.MovingUnit = tile.UnitOn;

            _startinPosition = _mapBuilder.GetButtonPosition(button);
            _currentPosition = _startinPosition;
            //CHANGE STATE TO 1
            State.SetState(new TileSelected(State, button));
        }
        else if (tile.UnitOn!.Type == UnitType.Enemy)
        {
            EnemyPathAlgorithm = new PathAlgorithm(button, _mapCosmetics);
            EnemyPathAlgorithm.Execute();
        }
    }

    public PathAlgorithm EnemyPathAlgorithm { get; set; }

    public override void Back_Action(object sender, MouseButtonEventArgs e)
    {
        EnemyPathAlgorithm?.ResetAll();
    }

    public override void Single_Click(object sender, RoutedEventArgs e)
    {
    }
}