using Engine.FEMap;
using Engine.Models;
using System.Windows;
using System.Windows.Controls;

namespace WpfUI.TurnLogic.Actions;

public class TileToBeSelected(MapLogic turnMapLogic) : ActionState(turnMapLogic)
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

    }

    public void ClearCurrentSelectedButton(Button currentSelectedTileButton)
    {
        TurnMapLogic.MapBuilder.MapCosmetics.TileDeSelected(currentSelectedTileButton);
        currentSelectedTileButton.Content = null;
        TurnMapLogic.MapBuilder.MovingUnit = null;
        TurnMapLogic.MapBuilder.CurrentSelectedTile = null;
    }

   
}