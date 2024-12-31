using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using System.Collections.Generic;

public class PolymorphicJsonConverter<TBase> : JsonConverter<TBase> where TBase : class
{
    // Crea un dizionario statico per memorizzare i tipi
    private static readonly Dictionary<string, Type?> TypeCache;

    // Costruttore statico che popola il dizionario dei tipi
    static PolymorphicJsonConverter()
    {
        // Crea una mappa dai tipi disponibili, supponendo che i tipi siano nel namespace di TBase
        TypeCache = typeof(TBase).Assembly.GetTypes()
            .Where(t => typeof(TBase).IsAssignableFrom(t) && !t.IsAbstract)
            .ToDictionary(t => t.Name, t => t);
    }

    private Type ResolveType(string typeName)
    {
        // Risolve il tipo a partire dal dizionario
        if (TypeCache.TryGetValue(typeName, out var actualType))
        {
            return actualType;
        }

        throw new InvalidOperationException($"Tipo specificato '{typeName}' non trovato.");
    }

    public override TBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var element = JsonDocument.ParseValue(ref reader).RootElement;

        // Cerca la proprietà "Type" nel JSON per determinare il tipo da deserializzare
        if (element.TryGetProperty("Type", out var typeProperty))
        {
            var actualType = ResolveType(typeProperty.GetString());

            // Deserializza l'oggetto usando il tipo risolto
            var noCustomConvertersOptions = new JsonSerializerOptions();
            var deserializedObject = JsonSerializer.Deserialize(element.GetRawText(), actualType, noCustomConvertersOptions);

            return deserializedObject as TBase
                   ?? throw new InvalidOperationException("Errore nella deserializzazione del tipo specificato.");
        }

        throw new JsonException("Tipo di oggetto sconosciuto nel JSON.");
    }

    public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
    {
        var typeName = value.GetType().Name; // Ottieni il nome del tipo da serializzare
        var jsonObject = JsonSerializer.Serialize(value, value.GetType(), options);
        var document = JsonDocument.Parse(jsonObject);

        writer.WriteStartObject();
        writer.WriteString("Type", typeName); // Aggiungi il tipo al JSON
        foreach (var property in document.RootElement.EnumerateObject())
        {
            property.WriteTo(writer); // Scrivi le proprietà dell'oggetto
        }

        writer.WriteEndObject();
    }
}
