using Engine.FEMap;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using WpfUI.TurnLogic.Actions;
using WpfUI.TurnLogic;
using WpfUI;
using WpfUI.Utilities;

public class AllayTurn : TurnState
{
    public AllayTurn(MapLogic turnMapLogic) : base(turnMapLogic) { }

    public override void OnEnter()
    {
        SetState(new TileToBeSelected(this));
    }

    public override void OnExit()
    {
        // Ripristino la possibilità di movimento delle unità alleate
        foreach (var but in MapBuilder.AllayButtonList)
        {
            var tile = but.GetTile();
            tile.UnitOn.CanMove = true;
            but.Content = MapCosmetics.GetPolygon(tile.UnitOn);
        }
        CurrentActionState.OnExit();
    }

    public override void SetState(ActionState action)
    {
        if (CurrentActionState?.GetType() == action.GetType())
            return;
        if (action.GetType() != typeof(Attack))
            CurrentActionState?.OnExit();
        CurrentActionState = action;
        CurrentActionState.OnEnter();
    }

    public override void Doule_Click(object sender, RoutedEventArgs e)
    {
        CurrentActionState.Double_Click(sender, e);
    }

    public override void Single_Click(object sender, RoutedEventArgs e)
    {
        CurrentActionState.Single_Click(sender, e);
    }

}
