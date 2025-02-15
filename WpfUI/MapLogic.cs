using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Engine.FEMap;
using Engine.Models;
using WpfUI.TurnLogic;

namespace WpfUI;

public class MapLogic : INotifyPropertyChanged
{
    public readonly MapBuilder MapBuilder;
    public TurnState CurrentTurnState;

    public string TurnName => CurrentTurnState.GetType().Name;

    public MapLogic(MapBuilder mapBuilder)
    {
        MapBuilder = mapBuilder;
        SetState(new AllayTurn(this));
    }

    public void SetState(TurnState newTurnState)
    {
        //tolgo i gestori d'evento del vechhio stato
        foreach (var button in MapBuilder.ActualMap.SelectMany(row => row))
        {
            button.Click -= Move_unit;
            button.MouseDoubleClick -= UnitSelected;
        }

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
        CurrentTurnState.UnitSelected(sender, e);
    }

    public void Move_unit(object sender, RoutedEventArgs e)
    {
        CurrentTurnState.Move_unit(sender, e);
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
