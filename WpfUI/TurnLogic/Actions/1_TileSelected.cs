using Engine.FEMap;
using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfUI.PathLogic;

namespace WpfUI.TurnLogic.Actions;

public class TileSelected : ActionState
{
    private Node ONode { get; set; }
    private Tile Tile { get; }
    private int UnitMovement { get; }
    private int? Range { get; }
    private List<List<Node>> Matrix { get; set; }
    private PriorityQueue<Node, int?> PQueue { get; }
    private List<Button> Path { get;}
    private List<Button> Attack { get; }

    public TileSelected(TurnState state, Button button) : base(state)
    {
        Matrix = NodeMatrix();
        ONode = GetButtonCoordinates(button)!;
        ONode.G = 0;
        ONode.Parent = null;
        Tile = (Tile)button.Tag;
        UnitMovement = Tile.UnitOn!.Class.Movement;
        Range = Tile.UnitOn.EquipedWeapon?.Range ?? 0;
        _mapCosmetics.SetTileAsSelected(button);

        PQueue = new PriorityQueue<Node, int?>();
        AddNodeToQueue(ONode);
        Path = new List<Button>();
        Attack = new List<Button>();
    }

    public override void OnEnter()
    {
        while (PQueue.Count > 0)
        {
            var curr = GetNextNode()!;
            var button = curr.button;

            if ((curr == ONode || curr.passable) && curr.G < (UnitMovement + Range) && !Path.Contains(button))
            {
                foreach (var currNeighbour in curr.Neighbours.Where(n => n.G == null))
                {
                    currNeighbour.G = curr.G + 1;
                    currNeighbour.Parent = curr;
                    PQueue.Enqueue(currNeighbour, currNeighbour.G);
                }
            }

            if (curr.G > UnitMovement)
            {
                _mapCosmetics.SetGetAttackBrush(button);
                Attack.Add(button);
            }
            else
            {
                Path.Add(button);
                _mapCosmetics.SetGetPathBrush(button);
            }
        }
    }

    public override void OnExit()
    {
        Matrix = null;
        PQueue.Clear();
        ONode = null;

        foreach (var button in Path)
        {
            var tile = (Tile)button.Tag;
            button.Background = MapCosmetics.GetTileBrush(tile);
            _mapCosmetics.SetTileAsDeselected(button);
        }
        foreach (var button in Attack)
        {
            var tile = (Tile)button.Tag;
            button.Background = MapCosmetics.GetTileBrush(tile);
        }
        Attack.Clear();
        Path.Clear();
    }

    public override void CalculateTrail(object sender, RoutedEventArgs e)
    {
        if (sender is Button bx && !Path.Contains(bx))
            return;

        foreach (var button in Path)
            _mapCosmetics.SetTileAsDeselected(button);
        
        var currNode = GetButtonCoordinates((Button)sender);
        while (currNode != ONode)
        {
            _mapCosmetics.SetTrailSelector(currNode.button);
            currNode = currNode.Parent;
        }
    }

    public override void UnitSelected(object sender, RoutedEventArgs e)
    {
    }

    public override void Move_Unit(object sender, RoutedEventArgs e)
    {
        //il sender dell'evento deve essere un bottone nel range di movimento dell'unità
        if (sender is not Button { Tag: Tile { UnitOn: null, Walkable: true } tile } button || !Path.Contains(button))
            return;

        //ci deve essere un'unità selezionata e diversa dal sender
        if (_mapBuilder.CurrentSelectedTile is not { UnitOn: not null }) 
            return;

        //unit can't move anymore
        _mapBuilder.CurrentSelectedTile.UnitOn.CanMove = false;
        _mapBuilder.UnitCantMoveNoMore(button);

        var currentSelectedTileButton = _mapBuilder.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile)!;

        tile.UnitOn = _mapBuilder.MovingUnit;

        _gameSession.CurrentTile = tile;
        _gameSession.CurrentUnit = tile.UnitOn;
        _gameSession.ClassWeapons = string.Join("\n", _gameSession.CurrentUnit!.Class.UsableWeapons);

        //aggiorno AllayButtonList perchè i bottoni sono stati scambiati
        _mapBuilder.AllayButtonList.Remove(currentSelectedTileButton);
        _mapBuilder.AllayButtonList.Add(button);

        ClearCurrentSelectedButton(currentSelectedTileButton);

        //if alla units moved change state to enemy turn
        if (_mapBuilder.AllayButtonList.All(allay => !((Tile)allay.Tag).UnitOn!.CanMove))
            State._turnMapLogic.SetState(new EnemyTurn(State._turnMapLogic));

        //CHANGE STATE BACK TO 0
        State.SetState(new TileToBeSelected(State));
    }

    //UTILITY METHODS
    public void ClearCurrentSelectedButton(Button currentSelectedTileButton)
    {
        _mapCosmetics.SetTileAsDeselected(currentSelectedTileButton);
        currentSelectedTileButton.Content = null;
        _mapBuilder.MovingUnit = null;
        _mapBuilder.CurrentSelectedTile = null;
    }

    public List<List<Node>> NodeMatrix()
    {
        var baseMap = _mapBuilder.ActualMap;
        var result = new List<List<Node>>();

        //inizialize the matrix
        for (int i = 0; i < baseMap.Count; i++)
        {
            var row = new List<Node>();
            for (int j = 0; j < baseMap[0].Count; j++)
            {
                var tile = (Tile)baseMap[i][j].Tag;
                var cost = tile.TileName == "Bush" ? 2 : 1;
                var passable = tile is { Walkable: true, UnitOn: null };

                var node = new Node
                {
                    button = baseMap[i][j],
                    cost = cost,
                    passable = passable
                };
                row.Add(node);
            }
            result.Add(row);
        }

        //add neighbours
        for (int i = 0; i < baseMap.Count; i++)
        {
            for (int j = 0; j < baseMap[0].Count; j++)
            {
                var node = result[i][j];
                if (i > 0 && result[i - 1][j].passable) 
                    node.Neighbours.Add(result[i - 1][j]);

                if (j > 0 && result[i][j - 1].passable) 
                    node.Neighbours.Add(result[i][j - 1]);

                if (i < baseMap.Count - 1 && result[i + 1][j].passable) 
                    node.Neighbours.Add(result[i + 1][j]);

                if (j < baseMap[i].Count - 1 && result[i][j + 1].passable) 
                    node.Neighbours.Add(result[i][j + 1]);
            }
        }
        return result;
    }

    public Node? GetButtonCoordinates(Button button)
    {
        for (int i = 0; i < _mapBuilder.ActualMap.Count; i++)
        {
            int j = _mapBuilder.ActualMap[i].IndexOf(button);
            if (j != -1)
            {
                return Matrix[i][j];
            }
        }
        return null;
    }

    public void AddNodeToQueue(Node node)
    {
        PQueue.Enqueue(node, node.G);
    }

    public Node? GetNextNode()
    {
        return PQueue.Count == 0 ? null : PQueue.Dequeue();
    }
}
