using Engine.FEMap;
using Engine.Models;
using System.Windows;
using System.Windows.Controls;
using Engine.ViewModels;
using System.Windows.Input;

namespace WpfUI.TurnLogic.Actions;

public class TileSelected(TurnState state) : ActionState(state)
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
    }

    public override void Move_Unit(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Tile { UnitOn: null, Walkable: true } tile } button)
        {
            if (_mapBuilder.CurrentSelectedTile is { UnitOn: not null } && _mapBuilder.CurrentSelectedTile != tile)
            {
                tile.UnitOn = _mapBuilder.MovingUnit;//sposto l'unità

                // Deseleziona la Tile dell'unità che si vuole spostare
                var currentSelectedTileButton = _mapCosmetics.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile)!;

                button.Content = currentSelectedTileButton.Content;//copio il tipo/colore dell'unità
                _gameSession.CurrentTile = tile;
                _gameSession.CurrentUnit = tile.UnitOn;

                ClearCurrentSelectedButton(currentSelectedTileButton);

                _gameSession.ClassWeapons = string.Join("\n", _gameSession.CurrentUnit!.Class.UsableWeapons);

                //CHANGE STATE BACK TO 0
                State.SetState(new TileToBeSelected(State));
            }
        }
    }

    public void ClearCurrentSelectedButton(Button currentSelectedTileButton)
    {
        _mapCosmetics.TileDeSelected(currentSelectedTileButton);
        currentSelectedTileButton.Content = null;
        _mapBuilder.MovingUnit = null;
        _mapBuilder.CurrentSelectedTile = null;
    }

   
}