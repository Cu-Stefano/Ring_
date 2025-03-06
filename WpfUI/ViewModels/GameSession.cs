using System.Windows.Controls;
using System.Windows.Media;
using Engine;
using Engine.Factories;
using Engine.FEMap;
using Engine.Models;
using WpfUI.TurnLogic.Actions;

// ReSharper disable All

namespace WpfUI.ViewModels
{
    public class GameSession : BaseNotification
    {
        public PreviewAttack PreviewAttack { get; set; }
        internal DataGrid? DataGrid { get; set; }
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


        public GameSession(MainWindow mainWindow, PreviewAttack previewAttack)
        {
            PreviewAttack = previewAttack;
        }

        public void HighlightSelectedRow()
        {
            if (DataGrid == null) return;

            // Force the DataGrid to update its layout
            DataGrid.UpdateLayout();

            foreach (var item in DataGrid.Items)
            {
                // Ensure the row is generated
                DataGridRow row = (DataGridRow)DataGrid.ItemContainerGenerator.ContainerFromItem(item);

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
            OnPropertyChanged(nameof(CurrentUnit));
        }

    }

}

