using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Engine;
using Engine.FEMap;
using Engine.Models;
using WpfUI.TurnLogic;

namespace WpfUI;

public class MapLogic : BaseNotification
{
    internal readonly MapBuilder MapBuilder;
    public TurnState CurrentTurnState { get; set;}

    public string TurnName => CurrentTurnState.GetType().Name;

    public MapLogic(MapBuilder mapBuilder)
    {
        MapBuilder = mapBuilder;
        SetState(new AllayTurn(this));
    }

    public void SetState(TurnState newTurnState)
    {
        MapBuilder.ClearGamesessionGui();

        CurrentTurnState?.OnExit();
        CurrentTurnState = newTurnState;
        CurrentTurnState.OnEnter();

        //aggiungo i gestori d'evento del nuovo stato
        foreach (var button in MapBuilder.ActualMap.SelectMany(row => row))
        {
            button.MouseEnter += TileButton_Over;
            button.Click += Move_unit;
            button.MouseDoubleClick += UnitSelected;
        }

        OnPropertyChanged(nameof(TurnName));
    }

    public void TileButton_Over(object sender, RoutedEventArgs e)
    {
        CurrentTurnState.TileButton_Over(sender, e);
    }

    public void UnitSelected(object sender, RoutedEventArgs e)
    {
        CurrentTurnState.Doule_Click(sender, e);
    }

    public void Move_unit(object sender, RoutedEventArgs e)
    {
        CurrentTurnState.Single_Click(sender, e);
    }
}
