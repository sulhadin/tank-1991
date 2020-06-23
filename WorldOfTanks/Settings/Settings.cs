using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorldOfTanks.Repository;

namespace WorldOfTanks.Settings
{
    public static class Settings
    {
        public static int CharacterStepSize = 10;
        public static int BulletStepSize = 20;
        public static int SensorStepSize = 20;
        public static int BulletTimerInterval = 20;
        public static int SensorTimerInterval = 1;
        public static int EnemyTimerInterval = 100;
        public static int EnemyRefreshPathInterval = 5000;
        public static bool IsGameOver = false;
        public static short PlayerColor = 1;
        public static short EnemyColor = 2;

        public static Point GetRandomAvailableLocationOnMap()
        {
            var random = new Random();
            tryagain:
            var y = random.Next(0, GameForm.AreaRow);
            var x = random.Next(0, GameForm.AreaColumn);

            if (GameForm.Mapping[y, x].IsRock ||
                GameForm.Mapping[y, x].HasSource ||
                GameForm.Mapping[y, x].HasTarget ||
                GameForm.Mapping[y, x].Destroyable ||
                GameForm.Mapping[y, x].HasEnemyEagle ||
                GameForm.Mapping[y, x].HasPlayerEagle)
            {
                goto tryagain;
            }

            return GameForm.Mapping[y, x].Location + (Size)new Point(10, 10);
        }
        public static Image GetTankImage(short color, CharacterState cState)
        {
            return Image.FromFile(Path.Combine(Application.StartupPath, @"GameObjects\Tank\" + color + "Tank" + cState + ".png"));
        }
        public static Image GetBirck(short color)
        {
            return Image.FromFile(Path.Combine(Application.StartupPath, @"GameObjects\Brick\Rock" + color + ".jpg"));
        }
        public static Image GetEagle(char type)
        {
            return Image.FromFile(Path.Combine(Application.StartupPath, @"GameObjects\Eagle\" + type + "Eagle.jpg"));
        }
        public static void MessageInfo(string message)
        {
            MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void MessageError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
