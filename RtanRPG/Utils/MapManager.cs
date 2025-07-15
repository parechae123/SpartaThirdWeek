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
                    map[y, x] = '.'; // 기본은 빈 땅
        }

        public void Draw(int playerX, int playerY)
        {
            Console.Clear();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == playerX && y == playerY)
                        Console.Write("P ");
                    else
                        Console.Write($"{map[y, x]} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\nWASD 또는 방향키로 이동, ESC로 종료");
        }

        public bool IsInsideMap(int x, int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }
    }
}
