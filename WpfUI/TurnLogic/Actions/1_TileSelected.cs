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

    public override void Move_Unit(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Tile { UnitOn: null, Walkable: true } tile } button)
        {
            if (TurnMapLogic.MapBuilder.CurrentSelectedTile is { UnitOn: not null } && TurnMapLogic.MapBuilder.CurrentSelectedTile != tile)
            {
                tile.UnitOn = TurnMapLogic.MapBuilder.MovingUnit;//sposto l'unità

                // Deseleziona la Tile dell'unità che si vuole spostare
                var currentSelectedTileButton = TurnMapLogic.MapBuilder.MapCosmetics.GetButtonBasedOnTile(TurnMapLogic.MapBuilder.CurrentSelectedTile)!;

                button.Content = currentSelectedTileButton.Content;//copio il tipo/colore dell'unità
                TurnMapLogic.MapBuilder.GameSession.CurrentTile = tile;
                TurnMapLogic.MapBuilder.GameSession.CurrentUnit = tile.UnitOn;

                ClearCurrentSelectedButton(currentSelectedTileButton);

                TurnMapLogic.MapBuilder.GameSession.ClassWeapons = string.Join("\n", TurnMapLogic.MapBuilder.GameSession.CurrentUnit!.Class.UsableWeapons);
            }
        }
    }

    public void ClearCurrentSelectedButton(Button currentSelectedTileButton)
    {
        TurnMapLogic.MapBuilder.MapCosmetics.TileDeSelected(currentSelectedTileButton);
        currentSelectedTileButton.Content = null;
        TurnMapLogic.MapBuilder.MovingUnit = null;
        TurnMapLogic.MapBuilder.CurrentSelectedTile = null;
    }

   
}