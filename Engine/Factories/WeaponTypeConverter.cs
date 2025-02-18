using System.Text.Json;
using System.Text.Json.Serialization;
using Engine.Models;

namespace Engine.Factories;

public class WeaponTypeConverter : JsonConverter<WeaponType>
{
    public override WeaponType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (Enum.TryParse<WeaponType>(value, out var weaponType))
        {
            return weaponType;
        }

        throw new JsonException($"Value '{value}' cannot be converted to WeaponType.");
    }

    public override void Write(Utf8JsonWriter writer, WeaponType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}