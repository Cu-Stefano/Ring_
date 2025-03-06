using Engine.FEMap;
using Engine.Models;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfUI.TurnLogic.Actions;

namespace WpfUI.TurnLogic;

public class EnemyTurn(MapLogic turnMapLogic) : TurnState(turnMapLogic)
{
    public override void OnEnter()
    {
        //_mapBuilder.EnemyButtonList; 
        // No specific logic for entering EnemyTurn
    }
    public override void OnExit()
    {
    }

    public override void SetState(ActionState action)
    {
    }

    public override void Doule_Click(object sender, RoutedEventArgs e)
    {
    }

    public override void Single_Click(object sender, RoutedEventArgs e)
    {
    }
}