using Engine.Models;
using System.Text.Json;

namespace Engine.FEMap;

public static class MapFactory
{
    public static readonly List<Map> _allMaps;
    private const string MapsFilePath = "./Data/maps/maps.json";

    static MapFactory()
    {
        _allMaps = new List<Map>();
        // Impostiamo le opzioni di serializzazione
        var options = new JsonSerializerOptions
        {
            Converters = { new MapConverter() }
        };

        _allMaps = JsonSerializer.Deserialize<List<Map>>(File.ReadAllText(MapsFilePath), options) ?? throw new InvalidOperationException();
    }

    public static Map CreateMap(int mapID)
    {
        Map map = _allMaps.FirstOrDefault(m => m.mapID == mapID);

        if (map != null)
        {
            return map.Clone();
        }

        throw new Exception($"{_allMaps.ToArray()}MapID doesn't exist");
    }
}