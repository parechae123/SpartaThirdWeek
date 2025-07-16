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

        //public event Action<int, int> onmove;  이동 입력 이벤트
        public Action<ConsoleKey> InputCallback;  //모든키 입력처리 콜벡

        public void Start()
        {
            if(_running) return;

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
                if (System.Console.KeyAvailable)
                {
                    var key = System.Console.ReadKey(true).Key;

                    var callback = InputCallback;  //복사해서 호출
                    InputCallback?.Invoke(key);

                    if (key == ConsoleKey.Escape)
                        Stop();
                }
                Thread.Sleep(30);
            }
        }
    }
}
