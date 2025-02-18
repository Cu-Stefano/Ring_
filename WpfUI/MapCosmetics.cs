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

    public void SetTileAsSelected(Button button)
    {
        button.BorderBrush = Brushes.Red;
        button.BorderThickness = new Thickness(2);
        OnPropertyChanged("button");
    }

    public void SetTileAsDeselected(Button button)
    {
        button.BorderBrush = Brushes.Gray;
        button.BorderThickness = new Thickness(1);
        OnPropertyChanged("button");
    }

    public void SetGetPathBrush(Button button)
    {
        Random random = new Random();
        button.Background = GetColorVariant(Colors.LightSkyBlue, 15, 100);
        OnPropertyChanged("button");
    }
    public void SetGetAttackBrush(Button button)
    {
        Random random = new Random();
        button.Background = GetColorVariant(Colors.NavajoWhite, 10);
        OnPropertyChanged("button");
    }

    public void SetTrailSelector(Button button)
    {
        button.BorderBrush = Brushes.CornflowerBlue;
        button.BorderThickness = new Thickness(2.5);
        OnPropertyChanged("button");
    }

    public static Brush GetUnitColor(Unit unit)
    {
        Random random = new Random();

        if (unit.Type == UnitType.Allay)
        {
            if(unit.CanMove)
                return GetColorVariant(Colors.CornflowerBlue, 50);
            return GetColorVariant(Colors.DarkGray, 0, 200);
        }
        if (unit.Type == UnitType.Enemy)
        {
            if (unit.CanMove)
                return GetColorVariant(Colors.Tomato, 0);
            return GetColorVariant(Colors.DimGray, 50);
        }
        return Brushes.Black; // Default color for other unit types
    }

    public static Brush GetTileBrush(Tile tile)
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

    public static Polygon GetTriangle(Unit unit)
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

}
