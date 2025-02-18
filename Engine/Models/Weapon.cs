using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Engine.Models;

public enum WeaponType
{
    Sword,
    Axe,
    Lance,
    Tome,
    Bow,
    Staff
}


public class Weapon : GameItem
{
    public int Range { get; set; }
    public int MaxDamage { get; set; }
    public int Durability { get; set; }
    public int Critical { get; set; }
    public WeaponType WeaponType { get; set; } 
    public bool IsEquipped { get; set; }

    [JsonConstructor]
    public Weapon() { }

    public Weapon(int itemTypeId, string name, int price, int range, int maxdamage, int durability, int critical, WeaponType weaponType)
        : base(itemTypeId, name, price)
    {
        Range = range;
        MaxDamage = maxdamage;
        Durability = durability;
        Critical = critical;
        WeaponType = weaponType;
    }

    public new GameItem Clone()
    { 
        return new Weapon(ItemTypeId, Name, Price, Range, MaxDamage, Durability, Critical, WeaponType);
    }
}
