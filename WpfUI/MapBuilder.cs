using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Engine.Models;
using Engine.Factories;
using Engine.FEMap;
using System.Windows;
using System.ComponentModel;
using System.Windows.Input;
using WpfUI.TurnLogic;
using WpfUI.TurnLogic.Actions;
using WpfUI.ViewModels;
using System.Reactive;
using Unit = Engine.Models.Unit;

namespace WpfUI
{
    public partial class MapBuilder : INotifyPropertyChanged
    {
        private int LevelIndex { get; set; }
        public string CurrentLevelName { get; set; }
        public List<List<Button?>> ActualMap { get; set; }
        public Tile? CurrentSelectedTile { get; set; }
        public Unit? MovingUnit
        {
            get => CurrentSelectedTile?.UnitOn; 
            set {}
        }

        public GameSession GameSession { get; set; }

        public MapCosmetics MapCosmetics { get; }

        public MapLogic MapLogic { get; }

        public List<Unit> AllayList
        {
            get
            {
                var allayList = UnitFactory.units.Where(u => u.Type == UnitType.Allay).ToList();
                return allayList;
            }
        }
        public List<Unit> EnemyList
        {
            get
            {
                var enemyList = UnitFactory.units.Where(u => u.Type == UnitType.Enemy).ToList();
                return enemyList;
            }
        }

        public static List<Button?> AllayButtonList { get; set; }

        public static List<Button?> EnemyButtonList { get; set; }


        public MapBuilder(GameSession gameSession)
        {
            InitializeComponent();
            DataContext = this;
            GameSession = gameSession;
            MapCosmetics = new MapCosmetics();//methods to change the appearance of the map
            

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
            ActualMap = levelTileMap
                .Select(t => Enumerable.Repeat<Button>(null, t.Count).ToList())
                .ToList();
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
            var allayList = AllayList;
            var enemyList = EnemyList;
            AllayButtonList = new List<Button?>();
            EnemyButtonList = new List<Button?>();

            for (int row = 0; row < tileMatrix.Count; row++)
            {
                for (int col = 0; col < tileMatrix[0].Count; col++)
                {
                    var tile = tileMatrix[row][col];

                    // Crea un pulsante per poter interagire col tile
                    var button = new Button
                    {
                        Width = 48.5, 
                        Height = 31.5,
                        BorderThickness = new Thickness(1),
                        Background = MapCosmetics.GetTileBrush(tile), 
                        Tag = tile
                    };

                    tile.UnitOn = null;// !se no lo fai, si bugga, quando cambi livello e le unità che ho spostato prima rimangono in memoria!  

                    if (tile.TileID == 0 && allayList.Any() ||
                        tile.TileID == -1 && enemyList.Any())
                    {
                        // Scegli un'unità casuale dalla lista da mettere dove deve andare in base a tileMatrix
                        var allayRandomIndex = new Random().Next(allayList.Count);
                        var enemyRandomIndex = new Random().Next(enemyList.Count);

                        Unit unit;
                        if (tile.TileID == 0)
                        {
                            unit = allayList[allayRandomIndex];
                            allayList.RemoveAt(allayRandomIndex);
                            AllayButtonList.Add(button);
                        }
                        else
                        {
                            unit = enemyList[enemyRandomIndex];
                            enemyList.RemoveAt(enemyRandomIndex);
                            EnemyButtonList.Add(button);
                        }
                        var triangle = MapCosmetics.GetPolygon(unit);

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
            MapLogic.CurrentTurnState.Back_Action(sender, e);
        }

        public void ClearGamesessionGui()
        {
            if (CurrentSelectedTile != null)
            {
                var currentSelectedTileButton = GetButtonBasedOnTile(CurrentSelectedTile);
                MapCosmetics.SetButtonAsDeselected(currentSelectedTileButton!);
                CurrentSelectedTile = null;
            }
           
        }

        public void UnitCantMoveNoMore(Button? button)
        {
            //unit can't move till next turn
            var buttUnit = ((Tile)button.Tag).UnitOn;
            buttUnit.CanMove = false;
            button.Content = MapCosmetics.GetPolygon(buttUnit);
            OnPropertyChanged("button");
        }
        public Button? GetButtonBasedOnTile(Tile tile)
        {
            return ActualMap.SelectMany(row => row).FirstOrDefault(b => b?.Tag == tile);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public (int x, int y) GetButtonPosition(Button button)
        {
            for (int row = 0; row < ActualMap.Count; row++)
            {
                for (int col = 0; col < ActualMap[row].Count; col++)
                {
                    if (ActualMap[row][col] == button)
                    {
                        return (row, col);
                    }
                }
            }
            throw new ArgumentException("Button not found in the map.");
        }
    }
}

