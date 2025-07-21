using System.Text;
using OpenCvSharp;
using Test.Object;

namespace Test.Utils
{
    public class VideoRenderer
    {
        #region CONSTANT FIELD API
        
        private const int WidthDivisor = 1;
        private const int HeightDivisor = WidthDivisor * 1;
        private const int FpsDivisor = 2;

        private const int MaximumPixelBrightness = 256 * 3;

        #endregion

        private static readonly char[] CharSet = 
            { ' ', '.', ',', ':', ';', '!', 'i', '1', '[', 'L', 'C', 'O', 'G', '8', '$', '#', '@' };

        private readonly VideoCapture _capture;
        private Mat _image;

        private readonly List<string[]> _cache;
        private string[][] _frames = [];
        
        private readonly StringBuilder _builder;

        private int _index;
        
        public VideoRenderer(string path)
        {
            _capture = new VideoCapture();
            _capture.Open(path);
            
            _image = new Mat();

            _builder = new StringBuilder();
            
            _cache = new List<string[]>();
        }
        
        public void Prepare(Vector2D begin, Vector2D end)
        {
            // Set the width and height of the video screen.
            var width = end.Left -  begin.Left;
            var height = end.Top - begin.Top;
            
            while (_capture.IsOpened())
            {
                // Skip a few frames to adjust the FPS.
                for (var i = 0; i < FpsDivisor - 1; i++)
                {
                    _capture.Grab();
                }

                // Gets the image for that frame.
                _capture.Read(_image);
                if (_image.Empty())
                {
                    break;
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
                
                var lines = _builder.ToString().Split('\n').SkipLast(1).ToArray();
                
                _cache.Add(lines);

                _builder.Clear();
            }
            
            _frames = _cache.ToArray();
            
            _cache.Clear();
        }
        
        public string[] GetNextFrame()
        {
            return _frames[_index++ % _frames.Length];
        }

        public void Stop()
        {
            _index = 0;
        }
    }
}