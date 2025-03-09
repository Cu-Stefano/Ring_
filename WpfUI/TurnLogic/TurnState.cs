using Engine.FEMap;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfUI.TurnLogic.Actions;
using WpfUI.ViewModels;

namespace WpfUI.TurnLogic
{
    public abstract class TurnState
    {
        internal readonly MapLogic _turnMapLogic;
        protected readonly MapBuilder _mapBuilder;
        protected readonly GameSession _gameSession;
        protected readonly MapCosmetics _mapCosmetics;

        public ActionState CurrentActionState { get; set; }

        protected TurnState(MapLogic turnMapLogic)
        {
            _turnMapLogic = turnMapLogic;
            _mapBuilder = _turnMapLogic.MapBuilder;
            _gameSession = _mapBuilder.GameSession;
            _mapCosmetics = _mapBuilder.MapCosmetics;
        }

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void SetState(ActionState action);
        public void TileButton_Over(object sender, RoutedEventArgs e)
        {
            if (CurrentActionState is ChooseAttack or Attack) 
                return;
            if (_mapBuilder.CurrentSelectedTile != null)
            {
                _gameSession.CurrentTile = _mapBuilder.CurrentSelectedTile;
                _gameSession.CurrentUnit = _mapBuilder.MovingUnit!;
                _gameSession.ClassWeapons = string.Join("\n", _gameSession.CurrentUnit.Class.UsableWeapons);
            }
            else if (sender is Button { Tag: Tile tile })
            {
                _gameSession.CurrentUnit = tile.UnitOn;
                _gameSession.ClassWeapons = tile.UnitOn != null ? string.Join("\n", _gameSession.CurrentUnit!.Class.UsableWeapons) : "";
                _gameSession.CurrentTile = tile;
            }
            CurrentActionState?.Mouse_Over(sender, e);
        }
        public abstract void Doule_Click(object sender, RoutedEventArgs e);

        public abstract void Single_Click(object sender, RoutedEventArgs e);
    }
}