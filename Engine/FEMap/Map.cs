using System.Text.Json.Serialization;

namespace Engine.FEMap;

public class Map
{
    public int mapID;
    public string mapName;
    public readonly List<List<Tile>> levelMap;
    public static int mapSize = 10;

    [JsonConstructor]
    public Map() { }
    public Map(int mapId, string mapname, List<List<Tile>> levelmap)
    {
        mapID = mapId;
        mapName = mapname;
        levelMap = levelmap;
    }

    public Map Clone()
    {
        return new Map(mapID, mapName, levelMap);
    }
}