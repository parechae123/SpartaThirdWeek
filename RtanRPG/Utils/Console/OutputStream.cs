using System.Text;
using RtanRPG.Object;
using RtanRPG.Utils.Extension;

namespace RtanRPG.Utils.Console
{
    public static class OutputStream
    {
        private static readonly Stream StandardOutputStream = System.Console.OpenStandardOutput();
        
        private static readonly StringBuilder Builder = new StringBuilder();

        public static void SetBuffer(int width, int height)
        {
            Buffer = new char[height][];
            for (var i = 0; i < height; i++)
            {
                Buffer[i] = new char[width];
                    
                Array.Fill(Buffer[i], ' ');
            }
        }
        
        private static void Write(char value, int left, int top)
        {
            System.Console.SetCursorPosition(left, top);
            System.Console.Write(value);
        }

        public static void Write(string value, int left, int top)
        {
            Write(value, left, top, Layout.MaximumContentWidth);
        }
        
        public static void Write(string value, int left, int top, int length)
        {
            var paragraphs = value.Split('\n');
            for (var i = 0; i < paragraphs.Length; i++, top++)
            {
                var texts = paragraphs[i].WordWrap(length);
                for (var j = 0; j < texts.Count; j++)
                {
                    System.Console.SetCursorPosition(left, top + j);
                    Write(texts[j]);
                }
            }
        }
        
        public static void Write(string str) => Write(Encoding.UTF8.GetBytes(str));

        public static void Write(ref string str) => Write(Encoding.UTF8.GetBytes(str));

        public static void Write(byte[] buffer) => StandardOutputStream.Write(buffer, 0, buffer.Length);

        public static void Write(byte b) => StandardOutputStream.WriteByte(b);

        public static void WriteBuffer(string text, Vector2D position)
        {
            WriteBuffer(text, position, Layout.MaximumContentWidth);
        }

        public static void WriteBuffer(string text, Vector2D position, int length)
        {
            var texts = text.WordWrap(length);
            for (var i = 0; i < texts.Count; i++)
            {
                var whitespace = 0;
                for (var j = 0; j < texts[i].Length; j++)
                {
                    whitespace += texts[i][j].GetGraphicLength() - 1;
                    Buffer[position.Top + i][position.Left + j] = texts[i][j];
                }

                // for (var j = 0; j < whitespace; j++)
                // {
                //     Buffer[position.Top + i][position.Left + texts[i].Length + j] = '\0';
                // }
            }
        }
        
        public static void WriteBuffer(string[] texts, Vector2D begin, Vector2D end)
        {
            var width = end.Left - begin.Left;
            var height = end.Top - begin.Top;
            
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Buffer[begin.Top + i][begin.Left + j] = texts[i][j];
                }
            }
        }

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
            
            // Write the data from the second buffer to the output buffer.
            var text = Builder.ToString();
            Write(ref text);
            
            // Reset console screen cursor position.
            System.Console.SetCursorPosition(0, 0);
            
            Builder.Clear();
            
            Thread.Sleep(100);
        }

        public static char[][] Buffer { get; private set; } = [];
    }
}