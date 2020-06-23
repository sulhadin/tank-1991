

using System;

namespace WorldOfTanks.Creation
{
    using System.Text;
    using System.IO;
    using System.Windows.Forms;
    using System.Drawing;
    using Characters;
    using System.Collections.Generic;
    using WorldOfTanks;

    public class Map
    {
        public int MapColumn { get; set; }
        public int MapRow { get; set; }

        public Field[,] MappingClassic()
        {
            try
            {


                var lines = File.ReadAllLines(Path.Combine(Application.StartupPath, @"GameObjects\Map.txt"), Encoding.UTF8);


                MapColumn = lines[0].Length;
                MapRow = lines.Length;

                var f = new Field[MapRow, MapColumn];

                for (var i = 0; i < MapRow; i++)
                {
                    var l = lines[i];
                    for (var j = 0; j < MapColumn; j++)
                    {
                        var field = new Field();

                        field.BackColor = Color.Black;

                        if (l.Substring(j, 1) == "#")
                        {
                            field.IsRock = true;
                            //field.BackColor = Color.Brown;
                            field.SizeMode = PictureBoxSizeMode.StretchImage;

                            field.Image = Settings.Settings.GetBirck(3);
                            //field.Image = Properties.Resources.Rock;
                        }

                        //if the target is enemy, get instance of enemy class and add to the generic list.
                        switch (l.Substring(j, 1))
                        {
                            case "E":
                                var enemy = new Tank();
                                enemy.SetAsEnemy();
                                enemy.IsEnemy = true;
                                enemy.Location = new Point(j * field.Width, i * field.Height);
                                enemy.BringToFront();
                                field.HasSource = true;

                                GameForm.Enemies.Add(enemy);
                                break;
                            case "P":
                                GameForm.PlayerLocation = new Point(j * field.Width, i * field.Height);
                                field.HasTarget = true;
                                break;
                        }
                        field.SendToBack();
                        field.Location = new Point(j * field.Width, i * field.Height);
                        field.Row = i;
                        field.Column = j;
                        f[i, j] = field;
                        GameForm.MapList.Add(field);

                    }
                }


                return f;
            }
            catch (Exception e)
            {
                throw new Exception("Harita formatı doğru değil. Lütfen tekrar kontrol edin.");
            }
        }
        public Field[,] MappingWithEagle()
        {
            try
            {


                var lines = File.ReadAllLines(Path.Combine(Application.StartupPath, @"GameObjects\MapEagle.txt"), Encoding.UTF8);


                MapColumn = lines[0].Length;
                MapRow = lines.Length;

                var f = new Field[MapRow, MapColumn];

                for (var i = 0; i < MapRow; i++)
                {
                    var l = lines[i];
                    for (var j = 0; j < MapColumn; j++)
                    {
                        var nextnode = l.Substring(j, 1);
                        var field = new Field {BackColor = Color.Black};

                        switch (nextnode)
                        {
                            case "#": //undestroyable bricks
                                field.IsRock = true;
                                field.SizeMode = PictureBoxSizeMode.StretchImage;
                                field.Image = Settings.Settings.GetBirck(3);
                                break;
                            case "+": 
                                field.Destroyable = true;
                                field.ProtectEnemyEagle = true;
                                field.SizeMode = PictureBoxSizeMode.StretchImage;
                                field.Image = Settings.Settings.GetBirck(1);
                                break;
                            case "*": 
                                field.Destroyable = true;
                                field.ProtectPlayerEagle = true;
                                field.SizeMode = PictureBoxSizeMode.StretchImage;
                                field.Image = Settings.Settings.GetBirck(1);
                                break;
                            case "O":
                                field.HasPlayerEagle = true;
                                field.SizeMode = PictureBoxSizeMode.StretchImage;
                                field.Image = Settings.Settings.GetEagle('O');
                                break;
                            case "X":
                                field.HasEnemyEagle = true;
                                field.SizeMode = PictureBoxSizeMode.StretchImage;
                                field.Image = Settings.Settings.GetEagle('X');
                                break;
                        }

                        //if the target is enemy, get instance of enemy class and add to the generic list.
                        switch (nextnode)
                        {
                            case "E":
                                var enemy = new Tank();
                                enemy.SetAsEnemy();
                                enemy.IsEnemy = true;
                                enemy.Location = new Point(j * field.Width, i * field.Height);
                                enemy.BringToFront();
                                field.HasSource = true;
                                enemy.Health = 5;

                                GameForm.Enemies.Add(enemy);
                                break;
                            case "P":
                                GameForm.PlayerLocation = new Point(j * field.Width, i * field.Height);
                                field.HasTarget = true;
                                break;
                           
                        }
                        field.SendToBack();
                        field.Location = new Point(j * field.Width, i * field.Height);
                        field.Row = i;
                        field.Column = j;
                        f[i, j] = field;
                        GameForm.MapList.Add(field);

                    }
                }


                return f;
            }
            catch (Exception e)
            {
                throw new Exception("Harita formatı doğru değil. Lütfen tekrar kontrol edin.");
            }
        }


    }
}
