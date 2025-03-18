using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Engine.FEMap;
using Engine.Models;

namespace WpfUI.TurnLogic.Actions
{
    public static class AttackExtensions
    {
        public static async Task AttackCalculations(this Button UnitA, Button UnitB, int damageA, int damageB, MapCosmetics mapCosmetics)
        {
            if (UnitA.Tag is not Tile tileA || tileA.UnitOn == null || UnitB.Tag is not Tile tileB || tileB.UnitOn == null)
                throw new ArgumentException("buttons not compatible");

            var enemyHpLeft = tileB.UnitOn.Statistics.Hp -= damageA;
            TakeDamage(UnitB, mapCosmetics);

            if (enemyHpLeft <= 0)
            {

                if (tileA.UnitOn.Type == UnitType.Enemy)
                    MapBuilder.AllayButtonList.Remove(UnitB);
                else
                    MapBuilder.EnemyButtonList.Remove(UnitB);

                tileB.UnitOn = null;
                UnitB.Content = null;

                return;
            }

            await Task.Delay(600);
            if (damageB != 0)
            {
                var allayHpLeft = tileA.UnitOn.Statistics.Hp -= damageB;
                TakeDamage(UnitA, mapCosmetics);

                if (allayHpLeft <= 0)
                {
                    if (tileA.UnitOn.Type == UnitType.Enemy)
                        MapBuilder.EnemyButtonList.Remove(UnitA);
                    else
                        MapBuilder.AllayButtonList.Remove(UnitA);

                    tileA.UnitOn = null;
                    UnitA.Content = null;

                    return;
                }
            }

            await Task.Delay(800);
        }

        private static void TakeDamage(Button unitButton, MapCosmetics mapCosmetics)
        {
            var originalBrush = MapCosmetics.GetTileBrush((Tile)unitButton.Tag) as SolidColorBrush;
            if (originalBrush == null)
                return;

            var animation = new ColorAnimation
            {
                From = Colors.Red,
                To = originalBrush.Color,
                Duration = new Duration(TimeSpan.FromSeconds(0.5)),
            };

            var brush = new SolidColorBrush(Colors.Red);
            unitButton.Background = brush;
            brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }
    }
}
