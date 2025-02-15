namespace Engine.Models;

public class ClassType
{
    public string ClassName { get; set; }
    public List<string> UsableWeapons { get; set; }
    public int Movement { get; set; }
    public ClassType? PromotesTo { get; set; }

    public ClassType(string className, List<string> usableWeapons,int movement, ClassType? promotesTo)
    {
        ClassName = className;
        UsableWeapons = usableWeapons;
        Movement = movement;
        PromotesTo = promotesTo;
    }
}
public enum MovementType
{
    BaseMovement = 5,
    SlowMovement = 4,
    KnightMovement = 6
}
