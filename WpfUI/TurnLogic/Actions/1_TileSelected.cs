using Engine.FEMap;
using Engine.Models;
using System.Windows;
using System.Windows.Controls;

namespace WpfUI.TurnLogic.Actions;

public class TileSelected(MapLogic turnMapLogic) : ActionState(turnMapLogic)
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

    public override void Move(object sender, RoutedEventArgs e)
    {
        //if (sender is Button { Tag: Tile { UnitOn.Type: UnitType.Allay } tile } button)
        //{
        //    if (TurnMapLogic.MapBuilder.CurrentSelectedTile != null)
        //    {
        //        var selectedButton = TurnMapLogic.MapBuilder.MapCosmetics.GetButtonBasedOnTile(TurnMapLogic.MapBuilder.CurrentSelectedTile);
        //        TurnMapLogic.MapBuilder.MapCosmetics.TileDeSelected(selectedButton!);
        //        TurnMapLogic.MapBuilder.CurrentSelectedTile = null;
        //    }

        //    TurnMapLogic.MapBuilder.MapCosmetics.TileSelected(button);
        //    TurnMapLogic.MapBuilder.CurrentSelectedTile = tile;
        //    TurnMapLogic.MapBuilder.MovingUnit = tile.UnitOn;
        //}
    }

    public void ClearCurrentSelectedButton(Button currentSelectedTileButton)
    {
        TurnMapLogic.MapBuilder.MapCosmetics.TileDeSelected(currentSelectedTileButton);
        currentSelectedTileButton.Content = null;
        TurnMapLogic.MapBuilder.MovingUnit = null;
        TurnMapLogic.MapBuilder.CurrentSelectedTile = null;
    }

   
}