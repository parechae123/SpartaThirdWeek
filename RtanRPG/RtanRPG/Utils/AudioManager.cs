using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAudio.Wave;

namespace RtanRPG.Utils
{
    internal class AudioManager
    {
        private static AudioManager _instance;
        public static AudioManager Instance => _instance ??= new AudioManager();

        private IWavePlayer _outputDevice;
        private AudioFileReader _audioFile;

        private AudioManager() { }

        //H: dasdasd

        /// <summary>
        /// 단발 재생 (MP3/WAV 모두 지원)
        /// </summary>
        public void PlayOneShot(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"파일 없음: {path}");
                return;
            }

            try
            {
                var waveOut = new WaveOutEvent();
                var reader = new AudioFileReader(path);
                waveOut.Init(reader);
                waveOut.Play();

                // 자동 해제 (재생 완료 후)
                waveOut.PlaybackStopped += (s, e) =>
                {
                    waveOut.Dispose();
                    reader.Dispose();
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[오디오 재생 오류 - OneShot] {ex.Message}");
            }
        }

        /// <summary>
        /// 루프 재생
        /// </summary>
        public void Play(string path)
        {
            Stop(); // 먼저 정지

            if (!File.Exists(path))
            {
                Console.WriteLine($"파일 없음: {path}");
                return;
            }

            try
            {
                _outputDevice = new WaveOutEvent();
                _audioFile = new AudioFileReader(path);

                var loop = new LoopStream(_audioFile); // 루프 지원 클래스
                _outputDevice.Init(loop);
                _outputDevice.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[오디오 재생 오류 - Loop] {ex.Message}");
            }
        }

        /// <summary>
        /// 재생 정지
        /// </summary>
        public void Stop()
        {
            _outputDevice?.Stop();
            _outputDevice?.Dispose();
            _audioFile?.Dispose();
            _outputDevice = null;
            _audioFile = null;
        }
    }

    /// <summary>
    /// NAudio에서 루프 재생을 지원하게 만드는 보조 클래스
    /// </summary>
    internal class LoopStream : WaveStream
    {
        private readonly WaveStream _sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            _sourceStream = sourceStream;
        }

        public override WaveFormat WaveFormat => _sourceStream.WaveFormat;
        public override long Length => long.MaxValue; // 무한 루프처럼 보이게

        public override long Position
        {
            get => _sourceStream.Position;
            set => _sourceStream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = _sourceStream.Read(buffer, offset, count);
            if (read == 0)
            {
                _sourceStream.Position = 0;
                read = _sourceStream.Read(buffer, offset, count);
            }
            return read;
        }
    }
}
