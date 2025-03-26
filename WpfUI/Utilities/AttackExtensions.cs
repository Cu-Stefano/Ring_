using System;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Engine.FEMap;
using Engine.Models;
using WpfUI.Utilities;

namespace WpfUI.TurnLogic.Actions
{
    public static class AttackExtensions
    {
        private static readonly MediaPlayer hitPlayer = new MediaPlayer();
        private static readonly MediaPlayer noDamagePlayer = new MediaPlayer();
        private static readonly MediaPlayer missPlayer = new MediaPlayer();
        private static readonly MediaPlayer critPlayer = new MediaPlayer();

        static AttackExtensions()
        {
            InitializeMediaPlayer(hitPlayer, "./sounds/hit.wav");
            InitializeMediaPlayer(noDamagePlayer, "./sounds/no_damage.wav");
            InitializeMediaPlayer(missPlayer, "./sounds/miss.wav");
            InitializeMediaPlayer(critPlayer, "./sounds/crit.wav");
        }

        private static void InitializeMediaPlayer(MediaPlayer player, string filePath)
        {
            player.Open(new Uri(filePath, UriKind.Relative));
            player.Volume = 0.3;
        }

        public static async Task AttackCalculations(this Button unitA, MapCosmetics mapCosmetics,
            PreviewAttack previewAttack, MapBuilder _mapBuilder)
        {
            var unitB = previewAttack.EnemyButton;
            if (unitA.Tag is not Tile tileA || tileA.UnitOn == null || unitB!.Tag is not Tile tileB || tileB.UnitOn == null)
                throw new ArgumentException("buttons not compatible");

            var random = new Random();

            await Task.Delay(200);
            var res = await ExecuteAttack(unitB, tileB, previewAttack, mapCosmetics, random, true);
            if (res == 0)
            {
                await Task.Delay(1000);
                res = await ExecuteAttack(unitA, tileA, previewAttack, mapCosmetics, random, false);
                if (res == 0)
                    _mapBuilder.UnitCantMoveNoMore(unitA);
                return;
            }
            await Task.Delay(1500);
        }

        private static async Task<int> ExecuteAttack(Button attacked, Tile defenderTile, PreviewAttack previewAttack, MapCosmetics mapCosmetics, Random random, bool isAllay)
        {
            var damage = isAllay ? previewAttack.AllayDamage : previewAttack.EnemyDamage;
            var hit = isAllay ? previewAttack.AllayHit : previewAttack.EnemyHit;
            var crit = isAllay ? previewAttack.AllayCrit : previewAttack.EnemyCrit;

            if (damage == 0)
            {
                TakeDamage(attacked, mapCosmetics);
                PlaySound(noDamagePlayer);
                await Task.Delay(500);
                return 0;
            }

            var hitSuccess = random.Next(0, 100) <= hit;
            if (hitSuccess)
            {
                var critSuccess = random.Next(0, 100) <= crit;
                if (critSuccess)
                {
                    damage *= 3;
                    PlaySound(critPlayer);
                }
                else
                {
                    PlaySound(hitPlayer);
                }

                var defenderHpLeft = defenderTile.UnitOn.Statistics.Hp -= damage;
                TakeDamage(attacked, mapCosmetics);
                if (isAllay)
                {
                    previewAttack.AnimateEnemyHPbar();
                    previewAttack.EnemyHp = previewAttack.EnemyUnit.Statistics.Hp;
                }
                else
                {
                    previewAttack.AnimateAllayHPbar();
                    previewAttack.AllayHp = previewAttack.AllayUnit.Statistics.Hp;
                }
                await Task.Delay(500);

                if (defenderHpLeft <= 0)
                {
                    RemoveUnit(defenderTile, attacked, isAllay);
                    await Task.Delay(1200);
                    return -1;
                }
            }
            else
            {
                TakeDamage(attacked, mapCosmetics);
                PlaySound(missPlayer);
                await Task.Delay(500);
            }

            return 0;
        }

        private static void PlaySound(MediaPlayer player)
        {
            player.Position = TimeSpan.Zero;
            player.Play();
        }

        private static void RemoveUnit(Tile defenderTile, Button attacker, bool isAllay)
        {
            if (isAllay)
                MapBuilder.AllayButtonList.Remove(attacker);
            else
                MapBuilder.EnemyButtonList.Remove(attacker);

            defenderTile.UnitOn = null;
            attacker.Content = null;
        }

        private static void TakeDamage(Button unitButton, MapCosmetics mapCosmetics)
        {
            var originalBrush = MapCosmetics.GetTileBrush(unitButton.GetTile()) as SolidColorBrush;
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
