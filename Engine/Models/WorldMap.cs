using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Engine.Models
{
    public class WorldMap
    {
        private readonly List<Location> _locations = [];

        internal void AddLocation(int x, int y, string name, string Desc, string ImageName)
        {
            _locations.Add(new Location(x, y, name, Desc, ImageName));
        }

        /// <summary>
        /// returns the location of the coordinates, if there isn't then null
        /// </summary>
        public Location? LocationAt(int x, int y)
        {
            foreach (var location in _locations)
            {
                if (location.XCoordinate == x && location.YCoordinate == y)
                {
                    return location;
                }
            }
            return null;
        }
    }
}
