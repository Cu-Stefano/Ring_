using Engine.Models;
using System.Text.Json;

namespace Engine.FEMap
{
    public static class TileFactory
    {
        public static List<Tile> _standardTiles;
        private const string ItemsFilePath = "./Resources/tiles/tiles.json";

        static TileFactory()
        {
            _standardTiles = new List<Tile>();
            _standardTiles = JsonSerializer.Deserialize<List<Tile>>(File.ReadAllText(ItemsFilePath));
        }

        public static Tile CreateTile(int tileID)
        {
            Tile tile = _standardTiles.FirstOrDefault(item => item.TileID == tileID);

            if (tile != null)
            {
                return tile.Clone();
            }

            throw new Exception($"{_standardTiles.ToArray()}tileID doesn't exist");
        }
    }
}