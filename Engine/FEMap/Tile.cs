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

    [JsonConstructor]
    public Tile() { }
    public Tile(int tileID, string tileName, bool walkable, Unit? unitOn)
    {
        TileID = tileID;
        TileName = tileName;
        Walkable = walkable;
        UnitOn = unitOn;
    }

    public Tile Clone()
    {
        return new Tile(TileID, TileName, Walkable, UnitOn);
    }
}