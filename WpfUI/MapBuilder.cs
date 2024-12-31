using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Engine.Models;
using Engine.FEMap;
using System.Windows;
using Engine.ViewModels;
using System.ComponentModel;

namespace WpfUI
{
    public partial class MapBuilder : INotifyPropertyChanged
    {
        private int LevelIndex { get; set; }
        public string currentLevel { get; set; }

        public MapBuilder()
        {
            InitializeComponent();
            DataContext = this;
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

            // Aggiungi le Tile alla griglia
            for (int row = 0; row < tileMatrix.Count; row++)
            {
                for (int col = 0; col < tileMatrix[row].Count; col++)
                {
                    var tile = tileMatrix[row][col];

                    // Crea un rettangolo per rappresentare la Tile
                    var rectangle = new Rectangle
                    {
                        Width = 44.5, // Dimensioni della cella
                        Height = 27,
                        Fill = GetTileBrush(tile), // Colore basato sul tipo
                    };

                    // Posiziona il rettangolo nella griglia
                    Grid.SetRow(rectangle, row);
                    Grid.SetColumn(rectangle, col);

                    // Aggiungi il rettangolo alla griglia
                    MapGrid.Children.Add(rectangle);
                }
            }
        }

        // Metodo per determinare il colore della Tile in base al tipo
        private Brush GetTileBrush(Tile tile)
        {
            return tile.TileName switch
            {
                "Plains" => Brushes.PaleGreen,
                "Mountains" => Brushes.DarkGray,
                "Waters" => Brushes.LightBlue,
                _ => Brushes.White, // Default per tipi sconosciuti
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