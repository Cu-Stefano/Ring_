using Engine.FEMap;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Engine.Models;
using WpfUI.ViewModels;

namespace WpfUI.TurnLogic.Actions;

public abstract class ActionState
{
    protected internal TurnState State { get; }
    public readonly MapBuilder _mapBuilder;
    protected readonly GameSession _gameSession;
    protected readonly MapCosmetics _mapCosmetics;
    internal static (int x, int y) _startinPosition { get; set; }
    internal static (int x, int y) _currentPosition { get; set; }

    protected ActionState(TurnState state)
    {
        State = state;
        _mapBuilder = State._turnMapLogic.MapBuilder;
        _gameSession = _mapBuilder.GameSession;
        _mapCosmetics = _mapBuilder.MapCosmetics;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public virtual void Back_Action(object sender, MouseButtonEventArgs e)
    {
        if (GetType() == typeof(Attack) || GetType() == typeof(TileToBeSelected)) return;//non andare indietro se sto attaccando

        if (_currentPosition != _startinPosition)
            Move_Unit(GetButtonAtPosition(_currentPosition), GetButtonAtPosition(_startinPosition));

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
        _startinPosition = (0, 0);
        _currentPosition = (0, 0);
        //CHANGE STATE BACK TO 0 
        State.SetState(new TileToBeSelected(State));
    }

    public void Move_Unit(Button buttA, Button buttB)
    {
        var tileA = (Tile)buttA.Tag;
        var tileB = (Tile)buttB.Tag;

        var unit = tileA.UnitOn;

        buttB.Content = buttA.Content;
        buttA.Content = null;

        tileA.UnitOn = null;
        tileB.UnitOn = unit;

        //aggiorno le AllayList o EnemyList
        switch (unit.Type)
        {
            case UnitType.Allay:
               
                if (!MapBuilder.AllayButtonList.Remove(buttA))
                    throw new InvalidEnumArgumentException("Error in removing the button from the list");

                MapBuilder.AllayButtonList.Add(buttB);
                break;
            case UnitType.Enemy:
                MapBuilder.EnemyButtonList.Remove(buttA);
                MapBuilder.EnemyButtonList.Add(buttB);
                break;
            default:
                break;
        }
    }
    public Button? GetButtonAtPosition((int x, int y) position)
    {
        if (position.x < 0 || position.y < 0 || position.x >= _mapBuilder.ActualMap.Count || position.y >= _mapBuilder.ActualMap[0].Count)
        {
            return null;
        }
        return _mapBuilder.ActualMap[position.x][position.y];
    }
    public abstract void Mouse_Over(object sender, RoutedEventArgs e);
    public abstract void Double_Click(object sender, RoutedEventArgs e);
    public abstract void Single_Click(object sender, RoutedEventArgs e);
}