using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Engine.Factories;
using Engine.Models;
using Engine.FEMap;

public class MapConverter : JsonConverter<Map>
{
    public override Map Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            var level = JsonDocument.ParseValue(ref reader).RootElement;
            var newlevel =
                level.GetProperty("levelMap").EnumerateArray().Select(row =>
                        row.EnumerateArray().Select(element => TileFactory.CreateTile(element.GetInt32())).ToList())
                    .ToList();

            return new Map(level.GetProperty("MapID").GetInt32(), level.GetProperty("MapName").ToString(), newlevel);
        }
        catch (Exception e)
        {
            throw new JsonException($" {e} :Tipo di oggetto sconosciuto nel JSON.");
        }
    }


    public override void Write(Utf8JsonWriter writer, Map value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}