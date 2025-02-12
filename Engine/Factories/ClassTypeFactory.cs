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
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Sage",
                usableWeapons: new List<string> { "Tome", "Staff" },
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Mage",
                usableWeapons: new List<string> { "Tome" },
                promotesTo: ClassTypeList.Find(c => c.ClassName == "Sage")
            ));

            // New classes
            ClassTypeList.Add(new ClassType(
                className: "Swordmaster",
                usableWeapons: new List<string> { "Sword" },
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Warrior",
                usableWeapons: new List<string> { "Axe", "Bow" },
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "General",
                usableWeapons: new List<string> { "Lance", "Axe", "Sword" },
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "PegasusK",
                usableWeapons: new List<string> { "Lance", "Sword" },
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Hero",
                usableWeapons: new List<string> { "Sword", "Axe" },
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Berserker",
                usableWeapons: new List<string> { "Axe" },
                promotesTo: null
            ));
        }

        internal static ClassType GetClassTypeByName(string name)
        {
            return ClassTypeList.FirstOrDefault(c => c.ClassName == name) ?? throw new Exception("That Class type doesn't exist");
        }
    }
}
