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
                usableWeapons: ["Sword", "Lance", "Axe"],
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Sage",
                usableWeapons: ["Tome", "Staff"],
                promotesTo: null
            ));

            ClassTypeList.Add(new ClassType(
                className: "Mage",
                usableWeapons: ["Tome"],
                promotesTo: ClassTypeList.Find(c => c.ClassName == "Sage")
            ));

           
        }

        internal static ClassType GetClassTypeByName(string name)
        {
            return ClassTypeList.FirstOrDefault(c => c.ClassName == name) ?? throw new Exception("That Class type doesn't exist");
        }
    }

}