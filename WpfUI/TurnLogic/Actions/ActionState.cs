using Engine.FEMap;
using Engine.ViewModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfUI.TurnLogic.Actions;

public abstract class ActionState
{
    protected internal TurnState State { get; }
    protected readonly MapBuilder _mapBuilder;
    protected readonly GameSession _gameSession;
    protected readonly MapCosmetics _mapCosmetics;
    protected ActionState(TurnState state)
    {
        State = state;
        _mapBuilder = State._turnMapLogic.MapBuilder;
        _gameSession = _mapBuilder.GameSession;
        _mapCosmetics = _mapBuilder.MapCosmetics;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        // Verifica se l'elemento cliccato è un pulsante con un'unità selezionata
        if (e.OriginalSource is not Button { Tag: Tile })
        {
            // Deseleziona l'unità corrente e ripristina il bordo del pulsante
            if (_mapBuilder.CurrentSelectedTile != null)
            {
                var currentSelectedTileButton = _mapCosmetics.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile);

                _mapCosmetics.TileDeSelected(currentSelectedTileButton!);

                _mapBuilder.CurrentSelectedTile = null;
                _gameSession.CurrentTile = null;
                _gameSession.CurrentUnit = null;
                _gameSession.ClassWeapons = string.Empty;
            }
        }
        //CHANGE STATE BACK TO 0
        State.SetState(new TileToBeSelected(State));
    }
    public abstract void CalculateTrail(object sender, RoutedEventArgs e);
    public abstract void UnitSelected(object sender, RoutedEventArgs e);
    public abstract void Move_Unit(object sender, RoutedEventArgs e);
}