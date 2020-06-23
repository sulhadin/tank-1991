

using System.Threading;
using System.Threading.Tasks;

namespace WorldOfTanks
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Characters;
    using Creation;
    using Repository;
    using System.Collections.Generic;
    using Settings;
    using Tools;
    public partial class GameForm : Form
    {

        public GameForm(bool activateVoiceControl = false)
        {
            InitializeComponent();

            if (!activateVoiceControl) return;

            Speec = new SpeecRecognition();
            Speec.OnAction += GameForm_OnAction;
        }
        private readonly GameMode _mode = GameMode.Classic;

        public GameForm(GameMode mode)
        {
            InitializeComponent();

            this._mode = mode;
        }
        #region Definitions

        public static List<Tank> Enemies;
        public static List<Field> MapList;
        public static ProgressBar PlayerHealth;
        public static Label PlayerAmmo;
        public static Field[,] Mapping;
        public static Tank Player;
        CharacterState _cstate = CharacterState.Up;
        public static int AreaColumn = 0;
        public static int AreaRow = 0;
        public static Point PlayerLocation;
        public static Supplement Supply;
        public static Sounds Sounds = new Sounds();
        public static Score Score;
        public static GameOver GameOver;
        public static SpeecRecognition Speec;
        private CharacterGesture _characterGesture;
        #endregion
        #region Functions
        private void DrawCharacters()
        {
            Player = new Tank();
            Player.SetAsPlayer();
            Player.Location = PlayerLocation;
            Player.GameArea = GameArea;
            GameArea.Controls.Add(Player);
            Player.BringToFront();
            Player.Ammo = Enemies.Count * 1000;
            PlayerAmmo.Text = Player.Ammo.ToString();
            var distributeTargets = false;
            foreach (var item in Enemies)
            {

                item.GameArea = GameArea;
                GameArea.Controls.Add(item);
                item.BringToFront();
                if (!distributeTargets && _mode == GameMode.Eagle)
                    item.IsTargetEagle = true;
                distributeTargets = !distributeTargets;
                item.StartTracing();


            }


        }
        private void DrawMap()
        {
            var map = new Map();
            Mapping = _mode == GameMode.Eagle ? map.MappingWithEagle() : map.MappingClassic();
            AreaColumn = map.MapColumn;
            AreaRow = map.MapRow;

            for (var i = 0; i < AreaRow; i++)
            {
                for (var j = 0; j < AreaColumn; j++)
                {
                    GameArea.Controls.Add(Mapping[i, j]);
                }
            }

            Width = (map.MapColumn * Field.FieldSize) + Field.FieldSize / 2;
            Height = (map.MapRow * Field.FieldSize) + Field.FieldSize * 5 / 2;

        }
        private void DrawTheComponents()
        {
            var heartIcon = new PictureBox
            {

                Location = new Point(0, 30),
                Width = 30,
                Height = 30,
                Image = Properties.Resources.HeartWhite,
                SizeMode = PictureBoxSizeMode.StretchImage,
            };

            PlayerHealth = new ProgressBar
            {
                Location = new Point(40, 35),
                Width = 150,
                Height = 20,
                Minimum = 0,
                Maximum = 100
            };

            PlayerHealth.Value = PlayerHealth.Maximum;

            var playerAmmoIcon = new PictureBox
            {
                Location = new Point(200, 30),
                Width = 30,
                Height = 30,
                Image = Properties.Resources.Bullet,
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            PlayerAmmo = new Label
            {
                Location = new Point(230, 30),
                Width = 30,
                Height = 20,
                Font = new Font("Microsoft Sans Serif", 20F),
                ForeColor = Color.White,
                AutoSize = true

            };

            PaneHeader.Controls.Add(playerAmmoIcon);
            PaneHeader.Controls.Add(PlayerAmmo);
            PaneHeader.Controls.Add(heartIcon);
            PaneHeader.Controls.Add(PlayerHealth);
            MapList = new List<Field>();
            Enemies = new List<Tank>();

            GameOver = new GameOver(this);
            Score = new Score(PaneHeader);
            Supply = new Supplement { GameArea = GameArea };
            GameArea.Controls.Add(Supply);
            Supply.BringToFront();


        }

        #endregion
        #region Events
        private void GameForm_Load(object sender, EventArgs e)
        {

            try
            {
                DrawTheComponents();
                DrawMap();
                DrawCharacters();
                Sounds.PlayStart();
            }
            catch (Exception exception)
            {

                Settings.Settings.MessageError(exception.Message);
                ((GameMenu)Application.OpenForms["GameMenu"])?.Show();
                Close();
            }


        }
        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            Player?.Go(_cstate);
        }
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (Player == null) return;

            switch (e.KeyCode)
            {
                case Keys.W:
                    _cstate = CharacterState.Up;
                    Player.Go(_cstate);
                    break;
                case Keys.A:
                    _cstate = CharacterState.Left;
                    Player.Go(_cstate);
                    break;
                case Keys.D:
                    _cstate = CharacterState.Right;
                    Player.Go(_cstate);
                    break;
                case Keys.S:
                    _cstate = CharacterState.Down;
                    Player.Go(_cstate);
                    break;
                case Keys.Space:
                    Player.Shoot(_cstate, GameArea);
                    break;
            }
        }
        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var f = ((GameMenu)Application.OpenForms["GameMenu"]);
            f?.Show();
            Player = null;
        }


        private void GameForm_OnAction(CharacterGesture obj)
        {
            if (Player == null)
            {
                TimerSR.Stop();
                return;
            }
            Text = obj.ToString();

            _characterGesture = obj;
            switch (obj)
            {
                case CharacterGesture.Up:
                    _cstate = CharacterState.Up;
                    Player.Go(_cstate);
                    break;
                case CharacterGesture.Left:
                    _cstate = CharacterState.Left;
                    Player.Go(_cstate);
                    break;
                case CharacterGesture.Right:
                    _cstate = CharacterState.Right;
                    Player.Go(_cstate);
                    break;
                case CharacterGesture.Down:
                    _cstate = CharacterState.Down;
                    Player.Go(_cstate);
                    break;
                case CharacterGesture.Move:
                    TimerSR.Start();
                    break;
                case CharacterGesture.Fire:
                    Player.Shoot(_cstate, GameArea);
                    break;
                case CharacterGesture.Stop:
                    TimerSR.Stop();
                    break;
            }

        }
        private void TimerSR_Tick(object sender, EventArgs e)
        {
            if (_characterGesture == CharacterGesture.Fire)
            {
                Player.Shoot(_cstate, GameArea);
            }
            else
            {
                Player?.Go(_cstate);
            }
        }

        #endregion

    }

}
