using Engine.FEMap;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfUI.ViewModels;

namespace WpfUI.TurnLogic.Actions;

public abstract class ActionState
{
    protected internal TurnState State { get; }
    public readonly MapBuilder _mapBuilder;
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
        if(GetType() == typeof(ChooseAttack) || GetType() == typeof(Attack)) return;//non andare indietro se sto attaccando
        // Verifica se l'elemento cliccato è un pulsante con un'unità selezionata
        if (e.OriginalSource is not Button { Tag: Tile })
        {
            // Deseleziona l'unità corrente e ripristina il bordo del pulsante
            if (_mapBuilder.CurrentSelectedTile != null)
            {
                var currentSelectedTileButton = _mapBuilder.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile);

                _mapCosmetics.SetButtonAsDeselected(currentSelectedTileButton!);

                _mapBuilder.CurrentSelectedTile = null;
                _gameSession.CurrentTile = null;
                _gameSession.CurrentUnit = null;
                _gameSession.ClassWeapons = string.Empty;
            }
        }
        //CHANGE STATE BACK TO 0
        State.SetState(new TileToBeSelected(State));
    }
    public abstract void Mouse_Over(object sender, RoutedEventArgs e);
    public abstract void Double_Click(object sender, RoutedEventArgs e);
    public abstract void Single_Click(object sender, RoutedEventArgs e);
}