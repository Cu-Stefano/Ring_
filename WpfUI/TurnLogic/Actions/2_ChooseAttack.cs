using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Engine.FEMap;

namespace WpfUI.TurnLogic.Actions;

public class ChooseAttack(TurnState state, List<Button> enemyNear, Button buttonToThanDeselect) : ActionState(state)
{
    public override void OnEnter()
    {
        foreach (var button in enemyNear)
        {
            _mapCosmetics.SetButtonAsSelected(button);
        }
        _gameSession.PreviewAttack.chooseAttack = this;
        _gameSession.PreviewAttack.GameSession = _gameSession;
    }

    public override void OnExit()
    {
        foreach (var e in enemyNear)
        {
            _mapCosmetics.SetButtonAsDeselected(e);
        }
        _gameSession.PreviewAttack.PreviewAttackGrid.Visibility = Visibility.Hidden;
    }

    public override void Mouse_Over(object sender, RoutedEventArgs e)
    { 
    }

    public override void Double_Click(object sender, RoutedEventArgs e)
    {
        
    }

    public override void Single_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        if (sender is Button { Tag: Tile } && enemyNear.Contains(button))
        {
            _gameSession.PreviewAttack.Enemy = button;
            _gameSession.PreviewAttack.EnemyNear = enemyNear;
            _gameSession.PreviewAttack.ButtonToThanDeselect = buttonToThanDeselect;
            _gameSession.PreviewAttack.Start();
        }
    }

}