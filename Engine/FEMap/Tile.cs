using Engine.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Engine.FEMap;

public class Tile
{
   public int TileID { get; set; }
        public string TileName { get; set; }
        public bool Walkable { get; set; }
        public Unit? UnitOn { get; set; }
        public string Icon { get; set; }

        [JsonConstructor]
        public Tile() { }

        public Tile(int tileID, string tileName, bool walkable, Unit? unitOn)
        {
            TileID = tileID;
            TileName = tileName;
            Walkable = walkable;
            UnitOn = unitOn;
        Icon = GetTileIcon(tileName);
        }

    private string GetTileIcon(string tileName)
    {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string imagePath = tileName switch
        {
            "Plains" => System.IO.Path.Combine(basePath, "Images", "Locations", "plain.png"),
            "Mountains" => System.IO.Path.Combine(basePath, "Images", "Locations", "rock.png"),
            "Waters" => System.IO.Path.Combine(basePath, "Images", "Locations", "water.png"),
            _ => System.IO.Path.Combine(basePath, "Images", "Locations", "plain.png")
        };

        return imagePath;
    }


    public Tile Clone()
    {
        return new Tile(TileID, TileName, Walkable, UnitOn);
    }
}