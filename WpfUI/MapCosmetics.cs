using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Engine;
using Engine.FEMap;
using Engine.Models;
using WpfUI.Utilities;

namespace WpfUI;

public class MapCosmetics : BaseNotification
{
    public void SetPolygon(Button? button)
    {
        button.Content = GetPolygon(button.GetTile().UnitOn);
        OnPropertyChanged("button");
    }
    public void SetButtonAsSelected(Button? button)
    {
        button.BorderBrush = Brushes.Red;
        button.BorderThickness = new Thickness(2);
        OnPropertyChanged("button");
    }

    public void SetButtonAsDeselected(Button? button)
    {
        button.BorderBrush = Brushes.Gray;
        button.BorderThickness = new Thickness(1);
        OnPropertyChanged("button");
    }

    public void SetGetPathBrush(Button? button)
    {
        Random random = new Random();
        button.Background = GetColorVariant(Colors.LightSkyBlue, 15, 100);
        OnPropertyChanged("button");
    }
    public void SetGetAttackBrush(Button? button)
    {
        Random random = new Random();
        button.Background = GetColorVariant(Colors.CornflowerBlue, 10);
        OnPropertyChanged("button");
    }

    public void SetGetEnemyPathBrush(Button? button)
    {
		Random random = new Random();
        button.Background = GetColorVariant(Colors.LightSalmon, 15, 100);
        OnPropertyChanged("button");
	}
    public void SetGetEnemyAttackBrush(Button? button)
    {
		Random random = new Random();
        button.Background = GetColorVariant(Colors.OrangeRed, 10);
        OnPropertyChanged("button");
	}

	public void SetTrailSelector(Button? button)
    {
        button.BorderBrush = Brushes.SteelBlue;
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
            return GetColorVariant(Colors.DimGray, 30);
        }
        return Brushes.Black; // Default color for other unit types
    }

    public static Brush GetTileBrush(Tile tile)
    {
        return tile.TileName switch
        {
            "Plains" => GetColorVariant(Colors.LightGreen, 0),
            "Mountains" => GetColorVariant(Colors.DarkGray, 30),
            "Waters" => GetColorVariant(Colors.CornflowerBlue, 20, 180),
            _ => Brushes.PaleGreen, // Default per tipi sconosciuti
        };
    }


    public void SetTileBrush(Button button)
    {
        button.Background = GetTileBrush(button.GetTile());
        OnPropertyChanged("button");
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

    public static Polygon GetPolygon(Unit unit)
    {
        Polygon Polygon;
        if (unit == null)
            return null;

        switch (unit.Class.UsableWeapons[0])
        {
            case WeaponType.Sword:
                Polygon = new Polygon
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
                break;

            case WeaponType.Lance:
                Polygon = new Polygon
                {
                    Points = new PointCollection(new List<Point>
                    {
                        new Point(15, 0),
                        new Point(30, 15),
                        new Point(15, 30),
                        new Point(0, 15)
                    }),
                    Fill = GetUnitColor(unit),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                break;

            case WeaponType.Axe:
                Polygon = new Polygon
                {
                    Points = new PointCollection(new List<Point>
                    {
                        new Point(0, 0),
                        new Point(30, 0),
                        new Point(30, 15),
                        new Point(0, 15)
                    }),
                    Fill = GetUnitColor(unit),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                break;

            case WeaponType.Tome:
                Polygon = new Polygon
                {
                    Points = new PointCollection(new List<Point>
                {
                    new Point(0, 0),
                    new Point(20, 0),
                    new Point(20, 20),
                    new Point(0, 20)
                }),
                    Fill = GetUnitColor(unit),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                break;

            case WeaponType.Bow:
                Polygon = new Polygon
                {
                    Points = new PointCollection(new List<Point>
                {
                    new Point(10, 0),
                    new Point(20, 10),
                    new Point(15, 20),
                    new Point(5, 20),
                    new Point(0, 10)
                }),
                    Fill = GetUnitColor(unit),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                break;

            case WeaponType.Staff:
                Polygon = new Polygon
                {
                    Points = new PointCollection(new List<Point>
                {
                    new Point(10, 0),
                    new Point(20, 10),
                    new Point(10, 20),
                    new Point(0, 10)
                }),
                    Fill = GetUnitColor(unit),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                break;

            default:
                Polygon = new Polygon
                {
                    Points = new PointCollection(new List<Point>
                {
                    new Point(10, 0),
                    new Point(20, 10),
                    new Point(10, 20),
                    new Point(0, 10)
                }),
                    Fill = GetUnitColor(unit),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                break;
        }

        return Polygon;
    }


    
}
