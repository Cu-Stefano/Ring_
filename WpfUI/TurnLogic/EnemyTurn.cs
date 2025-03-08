using Engine.FEMap;
using Engine.Models;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfUI.TurnLogic.Actions;

namespace WpfUI.TurnLogic;

public class EnemyTurn(MapLogic turnMapLogic) : TurnState(turnMapLogic)
{
    public override async void OnEnter()
    {
        // Introduce a delay of 5 seconds
        await Task.Delay(100);
        _turnMapLogic.SetState(new AllayTurn(_turnMapLogic));
        // Logica specifica per entrare in EnemyTurn (se necessaria)
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