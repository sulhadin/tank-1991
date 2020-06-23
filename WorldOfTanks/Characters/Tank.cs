using System.IO;

namespace WorldOfTanks.Characters
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Settings;
    using Repository;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class Tank : PictureBox
    {
        public bool IsEnemy { get; set; }
        public Panel GameArea { get; set; }
        public int Health { get; set; }
        public int Ammo { get; set; }
        public short TankColor { get; set; }
        public bool IsTargetEagle { get; set; }
        #region  VARIABLES

        Timer _timerRefreshThePath;
        public static int TankSize = 40;
        public bool ActivateBulletForPlayer = true;
        public bool ActivateSensorForPlayer = true;
        public bool StopTimerRefreshThePath = false;

        #endregion

        public Tank()
        {
            IsEnemy = false;
            IsTargetEagle = false;
            this.Width = TankSize;
            this.Height = TankSize;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            //this.BackColor = Color.Transparent;
            Health = 100;
            Ammo = 10;
        }

        public void SetAsPlayer()
        {
            IsEnemy = false;
            TankColor = Settings.PlayerColor;
            this.Image = Settings.GetTankImage(TankColor, CharacterState.Up);
        }

        public void SetAsEnemy()
        {
            IsEnemy = true;
            TankColor = Settings.EnemyColor;
            this.Image = Settings.GetTankImage(TankColor, CharacterState.Up);

        }

        #region A* Algorithm

        readonly List<Field> _closeList = new List<Field>();
        readonly List<Field> _openList = new List<Field>();
        readonly List<Field> _pathWay = new List<Field>();
        bool _cancelPathFinding = false;
        Timer _tmrAttack;

        public void StartTracing()
        {
            //if (IsTargetEagle)
            //{
            //    _cancelPathFinding = false;
            //    _isDeterminingDirectionOfEnemyAvailable = true;
            //    if (newtarget != null)
            //        FindThePlayer(GameForm.Mapping[newtarget.Row, newtarget.Column]);

            //}
            //else
            //{
            _timerRefreshThePath = new Timer { Interval = Settings.EnemyRefreshPathInterval };
            _timerRefreshThePath.Tick += TimerRefreshThePath_Tick;
            _timerRefreshThePath.Start();
            //}

        }

        public void FindThePlayer(Field item)
        {
            if (_cancelPathFinding) return;

            item.IsPointed = true;
            _closeList.Add(item);
            var neighbours = IsTargetEagle ? GetNeighboursForEagle(item) : GetNeighboursForPlayer(item);
            foreach (var item2 in neighbours)
            {
                //Level 1
                if (IsTargetEagle)
                {
                    if (!item2.HasPlayerEagle) continue;

                    _closeList.Add(item2);
                    FindShortestPath();
                    return;
                }
                else
                {
                    if (!item2.HasTarget) continue;

                    _closeList.Add(item2);
                    FindShortestPath();
                    return;
                }
            }
            GetNexNeighbour(neighbours);

        }

        private void GetNexNeighbour(List<Field> neighbours)
        {
            if (_cancelPathFinding) return;

            var field = neighbours.OrderBy(x => x.F).FirstOrDefault();
            if (field != null)
            {
                field.IsPointed = true;
                //var field_neighbours = GetNeighboursForPlayer(field);
                var field_neighbours = IsTargetEagle ? GetNeighboursForEagle(field) : GetNeighboursForPlayer(field);

                if (field_neighbours.Count > 0)
                {
                    FindThePlayer(field);
                    return;
                }
                else
                {
                    neighbours.Remove(field);
                    GetNexNeighbour(neighbours);
                }
            }
            else
            {
                var max = _openList.Where(d => !d.IsPointed).OrderByDescending(x => x.F).FirstOrDefault();
                if (max != null)
                {
                    FindThePlayer(max);
                    return;
                }
                else
                {
                }
            }
        }

        private void FindShortestPath()
        {
            if (_cancelPathFinding) return;

            var targetItem = IsTargetEagle ? _closeList.Single(x => x.HasPlayerEagle) : _closeList.Single(x => x.HasTarget);
            GetPathItem(targetItem);

            for (var i = 0; i < GameForm.AreaRow; i++)
            {
                for (var j = 0; j < GameForm.AreaColumn; j++)
                {
                    //GameForm.mapping[i, j].HasTarget = false;
                    //GameForm.mapping[i, j].HasSource = false;
                    if (GameForm.Mapping[i, j].IsRock ||
                        GameForm.Mapping[i, j].HasEnemyEagle ||
                        GameForm.Mapping[i, j].HasPlayerEagle)
                        continue;

                    GameForm.Mapping[i, j].IsPointed = false;
                    GameForm.Mapping[i, j].F = 0;
                    GameForm.Mapping[i, j].G = 0;
                    GameForm.Mapping[i, j].H = 0;
                    if (!GameForm.Mapping[i, j].Destroyable) GameForm.Mapping[i, j].BackColor = Color.Black;
                }
            }


            GetNextNode();
            _tmrAttack = new Timer { Interval = Settings.EnemyTimerInterval };
            _tmrAttack.Tick += tmrAttack_Tick;
            _tmrAttack.Start();
        }

        private void GetPathItem(Field field)
        {
            if (_cancelPathFinding) return;

            //field.BackColor = Color.Green;
            _pathWay.Add(field);

            Field nextField = null;

            var avaiablePaths = IsTargetEagle ?
                GetNeighboursLiteForEagle(field).Where(x => _pathWay.All(t => t.Id != x.Id)) :
                GetNeighboursLiteForPlayer(field).Where(x => _pathWay.All(t => t.Id != x.Id));


            if (avaiablePaths.Count() == 1)
            {
                nextField = avaiablePaths.First();
            }
            else
            {
                nextField = avaiablePaths.OrderBy(t => t.G).FirstOrDefault();
            }

            if (nextField != null)
            {
                if (nextField.HasSource)
                {
                    return;
                }
                GetPathItem(nextField);
            }
        }

        private List<Field> GetNeighboursForPlayer(Field f)
        {
            var neigbours = new List<Field>();
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= +1; y++)
                {
                    if ((x == -1 && y == -1) || (x == 1 && y == -1) || (x == -1 && y == 1) || (x == 1 && y == 1) ||
                        f.Row + x < 0 || f.Column + y < 0 || f.Row + x >= GameForm.AreaRow ||
                        f.Column + y >= GameForm.AreaColumn || (x == 0 && y == 0) ||
                        GameForm.Mapping[f.Row + x, f.Column + y].IsPointed ||
                        GameForm.Mapping[f.Row + x, f.Column + y].IsRock ||
                        GameForm.Mapping[f.Row + x, f.Column + y].HasSource ||
                        GameForm.Mapping[f.Row + x, f.Column + y].Destroyable ||
                        GameForm.Mapping[f.Row + x, f.Column + y].HasPlayerEagle ||
                        GameForm.Mapping[f.Row + x, f.Column + y].HasEnemyEagle)
                    {
                        continue;
                    }
                    else
                    {
                        var neighbour = GameForm.Mapping[f.Row + x, f.Column + y];

                        if (neighbour.G == 0)
                            neighbour.G = (Math.Abs(neighbour.Column - this.Location.X / 50) +
                                           Math.Abs(neighbour.Row - this.Location.Y / 50));
                        //var player = GameForm.Player;
                        var point = IsTargetEagle ? GetTargetOfEagle() : GameForm.Player.Location;
                        neighbour.H = (Math.Abs(neighbour.Column - point.X / 50) +
                                       Math.Abs(neighbour.Row - point.Y / 50));
                        neighbour.F = neighbour.G + neighbour.H;
                        neigbours.Add(neighbour);
                        _openList.Add(neighbour);

                        //for seeing  values
                        //var label = new Label();
                        ////label.Text = "h:" + neighbour.H.ToString() + "g:" + neighbour.G.ToString();
                        //label.Text = "h:" + neighbour.H.ToString() + " g:" + neighbour.G.ToString() + " f:" + neighbour.F.ToString();
                        ////label.Text = neighbour.F.ToString();
                        //label.MaximumSize = new Size(40, 0);
                        //label.AutoSize = true;
                        //label.Font = new Font(FontFamily.GenericSansSerif, 9);
                        //label.Location = neighbour.Location;
                        //label.ForeColor = Color.White;
                        //label.BackColor = Color.Black;
                        //label.Width = 50;
                        //GameArea.Controls.Add(label);
                        //label.BringToFront();
                        //end
                    }
                }
            }
            return neigbours;
        }
        private List<Field> GetNeighboursForEagle(Field f)
        {
            var neigbours = new List<Field>();
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= +1; y++)
                {
                    if ((x == -1 && y == -1) || (x == 1 && y == -1) || (x == -1 && y == 1) || (x == 1 && y == 1) ||
                        f.Row + x < 0 || f.Column + y < 0 || f.Row + x >= GameForm.AreaRow ||
                        f.Column + y >= GameForm.AreaColumn || (x == 0 && y == 0) ||
                        GameForm.Mapping[f.Row + x, f.Column + y].IsPointed ||
                        GameForm.Mapping[f.Row + x, f.Column + y].IsRock ||
                        GameForm.Mapping[f.Row + x, f.Column + y].HasSource ||
                        GameForm.Mapping[f.Row + x, f.Column + y].HasTarget ||
                        GameForm.Mapping[f.Row + x, f.Column + y].HasEnemyEagle ||
                        GameForm.Mapping[f.Row + x, f.Column + y].ProtectEnemyEagle)
                    {
                        continue;
                    }
                    else
                    {
                        var neighbour = GameForm.Mapping[f.Row + x, f.Column + y];

                        if (neighbour.G == 0)
                            neighbour.G = (Math.Abs(neighbour.Column - this.Location.X / 50) +
                                           Math.Abs(neighbour.Row - this.Location.Y / 50));
                        //var player = GameForm.Player;
                        var point = GetTargetOfEagle();
                        neighbour.H = (Math.Abs(neighbour.Column - point.X / 50) +
                                       Math.Abs(neighbour.Row - point.Y / 50));
                        neighbour.F = neighbour.G + neighbour.H;
                        neigbours.Add(neighbour);
                        _openList.Add(neighbour);
                        //neighbour.BackColor = Color.Yellow;
                        ////for seeing values
                        //var label = new Label();
                        ////label.Text = "h:" + neighbour.H.ToString() + "g:" + neighbour.G.ToString();
                        //label.Text = "h:" + neighbour.H.ToString() + " g:" + neighbour.G.ToString() + " f:" + neighbour.F.ToString();
                        ////label.Text = neighbour.F.ToString();
                        //label.MaximumSize = new Size(40, 0);
                        //label.AutoSize = true;
                        //label.Font = new Font(FontFamily.GenericSansSerif, 9);
                        //label.Location = neighbour.Location;
                        //label.ForeColor = Color.White;
                        //label.BackColor = Color.Black;
                        //label.Width = 50;
                        //GameArea.Controls.Add(label);
                        //label.BringToFront();
                        ////end
                    }
                }
            }
            return neigbours;
        }

        private List<Field> GetNeighboursLiteForPlayer(Field f)
        {
            var list = new List<Field>();
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= +1; y++)
                {
                    if (f.Row + x < 0 || f.Column + y < 0 || f.Row + x >= GameForm.AreaRow ||
                        f.Column + y >= GameForm.AreaColumn || (x == 0 && y == 0) || (x == -1 && y == -1) ||
                        (x == 1 && y == -1) || (x == -1 && y == 1) || (x == 1 && y == 1) ||
                        GameForm.Mapping[f.Row + x, f.Column + y].IsRock ||
                        GameForm.Mapping[f.Row + x, f.Column + y].HasEnemyEagle ||
                        GameForm.Mapping[f.Row + x, f.Column + y].Destroyable ||
                        GameForm.Mapping[f.Row + x, f.Column + y].HasPlayerEagle
                        )
                    {
                        continue;
                    }
                    else
                    {
                        var neighbour = GameForm.Mapping[f.Row + x, f.Column + y];
                        if (_closeList.IndexOf(neighbour) == -1)
                        {
                            continue;
                        }
                        ;
                        list.Add(neighbour);
                    }
                }
            }
            return list;
        }
        private List<Field> GetNeighboursLiteForEagle(Field f)
        {
            var list = new List<Field>();
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= +1; y++)
                {
                    if (f.Row + x < 0 || f.Column + y < 0 || f.Row + x >= GameForm.AreaRow ||
                        f.Column + y >= GameForm.AreaColumn || (x == 0 && y == 0) || (x == -1 && y == -1) ||
                        (x == 1 && y == -1) || (x == -1 && y == 1) || (x == 1 && y == 1) ||
                        GameForm.Mapping[f.Row + x, f.Column + y].IsRock)
                    {
                        continue;
                    }
                    else
                    {
                        var neighbour = GameForm.Mapping[f.Row + x, f.Column + y];
                        if (_closeList.IndexOf(neighbour) == -1)
                        {
                            continue;
                        }

                        list.Add(neighbour);
                    }
                }
            }
            return list;
        }

        private Point GetTargetOfEagle()
        {
            var location = GameForm.MapList.FirstOrDefault(x => x.HasPlayerEagle)?.Location;
            if (location != null)
                return (Point)location;

            return new Point();
        }

        #endregion

        #region Attack

        Field _nextNode;
        bool _hasEnemyArrivedToNextNode = false;
        CharacterState _movingDirection;
        bool _isDeterminingDirectionOfEnemyAvailable = true;

        private void Attack(Field field)
        {
            if (_cancelPathFinding) return;

            if (_isDeterminingDirectionOfEnemyAvailable)
                DetermineDirection(field);
            //if (!Available)
            MoveEnemyThroughDirection(field);

            if (!_hasEnemyArrivedToNextNode) return;

            //TODO: IF PROBLEM ROLLBACK
            GetNextNode();
            //Task task1 = Task.Factory.StartNew(() => GetNextNode());
            //Task task2 = task1.ContinueWith(antTask => );
            if (ActivateSensorForPlayer)
                ShootSensor();
        }

        private void MoveEnemyThroughDirection(Field field)
        {
            switch (_movingDirection)
            {
                case CharacterState.Up:
                    GoUp();
                    if (this.Location.Y - field.Location.Y <= 0)
                    {
                        _hasEnemyArrivedToNextNode = true;
                    }
                    break;
                case CharacterState.Down:
                    GoDown();
                    if (this.Location.Y - field.Location.Y >= 0)
                    {
                        _hasEnemyArrivedToNextNode = true;
                    }
                    break;
                case CharacterState.Right:
                    GoRight();
                    if (this.Location.X - field.Location.X >= 0)
                    {
                        _hasEnemyArrivedToNextNode = true;
                    }
                    break;
                case CharacterState.Left:
                    GoLeft();
                    if (this.Location.X - field.Location.X <= 0)
                    {
                        _hasEnemyArrivedToNextNode = true;
                    }
                    break;
            }
        }

        private void DetermineDirection(Field field)
        {
            _isDeterminingDirectionOfEnemyAvailable = false;
            if (field.Location.X != this.Location.X)
            {
                var diff = this.Location.X - field.Location.X;
                _movingDirection = CharacterState.Left;
                if (diff < 0)
                {
                    _movingDirection = CharacterState.Right;
                }
            }

            if (field.Location.Y != this.Location.Y)
            {
                var diff = this.Location.Y - field.Location.Y;
                _movingDirection = CharacterState.Up;
                if (diff < 0)
                {
                    _movingDirection = CharacterState.Down;
                }
            }
        }

        private void tmrAttack_Tick(object sender, EventArgs e)
        {
            if (GameForm.Player == null)
            {
                _tmrAttack.Stop();
                return;
            }
            Attack(_nextNode);
        }

        private void GetNextNode()
        {
            if (_pathWay.Count == 1)
            {
                _tmrAttack?.Stop();
                return;
            }
            ;
            _nextNode = _pathWay[_pathWay.Count - 1];
            _pathWay.Remove(_nextNode);
            _hasEnemyArrivedToNextNode = false;
            _isDeterminingDirectionOfEnemyAvailable = true;
        }

        public void ShootSensor()
        {
            if (!ActivateBulletForPlayer) return;

            var diffx = Math.Abs(this.Location.X - (IsTargetEagle ? GetTargetOfEagle().X : GameForm.Player.Location.X));
            var diffy = Math.Abs(this.Location.Y - (IsTargetEagle ? GetTargetOfEagle().Y : GameForm.Player.Location.Y));
            //if (this.Location.X != GameForm.Player.Location.X && this.Location.Y != GameForm.Player.Location.Y)
            if (diffx > TankSize / 2 && diffy > TankSize / 2)
            {
                return;
            }
            ;

            _timerRefreshThePath.Stop();
            using (var bullet = new Sensor())
            {
                //if (this.Location.X == GameForm.Player.Location.X)
                if (diffx < TankSize / 2)
                {
                    bullet.CharState.Add(CharacterState.Down);
                    bullet.CharState.Add(CharacterState.Up);
                }
                //else if (this.Location.Y == GameForm.Player.Location.Y)
                else if (diffy < TankSize / 2)
                {
                    bullet.CharState.Add(CharacterState.Left);
                    bullet.CharState.Add(CharacterState.Right);
                }

                bullet.Location = new Point(this.Location.X + (TankSize / 2), this.Location.Y + (TankSize / 2));
                bullet.GameArea = GameArea;
                bullet.OwnerOfBullet = this;
                GameArea.Controls.Add(bullet);
                bullet.BringToFront();
                bullet.Fire();
            }

            _timerRefreshThePath.Start();
        }

        public void Shoot(CharacterState bulletDirection, Panel panel)
        {
            if (!ActivateBulletForPlayer) return;

            ActivateBulletForPlayer = false;
            if (!IsEnemy)
            {
                if (Ammo <= 0)
                {
                    return;
                }

                Ammo--;
                GameForm.PlayerAmmo.Text = Ammo.ToString();
            }

            Go(bulletDirection);
            _tmrAttack?.Stop();
            var bullet = new Bullet
            {
                IsEnemyBullet = IsEnemy,
                GameArea = panel,
                Location = new Point(this.Location.X + (TankSize / 3), this.Location.Y + (TankSize / 3)),
                BulletDirection = bulletDirection
            };
            if (IsEnemy)
            {
                bullet.SetAsEnemyBullet();
            }
            else
            {
                bullet.SetAsPlayerBullet();
            }
            panel.Controls.Add(bullet);
            bullet.BringToFront();
            this.BringToFront();
            bullet.OwnerOfBullet = this;
            bullet.Fire();
        }

        private void TimerRefreshThePath_Tick(object sender, EventArgs e)
        {
            if (GameForm.Player == null)
            {
                _timerRefreshThePath.Stop();
                return;
            }
            ChangeThePath();
        }

        private void ChangeThePath()
        {
            if (StopTimerRefreshThePath)
            {
                _timerRefreshThePath.Stop();
                StopTimerRefreshThePath = false;
                return;
            }
            _cancelPathFinding = true;
            var source = GameForm.MapList.FirstOrDefault(x => x.HasSource);
            source.HasSource = false;
            var newsource = GameForm.MapList.FirstOrDefault(x => x.Bounds.IntersectsWith(this.Bounds));
            newsource.HasSource = true;
            if (IsEnemy) this.Location = newsource.Location;

            var target = GameForm.MapList.FirstOrDefault(x => x.HasTarget);
            target.HasTarget = false;
            var newtarget = GameForm.MapList.FirstOrDefault(x => x.Bounds.IntersectsWith(GameForm.Player.Bounds));
            newtarget.HasTarget = true;

            _closeList.Clear();
            _openList.Clear();
            _pathWay.Clear();
            _cancelPathFinding = false;
            _tmrAttack?.Stop();
            _isDeterminingDirectionOfEnemyAvailable = true;
            FindThePlayer(GameForm.Mapping[newsource.Row, newsource.Column]);
        }

        #endregion

        #region Move

        public void GoLeft()
        {

            this.Image = Settings.GetTankImage(TankColor, CharacterState.Left);
            this.Location = new Point(this.Location.X - Settings.CharacterStepSize, this.Location.Y);
            if (DedectCollusiton())
                this.Location = new Point(this.Location.X + Settings.CharacterStepSize, this.Location.Y);
        }

        public void GoRight()
        {
            this.Image = Settings.GetTankImage(TankColor, CharacterState.Right);
            this.Location = new Point(this.Location.X + Settings.CharacterStepSize, this.Location.Y);
            if (DedectCollusiton())
                this.Location = new Point(this.Location.X - Settings.CharacterStepSize, this.Location.Y);
        }

        public void GoDown()
        {
            this.Image = Settings.GetTankImage(TankColor, CharacterState.Down);
            //this.Image = !IsEnemy ? Properties.Resources.PlayerDown : Properties.Resources.EnemyDown;

            this.Location = new Point(this.Location.X, this.Location.Y + Settings.CharacterStepSize);

            if (DedectCollusiton())
                this.Location = new Point(this.Location.X, this.Location.Y - Settings.CharacterStepSize);
        }

        public void GoUp()
        {
            this.Image = Settings.GetTankImage(TankColor, CharacterState.Up);
            //this.Image = !IsEnemy ? Properties.Resources.PlayerUp : Properties.Resources.EnemyUp;
            this.Location = new Point(this.Location.X, this.Location.Y - Settings.CharacterStepSize);

            if (DedectCollusiton())
            {
                this.Location = new Point(this.Location.X, this.Location.Y + Settings.CharacterStepSize);
            }
        }

        public void Go(CharacterState bulletDirection)
        {

            switch (bulletDirection)
            {
                case CharacterState.Up:
                    GoUp();
                    break;
                case CharacterState.Down:
                    GoDown();
                    break;
                case CharacterState.Left:
                    GoLeft();
                    break;
                case CharacterState.Right:
                    GoRight();
                    break;
            }
            IsCollidedWithSupplement();
        }

        #endregion

        #region COLLUSION

        private bool IsCollided()
        {
            //if (this.ClientRectangle.Contains(Player.Bounds))

            for (var i = 0; i < GameForm.AreaRow; i++)
            {
                for (var j = 0; j < GameForm.AreaColumn; j++)
                {
                    if ((!GameForm.Mapping[i, j].IsRock && !GameForm.Mapping[i, j].Destroyable &&
                         !GameForm.Mapping[i, j].HasEnemyEagle && !GameForm.Mapping[i, j].HasPlayerEagle))
                        continue;
                    if (GameForm.Mapping[i, j].Destroyable)
                    {
                        if (this.Bounds.IntersectsWith(GameForm.Mapping[i, j].Bounds))
                        {
                            if(IsEnemy)
                            Shoot(_movingDirection, GameArea);
                            return true;
                        }
                    }
                    else
                    {
                        if (this.Bounds.IntersectsWith(GameForm.Mapping[i, j].Bounds))
                        {
                            return true;
                        }
                    }


                }
            }

            return false;
        }

        private bool IsCollidedWithForm(Panel frm)
        {
            return (!frm.ClientRectangle.Contains(Bounds));
        }

        private bool IsCollidedWithTarget()
        {
            foreach (var item in GameForm.Enemies)
            {
                if (!item.IsEnemy) continue;
                if (item == this) continue;
                if (this.Bounds.IntersectsWith(item.Bounds))
                {
                    return true;
                }
            }

            return false;
        }

        private void IsCollidedWithSupplement()
        {
            if (IsEnemy) return;

            if (this.Bounds.IntersectsWith(GameForm.Supply.Bounds) && GameForm.Supply.Visible)
            {
                GameForm.Supply.Visible = false;

                switch (GameForm.Supply.SupplyType)
                {
                    case SupplyType.Health:
                        Health += Health + GameForm.Supply.Amount <= 100 ? GameForm.Supply.Amount : (100 - Health);
                        GameForm.PlayerHealth.Value = Health;
                        break;
                    case SupplyType.Bullet:
                        Ammo += GameForm.Supply.Amount;
                        GameForm.PlayerAmmo.Text = Ammo.ToString();
                        break;
                    case SupplyType.Point:
                        GameForm.Player.Location = Settings.GetRandomAvailableLocationOnMap();
                        break;
                    case SupplyType.DecreaseTankSpeed:
                        if (Settings.CharacterStepSize >= 5)
                            Settings.CharacterStepSize -= 5;
                        break;
                    case SupplyType.IncreaseTankSpeed:
                        Settings.CharacterStepSize += 5;
                        break;
                    case SupplyType.IncreaseBulltSpeed:
                        Settings.BulletStepSize += 5;
                        break;
                    case SupplyType.DecreaseBulletSpeed:
                        if (Settings.BulletStepSize >= 5)
                            Settings.BulletStepSize -= 5;
                        break;
                    case SupplyType.ExtraPoint:
                        GameForm.Score.SetScore(100000);
                        break;
                }
                GameForm.Sounds.PlaySuppliment2();
            }
        }

        private bool IsCollidedWithPlayer()
        {
            if (!IsEnemy || GameForm.Player == null) return false;

            return this.Bounds.IntersectsWith(GameForm.Player.Bounds);
        }

        private bool DedectCollusiton()
        {
            return (IsCollided() || IsCollidedWithForm(GameArea) || IsCollidedWithTarget() || IsCollidedWithPlayer());
        }

        #endregion

        #region PrepareForAnotherPlay

        private Timer _timerPrepareForAnotherPlay;

        public void PrepareForAnotherPlay()
        {
            var isThereAnybodyWhoKeepsTrackOfEagle = false;
            foreach (var item in GameForm.Enemies)
            {
                if (item.IsTargetEagle)
                    isThereAnybodyWhoKeepsTrackOfEagle = true;


            }
            if (!isThereAnybodyWhoKeepsTrackOfEagle)
                IsTargetEagle = true;

            _timerRefreshThePath.Stop();
            _tmrAttack?.Stop();
            _timerPrepareForAnotherPlay = new Timer { Interval = 5000 };
            _timerPrepareForAnotherPlay.Tick += _timerPrepareForAnotherPlay_Tick;
            _timerPrepareForAnotherPlay.Start();
        }

        private void _timerPrepareForAnotherPlay_Tick(object sender, EventArgs e)
        {
            _timerRefreshThePath.Start();
            GameForm.Enemies.Add(this);
            GameArea.Controls.Add(this);
            this.Location = Settings.GetRandomAvailableLocationOnMap();
            this.BringToFront();
            Health = 100;
            _timerPrepareForAnotherPlay.Stop();
        }

        #endregion
    }
}