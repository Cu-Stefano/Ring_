using Engine.FEMap;
using Engine.Models;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfUI.TurnLogic;

public class EnemyTurn(MapLogic turnMapLogic) : TurnState(turnMapLogic)
{
    public override void OnEnter()
    {
        // No specific logic for entering EnemyTurn
    }
    public override void OnExit()
    {
        // No specific logic for exiting EnemyTurn
    }

    public override void TileButton_Over(object sender, RoutedEventArgs e)
    {
    }

    public override void UnitSelected(object sender, RoutedEventArgs e)
    {
    }

    public override void Move_unit(object sender, RoutedEventArgs e)
    {
    }
}