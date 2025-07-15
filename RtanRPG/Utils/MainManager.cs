using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Utils
{
    internal class MainManager
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            var map = new MapManager(50, 25);
            var player = new Player(map);
            var input = new InputManager();

            input.onmove += player.Move;

            map.Draw(player.X, player.Y);
            input.Start();



            while (true)
            {
                //실행내용
                Thread.Sleep(100);
            }
        }
    }
}
