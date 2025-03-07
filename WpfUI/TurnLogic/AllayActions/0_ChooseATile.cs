using Engine.FEMap;
using Engine.Models;
using System.Windows;
using System.Windows.Controls;

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
        if (sender is Button { Tag: Tile { UnitOn: { Type: UnitType.Allay, CanMove: true } } tile } button)
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
    }

    public override void Single_Click(object sender, RoutedEventArgs e)
    {

    }
}