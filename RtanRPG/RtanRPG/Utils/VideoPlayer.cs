using System.Diagnostics;
using System.Text;
using NAudio.Wave;
using OpenCvSharp;
using RtanRPG.Utils.Console;

namespace RtanRPG.Utils
{
    public class VideoPlayer
    {
        #region CONSTANT FIELD API
        
        private const int FixedSecond = 1000;
        
        private const int DefaultFrameWidth = 320;
        private const int DefaultFrameHeight = 240;
        
        private const int WidthDivisor = 3;
        private const int HeightDivisor = WidthDivisor * 2;
        private const int FpsDivisor = 2;
    
        private const int MaximumPixelBrightness = 256 * 3;
        
        private const int AudioBufferLenght = 1000;

        #endregion
        
        private static readonly char[] CharSet =
            new [] { ' ', '.', ',', ':', ';', 'i', '1', 't', 'f', 'L', 'C', 'O', 'G', '0', '8', '@', '#' };
        
        //TODO: TODO: You'll need to manage that code separately.
        private static readonly string Path = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        private readonly VideoCapture _capture;
        
        private readonly Stopwatch _stopwatch;
        private readonly Stopwatch _timer;
        
        private readonly StringBuilder _builder;
        
        public VideoPlayer()
        {
            _capture = new VideoCapture();
            
            _stopwatch = new Stopwatch();
            _timer = new Stopwatch();
            
            _builder = new StringBuilder();
        }
        
        public void Play(string filename, int width = DefaultFrameWidth, int height = DefaultFrameHeight)
        {
            _capture.Open(Path + "/" + filename + ".mp4");
            
            var fixedDeltaTime = (int)(FixedSecond / (_capture.Fps / FpsDivisor));
            
            //TODO: You'll need to manage that code separately.
            var reader = new MediaFoundationReader(Path + "/" + filename + ".mp4");
            
            //TODO: You'll need to manage that code separately.
            var provider = new BufferedWaveProvider(reader.WaveFormat)
                                       {
                                           BufferDuration = TimeSpan.FromMilliseconds(AudioBufferLenght * 4),
                                           DiscardOnBufferOverflow = true,
                                           ReadFully = true
                                       };

            //TODO: You'll need to manage that code separately.
            var output = new WaveOutEvent();
            output.Init(provider);
            output.Play();
            
            var fps = (int)Math.Round(_capture.Fps) / FpsDivisor;

            var image = new Mat();

            var frame = fps;
            var second = 0;
            
            var indexes = new int[height / HeightDivisor + 4];

            _timer.Restart();
            
            _capture.Read(image);

            while (_capture.IsOpened())
            {
                System.Console.SetCursorPosition(0, 0);
                
                for (var i = 0; i < FpsDivisor - 1; i++)
                {
                    _capture.Grab();
                }

                _capture.Read(image);
                if (image.Empty())
                {
                    break;
                }
                
                image = image.Resize(new Size(width, height));
                
                if (frame >= fps)
                {
                    if (_timer.ElapsedMilliseconds / FixedSecond < second)
                    {
                        var delay = second * FixedSecond - (int)_timer.ElapsedMilliseconds - 1;
                        if (delay > fixedDeltaTime) fixedDeltaTime++;

                        Thread.Sleep(delay);
                    }
                    else if (fixedDeltaTime > 0)
                    {
                        fixedDeltaTime--;
                    }
                    
                    //TODO: You'll need to manage that code separately.
                    WriteAudioBuffer(provider, reader, second == 0 ? 2 : 1);

                    frame = 0;
                    second++;
                }

                for (var y = 0; y < height; y += HeightDivisor)
                {
                    for (var x = 0; x < width; x += WidthDivisor)
                    {
                        var pixel = image.At<Vec3b>(y, x);
                        var brightness = pixel.Item0 + pixel.Item1 + pixel.Item2;
                        var predicted = CharSet[brightness * CharSet.Length / MaximumPixelBrightness];

                        _builder.Append(predicted);
                    }

                    _builder.Append('\n');
                }

                var index = 0;

                var rendered = string.Join("\n", _builder.ToString().Split('\n').Select(line => Build(line.TrimEnd(), indexes, ref index)));

                _builder.Clear();

                Configuration.WriteConsole(ref rendered);
                Configuration.SetPosition(0);

                frame++;

                if (_stopwatch.ElapsedMilliseconds < fixedDeltaTime)
                {
                    Thread.Sleep(fixedDeltaTime - (int)_stopwatch.ElapsedMilliseconds);
                }

                _stopwatch.Restart();
            }
        }

        private string Build(string line, int[] indexes, ref int index)
        {
            var length = line.Length;

            var delta = indexes[index] - length;

            indexes[index++] = length;
            if (delta <= 0)
            {
                return line;
            }

            return line + new string(' ', delta);
        }
        
        //TODO: You'll need to manage that code separately.
        public static void WriteAudioBuffer(BufferedWaveProvider buffer, MediaFoundationReader reader, int second)
        {
            var audioBuffer = new byte[reader.WaveFormat.AverageBytesPerSecond * second];
            var bytesRead = reader.Read(audioBuffer, 0, audioBuffer.Length);
            buffer.AddSamples(audioBuffer, 0, bytesRead);
        }
        
        public bool IsLoop { get; set; }
    }
}