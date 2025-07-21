namespace Test.Utils
{
    public class InputSystem
    {
        private Thread? _thread = null;

        private readonly CancellationTokenSource _token = new();

        public void Start()
        {
            _thread = new Thread(GetInputKey) { IsBackground = true };
            _thread.Start();
        }

        private void GetInputKey()
        {
            while (IsRunning)
            {
                if (System.Console.KeyAvailable)
                {
                    InputCallback?.Invoke(System.Console.ReadKey(true).Key);
                }
            }
        }

        public void Stop()
        {
            _token.Cancel();

            _thread?.Join();
            _token.Dispose();

            _thread = null;
        }

        public bool IsRunning => _token.IsCancellationRequested == false;

        public Action<ConsoleKey>? InputCallback = null;
    }
}