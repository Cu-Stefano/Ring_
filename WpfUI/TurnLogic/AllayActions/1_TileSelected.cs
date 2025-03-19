using Engine.FEMap;
using Engine.Models;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using WpfUI.PathElements;
using WpfUI.Utilities;

namespace WpfUI.TurnLogic.Actions;

public class TileSelected : ActionState
{
    private Tile Tile { get; set; }
    private PathAlgorithm pathAlgorithm { get; set; }

    public TileSelected(TurnState state, Button? button) : base(state)
    {
        _mapCosmetics.SetButtonAsSelected(button);
        Tile = button.GetTile();
        pathAlgorithm = new PathAlgorithm(button, _mapCosmetics);
    }

    public override void OnEnter() 
    {
        pathAlgorithm.Execute();
    }

    public override void OnExit()
    {
        pathAlgorithm.ResetAll();
        _mapBuilder.MovingUnit = null;
        _mapBuilder.CurrentSelectedTile = null;
    }

    //CALCULATE_TRAIL
    public override void Mouse_Over(object sender, RoutedEventArgs e)
    {
        if (sender is Button bx && !pathAlgorithm.Path.Contains(bx))
            return;

        foreach (var button in pathAlgorithm.Path)
            _mapCosmetics.SetButtonAsDeselected(button);
        
        var currNode = pathAlgorithm.GetNOdeFromButton((Button)sender);
        while (currNode != pathAlgorithm.ONode)
        {
            _mapCosmetics.SetTrailSelector(currNode.button);
            currNode = currNode.Parent;
        }
    }

    public override void Double_Click(object sender, RoutedEventArgs e)
    {
    }

    //MOVE_UNIT
    public override void Single_Click(object sender, RoutedEventArgs e)
    {
        //il sender dell'evento deve essere un bottone nel range di movimento dell'unità
        var butt = (Button)sender;
        var til = butt.GetTile();
        var currentSelectedTileButton = _mapBuilder.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile)!;

        //se clicco su un nemico vicino
        if (til.UnitOn is { Type: UnitType.Enemy } && pathAlgorithm.NearEnemy.Contains(butt))
        {
            var aux = CloneButton(currentSelectedTileButton);

            currentSelectedTileButton.Content = null;

            currentSelectedTileButton.Content = aux.Content;
            currentSelectedTileButton.Tag = aux.Tag;

            State.SetState(new ChooseAttack(State, pathAlgorithm.NearEnemy, currentSelectedTileButton));

            return;
        }

        //condizioni di uscita
        if (sender is not Button { Tag: Tile { UnitOn: null, Walkable: true } tile } button || !pathAlgorithm.Path.Contains(button))
            return;
        if (_mapBuilder.CurrentSelectedTile is not { UnitOn: not null })
            return;

        _currentPosition = _mapBuilder.GetButtonPosition(butt);
        MoveUnit.Move_Unit(currentSelectedTileButton, button);

        const bool near = true;
        var newPathAlgorithm = new PathAlgorithm(button, _mapCosmetics);
        newPathAlgorithm.Execute(near);

        _gameSession.CurrentTile = tile;
        _gameSession.CurrentUnit = tile.UnitOn;
        _gameSession.ClassWeapons = string.Join("\n", _gameSession.CurrentUnit!.Class.UsableWeapons);

        ClearCurrentSelectedButton(currentSelectedTileButton);
        //se c'è un'unità nemica nel range di attacco allora cambio stato
        if (newPathAlgorithm.NearEnemy.Count > 0)
        {
            State.SetState(new ChooseAttack(State, newPathAlgorithm.NearEnemy, button));
            //unit can't move anymore
        }
        else
        {
            //unit can't move anymore
            _startinPosition = (0, 0);
            _currentPosition = (0, 0);

            _mapBuilder.UnitCantMoveNoMore(button);

            //if alla units moved change state to enemy turn
            if (MapBuilder.AllayButtonList.All(allay => !(allay.GetTile()).UnitOn!.CanMove))
                State._turnMapLogic.SetState(new EnemyTurn(State._turnMapLogic));

            //CHANGE STATE BACK TO 0
            State.SetState(new TileToBeSelected(State));
        }

    }


    //UTILITY METHODS
    public void ClearCurrentSelectedButton(Button? currentSelectedTileButton)
    {
        _mapCosmetics.SetButtonAsDeselected(currentSelectedTileButton);
        currentSelectedTileButton.Content = null;
        _mapBuilder.MovingUnit = null;
        _mapBuilder.CurrentSelectedTile = null;
    }

    private Button CloneButton(Button originalButton)
    {
        var newButton = new Button
        {
            Content = originalButton.Content,
            Tag = originalButton.Tag,
            Width = originalButton.Width,
            Height = originalButton.Height,
            Background = originalButton.Background,
            Foreground = originalButton.Foreground,
            // Copia altre proprietà rilevanti qui
        };

        // Copia gli event handler se necessario
        newButton.Click += _mapBuilder.MapLogic.CurrentTurnState.Single_Click;
        newButton.MouseEnter += Mouse_Over;

        return newButton;
    }
}
