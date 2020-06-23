using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfTanks.Repository
{
    public enum CharacterState { Up, Left, Right, Down }
    public enum CharacterGesture { Up, Left, Right, Down, Move, Fire, Stop }
    public enum CollidedWith { Enemy, Player, Rock, Form, Nothing }
    public enum SupplyType { Health = 0, Bullet = 1, Point = 2, DecreaseTankSpeed = 3, IncreaseTankSpeed = 4, IncreaseBulltSpeed = 5, DecreaseBulletSpeed = 6, ExtraPoint = 7 }
    public enum GameMode { Eagle, Classic }

}
