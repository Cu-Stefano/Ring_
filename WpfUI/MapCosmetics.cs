using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Engine;
using Engine.FEMap;
using Engine.Models;

namespace WpfUI;

public class MapCosmetics : BaseNotification
{
    public MapBuilder _mapBuilder;

    public MapCosmetics(MapBuilder mapBuilder)
    {
        _mapBuilder = mapBuilder;
    }

    public void TileSelected(Button button)
    {
        button.BorderBrush = Brushes.Red;
        button.BorderThickness = new Thickness(2);
        OnPropertyChanged("button");
    }

    public void TileDeSelected(Button button)
    {
        button.BorderBrush = Brushes.Gray;
        button.BorderThickness = new Thickness(1);
        OnPropertyChanged("button");
    }

    public void GetPathBrush(Button button)
    {
        Random random = new Random();
        button.Background = GetColorVariant(Colors.LightSkyBlue, 15, 100);
        OnPropertyChanged("button");
    }
    public void GetAttackBrush(Button button)
    {
        Random random = new Random();
        button.Background = GetColorVariant(Colors.NavajoWhite, 10);
        OnPropertyChanged("button");
    }

    public void TrailSelector(Button button)
    {
        button.BorderBrush = Brushes.Black;
        button.BorderThickness = new Thickness(3);
        OnPropertyChanged("button");
    }

    public static Brush GetUnitColor(Unit unit)
    {
        Random random = new Random();

        if (unit.Type == UnitType.Allay)
        {
            return GetColorVariant(Colors.CornflowerBlue, 50);
        }
        if (unit.Type == UnitType.Enemy)
        {
            return GetColorVariant(Colors.Tomato, 50);
        }
        return Brushes.Black; // Default color for other unit types
    }

    public Brush GetTileBrush(Tile tile)
    {
        return tile.TileName switch
        {
            "Plains" => GetColorVariant(Colors.LightGreen, 30),
            "Mountains" => GetColorVariant(Colors.DarkGray, 30),
            "Waters" => GetColorVariant(Colors.CornflowerBlue, 20, 180),
            _ => Brushes.PaleGreen, // Default per tipi sconosciuti
        };
    }

    public static Brush GetColorVariant(Color baseColor, byte maxVariation, byte alpha = 255)
    {
        Random random = new Random();
        byte variation = (byte)random.Next(0, maxVariation); // Small variation

        byte r = (byte)Math.Max(0, Math.Min(255, baseColor.R - variation));
        byte g = (byte)Math.Max(0, Math.Min(255, baseColor.G - variation));
        byte b = (byte)Math.Max(0, Math.Min(255, baseColor.B - variation));

        return new SolidColorBrush(Color.FromArgb(alpha, r, g, b));
    }


    public static Polygon Triangle(Unit unit)
    {
        var triangle = new Polygon
        {
            Points = new PointCollection(new List<Point>
            {
                new Point(20, 0),
                new Point(30, 20),
                new Point(0, 20)
            }),
            Fill = GetUnitColor(unit),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        return triangle;
    }

    public Button? GetButtonBasedOnTile(Tile tile)
    {
        return _mapBuilder.ActualMap.SelectMany(row => row).FirstOrDefault(b => b?.Tag == tile);
    }
}
