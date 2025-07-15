using RtanRPG.Utils.Extension;

namespace RtanRPG.Utils.Console
{
    public static class Screen
    {
        private const int Width = 100;
        private const int Height = 34;
        
        public const int MaximumTextWidth = 400;
        public const int MaximumTextHeight = 96;

        private const int Separator = 28;
        
        private const int LeftMargin = 3;
        private const int TopMargin = 3;

        public static void DrawBorderLine()
        {
            for (var i = 2; i < Height; i++)
            {
                Write(Border.VerticalLine, 1, i);
                Write(Border.VerticalLine, Width, i);
            }

            for (var i = 2; i < Width; i++)
            {
                Write(Border.HorizontalLine, i, 1);
                Write(Border.HorizontalLine, i, Separator);
                Write(Border.HorizontalLine, i, Height);
            }

            Write(Border.LeftTopEdge, 1, 1);
            Write(Border.RightTopEdge, Width, 1);
            Write(Border.LeftBottomEdge, 1, Height);
            Write(Border.RightBottomEdge, Width, Height);

            Write(Border.LeftCross, 1, Separator);
            Write(Border.RightCross, Width, Separator);
        }

        private static void Write(char value, int left, int top)
        {
            System.Console.SetCursorPosition(left, top);
            System.Console.Write(value);
        }

        public static void Write(string value, int top)
        {
            var paragraphs = value.Split('\n');
            for (var i = 0; i < paragraphs.Length; i++, top++)
            {
                var texts = paragraphs[i].WordWrap(MaximumTextWidth);
                for (var j = 0; j < texts.Count; j++)
                {
                    System.Console.SetCursorPosition(LeftMargin, top + j);
                    System.Console.Write(texts[j]);
                }
            }
        }

        public static void Write(string value, int top, ConsoleColor foregroundColor)
        {
            System.Console.ForegroundColor = foregroundColor;
            
            Write(value, top);
            
            System.Console.ResetColor();
        }

        public struct Border
        {
            public const char VerticalLine = '│';
            public const char HorizontalLine = '─';
            
            public const char LeftTopEdge = '┌';
            public const char RightTopEdge = '┐';
            public const char RightBottomEdge = '┘';
            public const char LeftBottomEdge = '└';

            public const char LeftCross = '├';
            public const char RightCross = '┤';
            public const char TopCross = '┬';
            public const char BottomCross = '┴';
            public const char CenterCross = '┼';
        }

        public struct Layout
        {
            public const int Title = 3;
            public const int Description = 5;
            public const int Content = 8;
            public const int Menu = 22;
            public const int Message = 30;
            public const int Request = 31;
            public const int Input = 32;
        }
    }
}