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

    [JsonConstructor]
    public Tile() { }
    public Tile(int tileID, string tileName, bool walkable)
    {
        TileID = tileID;
        TileName = tileName;
        Walkable = walkable;
    }

    public Tile Clone()
    {
        return new Tile(TileID, TileName, Walkable);
    }
}