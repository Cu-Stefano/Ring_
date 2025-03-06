using System;
using System.Windows.Controls;
using Engine.FEMap;

namespace WpfUI.TurnLogic.Actions
{
    public static class AttackExtensions
    {
        public static int AttackCalculations(this Button UnitA, Button UnitB, int damageA, int damageB)
        {
            if (UnitA.Tag is not Tile tagA || tagA.UnitOn == null || UnitB.Tag is not Tile tagB || tagB.UnitOn == null)
                throw new ArgumentException("buttons not compatible");

            var enemyHpLeft = tagB.UnitOn.Statistics.Hp -= damageA;
            if (enemyHpLeft <= 0)
            {
                tagB.UnitOn = null;
                UnitB.Content = null;
                MapBuilder.EnemyButtonList.Remove(UnitB);
                return 1;
            }

            var allayHpLeft = tagA.UnitOn.Statistics.Hp -= damageB;
            if (allayHpLeft <= 0)
            {
                tagA.UnitOn = null;
                UnitA.Content = null;
                MapBuilder.AllayButtonList.Remove(UnitA);
                return 1;
            }

            return 0;
        }
    }
}
