using System.Text.Json.Serialization;

namespace Engine.Models;

public class Weapon : GameItem
{
    public int MinDamage { get; set; }
    public int MaxDamage { get; set; }
    public int Durability { get; set; }
    public int Critical { get; set; }
    public string WeaponType { get; set; }

    [JsonConstructor]
    public Weapon(){}
    public Weapon(int itemTypeId, string name, int price, int mindamage, int maxdamage, int durability, int critical, string weaponType)
        : base(itemTypeId, name, price)
    {
        MinDamage = mindamage;
        MaxDamage = maxdamage;
        Durability = durability;
        Critical = critical;
        WeaponType = weaponType;
    }

    public new GameItem Clone()
    {
        return new Weapon(ItemTypeId, Name, Price, MinDamage, MaxDamage, Durability, Critical, WeaponType);
    }
}