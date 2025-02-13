using Engine.ViewModels;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfUI.TurnLogic;

public class AllayTurn(MapLogic turnMapLogic) : TurnState(turnMapLogic)
{
    public override async void OnEnter()
    {
        foreach (var button in TurnMapLogic.MapBuilder.ActualMap.SelectMany(row => row))
        {
            button.MouseEnter += TurnMapLogic.TileButton_Over;
            button.Click += TurnMapLogic.Move_unit;
            button.MouseDoubleClick += TurnMapLogic.UnitSelected;
        }
    }
    public override void OnExit()
    {
        TurnMapLogic.MapBuilder.ClearGamesessionGui();
    }
}