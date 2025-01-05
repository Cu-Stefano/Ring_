namespace Engine.Models;

public class ClassType
{
    public string ClassName { get; set; }
    public List<string> UsableWeapons { get; set; }
    public ClassType? PromotesTo { get; set; }

    public ClassType(string className, List<string> usableWeapons, ClassType? promotesTo)
    {
        ClassName = className;
        UsableWeapons = usableWeapons;
        PromotesTo = promotesTo;
    }
}