using System.ComponentModel;
using System.Windows.Controls;
using Engine.FEMap;
using Engine.Models;

namespace WpfUI.Utilities;

public static class MoveUnit
{

    public static void Move_Unit(Button buttA, Button buttB)
    {
        var tileA = buttA.GetTile();
        var tileB = buttB.GetTile();

        var unit = tileA.UnitOn;

        buttB.Content = buttA.Content;
        buttA.Content = null;

        tileA.UnitOn = null;
        tileB.UnitOn = unit;

        //aggiorno le AllayList o EnemyList
        switch (unit.Type)
        {
            case UnitType.Allay:
               
                if (!MapBuilder.AllayButtonList.Remove(buttA))
                    throw new InvalidEnumArgumentException("Error in removing the button from the list");

                MapBuilder.AllayButtonList.Add(buttB);
                break;
            case UnitType.Enemy:
                MapBuilder.EnemyButtonList.Remove(buttA);
                MapBuilder.EnemyButtonList.Add(buttB);
                break;
            default:
                break;
        }
    }
}