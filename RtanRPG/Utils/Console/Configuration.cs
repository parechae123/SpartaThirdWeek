using System.Runtime.InteropServices;

namespace RtanRPG.Utils.Console
{
    public static class Configuration
    {
        public static void MaximizeConsoleScreenSize()
        {
            IntPtr handle = Win32.GetConsoleWindow();
            Win32.SetForegroundWindow(handle);

            Log("Set console screen to foreground", Status.Done);

            handle = Win32.GetForegroundWindow();
            Win32.ShowWindow(handle, Win32.SW_MAXIMIZE);

            Log("Set console screen size to maximum", Status.Done);

            Thread.Sleep(500);
        }

        public static void AdjustBufferSizeToWindow()
        {
            IntPtr hConsole = Win32.GetStdHandle(Win32.StandardOutputHandle);
            if (Win32.GetConsoleScreenBufferInfo(hConsole, out Win32.CONSOLE_SCREEN_BUFFER_INFO info))
            {
                var width = (short)(info.srWindow.Right - info.srWindow.Left + 1);
                var height = (short)(info.srWindow.Bottom - info.srWindow.Top + 1);

                var size = new Win32.COORD { X = width, Y = height };

                var result = Win32.SetConsoleScreenBufferSize(hConsole, size);
                if (result == false)
                {
                    Log($"Fail to set buffer size : {Marshal.GetLastWin32Error()}", Status.Fail);

                    throw new Exception();
                }

                Window.DefaultWidth = width;
                Window.DefaultHeight = height - 1;
                Window.Buffer = new char[height][];
                for (var i = 0; i < height; i++)
                {
                    Window.Buffer[i] = new char[width];
                    Array.Fill(Window.Buffer[i], ' ');
                }

                Log($"Set console screen buffer size to maximum", Status.Done);
                Log($"Current console screen buffer size is {width}X{height}", Status.Done);
            }
            else
            {
                Log($"Failed to get console information : {Marshal.GetLastWin32Error()}", Status.Fail);

                throw new Exception();
            }
        }

        private static void Log(string message, Status status)
        {
            System.Console.Write("[");

            switch (status)
            {
                case Status.Fail:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.Write("FAIL");
                    break;
                case Status.Done:
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.Write("DONE");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Status));
            }
            
            System.Console.ResetColor();
            System.Console.WriteLine($"] {message}");

            Thread.Sleep(100);
        }

        private enum Status
        {
            Fail, Done
        }
    }
}