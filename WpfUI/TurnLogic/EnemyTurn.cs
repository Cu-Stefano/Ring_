using Engine.FEMap;
using Engine.Models;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfUI.PathElements;
using WpfUI.TurnLogic.Actions;
using WpfUI.Utilities;

namespace WpfUI.TurnLogic;

public class EnemyTurn(MapLogic turnMapLogic) : TurnState(turnMapLogic)
{
    private PathAlgorithm _pathAlgorithm { get; set; }
    public override async void OnEnter()
    {
        var enemyButtonListCopy = MapBuilder.EnemyButtonList.ToList();
        foreach (var enemy in enemyButtonListCopy)
        {
            await Task.Delay(1000);
            if (((Tile)enemy.Tag).UnitOn.CanMove == false)
                continue;
            _pathAlgorithm = new PathAlgorithm(_mapCosmetics);
            _pathAlgorithm.SetONode(enemy!);
            _pathAlgorithm.Execute(); // calcola il path
            _pathAlgorithm.ResetAll();
            _mapCosmetics.SetButtonAsSelected(enemy);
            await Task.Delay(200);
            // enemy ai logic
            var allayToAttack = _pathAlgorithm.AttackList.Find(x => ((Tile)x.Tag).UnitOn != null);
            if (allayToAttack == null)
            {
                _mapCosmetics.SetButtonAsDeselected(enemy);
                _mapBuilder.UnitCantMoveNoMore(enemy);
                _pathAlgorithm.ResetAll();
                await Task.Delay(100);
                continue;
            }

            if (_pathAlgorithm.NearEnemy.Contains(allayToAttack)) // è vicino al nemico quindi non serve che si sposti, attaccalo e bah
            {
                SetState(new ChooseAttack(this, [allayToAttack], enemy));
                CurrentActionState.Single_Click(allayToAttack, new RoutedEventArgs());

                _mapCosmetics.SetButtonAsDeselected(enemy);
                _pathAlgorithm.ResetAll();
                await Task.Delay(3000);
                continue;
            }

            var allayNode = _pathAlgorithm.GetNOdeFromButton(allayToAttack);
            var tileToLand = FindTileToLand(allayNode, ((Tile)enemy.Tag).UnitOn.EquipedWeapon.Range);
            if (tileToLand == null)
                continue;

            _mapCosmetics.SetButtonAsSelected(tileToLand.button);

            // stampo il percorso che il nemico farà per raggiungere l'unità da attaccare
            var currNode = tileToLand;
            while (currNode != _pathAlgorithm.ONode)
            {
                currNode = currNode.Parent;
                _mapCosmetics.SetTrailSelector(currNode.button);
            }
            await Task.Delay(2000);
            MoveUnit.Move_Unit(enemy, tileToLand.button);

            SetState(new ChooseAttack(this, [allayToAttack], tileToLand.button));
            //il nemico attacca l'unità
            CurrentActionState.Single_Click(allayToAttack, new RoutedEventArgs());

            _mapCosmetics.SetButtonAsDeselected(enemy);
            _pathAlgorithm.ResetAll();
            await Task.Delay(3000);
        }

        await Task.Delay(1300);
        _turnMapLogic.SetState(new AllayTurn(_turnMapLogic));
    }

    public Node? FindTileToLand(Node attackedUnit, int attackRange)
    {
        if (attackRange == -1)
            return null;

        // Check if the current node is walkable and within the path
        if (((Tile)attackedUnit.button.Tag).Walkable && _pathAlgorithm.Path.Contains(attackedUnit.button))
            return attackedUnit;

        // Recursively check the neighbors within the specified attack range
        foreach (var neighbor in attackedUnit.Neighbours)
        {
            var result = FindTileToLand(neighbor, attackRange - 1);
            if (result != null)
                return result;
        }

        return null;
    }


    public override async void OnExit()
    {
        await Task.Delay(400);

        // Ripristino la possibilità di movimento delle unità nemiche
        foreach (var but in MapBuilder.EnemyButtonList)
        {
            var tile = (Tile)but.Tag;
            tile.UnitOn.CanMove = true;
            _mapCosmetics.SetPolygon(but);
        }
        await Task.Delay(400);
    }
    public override void SetState(ActionState action)
    {
        if (CurrentActionState?.GetType() == action.GetType())
            return;
        if (action.GetType() != typeof(Attack))
            CurrentActionState?.OnExit();
        CurrentActionState = action;
        CurrentActionState.OnEnter();
    }

    public override void Doule_Click(object sender, RoutedEventArgs e)
    {
    }

    public override void Single_Click(object sender, RoutedEventArgs e)
    {
    }
}