using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Engine.Models;
using Engine.Factories;
using Engine.FEMap;
using System.Windows;
using Engine.ViewModels;
using System.ComponentModel;
using System.Windows.Input;

namespace WpfUI
{
    public partial class MapBuilder : INotifyPropertyChanged
    {
        private int LevelIndex { get; set; }
        public string CurrentLevelName { get; set; }
        public List<List<Button>> ActualMap { get; set; }
        public Tile? CurrentSelectedTile { get; set; }
        public Unit? MovingUnit
        {
            get => CurrentSelectedTile?.UnitOn; 
            set => CurrentSelectedTile!.UnitOn = value;
        }

        public GameSession GameSession { get; set; }

        public MapCosmetics MapCosmetics { get; }

        public MapBuilder(GameSession gameSession)
        {
            InitializeComponent();
            DataContext = this;
            GameSession = gameSession;
            MapCosmetics = new MapCosmetics(this);//methods to change the appearance of the map

            LevelIndex = 1;

            var map = MapFactory.CreateMap(LevelIndex);
            CurrentLevelName = map.mapName;
            var levelMap = map.levelMap;

            InitializeMap(levelMap);
            
            BuildMap(levelMap);
        }

        private void InitializeMap(List<List<Tile>> levelTileMap)
        {
            ActualMap = new List<List<Button>>();

            foreach (var t in levelTileMap)
            {
                var row = new List<Button>();
                for (int j = 0; j < t.Count; j++)
                {
                    row.Add(null);
                }
                ActualMap.Add(row);
            }
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
            ClearMapLyaout();

            for (int i = 0; i < tileMatrix.Count; i++)
            {
                MapGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int j = 0; j < tileMatrix[0].Count; j++)
            {
                MapGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Crea una copia della lista di tutte le unità allay e nemiche che ho
            var allayList = UnitFactory.units.Where(u => u.Type == UnitType.Allay).ToList();
            var enemyList = UnitFactory.units.Where(u => u.Type == UnitType.Enemy).ToList();

            for (int row = 0; row < tileMatrix.Count; row++)
            {
                for (int col = 0; col < tileMatrix[row].Count; col++)
                {
                    var tile = tileMatrix[row][col];

                    // Crea un pulsante per poter interagire col tile
                    var button = new Button
                    {
                        Width = 48.5, // Dimensioni della cella
                        Height = 31.5,
                        BorderThickness = new Thickness(1),
                        Background = MapCosmetics.GetTileBrush(tile), // Colore basato sul tipo
                        Tag = tile // Associa il Tile al pulsante
                    };

                    if (tile.TileID <= 0 && allayList.Any() ||
                        tile.TileID < 0 && enemyList.Any())
                    {
                        // Scegli un'unità casuale dalla lista da mettere dove deve andare in base a tileMatrix
                        var allayRandomIndex = new Random().Next(allayList.Count);
                        var enemyRandomIndex = new Random().Next(enemyList.Count);

                        //inizializzo l'unità
                        Unit unit;
                        if (tile.TileID == 0)
                        {
                            unit = allayList[allayRandomIndex];
                            allayList.RemoveAt(allayRandomIndex); 
                        }
                        else
                        {
                            unit = enemyList[enemyRandomIndex];
                            enemyList.RemoveAt(enemyRandomIndex);
                        }
                        var triangle = MapCosmetics.Triangle(unit);

                        // aggiungo l'unità al tile
                        button.Content = triangle;
                        tile.UnitOn = unit;
                    }

                    // Aggiungi i gestori d'eventi
                    button.MouseEnter += TileButton_Over;
                    button.Click += Move_unit;
                    button.MouseDoubleClick += UnitSelected;
                    MouseDown += Window_MouseDown;

                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    ActualMap[row][col] = button;//aggiungo prima il button alla mia matrice per poterla poi accedere facilmente e modificarla in futuro

                    MapGrid.Children.Add(ActualMap[row][col]);//aggiungo il button all'effettiva MapGrid di Xaml
                }
            }
        }

        
        private void TileButton_Over(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectedTile != null)
            {
                GameSession.CurrentTile = CurrentSelectedTile;
                GameSession.CurrentUnit = MovingUnit!;
                GameSession.ClassWeapons = string.Join("\n", GameSession.CurrentUnit.Class.UsableWeapons);
            }
            else if (sender is Button { Tag: Tile tile })
            {
                GameSession.CurrentUnit = tile.UnitOn;
                GameSession.ClassWeapons = tile.UnitOn != null ? string.Join("\n", GameSession.CurrentUnit!.Class.UsableWeapons) : "";
                GameSession.CurrentTile = tile;
            }
        }

        private void UnitSelected(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: Tile { UnitOn: not null } tile } button)
            {
                if (CurrentSelectedTile != null)
                {
                    var selectedButton = MapCosmetics.GetButtonBasedOnTile(CurrentSelectedTile);
                    MapCosmetics.TileDeSelected(selectedButton!);
                    CurrentSelectedTile = null;
                }

                MapCosmetics.TileSelected(button);
                CurrentSelectedTile = tile;
                MovingUnit = tile.UnitOn;
            }
        }

        private void Move_unit(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: Tile { UnitOn: null, Walkable: true } tile } button)
            {
                if (CurrentSelectedTile is { UnitOn: not null } && CurrentSelectedTile != tile)
                {
                    tile.UnitOn = MovingUnit;//sposto l'unità

                    // Deseleziona la Tile dell'unità che si vuole spostare
                    var currentSelectedTileButton = MapCosmetics.GetButtonBasedOnTile(CurrentSelectedTile)!;
                    MapCosmetics.TileDeSelected(currentSelectedTileButton);

                    button.Content = currentSelectedTileButton.Content;//copio il tipo/colore dell'unità
                    GameSession.CurrentTile = tile;
                    GameSession.CurrentUnit = tile.UnitOn;

                    currentSelectedTileButton.Content = null;
                    MovingUnit = null;
                    CurrentSelectedTile = null;


                    GameSession.ClassWeapons = string.Join("\n", GameSession.CurrentUnit!.Class.UsableWeapons);
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Verifica se l'elemento cliccato è un pulsante con un'unità selezionata
            if (e.OriginalSource is not Button { Tag: Tile })
            {
                // Deseleziona l'unità corrente e ripristina il bordo del pulsante
                if (CurrentSelectedTile != null)
                {
                    var currentSelectedTileButton = MapCosmetics.GetButtonBasedOnTile(CurrentSelectedTile);

                    MapCosmetics.TileDeSelected(currentSelectedTileButton!);

                    CurrentSelectedTile = null;
                    GameSession.CurrentTile = null;
                    GameSession.CurrentUnit = null;
                    GameSession.ClassWeapons = string.Empty;
                }
            }
        }



        private void Previus_Level(object sender, RoutedEventArgs e)
        {
            if (LevelIndex - 1 > 0)
            {
                var tmp = LevelIndex;
                LevelIndex -= 1;

                var map = MapFactory.CreateMap(tmp - 1);
                CurrentLevelName = map.mapName;
                OnPropertyChanged(nameof(CurrentLevelName));
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
                CurrentLevelName = map.mapName;
                OnPropertyChanged(nameof(CurrentLevelName));
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

