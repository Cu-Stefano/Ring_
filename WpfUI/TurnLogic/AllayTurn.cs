using Engine.FEMap;
using Engine.Models;
using Engine.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfUI.TurnLogic.Actions;

namespace WpfUI.TurnLogic;

public class AllayTurn(MapLogic turnMapLogic) : TurnState(turnMapLogic)
{
    public ActionState CurrentActionState;

    public override void OnEnter()
    {
        //SetState(new TileToBeSelected(TurnMapLogic));
    }

    public override void OnExit()
    {

    }

    public void SetState(ActionState action)
    {
        ////tolgo i gestori d'evento del vechhio stato
        //foreach (var button in TurnMapLogic.MapBuilder.ActualMap.SelectMany(row => row))
        //{
        //    button.Click -= Move;
        //    button.MouseDoubleClick -= CurrentActionState.CalculateTrail;
        //}
        //TurnMapLogic.MapBuilder.ClearGamesessionGui();

        CurrentActionState?.OnExit();
        CurrentActionState = action;
        CurrentActionState.OnEnter();

        //aggiungo i gestori d'evento del nuovo stato
        foreach (var button in TurnMapLogic.MapBuilder.ActualMap.SelectMany(row => row))
        {
            button.MouseEnter += CurrentActionState.CalculateTrail;
            button.Click += Move_unit;
        }

    }

    public override void TileButton_Over(object sender, RoutedEventArgs e)
    {
        if (TurnMapLogic.MapBuilder.CurrentSelectedTile != null)
        {
            TurnMapLogic.MapBuilder.GameSession.CurrentTile = TurnMapLogic.MapBuilder.CurrentSelectedTile;
            TurnMapLogic.MapBuilder.GameSession.CurrentUnit = TurnMapLogic.MapBuilder.MovingUnit!;
            TurnMapLogic.MapBuilder.GameSession.ClassWeapons = string.Join("\n", TurnMapLogic.MapBuilder.GameSession.CurrentUnit.Class.UsableWeapons);
        }
        else if (sender is Button { Tag: Tile tile })
        {
            TurnMapLogic.MapBuilder.GameSession.CurrentUnit = tile.UnitOn;
            TurnMapLogic.MapBuilder.GameSession.ClassWeapons = tile.UnitOn != null ? string.Join("\n", TurnMapLogic.MapBuilder.GameSession.CurrentUnit!.Class.UsableWeapons) : "";
            TurnMapLogic.MapBuilder.GameSession.CurrentTile = tile;
        }

        
    }

    public override void UnitSelected(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Tile { UnitOn.Type: UnitType.Allay } tile } button)
        {
            if (TurnMapLogic.MapBuilder.CurrentSelectedTile != null)
            {
                var selectedButton = TurnMapLogic.MapBuilder.MapCosmetics.GetButtonBasedOnTile(TurnMapLogic.MapBuilder.CurrentSelectedTile);
                TurnMapLogic.MapBuilder.MapCosmetics.TileDeSelected(selectedButton!);
                TurnMapLogic.MapBuilder.CurrentSelectedTile = null;
            }

            TurnMapLogic.MapBuilder.MapCosmetics.TileSelected(button);
            TurnMapLogic.MapBuilder.CurrentSelectedTile = tile;
            TurnMapLogic.MapBuilder.MovingUnit = tile.UnitOn;
        }
    }

    public override void Move_unit(object sender, RoutedEventArgs e)
    {
        //CurrentActionState.Move(sender, e);
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