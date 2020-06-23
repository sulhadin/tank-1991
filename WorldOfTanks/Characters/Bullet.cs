using WorldOfTanks.Tools;

namespace WorldOfTanks.Characters
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Repository;
    using Settings;
    using System.Collections.Generic;
    using System.Linq;

    public class Bullet : PictureBox
    {

        #region Properties And Variables

        private Timer _timer;
        public static int BulletSize = 10;
        public bool IsEnemyBullet { get; set; }
        public int HitPower { get; set; }
        public CharacterState BulletDirection { get; set; }
        public Panel GameArea { get; set; }
        public Tank OwnerOfBullet { get; set; }

        #endregion


        public Bullet()
        {
            this.Width = BulletSize;
            this.Height = BulletSize;
            HitPower = 10;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            GameForm.Sounds.PlayFire();
        }
        public void SetAsEnemyBullet()
        {
            IsEnemyBullet = true;
            this.Image = Properties.Resources.BlueBullet;
        }
        public void SetAsPlayerBullet()
        {
            IsEnemyBullet = false;
            this.Image = Properties.Resources.GreenBullet;
        }
        public void SetHitPower(int power)
        {
            HitPower = power;
        }

        #region Fire

        public void Fire()
        {

            OwnerOfBullet.ActivateBulletForPlayer = false;
            _timer = new Timer { Interval = Settings.BulletTimerInterval };
            _timer.Tick += Timer_Tick;
            _timer.Start();

        }
        private void Timer_Tick(object sender, EventArgs e)
        {

            switch (BulletDirection)
            {
                case CharacterState.Up:
                    this.Location = new Point(this.Location.X, this.Location.Y - Settings.BulletStepSize);
                    break;
                case CharacterState.Down:
                    this.Location = new Point(this.Location.X, this.Location.Y + Settings.BulletStepSize);

                    break;
                case CharacterState.Left:
                    this.Location = new Point(this.Location.X - Settings.BulletStepSize, this.Location.Y);
                    break;
                case CharacterState.Right:
                    this.Location = new Point(this.Location.X + Settings.BulletStepSize, this.Location.Y);
                    break;
            }

            if (GameForm.Player == null)
            {
                _timer.Stop();
                _timer.Dispose();
                GameArea.Controls.Remove(this);
                return;

            }
            if (IsCollided() || IsCollidedWithForm() || IsCollidedWithEnemy() || IsCollidedWithPlayer())
            {
                OwnerOfBullet.ActivateBulletForPlayer = true;
                _timer.Stop();
                _timer.Dispose();
                GameArea.Controls.Remove(this);
            }
            GameForm.Sounds.PlayNoise2();

        }



        #endregion

        #region Collisions


        public bool IsCollided()
        {

            for (var i = 0; i < GameForm.AreaRow; i++)
            {
                for (var j = 0; j < GameForm.AreaColumn; j++)
                {
                    if ((!GameForm.Mapping[i, j].IsRock && !GameForm.Mapping[i, j].Destroyable && !GameForm.Mapping[i, j].HasEnemyEagle && !GameForm.Mapping[i, j].HasPlayerEagle)) continue;

                    if (GameForm.Mapping[i, j].Destroyable)
                    {

                        if (this.Bounds.IntersectsWith(GameForm.Mapping[i, j].Bounds))
                        {
                            OwnerOfBullet.ActivateBulletForPlayer = true;

                            GameForm.Mapping[i, j].Destroyable = false;
                            GameForm.Mapping[i, j].Image = null;
                            GameForm.Sounds.PlayExplosion3();
                            return true;
                        }
                    }
                    else if (GameForm.Mapping[i, j].HasEnemyEagle)
                    {
                        if (this.Bounds.IntersectsWith(GameForm.Mapping[i, j].Bounds))
                        {
                            GameForm.Sounds.PlayExplosion3();

                            GameForm.Mapping[i, j].Health -= 10;
                            if (GameForm.Mapping[i, j].Health >= 0)
                            {
                                return true;
                            }
                            GameForm.Mapping[i, j].HasEnemyEagle = false;
                            GameForm.Mapping[i, j].Image = null;
                            GameForm.Player = null;
                            GameForm.GameOver.ShowYouWin();
                            OwnerOfBullet.StopTimerRefreshThePath = true;
                            GameForm.Sounds.PlayGameOver();
                            GameForm.Score.WriteScore();


                            return true;
                        }
                    }
                    else if (GameForm.Mapping[i, j].HasPlayerEagle)
                    {
                        if (this.Bounds.IntersectsWith(GameForm.Mapping[i, j].Bounds))
                        {

                            GameForm.Mapping[i, j].HasPlayerEagle = false;
                            GameForm.Mapping[i, j].Image = null;
                            GameForm.Player = null;
                            GameForm.Sounds.PlayExplosion3();
                            GameForm.GameOver.ShowYouLose();
                            OwnerOfBullet.StopTimerRefreshThePath = true;
                            GameForm.Sounds.PlayGameOver();
                            GameForm.Score.WriteScore();


                            return true;
                        }
                    }
                    //if (!GameForm.Mapping[i, j].IsRock) continue;
                    if (this.Bounds.IntersectsWith(GameForm.Mapping[i, j].Bounds))
                    {
                        return true;
                    }
                }

            }

            return false;

        }
        public bool IsCollidedWithEnemy()
        {


            foreach (var item in GameForm.Enemies)
            {
                if (item.IsEnemy && IsEnemyBullet) continue;

                if (!this.Bounds.IntersectsWith(item.Bounds)) continue;

                item.Health -= HitPower;
                GameForm.Score.SetScore(HitPower * 10);
                if (item.Health >= 1)
                {
                    return true;
                }
                GameForm.Enemies.Remove(item);
                GameArea.Controls.Remove(item);
                item.PrepareForAnotherPlay();
                GameForm.Sounds.PlayExplosion2();
                return true;
            }

            return false;

        }
        public bool IsCollidedWithPlayer()
        {
            if (!IsEnemyBullet) return false;
            if (this.Bounds.IntersectsWith(GameForm.Player.Bounds))
            {
                if (GameForm.Enemies.IndexOf(OwnerOfBullet) == -1) return false;
                GameForm.Player.Health -= HitPower;
                GameForm.PlayerHealth.Value = GameForm.Player.Health;

                if (GameForm.Player.Health >= 1)
                {
                    OwnerOfBullet.ShootSensor();
                    return true;
                }
                GameForm.Sounds.PlayExplosion3();
                GameForm.Sounds.PlayGameOver();
                GameArea.Controls.Remove(GameForm.Player);
                GameForm.Player.Image = Properties.Resources.PlayerUp;

                GameForm.Player = null;
                OwnerOfBullet.StopTimerRefreshThePath = true;
                GameForm.GameOver.Show();
                GameForm.Score.WriteScore();
                //TODO: GAMEOVER
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
