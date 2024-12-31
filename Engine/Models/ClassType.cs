namespace Engine.Models;

public class ClassType
{
    public string ClassName { get; set; }
    public List<Weapon> UsableWeapons { get; set; }
    public ClassType PromotesTo { get; set; }
}