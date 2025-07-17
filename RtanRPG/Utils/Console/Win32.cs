using System.Runtime.InteropServices;
using System.Text;

namespace RtanRPG.Utils.Console
{
    public static class Win32
    {
        private const int FixedWidthTrueType = 54;
        public const int StandardOutputHandle = -11;

        public const int SW_MAXIMIZE = 3;

        #region DLL IMPORTED API

        private const string Kernel32DllName = "kernel32.dll";
        private const string User32DllName = "user32.dll";

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport(Kernel32DllName, SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(Kernel32DllName, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow,
                                                        ref FontInfo ipConsoleCurrentFont);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(Kernel32DllName, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow,
                                                        ref FontInfo ipConsoleCurrentFont);

        [DllImport(Kernel32DllName, SetLastError = true)]
        public static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFOEX ConsoleScreenBufferInfoEx);

        [DllImport(Kernel32DllName, SetLastError = true)]
        public static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFOEX ConsoleScreenBufferInfoEx);

        [DllImport(User32DllName, SetLastError = true)]
        public static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

        [DllImport(Kernel32DllName, SetLastError = true)]
        public static extern bool SetCurrentConsoleFontEx(IntPtr consoleOutput, bool maximumWindow, ref CONSOLE_FONT_INFOEX consoleCurrentFontEx);

        [DllImport(Kernel32DllName, SetLastError = true)]
        public static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport(Kernel32DllName, SetLastError = true)]
        public static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, COORD dwSize);

        [DllImport(Kernel32DllName)]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);

        [DllImport(Kernel32DllName)]
        public static extern bool SetConsoleCursorPosition(IntPtr hConsoleOutput, int coord);

        [DllImport(Kernel32DllName)]
        public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport(Kernel32DllName)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport(User32DllName)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport(User32DllName)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport(User32DllName)]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport(User32DllName)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport(Kernel32DllName, ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport(User32DllName, SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        #endregion
        public static IntPtr ConsoleHandle = IntPtr.Zero;

        public static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

        public static FontInfo[] SetCurrentFont(string font, short fontSize = 0)
        {
            System.Console.WriteLine("Set Current Font: " + font);

            var before = new FontInfo { cbSize = Marshal.SizeOf<FontInfo>() };

            int error;
            if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
            {
                var info = new FontInfo
                {
                    cbSize = Marshal.SizeOf<FontInfo>(),
                    FontIndex = 0,
                    FontFamily = FixedWidthTrueType,
                    FontName = font,
                    FontWeight = 400,
                    FontSize = fontSize > 0 ? fontSize : before.FontSize
                };

                // Get some settings from current font.
                if (SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref info) == false)
                {
                    error = Marshal.GetLastWin32Error();
                    System.Console.WriteLine("Set Error " + error);

                    throw new System.ComponentModel.Win32Exception(error);
                }

                var after = new FontInfo { cbSize = Marshal.SizeOf<FontInfo>() };
                GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

                return new[] { before, info, after };
            }

            error = Marshal.GetLastWin32Error();
            System.Console.WriteLine("Get Error " + error);

            throw new System.ComponentModel.Win32Exception(error);
        }

        public static void SetPosition(int i) => SetConsoleCursorPosition(ConsoleHandle, i);

        //private static string Pastel(string text, int r, int g, int b) => "\u001b[38;2;" + r + ";" + g + ";" + b + "m" + text;
        public static string Pastel(char c, int r, int g, int b) => "\u001b[38;2;" + r + ";" + g + ";" + b + "m" + c;

        public static string SetPosition(int row, int collum) => "\u001b[" + row + ";" + collum + "H";

        public static bool ParseBool(string text, bool defaultValue = false)
        {
            string trimed = text.Trim();

            if (char.ToLower(trimed[0]) == 't' || trimed == "1") return true;
            else if (char.ToLower(trimed[0]) == 'f' || trimed == "0") return false;

            return bool.TryParse(text, out bool res) ? res : defaultValue;
        }

        #region ENUMERATOR API

        private enum ColorDifferenceMode
        {
            GlobalBrightNess,
            ColorDifferenceChange
        }

        #endregion

        #region STRUCTURE API

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct FontInfo
        {
            internal int cbSize;
            internal int FontIndex;
            internal short FontWidth;
            public short FontSize;
            public int FontFamily;
            public int FontWeight;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FontName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public short wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X; public short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_SCREEN_BUFFER_INFOEX
        {
            public uint cbSize;
            public COORD dwSize;
            public COORD dwCursorPosition;
            public ushort wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
            public ushort wPopupAttributes;
            public bool bFullscreenSupported;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public uint[] ColorTable;
            public uint ulInformationalMask;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_FONT_INFOEX
        {
            public uint cbSize;
            public uint nFont;
            public COORD dwFontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }

        #endregion
    }
}