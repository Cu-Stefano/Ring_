using System.Windows;

namespace WpfUI.TurnLogic
{
    public abstract class TurnState(MapLogic turnMapLogic)
    {
        protected MapLogic TurnMapLogic { get; } = turnMapLogic;

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void TileButton_Over(object sender, RoutedEventArgs e);

        public abstract void UnitSelected(object sender, RoutedEventArgs e);

        public abstract void Move_unit(object sender, RoutedEventArgs e);
    }
}