using System.ComponentModel;
using System.Runtime.CompilerServices;
using Engine.FEMap;
using Engine.Models;
using Engine.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        foreach (var but in _mapBuilder.AllayButtonList)
        {
            var tile = (Tile)but.Tag;
            tile.UnitOn.CanMove = true;
            but.Content = MapCosmetics.GetTriangle(tile.UnitOn);
        }
    }

    public override void SetState(ActionState action)
    {
        //rimuovo i gestori d'evento dello stato corrente
        if (CurrentActionState != null)
        {
            
            foreach (var button in _mapBuilder.ActualMap.SelectMany(row => row))
            {
                button.MouseEnter -= CurrentActionState.CalculateTrail;
                button.Click -= Move_unit;
            }
        }
        
        CurrentActionState?.OnExit();
        CurrentActionState = action;
        CurrentActionState.OnEnter();

        //aggiungo i gestori d'evento del nuovo stato
        foreach (var button in _mapBuilder.ActualMap.SelectMany(row => row))
        {
            button.MouseEnter += CurrentActionState.CalculateTrail;
            button.Click += Move_unit;
        }
    }

    public override void UnitSelected(object sender, RoutedEventArgs e)
    {
        CurrentActionState.UnitSelected(sender, e);
    }

    public override void Move_unit(object sender, RoutedEventArgs e)
    {
        CurrentActionState.Move_Unit(sender, e);
    }
}
