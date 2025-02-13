using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Engine.FEMap;

namespace WpfUI;

public class MapLogic
{
    private MapBuilder _mapBuilder;
    public TurnState Turno {get; set;}

    public MapLogic(MapBuilder mapBuilder)
    {
        _mapBuilder = mapBuilder;
    }

    public void AddEventHandlers(Button button)
    {
        button.MouseEnter += TileButton_Over;
        button.Click += Move_unit;
        button.MouseDoubleClick += UnitSelected;
    }

    public void TileButton_Over(object sender, RoutedEventArgs e)
    {
        if (_mapBuilder.CurrentSelectedTile != null)
        {
            _mapBuilder.GameSession.CurrentTile = _mapBuilder.CurrentSelectedTile;
            _mapBuilder.GameSession.CurrentUnit = _mapBuilder.MovingUnit!;
            _mapBuilder.GameSession.ClassWeapons = string.Join("\n", _mapBuilder.GameSession.CurrentUnit.Class.UsableWeapons);
        }
        else if (sender is Button { Tag: Tile tile })
        {
            _mapBuilder.GameSession.CurrentUnit = tile.UnitOn;
            _mapBuilder.GameSession.ClassWeapons = tile.UnitOn != null ? string.Join("\n", _mapBuilder.GameSession.CurrentUnit!.Class.UsableWeapons) : "";
            _mapBuilder.GameSession.CurrentTile = tile;
        }
    }

    public void UnitSelected(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Tile { UnitOn: not null } tile } button)
        {
            if (_mapBuilder.CurrentSelectedTile != null)
            {
                var selectedButton = _mapBuilder.MapCosmetics.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile);
                _mapBuilder.MapCosmetics.TileDeSelected(selectedButton!);
                _mapBuilder.CurrentSelectedTile = null;
            }

            _mapBuilder.MapCosmetics.TileSelected(button);
            _mapBuilder.CurrentSelectedTile = tile;
            _mapBuilder.MovingUnit = tile.UnitOn;
        }
    }

    public void Move_unit(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Tile { UnitOn: null, Walkable: true } tile } button)
        {
            if (_mapBuilder.CurrentSelectedTile is { UnitOn: not null } && _mapBuilder.CurrentSelectedTile != tile)
            {
                tile.UnitOn = _mapBuilder.MovingUnit;//sposto l'unità

                // Deseleziona la Tile dell'unità che si vuole spostare
                var currentSelectedTileButton = _mapBuilder.MapCosmetics.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile)!;
                _mapBuilder.MapCosmetics.TileDeSelected(currentSelectedTileButton);

                button.Content = currentSelectedTileButton.Content;//copio il tipo/colore dell'unità
                _mapBuilder.GameSession.CurrentTile = tile;
                _mapBuilder.GameSession.CurrentUnit = tile.UnitOn;

                currentSelectedTileButton.Content = null;
                _mapBuilder.MovingUnit = null;
                _mapBuilder.CurrentSelectedTile = null;

                _mapBuilder.GameSession.ClassWeapons = string.Join("\n", _mapBuilder.GameSession.CurrentUnit!.Class.UsableWeapons);
            }
        }
    }

    

}