using System.Text.Json.Serialization;

namespace Engine.Models;

public class GameItem
{
    public int ItemTypeId { get; set;}
    public string Name { get; set;}
    public int Price{ get; set;}

    [JsonConstructor]
    public GameItem(){}
    public GameItem(int itemTypeId, string name, int price)
    {
        ItemTypeId = itemTypeId;
        Name = name;
        Price = price;
    }

    public GameItem Clone()
    {
        return new GameItem(ItemTypeId, Name, Price);
    }
}