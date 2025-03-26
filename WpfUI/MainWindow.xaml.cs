using System.Windows;
using System.Windows.Controls;
using Engine.Models;
using WpfUI.TurnLogic.Actions;
using WpfUI.ViewModels;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        private readonly GameSession _gameSession;
        public MainWindow()
        {
            InitializeComponent();
            //FullScreen on start
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
                this.ResizeMode = ResizeMode.NoResize;
            }

            var previewAttack = new PreviewAttack();
            PreviewAttack.Content = previewAttack;

            _gameSession = new GameSession(this, previewAttack);
            DataContext = _gameSession;

            var mapBuilder = new MapBuilder(_gameSession);
            MapBuilderPlaceholder.Content = mapBuilder;
            previewAttack.MapBuilder = mapBuilder;

        }

        private void MapBuilder_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var selectedItem = dataGrid.SelectedItem;

            if (selectedItem is Weapon item 
                && _gameSession.CurrentUnit!.Class.UsableWeapons.Contains(item.WeaponType) 
                && _gameSession.CurrentUnit.Type == UnitType.Allay 
                && _gameSession.CurrentUnit.CanMove)
            { 
                _gameSession.CurrentUnit!.EquipedWeapon = item; 
                _gameSession._previewAttack.Start();
            }
            HighlightSelectedRow();
        }

        private void HighlightSelectedRow()
        {
            _gameSession.HighlightSelectedRow();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (_gameSession.DataGrid != null)
            {
                HighlightSelectedRow();
            }
            else
            {
                _gameSession.DataGrid = (DataGrid)sender;
            }
        }

    }
}