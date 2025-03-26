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
                stats: new Stats
                {
                    HpMax = 19,
                    Hp = 19,
                    Strength = 5,
                    Magic = 1,
                    Skill = 6,
                    Speed = 7,
                    Luck = 5,
                    Defense = 6,
                    Resistance = 0
                },
                equipedWeapon: WeaponFactory.CreateWeapon("IronSword")
            ));

            units.Add(new Unit(
                name: "Riko",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("Lord"),
                level: 1,
                stats: new Stats
                {
                    HpMax = 20,
                    Hp = 20,
                    Strength = 6,
                    Magic = 1,
                    Skill = 5,
                    Speed = 6,
                    Luck = 4,
                    Defense = 5,
                    Resistance = 1
                },
                equipedWeapon: WeaponFactory.CreateWeapon("IronSword")
            ));

            units.Add(new Unit(
                name: "Sora",
                type: UnitType.Enemy,
                classType: ClassTypeFactory.GetClassTypeByName("Mage"),
                level: 1,
                stats: new Stats
                {
                    HpMax = 16,
                    Hp = 16,
                    Strength = 0,
                    Magic = 4,
                    Skill = 4,
                    Speed = 5,
                    Luck = 2,
                    Defense = 2,
                    Resistance = 5
                },
                equipedWeapon: WeaponFactory.CreateWeapon("FireTome")
            ));

            units.Add(new Unit(
                name: "Aros",
                type: UnitType.Enemy,
                classType: ClassTypeFactory.GetClassTypeByName("Sage"),
                level: 1,
                stats: new Stats
                {
                    HpMax = 18,
                    Hp = 18,
                    Strength = 0,
                    Magic = 5,
                    Skill = 5,
                    Speed = 4,
                    Luck = 3,
                    Defense = 3,
                    Resistance = 6
                },
                equipedWeapon: WeaponFactory.CreateWeapon("FireTome")
            ));

            // New units
            units.Add(new Unit(
                name: "Lyn",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("Swordmaster"),
                level: 1,
                stats: new Stats
                {
                    HpMax = 18,
                    Hp = 18,
                    Strength = 4,
                    Magic = 0,
                    Skill = 9,
                    Speed = 11,
                    Luck = 5,
                    Defense = 2,
                    Resistance = 0
                },
                equipedWeapon: WeaponFactory.CreateWeapon("BronzeSword")
            ));

            units.Add(new Unit(
                name: "Hector",
                type: UnitType.Enemy,
                classType: ClassTypeFactory.GetClassTypeByName("Warrior"),
                level: 1,
                stats: new Stats
                {
                    HpMax = 23,
                    Hp = 23,
                    Strength = 7,
                    Magic = 0,
                    Skill = 4,
                    Speed = 5,
                    Luck = 3,
                    Defense = 8,
                    Resistance = 1
                },
                equipedWeapon: WeaponFactory.CreateWeapon("BronzeAxe")
            ));

            units.Add(new Unit(
                name: "Erk",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("Mage"),
                level: 1,
                stats: new Stats
                {
                    HpMax = 17,
                    Hp = 17,
                    Strength = 0,
                    Magic = 5,
                    Skill = 6,
                    Speed = 7,
                    Luck = 3,
                    Defense = 2,
                    Resistance = 4
                },
                equipedWeapon: WeaponFactory.CreateWeapon("FireTome")
            ));

            units.Add(new Unit(
                name: "Oswin",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("General"),
                level: 1,
                stats: new Stats
                {
                    HpMax = 20,
                    Hp = 20,
                    Strength = 9,
                    Magic = 0,
                    Skill = 5,
                    Speed = 3,
                    Luck = 3,
                    Defense = 11,
                    Resistance = 2
                },
                equipedWeapon: null
            ));

            units.Add(new Unit(
                name: "Florina",
                type: UnitType.Enemy,
                classType: ClassTypeFactory.GetClassTypeByName("PegasusK"),
                level: 1,
                stats: new Stats
                {
                    HpMax = 17,
                    Hp = 17,
                    Strength = 4,
                    Magic = 0,
                    Skill = 6,
                    Speed = 9,
                    Luck = 6,
                    Defense = 3,
                    Resistance = 5
                },
                equipedWeapon: WeaponFactory.CreateWeapon("IronSword")
            ));

            units.Add(new Unit(
                name: "Raven",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("Hero"),
                level: 1,
                stats: new Stats
                {
                    HpMax = 21,
                    Hp = 21,
                    Strength = 5,
                    Magic = 0,
                    Skill = 8,
                    Speed = 9,
                    Luck = 2,
                    Defense = 5,
                    Resistance = 1
                },
                equipedWeapon: WeaponFactory.CreateWeapon("SilverSword")
            ));

            units.Add(new Unit(
                name: "Dart",
                type: UnitType.Enemy,
                classType: ClassTypeFactory.GetClassTypeByName("Berserker"),
                level: 1,
                stats: new Stats
                {
                    HpMax = 24,
                    Hp = 24,
                    Strength = 8,
                    Magic = 0,
                    Skill = 5,
                    Speed = 6,
                    Luck = 2,
                    Defense = 4,
                    Resistance = 1
                },
                equipedWeapon: WeaponFactory.CreateWeapon("SteelAxe")
            ));

            units.Add(new Unit(
                name: "Pent",
                type: UnitType.Allay,
                classType: ClassTypeFactory.GetClassTypeByName("Sage"),
                level: 1,
                stats: new Stats
                {
                    HpMax = 18,
                    Hp = 18,
                    Strength = 0,
                    Magic = 6,
                    Skill = 7,
                    Speed = 8,
                    Luck = 4,
                    Defense = 3,
                    Resistance = 7
                },
                equipedWeapon: WeaponFactory.CreateWeapon("ThunderTome")
            ));
        }

        internal static Unit GetUnitByName(string name)
        {
            return units.FirstOrDefault(unit => unit.Name == name) ?? throw new Exception("That unit doesn't exist");
        }
    }
}
