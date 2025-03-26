namespace Engine.Factories
{
    using Engine.Models;
    using System.Text.Json;

    public static class ItemFactory
    {
        internal static readonly List<GameItem> StandardGameItems;
        public static List<ItemType> StandardTypeItems { get; }
        private const string ItemsFilePath = "./Resources/Items/Items.json";

        static ItemFactory()
        {
            StandardTypeItems = new List<ItemType>
            {
                new ItemType("Weapon"),
                new ItemType("GameItem"),
                new ItemType("Drop")
            };

            StandardGameItems = new List<GameItem>();

            // Impostiamo le opzioni di serializzazione
            var options = new JsonSerializerOptions
            {
                Converters = { new GameItemConverter(), new WeaponTypeConverter() }
            };

            StandardGameItems = JsonSerializer.Deserialize<List<GameItem>>(File.ReadAllText(ItemsFilePath), options) ?? throw new InvalidOperationException();
        }

        public static GameItem CreateGameItem(string itemName)
        {
            GameItem standardItem = StandardGameItems.FirstOrDefault(item => item.Name == itemName) ?? throw new Exception("The Item Doesn't exist:" + itemName);

            return standardItem.Clone();
        }
    }
}