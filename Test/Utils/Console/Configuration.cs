using System.Runtime.InteropServices;

namespace Test.Utils.Console
{
    public static class Configuration
    {
        public static void MaximizeConsoleScreenSize()
        {
            var handle = Win32.GetConsoleWindow();
            Win32.SetForegroundWindow(handle);

            Layout.Log("Set console screen to foreground", Layout.Status.Done);

            handle = Win32.GetForegroundWindow();
            Win32.ShowWindow(handle, Win32.SW_MAXIMIZE);

            Layout.Log("Set console screen size to maximum", Layout.Status.Done);

            Thread.Sleep(500);
        }

        public static void AdjustBufferSizeToWindow()
        {
            var hConsole = Win32.GetStdHandle(Win32.StandardOutputHandle);
            if (Win32.GetConsoleScreenBufferInfo(hConsole, out var info))
            {
                var width = (short)(info.srWindow.Right - info.srWindow.Left + 1);
                var height = (short)(info.srWindow.Bottom - info.srWindow.Top + 1);

                var size = new Win32.COORD { X = width, Y = height };

                var result = Win32.SetConsoleScreenBufferSize(hConsole, size);
                if (result == false)
                {
                    Layout.Log($"Fail to set buffer size : {Marshal.GetLastWin32Error()}", Layout.Status.Fail);

                    throw new Exception();
                }

                Layout.DefaultWidth = width;
                Layout.DefaultHeight = height - 2;
                
                OutputStream.SetBuffer(width, height - 2);

                Layout.Log("Set console screen buffer size to maximum", Layout.Status.Done);
                Layout.Log($"Current console screen buffer size is {width}X{height}", Layout.Status.Done);
            }
            else
            {
                Layout.Log($"Failed to get console information : {Marshal.GetLastWin32Error()}", Layout.Status.Fail);

                throw new Exception();
            }
        }
    }
}