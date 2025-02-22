using Engine.FEMap;
using System.Windows;
using WpfUI.TurnLogic.Actions;

namespace WpfUI.TurnLogic;

public class AllayTurn(MapLogic turnMapLogic): TurnState(turnMapLogic)
{
    public override void OnEnter()
    {
        SetState(new TileToBeSelected(this));
    }

    public override void OnExit()
    {
        //ripristino la possibilità di movimento delle unità alleate
        
        foreach (var but in _mapBuilder.AllayButtonList)
        {
            var tile = (Tile)but.Tag;
            tile.UnitOn.CanMove = true;
            but.Content = MapCosmetics.GetTriangle(tile.UnitOn);
        }
        CurrentActionState.OnExit();
    }

    public override void SetState(ActionState action)
    {
        if (CurrentActionState?.GetType() == action.GetType()) 
            return;
        //rimuovo i gestori d'evento dello stato corrente
        if (CurrentActionState != null)
        {
            foreach (var button in _mapBuilder.ActualMap.SelectMany(row => row))
            {
                button.MouseEnter -= CurrentActionState.Mouse_Over;
                button.Click -= Single_Click;
            }
        }

        CurrentActionState?.OnExit();
        CurrentActionState = action;
        CurrentActionState.OnEnter();

        //aggiungo i gestori d'evento del nuovo stato
        foreach (var button in _mapBuilder.ActualMap.SelectMany(row => row))
        {
            button.MouseEnter += CurrentActionState.Mouse_Over;
            button.Click += Single_Click;
        }
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
