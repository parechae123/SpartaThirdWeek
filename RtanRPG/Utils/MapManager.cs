using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Utils
{
    internal class MapManager
    {
        private int width;
        private int height;
        private char[,] map;

        public MapManager(int width, int height)
        {
            this.width = width;
            this.height = height;
            map = new char[height, width];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map[y, x] = '.';
        }

        public void Draw(int playerX, int playerY)
        {
            System.Console.Clear();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == playerX && y == playerY)
                        System.Console.Write("P ");
                    else
                        System.Console.Write($"{map[y, x]} ");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine("\nWASD 또는 방향키로 이동");
        }

        public bool IsInsideMap(int x, int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }
    }
}
