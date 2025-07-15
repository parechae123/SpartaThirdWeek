using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Utils
{
    internal class Player
    {
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;

        private MapManager map;
        public Player(MapManager map)
        {
            this.map = map;
        }
        public void Move(int dx, int dy)
        {
            int newX = X + dx;
            int newY = Y + dy;
            if (map.IsInsideMap(newX, newY))
            {
                X = newX;
                Y = newY;
            }
            map.Draw(X, Y);
        }
    }
}
