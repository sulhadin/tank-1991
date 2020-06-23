namespace WorldOfTanks.Settings
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public class Sounds
    {
        public WMPLib.WindowsMediaPlayer Explosion { get; set; }
        public WMPLib.WindowsMediaPlayer Explosion2 { get; set; }
        public WMPLib.WindowsMediaPlayer Fire { get; set; }
        public WMPLib.WindowsMediaPlayer Noise { get; set; }
        public WMPLib.WindowsMediaPlayer Noise2 { get; set; }
        public WMPLib.WindowsMediaPlayer Start { get; set; }
        public WMPLib.WindowsMediaPlayer Suppliment { get; set; }
        public WMPLib.WindowsMediaPlayer Suppliment2 { get; set; }
        public WMPLib.WindowsMediaPlayer GameOver { get; set; }
        public WMPLib.WindowsMediaPlayer Menu { get; set; }

        public Sounds()
        {
            Explosion2 = new WMPLib.WindowsMediaPlayer();
            Fire = new WMPLib.WindowsMediaPlayer();
            Noise = new WMPLib.WindowsMediaPlayer();
            Noise2 = new WMPLib.WindowsMediaPlayer();
            Start = new WMPLib.WindowsMediaPlayer();
            Suppliment = new WMPLib.WindowsMediaPlayer();
            Suppliment2 = new WMPLib.WindowsMediaPlayer();
            GameOver = new WMPLib.WindowsMediaPlayer();
            Menu = new WMPLib.WindowsMediaPlayer();
            //Explosion2 = new SoundPlayer(Path.Combine(Application.StartupPath, @"GameObjects\Sounds\explosion2.wav"));
            //   Fire = new SoundPlayer(Path.Combine(Application.StartupPath, @"GameObjects\Sounds\fire.wav"));
            //   Noise = new SoundPlayer(Path.Combine(Application.StartupPath, @"GameObjects\Sounds\noise.wav"));
            //   Noise2 = new SoundPlayer(Path.Combine(Application.StartupPath, @"GameObjects\Sounds\noise2.wav"));
            //   Start = new SoundPlayer(Path.Combine(Application.StartupPath, @"GameObjects\Sounds\start.wav"));
            //   Suppliment = new SoundPlayer(Path.Combine(Application.StartupPath, @"GameObjects\suppliment.wav"));
            //   Suppliment2 = new SoundPlayer(Path.Combine(Application.StartupPath, @"GameObjects\Sounds\suppliment2.wav"));


        }

        public void PlayStart()
        {

            Start.URL = Path.Combine(Application.StartupPath, @"GameObjects\Sounds\start.wav");
        }

        public void PlayExplosion()
        {
            Explosion2.URL = Path.Combine(Application.StartupPath, @"GameObjects\Sounds\explosion.wav");
            
        }
        public void PlayExplosion2()
        {
            Explosion2.URL = Path.Combine(Application.StartupPath, @"GameObjects\Sounds\explosion2.wav");

        }
        public void PlayExplosion3()
        {
            Explosion2.URL = Path.Combine(Application.StartupPath, @"GameObjects\Sounds\explosion3.wav");

        }
        public void PlayNoise2()
        {
            Noise2.URL = Path.Combine(Application.StartupPath, @"GameObjects\Sounds\noise2.wav");
        }
        public void PlaySuppliment()
        {
            Noise2.URL = Path.Combine(Application.StartupPath, @"GameObjects\Sounds\suppliment.wav");
        }
        public void PlaySuppliment2()
        {
            Noise2.URL = Path.Combine(Application.StartupPath, @"GameObjects\Sounds\suppliment2.wav");
        }

        public void PlayFire()
        {
            Fire.URL = Path.Combine(Application.StartupPath, @"GameObjects\Sounds\fire.wav");
        }

        public void PlayGameOver()
        {
            GameOver.URL = Path.Combine(Application.StartupPath, @"GameObjects\Sounds\GameOver.wav");
        }
        public void PlayMenu()
        {
            Menu.URL = Path.Combine(Application.StartupPath, @"GameObjects\Sounds\menu.mp3");
        }

    }
}
