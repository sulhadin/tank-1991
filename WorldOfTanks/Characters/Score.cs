using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldOfTanks.Characters
{
    public class Score : Label
    {
        public long TotalScore { get; set; }
        public Panel GameArea { get; set; }
        public Score(Panel panel)
        {
            GameArea = panel;
            this.Location = new Point(340, 30);
            this.Font = new Font("Microsoft Sans Serif", 25F);
            this.ForeColor = Color.White;
            this.AutoSize = true;
            ShowScore();
            GameArea.Controls.Add(this);
        }

        public void SetScore(int score)
        {
            TotalScore += score;
            ShowScore();
        }

        private void ShowScore()
        {
            this.Text = TotalScore.ToString("000000000000");

        }

        public void WriteScore()
        {
            if(long.Parse(ReadTheScore())> TotalScore) return;
            
            var mappath = Path.Combine(Application.StartupPath, @"GameObjects\Score.txt");
            var fs = new FileStream(mappath, FileMode.OpenOrCreate, FileAccess.Write);
            var sw = new StreamWriter(fs);
            sw.Write(TotalScore.ToString("000000000000"));
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        public string ReadTheScore()
        {

            return File.ReadAllLines(Path.Combine(Application.StartupPath, @"GameObjects\Score.txt"), Encoding.UTF8)[0];
            //var mappath = Path.Combine(Application.StartupPath, "test.txt");
            //var fs = new FileStream(mappath, FileMode.Open, FileAccess.Read);
            //var sw = new StreamReader(fs);
            //var yazi = sw.ReadLine();
            //while (yazi != null)
            //{
            //    rtfMap.Text += (yazi);
            //    yazi = sw.ReadLine();
            //    rtfMap.Text += yazi != null ? "\n" : "";
            //}
            //sw.Close();
            //fs.Close();
        }
    }
}
