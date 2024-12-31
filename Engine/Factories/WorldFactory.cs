using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        internal static WorldMap CreateWorld()
        {
            var newWorld = new WorldMap();
            var libraryPath = Path.GetDirectoryName(typeof(WorldFactory).Assembly.Location);
            newWorld.AddLocation(-2, -1, "Farmer's Field",
                "There are rows of corn growing here, with giant rats hiding between them.",
                Path.Combine(libraryPath, "Images", "Locations", "field1.png"));
            newWorld.AddLocation(-1, -1, "Farmer's House",
                "This is the house of your neighbor, Farmer Ted.",
                Path.Combine(libraryPath, "Images", "Locations", "farm2.png"));
            newWorld.AddLocation(0, -1, "Home",
                "This is your home",
                Path.Combine(libraryPath, "Images", "Locations", "home1.png"));
            newWorld.AddLocation(-1, 0, "Trading Shop",
                "The shop of Susan, the trader.",
                Path.Combine(libraryPath, "Images", "Locations", "shop1.png"));
            newWorld.AddLocation(0, 0, "Town square",
                "You see a fountain here.",
                Path.Combine(libraryPath, "Images", "Locations", "square1.png"));
            newWorld.AddLocation(1, 0, "Town Gate",
                "There is a gate here, protecting the town from giant spiders.",
                Path.Combine(libraryPath, "Images", "Locations", "gate1.png"));
            newWorld.AddLocation(2, 0, "Spider Forest",
                "The trees in this forest are covered with spider webs.",
                Path.Combine(libraryPath, "Images", "Locations", "forest1.png"));

            newWorld.AddLocation(0, 1, "Herbalist's hut",
                "You see a small hut, with plants drying from the roof.",
                Path.Combine(libraryPath, "Images", "Locations", "hut2.png"));
            //add a qeust 1 to the 0,1 location
            newWorld.LocationAt(0, 1).QuestsAvailableHere.Add(QuestFactory.GetQuestById(1));

            newWorld.AddLocation(0, 2, "Herbalist's garden",
                "There are many plants here, with snakes hiding behind them.",
                Path.Combine(libraryPath, "Images", "Locations", "garden1.png"));

            return newWorld;
        }
    }
}
