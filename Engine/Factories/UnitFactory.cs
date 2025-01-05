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
                classType: ClassTypeFactory.GetClassTypeByName("Lord"),
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
                equipedWeapon: ItemFactory.CreateGameItem("IronSword") as Weapon
            ));

            // Example units
            units.Add(new Unit(
                name: "riko",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("Lord"),
                level: 2,
                move: 5,
                stats: new Stats
                {
                    Strength = 13,
                    Magic = 2,
                    Skill = 7,
                    Speed = 4,
                    Luck = 5,
                    Defense = 6,
                    Resistance = 2
                },
                equipedWeapon: ItemFactory.CreateGameItem("IronSword") as Weapon
            ));

            units.Add(new Unit(
                name: "sora",
                type: UnitType.Enemy,
                classType: ClassTypeFactory.GetClassTypeByName("Mage"),
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
                equipedWeapon: ItemFactory.CreateGameItem("FireTome") as Weapon
            ));
            
            units.Add(new Unit(
                name: "aros",
                type: UnitType.Enemy,
                classType: ClassTypeFactory.GetClassTypeByName("Sage"),
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
                equipedWeapon: ItemFactory.CreateGameItem("FireTome") as Weapon
            ));
        }

        internal static Unit GetUnitByName(string name)
        {
            return units.FirstOrDefault(unit => unit.Name == name) ?? throw new Exception("That unit doesn't exist");
        }
    }
}
