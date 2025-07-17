using System.Text;
using RtanRPG.Utils.Extension;

namespace RtanRPG.Utils.Console
{
    public static class Window
    {
        public const int DefaultLeftMargin = 2;
        public const int DefaultTopMargin = 2;

        public static readonly Stream OutputStream = System.Console.OpenStandardOutput();

        public static char[][] Buffer = [];

        private static readonly StringBuilder Builder = new StringBuilder();
        
        public static void SetWindowBorder()
        {
            for (var i = 0; i < DefaultHeight - 1; i++)
            {
                Buffer[i][0] = Border.Bold.VerticalLine;
                Buffer[i][DefaultWidth - 1] = Border.Bold.VerticalLine;
            }

            for (var i = 0; i < DefaultWidth; i++)
            {
                Buffer[0][i] = Border.Bold.HorizontalLine;
                Buffer[DefaultHeight - 1][i] = Border.Bold.HorizontalLine;
            }

            Buffer[0][0] = Border.Bold.LeftTopEdge;
            Buffer[0][DefaultWidth - 1] = Border.Bold.RightTopEdge;
            Buffer[DefaultHeight - 1][0] = Border.Bold.LeftBottomEdge;
            Buffer[DefaultHeight - 1][DefaultWidth - 1] = Border.Bold.RightBottomEdge;
        }

        private static void Write(char value, int left, int top)
        {
            System.Console.SetCursorPosition(left, top);
            System.Console.Write(value);
        }

        public static void Write(string value, int left, int top, ConsoleColor color = ConsoleColor.White)
        {
            Write(value, left, top, MaximumContentWidth);
        }
        
        public static void Write(string value, int left, int top, int length, ConsoleColor color = ConsoleColor.White)
        {
            var paragraphs = value.Split('\n');
            for (var i = 0; i < paragraphs.Length; i++, top++)
            {
                System.Console.ForegroundColor = color;
                var texts = paragraphs[i].WordWrap(length);
                for (var j = 0; j < texts.Count; j++)
                {
                    System.Console.SetCursorPosition(left, top + j);
                    System.Console.Write(texts[j]);
                }
                System.Console.ResetColor();
            }
        }
        
        public static void WriteConsole(string str) => WriteConsole(Encoding.UTF8.GetBytes(str));

        public static void WriteConsole(ref string str) => WriteConsole(Encoding.UTF8.GetBytes(str));

        public static void WriteConsole(byte[] buffer) => OutputStream.Write(buffer, 0, buffer.Length);

        public static void WriteConsole(byte b) => OutputStream.WriteByte(b);

        public static void Render()
        {
            for (var i = 0; i < Buffer.Length - 1; i++)
            {
                for (var j = 0; j < Buffer[i].Length; j++)
                {
                    Builder.Append(Buffer[i][j]);
                }
                
                Array.Fill(Buffer[i], ' ');
                
                Builder.Append(Environment.NewLine);
            }
            
            var text = Builder.ToString();
            WriteConsole(ref text);
            
            System.Console.SetCursorPosition(0, 0);
            Builder.Clear();
            
            Thread.Sleep(1000);
        }
        
        public static int DefaultWidth { get; set; }

        public static int DefaultHeight { get; set; }

        public static int MaximumContentWidth => DefaultWidth - 2;

        public static int MaximumContentHeight => DefaultHeight - 2;
    }
}