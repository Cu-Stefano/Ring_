using System.Windows.Controls;
using Engine.FEMap;
using Engine.Models;
using WpfUI;
using WpfUI.PathElements;

public static class PathAlgorithm
{
    public static void Execute(Node ONode, Unit unit, List<Button?>? attackList, List<Button?> path, List<Button?> _nearEnemy, MapCosmetics _mapCosmetics)
    {
        int movement = unit.Class.Movement;
        int? range = unit.EquipedWeapon?.Range ?? 0;

        var queue = new PriorityQueue<Node, int?>();
        queue.Enqueue(ONode, ONode.G);

        while (queue.Count > 0)
        {
            var curr = GetNextNode(queue);
            if (curr == null) continue;

            var button = curr.button;

            if ((curr == ONode || curr.passable) && curr.G < (movement + range) && !path.Contains(button))
            {
                foreach (var currNeighbour in curr.Neighbours.Where(n => n.G == null))
                {
                    var tile = (Tile)(currNeighbour.button.Tag);

                    if (tile.UnitOn?.Type == UnitType.Enemy && curr.G < range && !_nearEnemy.Contains(currNeighbour.button))
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

    private static Node? GetNextNode(PriorityQueue<Node, int?> queue)
    {
        return queue.Count > 0 ? queue.Dequeue() : null;
    }
}