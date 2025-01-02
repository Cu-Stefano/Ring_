using Engine.Models;
using System.Collections.Generic;

namespace Engine.Factories
{
    public static class UnitFactory
    {
        public static readonly List<Unit> units = new List<Unit>();

        static UnitFactory()
        {
            // Example units
            units.Add(new Unit(
                name: "Ike",
                type: UnitType.Allay,
                classType: new ClassType { ClassName = "Lord" },
                level: 1,
                move: 5,
                stats: new Stats
                {
                    Strength = 10,
                    Magic = 2,
                    Skill = 5,
                    Speed = 4,
                    Luck = 3,
                    Defense = 8,
                    Resistance = 2
                },
                equipedWeapon: ItemFactory.CreateGameItem(100) as Weapon
            ));

            units.Add(new Unit(
                name: "sora",
                type: UnitType.Enemy,
                classType: new ClassType { ClassName = "mage" },
                level: 1,
                move: 4,
                stats: new Stats
                {
                    Strength = 2,
                    Magic = 10,
                    Skill = 5,
                    Speed = 4,
                    Luck = 3,
                    Defense = 2,
                    Resistance = 8
                },
                equipedWeapon: ItemFactory.CreateGameItem(100) as Weapon
            ));
        }

        internal static Unit GetUnitByName(string name)
        {
            return units.FirstOrDefault(unit => unit.Name == name) ?? throw new Exception("That unit doesn't exist");
        }
    }
}
