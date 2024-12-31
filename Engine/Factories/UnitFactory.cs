using Engine.Models;
using System.Collections.Generic;

namespace Engine.Factories
{
    internal static class UnitFactory
    {
        private static readonly List<Unit> _units = new List<Unit>();

        static UnitFactory()
        {
            // Example units
            _units.Add(new Unit(
                name: "warrior",
                type: UnitType.Allay,
                classType: new ClassType { ClassName = "warrior" },
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

            _units.Add(new Unit(
                name: "mage",
                type: UnitType.Allay,
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
            return _units.FirstOrDefault(unit => unit.Name == name) ?? throw new Exception("That unit doesn't exist");
        }
    }
}
