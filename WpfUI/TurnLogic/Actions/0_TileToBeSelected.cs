using Engine.FEMap;
using Engine.Models;
using Engine.ViewModels;
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
        if (sender is Button { Tag: Tile { UnitOn.Type: UnitType.Allay } tile } button)
        {
            if (_mapBuilder.CurrentSelectedTile != null)
            {
                var selectedButton = _mapCosmetics.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile);
                _mapCosmetics.TileDeSelected(selectedButton!);
                _mapBuilder.CurrentSelectedTile = null;
            }
            _mapBuilder.CurrentSelectedTile = tile;
            _mapBuilder.MovingUnit = tile.UnitOn;

            //CHANGE STATE TO 1
            State.SetState(new TileSelected(State, (Button)sender));
        }
    }

    public override void Move_Unit(object sender, RoutedEventArgs e)
    {

    }

    public void ClearCurrentSelectedButton(Button currentSelectedTileButton)
    {
        _mapCosmetics.TileDeSelected(currentSelectedTileButton);
        currentSelectedTileButton.Content = null;
        _mapBuilder.MovingUnit = null;
        _mapBuilder.CurrentSelectedTile = null;
    }

   
}