using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Engine.FEMap;
using WpfUI.TurnLogic;

namespace WpfUI;

public class MapLogic : INotifyPropertyChanged
{
    public readonly MapBuilder MapBuilder;
    public ITurnState CurrentTurnState;

    public string TurnName => CurrentTurnState.GetType().Name;

    public MapLogic(MapBuilder mapBuilder)
    {
        MapBuilder = mapBuilder;
        SetState(new AllayTurn(this));
    }

    public void SetState(ITurnState newTurnState)
    {
        CurrentTurnState?.OnExit();
        CurrentTurnState = newTurnState;
        CurrentTurnState.OnEnter();
        OnPropertyChanged(nameof(TurnName));
    }

    public void TileButton_Over(object sender, RoutedEventArgs e)
    {
        if (MapBuilder.CurrentSelectedTile != null)
        {
            MapBuilder.GameSession.CurrentTile = MapBuilder.CurrentSelectedTile;
            MapBuilder.GameSession.CurrentUnit = MapBuilder.MovingUnit!;
            MapBuilder.GameSession.ClassWeapons = string.Join("\n", MapBuilder.GameSession.CurrentUnit.Class.UsableWeapons);
        }
        else if (sender is Button { Tag: Tile tile })
        {
            MapBuilder.GameSession.CurrentUnit = tile.UnitOn;
            MapBuilder.GameSession.ClassWeapons = tile.UnitOn != null ? string.Join("\n", MapBuilder.GameSession.CurrentUnit!.Class.UsableWeapons) : "";
            MapBuilder.GameSession.CurrentTile = tile;
        }
    }

    public void UnitSelected(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Tile { UnitOn: not null } tile } button)
        {
            if (MapBuilder.CurrentSelectedTile != null)
            {
                var selectedButton = MapBuilder.MapCosmetics.GetButtonBasedOnTile(MapBuilder.CurrentSelectedTile);
                MapBuilder.MapCosmetics.TileDeSelected(selectedButton!);
                MapBuilder.CurrentSelectedTile = null;
            }

            MapBuilder.MapCosmetics.TileSelected(button);
            MapBuilder.CurrentSelectedTile = tile;
            MapBuilder.MovingUnit = tile.UnitOn;
        }
    }

    public void Move_unit(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Tile { UnitOn: null, Walkable: true } tile } button)
        {
            if (MapBuilder.CurrentSelectedTile is { UnitOn: not null } && MapBuilder.CurrentSelectedTile != tile)
            {
                tile.UnitOn = MapBuilder.MovingUnit;//sposto l'unità

                // Deseleziona la Tile dell'unità che si vuole spostare
                var currentSelectedTileButton = MapBuilder.MapCosmetics.GetButtonBasedOnTile(MapBuilder.CurrentSelectedTile)!;

                button.Content = currentSelectedTileButton.Content;//copio il tipo/colore dell'unità
                MapBuilder.GameSession.CurrentTile = tile;
                MapBuilder.GameSession.CurrentUnit = tile.UnitOn;

                ClearCurrentSelectedButton(currentSelectedTileButton);

                MapBuilder.GameSession.ClassWeapons = string.Join("\n", MapBuilder.GameSession.CurrentUnit!.Class.UsableWeapons);
            }
        }
    }

    public void ClearCurrentSelectedButton(Button currentSelectedTileButton)
    {
        MapBuilder.MapCosmetics.TileDeSelected(currentSelectedTileButton);
        currentSelectedTileButton.Content = null;
        MapBuilder.MovingUnit = null;
        MapBuilder.CurrentSelectedTile = null;
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