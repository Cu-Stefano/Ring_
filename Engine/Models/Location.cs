namespace Engine.Models
{
    public class Location(int x, int y, string name, string desc, string imagName)
    {
        public int XCoordinate { get; set; } = x;
        public int YCoordinate { get; set; } = y;
        public string Name { get; set; } = name;
        public string Description { get; set; } = desc;
        public string ImageName { get; set; } = imagName;
    }
}
