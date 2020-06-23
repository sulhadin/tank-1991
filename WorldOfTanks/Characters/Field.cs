
namespace WorldOfTanks.Characters
{
   
    using System.Windows.Forms;
    public class Field : PictureBox
    {
        public static int FieldSize = 50;
        public bool IsRock { get; set; }
        public string Id => Row + "_" + Column;
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsPointed { get; set; }
        public int G { get; set; }
        public int F { get; set; }
        public int H { get; set; }
        public bool HasTarget { get;  set; }
        public bool HasSource { get;  set; }
        //LEVEL2
        public bool HasEnemyEagle { get;  set; }
        public bool HasPlayerEagle { get;  set; }
        public bool Destroyable { get;  set; }
        public bool ProtectEnemyEagle { get; set; }
        public bool ProtectPlayerEagle { get; set; }
        public int Health { get; set; }


        public Field()
        {
            this.Width = FieldSize;
            this.Height = FieldSize;
            IsRock = false;
            HasTarget = false;
            HasSource = false;

            //LEVEL2
            HasEnemyEagle = false;
            HasPlayerEagle = false;
            Destroyable = false;
            ProtectEnemyEagle = false;
            ProtectPlayerEagle = false;
            Health = 100;

        }

    }
}
