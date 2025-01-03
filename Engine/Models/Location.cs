﻿namespace Engine.Models
{
    public class Location
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public List<Quest> QuestsAvailableHere { get; set; } = new List<Quest>();

        public Location(int X, int Y, string name, string Desc, string ImagName)
        {
            XCoordinate = X;
            YCoordinate = Y;
            Name = name;
            Description = Desc;
            ImageName = ImagName;
        }
       
    }
}
