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
using WpfUI.TurnLogic;
using WpfUI.TurnLogic.Actions;

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

        public MapLogic MapLogic { get; }

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
            MapLogic = new MapLogic(this);

            MouseDown += Window_MouseDown;

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

                    tile.UnitOn = null;// !se no si bugga quando cambi livello e le unità che ho spostato prima rimangono in memoria!  

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

                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    ActualMap[row][col] = button;//aggiungo prima il button alla mia matrice per poterla poi accedere facilmente e modificarla in futuro

                    MapGrid.Children.Add(ActualMap[row][col]);//aggiungo il button all'effettiva MapGrid di Xaml
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
                CurrentSelectedTile = null;
                ClearGamesessionGui();
                BuildMap(map.levelMap);
                MapLogic.SetState(new AllayTurn(MapLogic));

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
                CurrentSelectedTile = null;
                ClearGamesessionGui();
                
                BuildMap(map.levelMap);
                MapLogic.SetState(new AllayTurn(MapLogic));

                // Aggiorna visivamente il controllo
                MapGrid.InvalidateVisual();
                MapGrid.UpdateLayout();
            }
        }

        private void ChangeTurn(object sender, RoutedEventArgs e)
        {
            switch (MapLogic.CurrentTurnState)
            {
                case AllayTurn:
                    MapLogic.SetState(new EnemyTurn(MapLogic));
                    break;
                case EnemyTurn:
                    MapLogic.SetState(new AllayTurn(MapLogic));
                    break;
            }
        }

        public void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MapLogic.CurrentTurnState.Window_MouseDown(sender, e);
        }

        public void ClearGamesessionGui()
        {
            if (CurrentSelectedTile != null)
            {
                var currentSelectedTileButton = MapCosmetics.GetButtonBasedOnTile(CurrentSelectedTile);
                MapCosmetics.TileDeSelected(currentSelectedTileButton!);
                CurrentSelectedTile = null;
            }
           
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

