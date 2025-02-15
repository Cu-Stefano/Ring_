using Engine.Models;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Factories
{
    public static class WeaponFactory
    {
        public static readonly List<Weapon> Weapons;

        static WeaponFactory()
        {
            Weapons = ItemFactory.StandardGameItems
                .OfType<Weapon>() // Filtra e cast automatico
                .ToList();
        }

        public static Weapon? CreateWeapon(string weaponName)
        {
            Weapon weapon = Weapons.FirstOrDefault(w => w.Name.Equals(weaponName, StringComparison.OrdinalIgnoreCase)) 
                            ?? throw new Exception("The Weapon Doesn't exist: " + weaponName);

            return weapon.Clone() as Weapon ?? throw new Exception("Failed to clone the weapon: " + weaponName);
        }
    }
}
