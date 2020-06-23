using System;
using WorldOfTanks.Repository;

namespace WorldOfTanks
{
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using Settings;
    public partial class GameMenu : Form
    {
        public GameMenu()
        {
            InitializeComponent();
        }
        private readonly Sounds _sounds = new Sounds();
        private void GameMenu_Load(object sender, EventArgs e)
        {

            lbMenuList.SelectedIndex = 0;
        }

        private void lbMenuList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (lbMenuList.SelectedIndex == 0)
                    {
                        pbMenuIcon.Location = new Point(pbMenuIcon.Location.X, pbMenuIcon.Location.Y + 23);
                    }
                    pbMenuIcon.Location = new Point(pbMenuIcon.Location.X, pbMenuIcon.Location.Y - 23);
                    //lbMenuList.SelectedIndex = lbMenuList.SelectedIndex - 1;
                    break;
                case Keys.Down:
                    if (lbMenuList.SelectedIndex == lbMenuList.Items.Count - 1)
                    {
                        pbMenuIcon.Location = new Point(pbMenuIcon.Location.X, pbMenuIcon.Location.Y - 23);
                    }
                    pbMenuIcon.Location = new Point(pbMenuIcon.Location.X, pbMenuIcon.Location.Y + 23);

                    break;
                case Keys.Enter:
                    Show(lbMenuList.SelectedIndex);

                    break;
            }
            //_sounds.PlayMenu();

        }

        private void Show(int index)
        {
            try
            {
                switch (index)
                {
                    case 0:
                        Hide();
                        var k = new GameForm();
                        k.Show();
                        break;
                    case 1:
                        Hide();
                        var v = new GameForm(true);
                        v.Show();
                        break;
                    case 2:
                        Hide();
                        var a = new GameForm(GameMode.Eagle);
                        a.Show();
                        break;
                    case 3:
                        Process.Start(Path.Combine(Application.StartupPath, @"GameObjects\Map.txt"));
                        break;
                    case 4:
                        Process.Start(Path.Combine(Application.StartupPath, @"GameObjects\MapEagle.txt"));
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                }


            }
            catch (Exception e)
            {
                Settings.Settings.MessageError(e.Message);

            }

        }

        private void GameMenu_Activated(object sender, EventArgs e)
        {
            var x = File.ReadAllLines(Path.Combine(Application.StartupPath, @"GameObjects\Score.txt"), Encoding.UTF8);
            lblScore.Text = x == null ? "000000000000" : x[0];

        }
    }
}
