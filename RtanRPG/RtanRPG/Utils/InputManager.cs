using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Utils
{
    internal class InputManager
    {
        Thread _thread;
        bool _running = false;

        public event Action<int, int> onmove;

        public void Start()
        {
            _running = true;
            _thread = new Thread(GetConsoleKey);
            _thread.Start();
        }

        public void Stop()
        {
            _running = false;
            _thread?.Join();
        }

        private void GetConsoleKey()
        {
            while (_running)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    int dx = 0, dy = 0;
                    switch (key)
                    {
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            dy = -1; break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:
                            dy = 1; break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            dx = -1; break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            dx = 1; break;
                        case ConsoleKey.Z:
                                break;
                        case ConsoleKey.X:
                                break;
                        case ConsoleKey.Escape:
                        Stop();
                        break;
                    }
                    if(dx !=0 || dy != 0)
                    {
                        onmove?.Invoke(dx, dy);
                    }
                }
                Thread.Sleep(50);
            }
        }
    }
}
