using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldOfTanks.Tools
{
    public class GameOver
    {
        private readonly GameForm _gameForm;

        public GameOver(GameForm gameForm)
        {
            this._gameForm = gameForm;
        }

        internal void Show()
        {
            var go = new PictureBox()
            {
                Width = 300,
                Height = 150,
                Image = Properties.Resources.gameover,
                SizeMode =  PictureBoxSizeMode.StretchImage

            };
            go.Location = new Point(_gameForm.Width / 2 - go.Width / 2, _gameForm.Height / 2 - go.Height / 2);

            _gameForm.Controls.Add(go);
            go.BringToFront();
        }

        public void ShowYouWin()
        {
            var go = new PictureBox()
            {
                Width = 300,
                Height = 150,
                Image = Properties.Resources.gameover,
                SizeMode = PictureBoxSizeMode.StretchImage

            };
            go.Location = new Point(_gameForm.Width / 2 - go.Width / 2, _gameForm.Height / 2 - go.Height / 2);

            _gameForm.Controls.Add(go);
            go.BringToFront();
        }

        public void ShowYouLose()
        {
            var go = new PictureBox()
            {
                Width = 300,
                Height = 150,
                Image = Properties.Resources.gameover,
                SizeMode = PictureBoxSizeMode.StretchImage

            };
            go.Location = new Point(_gameForm.Width / 2 - go.Width / 2, _gameForm.Height / 2 - go.Height / 2);

            _gameForm.Controls.Add(go);
            go.BringToFront();
        }
    }
}
