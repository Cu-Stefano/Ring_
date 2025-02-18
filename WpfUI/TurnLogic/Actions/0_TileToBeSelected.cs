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

    public override void CalculateTrail(object sender, RoutedEventArgs e)
    {
        
    }

    public override void UnitSelected(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Tile { UnitOn: { Type: UnitType.Allay, CanMove: true } } tile } button)
        {
            if (_mapBuilder.CurrentSelectedTile != null)
            {
                var selectedButton = _mapBuilder.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile);
                _mapCosmetics.SetTileAsDeselected(selectedButton!);
                _mapBuilder.CurrentSelectedTile = null;
            }
            _mapBuilder.CurrentSelectedTile = tile;
            _mapBuilder.MovingUnit = tile.UnitOn;

            //CHANGE STATE TO 1
            State.SetState(new TileSelected(State, button));
        }
    }

    public override void Move_Unit(object sender, RoutedEventArgs e)
    {

    }
}