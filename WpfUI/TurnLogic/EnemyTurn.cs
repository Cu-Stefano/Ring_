using Engine.FEMap;
using System;
using System.Windows;
using System.Windows.Controls;
using WpfUI.PathElements;
using WpfUI.TurnLogic.Actions;
using WpfUI.Utilities;

namespace WpfUI.TurnLogic;

public class EnemyTurn(MapLogic turnMapLogic) : TurnState(turnMapLogic)
{
    private PathAlgorithm PathAlgorithm { get; set; }

    public override async void OnEnter()
    {
        var enemyButtonListCopy = MapBuilder.EnemyButtonList.ToList();
        foreach (var enemy in enemyButtonListCopy)
        {
            await Task.Delay(1000);
            if (!enemy.GetTile().UnitOn!.CanMove) continue;

            InitializePathAlgorithm(enemy);
            await Task.Delay(200);

            var allayToAttack = PathAlgorithm.AttackList.Find(x => x.GetTile().UnitOn != null);
            if (allayToAttack == null)
            {
                HandleNoAllayToAttack(enemy);
                continue;
            }

            if (PathAlgorithm.NearEnemy.Contains(allayToAttack))
            {
                await AttackNearEnemy(enemy, allayToAttack);
                continue;
            }

            var tileToLand = FindTileToLandForAttack(enemy, allayToAttack);
            if (tileToLand == null) continue;
            _mapCosmetics.SetButtonAsSelected(tileToLand.button);

            await MoveAndAttack(enemy, allayToAttack, tileToLand);
            _mapCosmetics.SetButtonAsDeselected(tileToLand.button);
        }

        await Task.Delay(1600);

        _turnMapLogic.SetState(new AllayTurn(_turnMapLogic));
    }

    private void InitializePathAlgorithm(Button enemy)
    {
        PathAlgorithm = new PathAlgorithm(_mapCosmetics);
        PathAlgorithm.SetONode(enemy);
        PathAlgorithm.Execute();
        _mapCosmetics.SetButtonAsSelected(enemy);
    }

    private void HandleNoAllayToAttack(Button enemy)
    {
        _mapCosmetics.SetButtonAsDeselected(enemy);
        _mapBuilder.UnitCantMoveNoMore(enemy);
        PathAlgorithm.ResetAll();
    }

    private async Task AttackNearEnemy(Button enemy, Button allayToAttack)
    {
        PathAlgorithm.ResetAll();
        SetState(new ChooseAttack(this, [allayToAttack], enemy));
        CurrentActionState.Single_Click(allayToAttack, new RoutedEventArgs());

        _mapCosmetics.SetButtonAsDeselected(enemy);
        PathAlgorithm.ResetAll();
        await Task.Delay(3000);
    }

    private Node? FindTileToLandForAttack(Button enemy, Button allayToAttack)
    {
        var allayNode = PathAlgorithm.GetNOdeFromButton(allayToAttack);
        return FindTileToLand(allayNode, (enemy.GetTile().UnitOn.EquipedWeapon.Range));
    }

    private async Task MoveAndAttack(Button enemy, Button allayToAttack, Node tileToLand)
    {
        HighlightPath(tileToLand);
        await Task.Delay(2000);

        MoveUnit.Move_Unit(enemy, tileToLand.button);
        PathAlgorithm.ResetAll();

        SetState(new ChooseAttack(this, [allayToAttack], tileToLand.button));
        CurrentActionState.Single_Click(allayToAttack, new RoutedEventArgs());

        _mapCosmetics.SetButtonAsDeselected(enemy);
        PathAlgorithm.ResetAll();
        await Task.Delay(3000);
    }

    private void HighlightPath(Node tileToLand)
    {
        var currNode = tileToLand;
        while (currNode != PathAlgorithm.ONode)
        {
            currNode = currNode.Parent;
            _mapCosmetics.SetTrailSelector(currNode.button);
        }
    }

    public Node? FindTileToLand(Node attackedUnit, int attackRange)
    {
        var cost = int.MaxValue;
        Node? finalNode = null;
        FindTileToLandAux(attackedUnit, attackRange, ref cost, ref finalNode);
        return finalNode;
    }

    public void FindTileToLandAux(Node attackedUnit, int attackRange, ref int cost, ref Node? finalNode)
    {
        if (attackRange < 0) return;

        if (attackedUnit.button!.GetTile().Walkable && PathAlgorithm.Path.Contains(attackedUnit.button))
        {
            var currentCost = PathAlgorithm.Calculate_Distance(attackedUnit);
            if (currentCost < cost)
            {
                cost = currentCost;
                finalNode = attackedUnit;
            }
        }

        foreach (var neighbor in attackedUnit.Neighbours)
        {
            FindTileToLandAux(neighbor, attackRange - 1, ref cost, ref finalNode);
        }
    }

    public override async void OnExit()
    {
        await Task.Delay(400);
        ResetEnemyUnits();
        await Task.Delay(400);
    }

    private void ResetEnemyUnits()
    {
        foreach (var but in MapBuilder.EnemyButtonList)
        {
            var tile = but.GetTile();
            tile.UnitOn!.CanMove = true;
            _mapCosmetics.SetPolygon(but);
        }
    }

    public override void SetState(ActionState action)
    {
        if (CurrentActionState?.GetType() == action.GetType()) return;
        if (action.GetType() != typeof(Attack)) CurrentActionState?.OnExit();
        CurrentActionState = action;
        CurrentActionState.OnEnter();
    }

    public override void Doule_Click(object sender, RoutedEventArgs e) { }

    public override void Single_Click(object sender, RoutedEventArgs e) { }
}
