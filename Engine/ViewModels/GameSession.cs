using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Engine.Exceptions;
using Engine.Factories;
using Engine.FEMap;
using Engine.Models;
// ReSharper disable All

namespace Engine.ViewModels
{
    public class GameSession : BaseNotification
    {
        public WorldMap CurrentWorldMap { get; set; }//SOSCRPG map not mine
        private Location _currentLocation = null!;
        private Tile _currenTile = null!;

        private Unit _currentUnit;
        public List<Unit> AllayList{ get; set; }
     
        public Location CurrentLocation
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

        public Unit CurrentUnit
        {
            get { return _currentUnit; }
            set
            {
                _currentUnit = value;
                OnPropertyChanged(nameof(CurrentUnit));
            }
        }

        public Tile CurrentTile
        {
            get { return _currenTile; }
            set
            {
                _currenTile = value;
                OnPropertyChanged(nameof(CurrentTile));
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

        public GameSession()
        {
            CurrentUnit = UnitFactory.GetUnitByName("Ike");

            CurrentWorldMap = WorldFactory.CreateWorld();
            CurrentLocation = CurrentWorldMap.LocationAt(0, 0)!;

            

            CurrentUnit.Inventory.Add(ItemFactory.CreateGameItem(100));
            CurrentUnit.Inventory.Add(ItemFactory.CreateGameItem(101));
            CurrentUnit.Inventory.Add(ItemFactory.CreateGameItem(200));
            CurrentUnit.EquipedWeapon = CurrentUnit.Inventory[0] as Weapon;

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
    }

}

