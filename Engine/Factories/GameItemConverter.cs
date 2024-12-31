using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Engine.Factories;
using Engine.Models;

public class GameItemConverter : JsonConverter<GameItem>
{
    private static readonly Dictionary<string, Type?> TypeCache = ItemFactory.StandardTypeItems
        .ToDictionary(type => type.Name, type => Type.GetType("Engine.Models." + type.Name));

    private Type ResolveType(string typeName)
    {
        if (TypeCache.TryGetValue(typeName, out var actualType))
        {
            return actualType;
        }

        throw new InvalidOperationException($"Tipo specificato '{typeName}' non trovato.");
    }

    public override GameItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var element = JsonDocument.ParseValue(ref reader).RootElement;

        if (element.TryGetProperty("Type", out var itemType))
        {
            var actualType = ResolveType(itemType.ToString());

            //resetto le options
            var noCustomConvertersOptions = new JsonSerializerOptions();
            var deserializedObject =
                JsonSerializer.Deserialize(element.GetRawText(), actualType, noCustomConvertersOptions);

            return deserializedObject as GameItem
                   ?? throw new InvalidOperationException("Errore nella deserializzazione del tipo specificato.");
        }

        throw new JsonException("Tipo di oggetto sconosciuto nel JSON.");
    }


    public override void Write(Utf8JsonWriter writer, GameItem value, JsonSerializerOptions options)
    {
        var typeName = value.GetType().Name; // Otteniamo il nome del tipo
        var jsonObject = JsonSerializer.Serialize(value, value.GetType(), options);
        var document = JsonDocument.Parse(jsonObject);

        writer.WriteStartObject();
        writer.WriteString("Type", typeName); // Aggiungiamo il tipo al JSON
        foreach (var property in document.RootElement.EnumerateObject())
        {
            property.WriteTo(writer);
        }

        writer.WriteEndObject();
    }
}