namespace RtanRPG.Utils
{
    public class InputSystem
    {
        private Thread? _thread;

        private readonly CancellationTokenSource _token;

        public InputSystem()
        {
            _thread = null;
            _token = new CancellationTokenSource();

            InputCallback = null;
        }

        public void Start()
        {
            _thread = new Thread(GetInputKey) { IsBackground = true };
            _thread.Start();
        }

        public void GetInputKey()
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

        public Action<ConsoleKey>? InputCallback;
    }
}