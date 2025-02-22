﻿using Engine.FEMap;
using Engine.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WpfUI.PathElements;
using WpfUI.PathElements;

namespace WpfUI.TurnLogic.Actions;

public class TileSelected : ActionState
{
    private Node ONode { get; set; }
    private Tile Tile { get; set; }
    private int UnitMovement { get; set; }
    private int? Range { get; set; }
    private List<List<Node>> Matrix { get; set; }
    private PriorityQueue<Node, int?> PQueue { get; set; }
    private List<Button> Path { get;}
    private List<Button> Attack { get; set; }
    private List<Button> _nearEnemy = [];

    public TileSelected(TurnState state, Button button) : base(state)
    {
        Matrix = NodeMatrix();
        ONode = GetButtonCoordinates(button)!;
        ONode.G = 0;
        ONode.Parent = null;
        Tile = (Tile)button.Tag;
        UnitMovement = Tile.UnitOn!.Class.Movement;
        Range = Tile.UnitOn.EquipedWeapon?.Range ?? 0;
        _mapCosmetics.SetButtonAsSelected(button);

        PQueue = new PriorityQueue<Node, int?>();
        AddNodeToQueue(ONode);
        Path = new List<Button>();
        Attack = new List<Button>();
    }

    private void PathAlgorithm(PriorityQueue<Node, int?> queue, int movement, int? range, List<Button> attackList, List<Button> path)
    {
        while (queue.Count > 0)
        {
            var curr = GetNextNode()!;
            var button = curr.button;

            if ((curr == ONode || curr.passable) && curr.G < (movement + range) && !path.Contains(button))
            {
                foreach (var currNeighbour in curr.Neighbours.Where(n => n.G == null))
                {
                    //non lo considero se c'è un'unità sopra
                    var tile = (Tile)(currNeighbour.button.Tag);

                    if (tile.UnitOn?.Type == UnitType.Enemy && curr.G < Range && !_nearEnemy.Contains(currNeighbour.button))
                    {
                        _nearEnemy.Add(currNeighbour.button);
                        continue;
                    }
                    if (tile.UnitOn != null)
                    {
                        continue;
                    }

                    currNeighbour.G = curr.G + 1;
                    currNeighbour.Parent = curr;
                    queue.Enqueue(currNeighbour, currNeighbour.G);
                }
            }

            if (curr.G > movement)
            {
                _mapCosmetics.SetGetAttackBrush(button);
                attackList.Add(button);
            }
            else
            {
                path.Add(button);
                _mapCosmetics.SetGetPathBrush(button);
            }
        }
    }


    public override void OnEnter()
    {
        PathAlgorithm(PQueue, UnitMovement, Range, Attack, Path);
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
            _mapCosmetics.SetButtonAsDeselected(button);
        }
        foreach (var button in Attack)
        {
            var tile = (Tile)button.Tag;
            button.Background = MapCosmetics.GetTileBrush(tile);
        }
        Attack.Clear();
        Path.Clear();
    }

    //CALCULATE_TRAIL
    public override void Mouse_Over(object sender, RoutedEventArgs e)
    {
        if (sender is Button bx && !Path.Contains(bx))
            return;

        foreach (var button in Path)
            _mapCosmetics.SetButtonAsDeselected(button);
        
        var currNode = GetButtonCoordinates((Button)sender);
        while (currNode != ONode)
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
        var til = (Tile)butt.Tag;
        if (til.UnitOn is { Type: UnitType.Enemy } && _nearEnemy.Contains(butt))
        {
            var curselecButt = _mapBuilder.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile)!;
            var aux = CloneButton(curselecButt);

            /*
            _mapCosmetics.SetButtonAsDeselected(currentSelectedTileButton);
            currentSelectedTileButton.Content = null;
            _mapBuilder.MovingUnit = null;
            _mapBuilder.CurrentSelectedTile = null;
            */
            curselecButt.Content = null;

            curselecButt.Content = aux.Content;
            curselecButt.Tag = aux.Tag;

            State.SetState(new ChooseAttack(State, _nearEnemy, butt));
            return;
        }
        if (sender is not Button { Tag: Tile { UnitOn: null, Walkable: true } tile } button || !Path.Contains(button))
            return;

        //ci deve essere un'unità selezionata
        if (_mapBuilder.CurrentSelectedTile is not { UnitOn: not null })
            return;

        //unit can't move anymore
        //_mapBuilder.CurrentSelectedTile.UnitOn.CanMove = false;
        //_mapBuilder.UnitCantMoveNoMore(button);

        var currentSelectedTileButton = _mapBuilder.GetButtonBasedOnTile(_mapBuilder.CurrentSelectedTile)!;

        if (_nearEnemy.Contains(button))
        {
            var aux = currentSelectedTileButton;
            ClearCurrentSelectedButton(currentSelectedTileButton);
            button = aux;
            State.SetState(new ChooseAttack(State, _nearEnemy, button));
        }

        //l'effetivo spostamento dell'unità
        tile.UnitOn = _mapBuilder.MovingUnit;
        button.Content = MapCosmetics.GetTriangle(tile.UnitOn);

        _gameSession.CurrentTile = tile;
        _gameSession.CurrentUnit = tile.UnitOn;
        _gameSession.ClassWeapons = string.Join("\n", _gameSession.CurrentUnit!.Class.UsableWeapons);

        //aggiorno AllayButtonList perchè i bottoni sono stati scambiati
        _mapBuilder.AllayButtonList.Remove(currentSelectedTileButton);
        _mapBuilder.AllayButtonList.Add(button);


        //controllo se c'è un'unita nemica è nel suo range di attacco
        var enemyNear = new List<Button>();
        {
            ResetAll();
            ONode = GetButtonCoordinates(button)!;
            ONode.G = 0;
            ONode.Parent = null;
            AddNodeToQueue(ONode);

            var path = new List<Button>();

            while (PQueue.Count > 0)
            {
                var curr = GetNextNode()!;
                var b = curr.button;

                if ((curr == ONode || curr.passable) && curr.G < Range && !path.Contains(button))
                {
                    foreach (var currNeighbour in curr.Neighbours.Where(n => n.G == null))
                    {
                        currNeighbour.G = curr.G + 1;
                        currNeighbour.Parent = curr;
                        PQueue.Enqueue(currNeighbour, currNeighbour.G);
                    }
                }

                if (curr.G > 0 && ((Tile)curr.button.Tag).UnitOn?.Type == UnitType.Enemy)
                    enemyNear.Add(b);
                else
                    Path.Add(button);
            }
        }
        ClearCurrentSelectedButton(currentSelectedTileButton);
        //se c'è un'unità nemica nel range di attacco allora cambio stato
        if (enemyNear.Count > 0)
        {
            State.SetState(new ChooseAttack(State, enemyNear, button));
            //unit can't move anymore
        }
        else
        {
            //unit can't move anymore
            tile.UnitOn.CanMove = false;
            _mapBuilder.UnitCantMoveNoMore(button);

            //if alla units moved change state to enemy turn
            if (_mapBuilder.AllayButtonList.All(allay => !((Tile)allay.Tag).UnitOn!.CanMove))
                State._turnMapLogic.SetState(new EnemyTurn(State._turnMapLogic));

            //CHANGE STATE BACK TO 0
            State.SetState(new TileToBeSelected(State));
        }
    }

    //UTILITY METHODS
    public void ClearCurrentSelectedButton(Button currentSelectedTileButton)
    {
        _mapCosmetics.SetButtonAsDeselected(currentSelectedTileButton);
        currentSelectedTileButton.Content = null;
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
                var passable = tile is { Walkable: true};

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

    public void AddNodeToQueue(Node node)
    {
        PQueue.Enqueue(node, node.G);
    }

    public Node? GetNextNode()
    {
        return PQueue.Count == 0 ? null : PQueue.Dequeue();
    }

    public void ResetAll()
    {
        // Reset all nodes in the Matrix
        foreach (var row in Matrix)
        {
            foreach (var node in row)
            {
                node.Parent = null;
                node.G = null;
            }
        }
        // Clear the PriorityQueue
        PQueue.Clear();

        // Reset ONode
        ONode = null;

        // Reset Tile, UnitMovement, and Range
        Tile = null;
        UnitMovement = 0;
    }

}
