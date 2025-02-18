namespace Engine.Models;
public enum MovementType
{
    BaseMovement = 5,
    SlowMovement = 4,
    KnightMovement = 6
}

public class ClassType
{
    public string ClassName { get; set; }
    public List<WeaponType> UsableWeapons { get; set; }
    public int Movement { get; set; }
    public ClassType? PromotesTo { get; set; }

    public ClassType(string className, List<WeaponType> usableWeapons,int movement, ClassType? promotesTo)
    {
        ClassName = className;
        UsableWeapons = usableWeapons;
        Movement = movement;
        PromotesTo = promotesTo;
    }
}
