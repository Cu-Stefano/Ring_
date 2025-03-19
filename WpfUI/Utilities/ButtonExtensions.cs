using System.Windows.Controls;
using Engine.FEMap;
using Engine.Models;

namespace WpfUI.Utilities;

public static class ButtonExtensions
{
    public static Tile GetTile(this Button button)
    {
        return (Tile)button.Tag;
    }
}