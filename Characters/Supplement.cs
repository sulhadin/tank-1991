using System;

namespace WorldOfTanks.Characters
{
    using System.Windows.Forms;
    using Repository;
    using Settings;
    public class Supplement : PictureBox
    {
        public static int SupplySize = 30;
        public SupplyType SupplyType { get; set; }
        public int Amount { get; set; }
        public Panel GameArea { get; set; }

        public Supplement()
        {
            this.Height = SupplySize;
            this.Width = SupplySize;
            SizeMode = PictureBoxSizeMode.StretchImage;
            Amount = 0;
            var timerChangeSupply = new Timer { Interval = 6000 };
            timerChangeSupply.Tick += TimerChangeSupply_Tick;
            timerChangeSupply.Start();
        }

        private void TimerChangeSupply_Tick(object sender, EventArgs e)
        {
            if (GameForm.Player == null) return;
            GenerateSupply();
        }

        public void GenerateSupply()
        {
            var random = new Random();
            SupplyType = (SupplyType)random.Next(0, GameForm.Player.Health <= 40 || GameForm.Player.Ammo <= 1 ? 2 : 7);
            switch (SupplyType)
            {
                case SupplyType.Health:
                    this.Image = Properties.Resources.Health;
                    Amount = 30;
                    break;
                case SupplyType.Bullet:
                    this.Image = Properties.Resources.Bullet;
                    Amount = 20;
                    break;
                default:
                    this.Image = Properties.Resources.Suprise;
                    break;

            }
            this.Location = Settings.GetRandomAvailableLocationOnMap();

            GameForm.Supply.Visible = true;
            GameForm.Sounds.PlaySuppliment();

        }

    }
}
