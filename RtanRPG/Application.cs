using System.Text;
using RtanRPG.Utils;
using RtanRPG.Utils.Console;

namespace RtanRPG
{
    public class Application
    {
        private readonly InputSystem _inputSystem;

        public Application()
        {
            _inputSystem = new InputSystem();
        }

        public void Play()
        {
            Configuration.MaximizeConsoleScreenSize();
            Configuration.AdjustBufferSizeToWindow();
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();
            
            _inputSystem.Start();
            _inputSystem.InputCallback = SceneManager.Instance.Execute;
            
            SceneManager.Instance.Load(0);
        }
    }
}