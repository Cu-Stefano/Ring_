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
                usableWeapons: new List<WeaponType> { WeaponType.Sword, WeaponType.Lance, WeaponType.Axe },
                movement: (int)MovementType.BaseMovement + 1,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Sage",
                usableWeapons: new List<WeaponType> { WeaponType.Tome, WeaponType.Staff },
                movement: (int)MovementType.SlowMovement + 1,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Mage",
                usableWeapons: new List<WeaponType> { WeaponType.Tome },
                movement: (int)MovementType.SlowMovement,
                promotesTo: ClassTypeList.Find(c => c.ClassName == "Sage")
            ));

            // New classes
            ClassTypeList.Add(new ClassType(
                className: "Swordmaster",
                usableWeapons: new List<WeaponType> { WeaponType.Sword },
                movement: (int)MovementType.BaseMovement + 1,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Warrior",
                usableWeapons: new List<WeaponType> { WeaponType.Axe, WeaponType.Bow },
                movement: (int)MovementType.BaseMovement,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "General",
                usableWeapons: new List<WeaponType> { WeaponType.Lance, WeaponType.Axe, WeaponType.Sword },
                movement: (int)MovementType.SlowMovement + 1,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "PegasusK",
                usableWeapons: new List<WeaponType> { WeaponType.Lance, WeaponType.Sword },
                movement: (int)MovementType.KnightMovement,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Hero",
                usableWeapons: new List<WeaponType> { WeaponType.Sword, WeaponType.Axe },
                movement: (int)MovementType.BaseMovement,
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Berserker",
                usableWeapons: new List<WeaponType> { WeaponType.Axe },
                movement: (int)MovementType.SlowMovement + 1,
                promotesTo: null
            ));
        }

        internal static ClassType GetClassTypeByName(string name)
        {
            return ClassTypeList.FirstOrDefault(c => c.ClassName == name) ?? throw new Exception("That Class type doesn't exist");
        }
    }
}
