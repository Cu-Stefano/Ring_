using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfUI.TurnLogic;

public class EnemyTurn(MapLogic turnMapLogic) : TurnState(turnMapLogic)
{
    public override async void OnEnter()
    {
        foreach (var button in TurnMapLogic.MapBuilder.ActualMap.SelectMany(row => row))
        {
            button.Click -= TurnMapLogic.Move_unit;
            button.MouseDoubleClick -= TurnMapLogic.UnitSelected;
        }
    }
    public override void OnExit()
    {
        // No specific logic for exiting EnemyTurn
    }
}