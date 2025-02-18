using System.Windows.Controls;

namespace WpfUI.PathLogic;

public class Node
{ 
    public int? G { get; set; }
    public int cost { get; set; }
    public bool passable { get; set; }
    public Button button { get; set; }
    public Node? Parent { get; set; }
    public List<Node> Neighbours { get; set; }

    public Node()
    {
        Neighbours = new List<Node>(4);
    }
}