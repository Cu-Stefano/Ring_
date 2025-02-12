using Engine.Models;

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

            units.Add(new Unit(
                name: "Riko",
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
                name: "Sora",
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
                name: "Aros",
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

            // New units
            units.Add(new Unit(
                name: "Lyn",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("Swordmaster"),
                level: 3,
                move: 6,
                stats: new Stats
                {
                    Strength = 15,
                    Magic = 3,
                    Skill = 10,
                    Speed = 8,
                    Luck = 7,
                    Defense = 5,
                    Resistance = 4
                },
                equipedWeapon: ItemFactory.CreateGameItem("BronzeSword") as Weapon
            ));

            units.Add(new Unit(
                name: "Hector",
                type: UnitType.Enemy,
                classType: ClassTypeFactory.GetClassTypeByName("Warrior"),
                level: 4,
                move: 5,
                stats: new Stats
                {
                    Strength = 18,
                    Magic = 1,
                    Skill = 8,
                    Speed = 5,
                    Luck = 6,
                    Defense = 12,
                    Resistance = 3
                },
                equipedWeapon: ItemFactory.CreateGameItem("BronzeAxe") as Weapon
            ));

            units.Add(new Unit(
                name: "Erk",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("Mage"),
                level: 2,
                move: 5,
                stats: new Stats
                {
                    Strength = 1,
                    Magic = 12,
                    Skill = 7,
                    Speed = 6,
                    Luck = 5,
                    Defense = 3,
                    Resistance = 10
                },
                equipedWeapon: ItemFactory.CreateGameItem("FireTome") as Weapon
            ));

            units.Add(new Unit(
                name: "Oswin",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("General"),
                level: 5,
                move: 4,
                stats: new Stats
                {
                    Strength = 20,
                    Magic = 0,
                    Skill = 9,
                    Speed = 3,
                    Luck = 4,
                    Defense = 15,
                    Resistance = 5
                },
                equipedWeapon: ItemFactory.CreateGameItem("IronShield") as Weapon
            ));

            units.Add(new Unit(
                name: "Florina",
                type: UnitType.Enemy,
                classType: ClassTypeFactory.GetClassTypeByName("PegasusK"),
                level: 3,
                move: 7,
                stats: new Stats
                {
                    Strength = 12,
                    Magic = 2,
                    Skill = 8,
                    Speed = 10,
                    Luck = 6,
                    Defense = 4,
                    Resistance = 7
                },
                equipedWeapon: ItemFactory.CreateGameItem("IronSword") as Weapon
            ));

            units.Add(new Unit(
                name: "Raven",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("Hero"),
                level: 4,
                move: 6,
                stats: new Stats
                {
                    Strength = 17,
                    Magic = 1,
                    Skill = 11,
                    Speed = 9,
                    Luck = 5,
                    Defense = 8,
                    Resistance = 4
                },
                equipedWeapon: ItemFactory.CreateGameItem("SilverSword") as Weapon
            ));

            units.Add(new Unit(
                name: "Dart",
                type: UnitType.Enemy,
                classType: ClassTypeFactory.GetClassTypeByName("Berserker"),
                level: 5,
                move: 5,
                stats: new Stats
                {
                    Strength = 20,
                    Magic = 0,
                    Skill = 10,
                    Speed = 7,
                    Luck = 4,
                    Defense = 6,
                    Resistance = 3
                },
                equipedWeapon: ItemFactory.CreateGameItem("SteelAxe") as Weapon
            ));

            units.Add(new Unit(
                name: "Pent",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("Sage"),
                level: 6,
                move: 5,
                stats: new Stats
                {
                    Strength = 2,
                    Magic = 18,
                    Skill = 12,
                    Speed = 8,
                    Luck = 7,
                    Defense = 5,
                    Resistance = 15
                },
                equipedWeapon: ItemFactory.CreateGameItem("ThunderTome") as Weapon
            ));
        }

        internal static Unit GetUnitByName(string name)
        {
            return units.FirstOrDefault(unit => unit.Name == name) ?? throw new Exception("That unit doesn't exist");
        }
    }
}
