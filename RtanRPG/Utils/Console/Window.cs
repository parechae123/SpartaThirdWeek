using System.Diagnostics.Contracts;
using System.Net;
using Test.Utils.Extension;

namespace Test.Utils.Console
{
    public static class Window
    {
        public const int DefaultWidth = 207;
        public const int DefaultHeight = 50;

        public const int DefaultLeftMargin = 3;
        public const int DefaultTopMargin = 3;

        public const int MaximumContentWidth = DefaultWidth - 2;
        public const int MaximumContentHeight = DefaultHeight - 2;

        public static void DrawWindowBorder()
        {
            for (var i = 0; i < DefaultHeight; i++)
            {
                Write(Border.Bold.VerticalLine, 0, i);
                Write(Border.Bold.VerticalLine, DefaultWidth - 1, i);
            }

            for (var i = 0; i < DefaultWidth; i++)
            {
                Write(Border.Bold.HorizontalLine, i, 0);
                Write(Border.Bold.HorizontalLine, i, DefaultHeight - 1);
            }

            Write(Border.Bold.LeftTopEdge, 0, 0);
            Write(Border.Bold.RightTopEdge, DefaultWidth - 1, 0);
            Write(Border.Bold.LeftBottomEdge, 0, DefaultHeight - 1);
            Write(Border.Bold.RightBottomEdge, DefaultWidth - 1, DefaultHeight - 1);  
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
                var texts = paragraphs[i].WordWrap(MaximumContentWidth);
                for (var j = 0; j < texts.Count; j++)
                {
                    System.Console.SetCursorPosition(DefaultLeftMargin, top + j);
                    System.Console.Write(texts[j]);
                }
            }
        }

        public struct Layout
        {

        }
    }
}