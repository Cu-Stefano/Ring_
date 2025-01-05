using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Engine.Models;
using Engine.Factories;
using Engine.FEMap;
using System.Windows;
using Engine.ViewModels;
using System.ComponentModel;
using System;

namespace WpfUI
{
    public partial class MapBuilder : UserControl, INotifyPropertyChanged
    {
        private int LevelIndex { get; set; }
        public string currentLevel { get; set; }
        public GameSession GameSession { get; set; }

        public MapBuilder(GameSession gameSession)
        {
            InitializeComponent();
            DataContext = this;
            GameSession = gameSession;
            LevelIndex = 1;
            var map = MapFactory.CreateMap(LevelIndex);
            currentLevel = map.mapName;
            var prova = map.levelMap;

            BuildMap(prova);
        }

        private void ClearMap(object sender, RoutedEventArgs e)
        {
            ClearMapLyaout();
        }

        private void ClearMapLyaout()
        {
            MapGrid.RowDefinitions.Clear();
            MapGrid.ColumnDefinitions.Clear();
            MapGrid.Children.Clear();
        }

        // Metodo per costruire la griglia
        private void BuildMap(List<List<Tile>> tileMatrix)
        {
            // Imposta le righe e colonne della griglia
            ClearMapLyaout();

            for (int i = 0; i < tileMatrix.Count; i++)
            {
                MapGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int j = 0; j < tileMatrix[0].Count; j++)
            {
                MapGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Crea una copia della lista delle unità alleate
            var allayList = UnitFactory.units.Where(u => u.Type == UnitType.Allay).ToList();
            var enemyList = UnitFactory.units.Where(u => u.Type == UnitType.Enemy).ToList();

            // Aggiungi le Tile alla griglia
            for (int row = 0; row < tileMatrix.Count; row++)
            {
                for (int col = 0; col < tileMatrix[row].Count; col++)
                {
                    var tile = tileMatrix[row][col];

                    // Crea un pulsante per rappresentare la Tile
                    var button = new Button
                    {
                        Width = 48.5, // Dimensioni della cella
                        Height = 31.5,
                        BorderThickness = new Thickness(1),
                        Background = GetTileBrush(tile), // Colore basato sul tipo
                        Tag = tile // Associa il Tile al pulsante
                    };

                    // Crea un triangolo per rappresentare l'unità
                    if (tile.TileID <= 0 && allayList.Any() ||
                        tile.TileID < 0 && enemyList.Any())
                    {
                        var randomIndex = new Random().Next(allayList.Count);

                        Unit unit;
                        if (tile.TileID == 0)
                        {
                            unit = allayList[randomIndex];
                            allayList.RemoveAt(randomIndex); 
                        }
                        else
                        {
                            unit = enemyList[randomIndex];
                            enemyList.RemoveAt(randomIndex);
                        }

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

                        // Aggiungi il triangolo al contenuto del pulsante
                        var grid = new Grid();
                        grid.Children.Add(triangle);
                        button.Content = grid;

                        tile.UnitOn = unit;
                    }

                    // Aggiungi il gestore dell'evento Click
                    button.Click += TileButton_Click;

                    // Posiziona il pulsante nella griglia
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);

                    // Aggiungi il pulsante alla griglia
                    MapGrid.Children.Add(button);
                }
            }
        }

        private void TileButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: Tile tile })
            {
                if (tile.UnitOn != null)
                {
                    GameSession.CurrentUnit = tile.UnitOn;
                    GameSession.ClassWeapons = string.Join("\n", GameSession.CurrentUnit.Class.UsableWeapons);
                }
                GameSession.CurrentTile = tile;
            }
        }

        private static Brush GetUnitColor(Unit unit)
        {
            if (unit.Type == UnitType.Allay)
            {
                Random random = new Random();
                byte variation = (byte)random.Next(0, 10); // Small variation
                return new SolidColorBrush(Color.FromRgb((byte)(255 - variation), (byte)(255 - variation), (byte)(255 - variation)));
            }
            return Brushes.Orange;
        }

        // Metodo per determinare il colore della Tile in base al tipo
        private Brush GetTileBrush(Tile tile)
        {
            return tile.TileName switch
            {
                "Plains" => Brushes.PaleGreen,
                "Mountains" => Brushes.DarkGray,
                "Waters" => Brushes.LightBlue,
                _ => Brushes.PaleGreen, // Default per tipi sconosciuti
            };
        }

        private void Previus_Level(object sender, RoutedEventArgs e)
        {
            if (LevelIndex - 1 > 0)
            {
                var tmp = LevelIndex;
                LevelIndex -= 1;

                var map = MapFactory.CreateMap(tmp - 1);
                currentLevel = map.mapName;
                OnPropertyChanged(nameof(currentLevel));
                BuildMap(map.levelMap);

                // Aggiorna visivamente il controllo
                MapGrid.InvalidateVisual();
                MapGrid.UpdateLayout();

            }
        }

        private void Next_Level(object sender, RoutedEventArgs e)
        {
            if (!(LevelIndex + 1 > MapFactory._allMaps.Count))
            {
                var tmp = LevelIndex;
                LevelIndex += 1;

                var map = MapFactory.CreateMap(tmp + 1);
                currentLevel = map.mapName;
                OnPropertyChanged(nameof(currentLevel));
                BuildMap(map.levelMap);

                // Aggiorna visivamente il controllo
                MapGrid.InvalidateVisual();
                MapGrid.UpdateLayout();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

