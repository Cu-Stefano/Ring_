using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Engine.FEMap;

namespace WpfUI.TurnLogic.Actions;

public class ChooseAttack(TurnState state, List<Button?> enemyNear, Button? attackingAllay) : ActionState(state)
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
            _mapCosmetics.SetButtonAsDeselected(e);
    }

    public override void Mouse_Over(object sender, RoutedEventArgs e)
    { 
    }
    public override void Double_Click(object sender, RoutedEventArgs e)
    {
    }
    public override void Back_Action(object sender, MouseButtonEventArgs e)
    {
        //richiamo il vecchio metodo Back_Action e aggiungo che devo nascondere la griglia di attacco
        base.Back_Action(sender, e);
        _gameSession.PreviewAttack.PreviewAttackGrid.Visibility = Visibility.Hidden;
        _gameSession.PreviewAttack.EnemyButton = null;
        _gameSession.PreviewAttack.AllayButton = null;
        _gameSession.PreviewAttack.EnemyNear?.Clear();
    }
    public override void Single_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        if (sender is Button { Tag: Tile } && enemyNear.Contains(button))
        {
            _gameSession.PreviewAttack.EnemyButton = button;
            _gameSession.PreviewAttack.EnemyNear = enemyNear;
            _gameSession.PreviewAttack.attackingAllay = attackingAllay;
            _gameSession.PreviewAttack.Start();
        }
    }

}