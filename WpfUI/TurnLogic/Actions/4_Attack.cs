using System.Windows;
using System.Windows.Controls;

namespace WpfUI.TurnLogic.Actions;

public class Attack(MapLogic turnMapLogic) : ActionState(turnMapLogic)
{
    public override void OnEnter()
    {
        throw new NotImplementedException();
    }

    public override void OnExit()
    {
        throw new NotImplementedException();
    }

    public override void CalculateTrail(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    public override void Move(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    public void ClearCurrentSelectedButton(Button currentSelectedTileButton)
    {
        TurnMapLogic.MapBuilder.MapCosmetics.TileDeSelected(currentSelectedTileButton);
        currentSelectedTileButton.Content = null;
        TurnMapLogic.MapBuilder.MovingUnit = null;
        TurnMapLogic.MapBuilder.CurrentSelectedTile = null;
    }

   
}