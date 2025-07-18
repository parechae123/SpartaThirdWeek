using System.Text;
using RtanRPG.Utils;
using RtanRPG.Utils.Console;

namespace RtanRPG
{
    public class Application
    {
        private readonly InputSystem _inputSystem = new InputSystem();

        private void Bootstrap()
        {
            Configuration.MaximizeConsoleScreenSize();
            Configuration.AdjustBufferSizeToWindow();
            
            Console.OutputEncoding = Encoding.UTF8;
            Layout.Log("Set console output encoding to UTF-8", Layout.Status.Done);
            Layout.Log("Loading ...", Layout.Status.Done);
            
            _inputSystem.Start();
            _inputSystem.InputCallback = SceneManager.Instance.Execute;
            
            Layout.Log("Bootstrap is complete", Layout.Status.Done);
            
            Thread.Sleep(1000);
            
            Console.Clear();
        }

        public void Play()
        {
            Bootstrap();

            while (true)
            {
                SceneManager.Instance.Load(SceneManager.Instance.Index);
            }
        }
    }
}