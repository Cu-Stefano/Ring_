using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Engine.FEMap;
using Engine.Models;

namespace WpfUI.TurnLogic.Actions;

public class ChooseAttack(TurnState state, List<Button
> enemyNear, Button? attackingAllay) : ActionState(state)
{
    private PreviewAttack _previewAttack { get; set; }
    public override void OnEnter()
    {
        _previewAttack = _gameSession._previewAttack;
        if (state.GetType() == typeof(EnemyTurn))
        {
            _previewAttack.chooseAttack = this;
            _previewAttack.GameSession = _gameSession;
            return;
        }

        foreach (var button in enemyNear)
        {
            _mapCosmetics.SetButtonAsSelected(button);
        }
        _previewAttack.chooseAttack = this;
        _previewAttack.GameSession = _gameSession;
    }

    public override void OnExit()
    {
        foreach (var e in enemyNear)
            _mapCosmetics.SetButtonAsDeselected(e);
    }

    public override void Mouse_Over(object sender, RoutedEventArgs e)
    { 
    }
    public override void Double_Click(object sender, RoutedEventArgs e)
    {
        if (state.GetType() == typeof(EnemyTurn)) return;
        if (sender is not Button { Tag: Tile { UnitOn: { Type: UnitType.Allay, CanMove: true } } tile } button) return;

        _mapBuilder.UnitCantMoveNoMore(attackingAllay);
        _mapBuilder.CurrentSelectedTile = tile;
        _mapBuilder.MovingUnit = tile.UnitOn;

        _startinPosition = _mapBuilder.GetButtonPosition(button);
        _currentPosition = _startinPosition;
        //CHANGE STATE TO 1
        State.SetState(new TileSelected(State, button));

    }
    public override void Back_Action(object sender, MouseButtonEventArgs e)
    {
        if (state.GetType() == typeof(EnemyTurn)) return;
        //richiamo il vecchio metodo Back_Action e aggiungo che devo nascondere la griglia di attacco
        base.Back_Action(sender, e);
        _previewAttack.PreviewAttackGrid.Visibility = Visibility.Hidden;
        _previewAttack.EnemyButton = null;
        _previewAttack.AllayButton = null;
        _previewAttack.EnemyNear?.Clear();
    }
    public override async void Single_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        if (sender is Button { Tag: Tile } && enemyNear.Contains(button))
        {
            _previewAttack.EnemyButton = button;
            _previewAttack.EnemyNear = enemyNear;
            _previewAttack.attackingAllay = attackingAllay;
            _previewAttack.AttackButton.Visibility = Visibility.Visible;

            _previewAttack.Start();
            if (state.GetType() == typeof(EnemyTurn))
            {
                await Task.Delay(1000);
                _previewAttack.PreviewAttackGrid.Visibility = Visibility.Hidden;
                _previewAttack.AttackButton.Visibility = Visibility.Collapsed;
                _previewAttack.StartAttack(null, null);
            }
        }
    }

}