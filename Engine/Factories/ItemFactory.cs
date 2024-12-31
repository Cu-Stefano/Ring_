namespace Engine.Factories
{
    using Engine.Models;
    using System.Text.Json;

    public static class ItemFactory
    {
        private static readonly List<GameItem> _standardGameItems;
        public static List<ItemType> StandardTypeItems { get; }
        private const string ItemsFilePath = "./Data/Items/Items.json";

        static ItemFactory()
        {
            StandardTypeItems =
            [
                new ItemType("Weapon"),
                new ItemType("GameItem"),
                new ItemType("Drop")
            ];

            _standardGameItems = new List<GameItem>();

            // Impostiamo le opzioni di serializzazione
            var options = new JsonSerializerOptions
            {
                Converters = { new GameItemConverter() }
            };

            _standardGameItems = JsonSerializer.Deserialize<List<GameItem>>(File.ReadAllText(ItemsFilePath), options) ?? throw new InvalidOperationException();
        }

        public static GameItem CreateGameItem(int itemTypeId)
        {
            GameItem standardItem = _standardGameItems.FirstOrDefault(item => item.ItemTypeId == itemTypeId);

            if (standardItem != null)
            {
                return standardItem.Clone();
            }

            throw new Exception($"{_standardGameItems.ToArray()}ItemId doesn't exist");
        }
    }
}