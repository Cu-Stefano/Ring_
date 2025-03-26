using System.Windows.Controls;
using Engine.FEMap;
using Engine.Models;
using WpfUI;
using WpfUI.PathElements;
using WpfUI.Utilities;

public class PathAlgorithm
{
    internal Node ONode { get; set; }
    internal Unit Unit { get; set; }
    internal List<List<Node>> Matrix { get; set; }
    private PriorityQueue<Node, int?> PQueue { get; set; }
    internal List<Button?> Path { get; }
    private List<Button?> Attack { get; set; }//i bordi d'attacco del path
    public List<Button> AttackList { get; set; }//tutti i nemici dentro al path, ma fuori dal range di attaco
    internal List<Button?> NearEnemy { get; set; }//nemici nel range di attacco
    private MapCosmetics MapCosmetics { get; set; } //cosmetics

    //constructor
    public PathAlgorithm(Button oNode, MapCosmetics _mapCosmetics)
    {
        Matrix = NodeMatrix();
        PQueue = new PriorityQueue<Node, int?>();
        Path = [];
        Attack = [];
        AttackList = [];
        NearEnemy = [];
        this.MapCosmetics = _mapCosmetics;
        Unit = oNode.GetTile().UnitOn!;

        ONode = GetNOdeFromButton(oNode)!;
        ONode.G = 0;
        ONode.Parent = null;
        AddNodeToQueue(ONode);
    }

    public PathAlgorithm(MapCosmetics _mapCosmetics)
    {
        Matrix = NodeMatrix();
        PQueue = new PriorityQueue<Node, int?>();
        Path = [];
        Attack = [];
        AttackList = [];
        NearEnemy = [];
        this.MapCosmetics = _mapCosmetics;
    }

    public void SetONode(Button button)
    {
        Unit = button.GetTile().UnitOn!;

        ONode = GetNOdeFromButton(button)!;
        ONode.G = 0;
        ONode.Parent = null;
        AddNodeToQueue(ONode);
    }

    public void Execute(bool? near = null)//se near ha un valore vuol dire che vogliono sapere se ci sono nemici nel range di attacco
    {
        var enemyType = Unit.Type == UnitType.Allay ? UnitType.Enemy: UnitType.Allay;

        var movement = Unit.Class.Movement;
        var range = Unit.EquipedWeapon?.Range ?? 0;
        var totalRange = near.HasValue ? range : movement + range;

        while (PQueue.Count > 0)
        {
            var curr = GetNextNode(PQueue);
            if (curr == null) continue;

            var button = curr.button;

            if ((curr == ONode || curr.passable) && curr.G < totalRange && !Path.Contains(button))
            {
                foreach (var currNeighbour in curr.Neighbours.Where(n => n.G == null))
                {
                    var tile = currNeighbour.button.GetTile();

                    if (tile.UnitOn?.Type == enemyType && curr.G < range && !NearEnemy.Contains(currNeighbour.button))
                    {
                        NearEnemy.Add(currNeighbour.button);
                        continue;
                    }
					if (tile.UnitOn != null)
					{
						//se c'è un nemico dentro al path allora segnalo così si nota il colore diverso 
						if (tile.UnitOn.Type == enemyType && Unit.EquipedWeapon != null && !AttackList.Contains(currNeighbour.button))
						{
							if (Unit.Type == UnitType.Enemy)
								MapCosmetics.SetGetEnemyAttackBrush(currNeighbour.button);
							else MapCosmetics.SetGetAttackBrush(currNeighbour.button);
							AttackList.Add(currNeighbour.button);
						}
						continue;
					}

					currNeighbour.G = curr.G + 1;
                    currNeighbour.Parent = curr;
                    PQueue.Enqueue(currNeighbour, currNeighbour.G);
                }
            }

            if (curr.G > movement)
            {
                if (Unit.Type == UnitType.Enemy)
                    MapCosmetics.SetGetEnemyAttackBrush(button);
                else MapCosmetics.SetGetAttackBrush(button);

                AttackList.Add(button);
            }
            else
            {
                if (Unit.Type == UnitType.Enemy) 
                    MapCosmetics.SetGetEnemyPathBrush(button);
                else MapCosmetics.SetGetPathBrush(button);

                Path.Add(button);
            }
        }

        foreach (var butt in NearEnemy)
        {
            if (!AttackList.Contains(butt))
                AttackList.Add(butt);
        }
    }


    //UTILITY METHODS
    private static Node? GetNextNode(PriorityQueue<Node, int?> queue)
    {
        return queue.Count > 0 ? queue.Dequeue() : null;
    }

    public static List<List<Node>> NodeMatrix()
    {
        var baseMap = MapBuilder.ActualMap;
        var result = new List<List<Node>>();

        //inizialize the matrix
        for (int i = 0; i < baseMap.Count; i++)
        {
            var row = new List<Node>();
            for (int j = 0; j < baseMap[0].Count; j++)
            {
                var tile = baseMap[i][j].GetTile();
                var cost = tile.TileName == "Bush" ? 2 : 1;
                var passable = tile is { Walkable: true };

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

    public void ResetAll()
    {
        foreach (var button in Path)
        {
            var tile = button.GetTile();
            button.Background = MapCosmetics.GetTileBrush(tile);
            MapCosmetics.SetButtonAsDeselected(button);
        }
        foreach (var button in Attack)
        {
            var tile = button.GetTile();
            button.Background = MapCosmetics.GetTileBrush(tile);
        }
        foreach (var button in AttackList)
        {
            var tile = button.GetTile();
            button.Background = MapCosmetics.GetTileBrush(tile);
        }
    }
    public int Calculate_Distance(Node nodeToReach)
    {
        var c = 0;
        var currNode = nodeToReach;
        while (currNode != ONode)
        {
            currNode = currNode.Parent;
            c++;
        }
        return c;
    }
    public void AddNodeToQueue(Node node)
    {
        PQueue.Enqueue(node, node.G);
    }

    public Node? GetNOdeFromButton(Button? button)
    {
        for (int i = 0; i < MapBuilder.ActualMap.Count; i++)
        {
            int j = MapBuilder.ActualMap[i].IndexOf(button);
            if (j != -1)
            {
                return Matrix[i][j];
            }
        }
        return null;
    }

}