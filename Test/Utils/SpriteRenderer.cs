using System.Text;
using OpenCvSharp;

namespace Test.Utils
{
    public class SpriteRenderer
    {
        #region CONSTANT FIELD API
        
        private const int WidthDivisor = 1;
        private const int HeightDivisor = WidthDivisor * 1;

        private const int MaximumPixelBrightness = 256 * 3;

        #endregion
        
        private static readonly char[] CharSet = 
            { ' ', '.', ',', ':', ';', '!', 'i', '1', '[', 'L', 'C', 'O', 'G', '8', '$', '#', '@' };
        
        private Mat _image;
        
        private readonly StringBuilder _builder;
        
        private string[] _frames = [];
        
        public SpriteRenderer(string path)
        {
            _image = new Mat(path);

            _builder = new StringBuilder();
        }

        public void Prepare(int width, int height)
        {
            if (_image.Empty())
            {
                return;
            }

            // Resizes the image to the given width and height.
            _image = _image.Resize(new Size(width, height));
            
            // Build frame in video.
            for (var y = 0; y < height; y += HeightDivisor)
            {
                for (var x = 0; x < width; x += WidthDivisor)
                {
                    var pixel = _image.At<Vec3b>(y, x);
                    var brightness = pixel.Item0 + pixel.Item1 + pixel.Item2;
                    var predicted = CharSet[brightness * CharSet.Length / MaximumPixelBrightness];

                    _builder.Append(predicted);
                }

                _builder.Append('\n');
            }
            
            _frames = _builder.ToString().Split('\n').SkipLast(1).ToArray();
                
            _builder.Clear();
        }

        public string[] GetFrame()
        {
            return _frames;
        }
    }
}