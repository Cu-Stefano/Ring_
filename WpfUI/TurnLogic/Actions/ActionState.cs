using System.Windows;

namespace WpfUI.TurnLogic.Actions;

public abstract class ActionState(MapLogic turnMapLogic){
    protected MapLogic TurnMapLogic { get; } = turnMapLogic;
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void CalculateTrail(object sender, RoutedEventArgs e);
    public abstract void Move(object sender, RoutedEventArgs e);
}