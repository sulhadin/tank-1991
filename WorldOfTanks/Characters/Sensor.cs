namespace WorldOfTanks.Characters
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Repository;
    using Settings;
    using System.Collections.Generic;

    public class Sensor : PictureBox
    {
        private Timer _timer;
        public List<CharacterState> CharState = new List<CharacterState>();
        public static int SensorSize = 30;
        private CharacterState _sensorDirection;
        public Panel GameArea { get; set; }
        public Tank OwnerOfBullet { get; set; }
        public Sensor()
        {
            this.Width = SensorSize;
            this.Height = SensorSize;
            this.BackColor = Color.Yellow;

        }
        public void Fire()
        {
            ChangeSensorDirection();
            OwnerOfBullet.ActivateSensorForPlayer = false;
            OwnerOfBullet.ActivateBulletForPlayer = false;

            _timer = new Timer { Interval = Settings.SensorTimerInterval };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            //this.BackColor = Color.Yellow;
            switch (_sensorDirection)
            {
                case CharacterState.Up:
                    this.Location = new Point(this.Location.X, this.Location.Y - Settings.SensorStepSize);
                    break;
                case CharacterState.Down:
                    this.Location = new Point(this.Location.X, this.Location.Y + Settings.SensorStepSize);

                    break;
                case CharacterState.Left:
                    this.Location = new Point(this.Location.X - Settings.SensorStepSize, this.Location.Y);
                    break;
                case CharacterState.Right:
                    this.Location = new Point(this.Location.X + Settings.SensorStepSize, this.Location.Y);
                    break;
            }



            if (IsCollided(GameForm.Mapping) || IsCollidedWithForm() || IsSensorCollidedWithPlayer())
            {
                OwnerOfBullet.ActivateBulletForPlayer = true;
                OwnerOfBullet.ActivateSensorForPlayer = true;
                GameArea.Controls.Remove(this);
                ChangeSensorDirection();

            }
        }
        private void ChangeSensorDirection()
        {
            if (CharState.Count == 0)
            {
                if (_timer == null) return;
                _timer.Stop();
                _timer.Dispose();
                Distruct();
                return;
            }
            var c = CharState[0];
            _sensorDirection = c;
            CharState.Remove(c);
            this.Location = new Point(OwnerOfBullet.Location.X + (Tank.TankSize / 2), OwnerOfBullet.Location.Y + (Tank.TankSize / 2));

        }
        private void Distruct()
        {

            _timer = null;
        }

        #region COLLUSIONS
        private bool IsCollided(Field[,] mapping)
        {

            for (var i = 0; i < GameForm.AreaRow; i++)
            {
                for (var j = 0; j < GameForm.AreaColumn; j++)
                {
                    if (!mapping[i, j].IsRock && !mapping[i, j].HasPlayerEagle) continue;

                    if (mapping[i, j].HasPlayerEagle)
                    {
                        if (this.Bounds.IntersectsWith(mapping[i, j].Bounds))
                        {
                            OwnerOfBullet.ActivateBulletForPlayer = true;
                            OwnerOfBullet.ActivateSensorForPlayer = true;
                            _timer.Stop();
                            _timer.Dispose();
                            Distruct();
                            OwnerOfBullet.Shoot(_sensorDirection, OwnerOfBullet.GameArea);
                            return true;
                        }
                    }
                    else
                    {
                        if (this.Bounds.IntersectsWith(mapping[i, j].Bounds))
                        {
                            return true;
                        }
                    }

                }

            }

            return false;

        }
        public bool IsSensorCollidedWithPlayer()
        {
            if (GameForm.Player == null) return false;
            if (this.Bounds.IntersectsWith(GameForm.Player.Bounds))
            {
                OwnerOfBullet.ActivateBulletForPlayer = true;
                OwnerOfBullet.ActivateSensorForPlayer = true;
                _timer.Stop();
                _timer.Dispose();
                Distruct();
                OwnerOfBullet.Shoot(_sensorDirection, OwnerOfBullet.GameArea);

                return true;
            }
            return false;
        }
        public bool IsCollidedWithForm()
        {
            return (!GameArea.ClientRectangle.Contains(Bounds));
        }
        #endregion

    }
}