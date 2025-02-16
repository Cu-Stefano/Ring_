using Engine.FEMap;
using Engine.Models;
using Engine.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfUI.TurnLogic.Actions;

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

    public override void SetState(ActionState action)
    {
    }

    public override void UnitSelected(object sender, RoutedEventArgs e)
    {
    }

    public override void Move_unit(object sender, RoutedEventArgs e)
    {
    }
}