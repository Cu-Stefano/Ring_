using Engine.FEMap;
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
    private PriorityQueue<Node, int?> PQueue { get; set; }
    private List<Button> Path { get; set; }
    private List<Button> Attack { get; set; }

    public TileSelected(TurnState state, Button button) : base(state)
    {
        Matrix = NodeMatrix();
        ONode = GetButtonCoordinates(button)!;
        ONode.G = 0;
        ONode.Parent = null;
        Tile = (Tile)button.Tag;
        UnitMovement = Tile.UnitOn!.Class.Movement;
        Range = Tile.UnitOn.EquipedWeapon?.Range ?? 0;
        _mapCosmetics.TileSelected(button);

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
                _mapCosmetics.GetAttackBrush(button);
                Attack.Add(button);
            }
            else
            {
                Path.Add(button);
                _mapCosmetics.GetPathBrush(button);
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
            button.Background = _mapCosmetics.GetTileBrush(tile);
            _mapCosmetics.TileDeSelected(button);
        }
        foreach (var button in Attack)
        {
            var tile = (Tile)button.Tag;
            button.Background = _mapCosmetics.GetTileBrush(tile);
        }
        Attack.Clear();
        Path.Clear();
    }

    public override void CalculateTrail(object sender, RoutedEventArgs e)
    {
        if (sender is Button bx && !Path.Contains(bx))
            return;

        foreach (var button in Path)
        {
            _mapCosmetics.TileDeSelected(button);
        }

        var currNode = GetButtonCoordinates((Button)sender);
        while (currNode != ONode)
        {
            _mapCosmetics.TrailSelector(currNode.button);
            currNode = currNode.Parent;
        }
    }

    public override void UnitSelected(object sender, RoutedEventArgs e)
    {
    }

    public override void Move_Unit(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: Tile { UnitOn: null, Walkable: true } tile } button && Path.Contains(button))
        {
            if (_mapBuilder.CurrentSelectedTile is { UnitOn: not null } && _mapBuilder.CurrentSelectedTile != tile)
            {
                tile.UnitOn = _mapBuilder.MovingUnit;

                var currentSelectedTileButton = _mapCosmetics.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile)!;

                button.Content = currentSelectedTileButton.Content;
                _gameSession.CurrentTile = tile;
                _gameSession.CurrentUnit = tile.UnitOn;

                ClearCurrentSelectedButton(currentSelectedTileButton);

                _gameSession.ClassWeapons = string.Join("\n", _gameSession.CurrentUnit!.Class.UsableWeapons);

                State.SetState(new TileToBeSelected(State));
            }
        }
    }

    //UTILITY METHODS
    public void ClearCurrentSelectedButton(Button currentSelectedTileButton)
    {
        _mapCosmetics.TileDeSelected(currentSelectedTileButton);
        currentSelectedTileButton.Content = null;
        _mapBuilder.MovingUnit = null;
        _mapBuilder.CurrentSelectedTile = null;
    }

    public List<List<Node>> NodeMatrix()
    {
        var baseMap = _mapBuilder.ActualMap;
        var result = new List<List<Node>>();

        for (int i = 0; i < 15; i++)
        {
            var row = new List<Node>();
            for (int j = 0; j < 20; j++)
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

        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 20; j++)
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
