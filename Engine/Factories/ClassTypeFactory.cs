using Engine.Models;
using System.Collections.Generic;

namespace Engine.Factories
{
    public static class ClassTypeFactory
    {
        public static readonly List<ClassType> ClassTypeList = new List<ClassType>();

        static ClassTypeFactory()
        {
            // Example units
            ClassTypeList.Add(new ClassType(
                className: "Lord",
                usableWeapons: new List<string> { "Sword", "Lance", "Axe" },
                movement: (int)MovementType.BaseMovement+1,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Sage",
                movement: (int)MovementType.SlowMovement+1,
                usableWeapons: new List<string> { "Tome", "Staff" },
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Mage",
                usableWeapons: new List<string> { "Tome" },
                movement: (int)MovementType.SlowMovement,
                promotesTo: ClassTypeList.Find(c => c.ClassName == "Sage")
            ));

            // New classes
            ClassTypeList.Add(new ClassType(
                className: "Swordmaster",
                usableWeapons: new List<string> { "Sword" },
                movement: (int)MovementType.BaseMovement + 1,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Warrior",
                usableWeapons: new List<string> { "Axe", "Bow" },
                movement: (int)MovementType.BaseMovement,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "General",
                usableWeapons: new List<string> { "Lance", "Axe", "Sword" },
                movement: (int)MovementType.SlowMovement+1,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "PegasusK",
                usableWeapons: new List<string> { "Lance", "Sword" },
                movement: (int)MovementType.KnightMovement,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Hero",
                usableWeapons: new List<string> { "Sword", "Axe" },
                movement: (int)MovementType.BaseMovement,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Berserker",
                usableWeapons: new List<string> { "Axe" },
                movement: (int)MovementType.SlowMovement+1,
                promotesTo: null
            ));
        }

        internal static ClassType GetClassTypeByName(string name)
        {
            return ClassTypeList.FirstOrDefault(c => c.ClassName == name) ?? throw new Exception("That Class type doesn't exist");
        }
    }
}
