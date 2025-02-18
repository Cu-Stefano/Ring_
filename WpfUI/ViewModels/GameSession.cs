using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using Engine;
using Engine.Exceptions;
using Engine.Factories;
using Engine.FEMap;
using Engine.Models;
// ReSharper disable All

namespace WpfUI.ViewModels
{
    public class GameSession : BaseNotification
    {
        public WorldMap CurrentWorldMap { get; set; }//SOSCRPG map not mine
        private Location? _currentLocation = null!;
        internal DataGrid dataGrid { get; set; }
        private Tile? _currenTile = null!;
        private string _classWeapons;
        private Unit? _currentUnit;
        private GameItem? _selectedInventoryWeapon;
        public GameItem? SelectedInventoryWeapon
        {
            get => _selectedInventoryWeapon;
            set
            {
                if (_selectedInventoryWeapon != value)
                {
                    _selectedInventoryWeapon = value;
                    OnPropertyChanged(nameof(SelectedInventoryWeapon));
                }
            }
        }
        public List<Unit> AllayList{ get; set; }
     
        public Location? CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(IsThereLocationUp));
                OnPropertyChanged(nameof(IsThereLocationDown));
                OnPropertyChanged(nameof(IsThereLocationLeft));
                OnPropertyChanged(nameof(IsThereLocationRight));
                GivePlayerQuestsAtLocation();
            }
        }

        public Unit? CurrentUnit
        {
            get {return _currentUnit; }
            set
            {
                HighlightSelectedRow();
                _currentUnit = value;
                OnPropertyChanged(nameof(CurrentUnit));
                HighlightSelectedRow();
            }
        }

        public Tile? CurrentTile
        {
            get { return _currenTile; }
            set
            {
                _currenTile = value;
                OnPropertyChanged(nameof(CurrentTile));
            }
        }

        public string ClassWeapons
        {
            get { return _classWeapons; }
            set
            {
                _classWeapons = value;
                OnPropertyChanged(nameof(ClassWeapons));
            }
        }

        public bool IsThereLocationUp
        {
            get
            {
                return CurrentWorldMap.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
            }
        }

        public bool IsThereLocationDown
        {
            get
            {
                return CurrentWorldMap.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
            }
        }

        public bool IsThereLocationLeft
        {
            get
            {
                return CurrentWorldMap.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
            }
        }

        public bool IsThereLocationRight
        {
            get
            {
                return CurrentWorldMap.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
            }
        }

        public GameSession(MainWindow mainWindow)
        {
            //CurrentUnit = UnitFactory.GetUnitByName("Ike");
            //ClassWeapons = string.Join("\n", CurrentUnit.Class.UsableWeapons);

            CurrentWorldMap = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorldMap.LocationAt(0, 0)!;//soscsrpg world



            //CurrentUnit.Inventory.Add(ItemFactory.CreateGameItem("BronzeSword"));
            //CurrentUnit.Inventory.Add(ItemFactory.CreateGameItem("IronSword"));
            //CurrentUnit.Inventory.Add(ItemFactory.CreateGameItem("WoodShield"));
            //CurrentUnit.EquipedWeapon = CurrentUnit.Inventory[0] as Weapon;

        }

        public void MoveUp()
        {
            if (IsThereLocationUp)
            {
                CurrentLocation =
                    CurrentWorldMap.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1)!;
            }
        }

        public void MoveDown()
        {
            if (IsThereLocationDown)
            {
                CurrentLocation =
                    CurrentWorldMap.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1)!;
            }
        }

        public void MoveLeft()
        {
            if (IsThereLocationLeft)
            {
                CurrentLocation =
                    CurrentWorldMap.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate)!;
            }
        }

        public void MoveRight()
        {
            if (IsThereLocationRight)
            {
                {
                    CurrentLocation =
                        CurrentWorldMap.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate)!;
                }
            }
        }
        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentUnit.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentUnit.Quests.Add(new QuestStatus(quest));
                }
            }
        }
        public void HighlightSelectedRow()
        {
            if (dataGrid == null) return;

            // Force the DataGrid to update its layout
            dataGrid.UpdateLayout();

            foreach (var item in dataGrid.Items)
            {
                // Ensure the row is generated
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(item);

                if (row != null)
                {
                    if (item == CurrentUnit!.EquipedWeapon)
                    {
                        row.Background = new SolidColorBrush(Colors.DodgerBlue);
                    }
                    else
                    {
                        row.Background = new SolidColorBrush(Colors.Transparent);
                    }
                }
            }
        }

    }

}

