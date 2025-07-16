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
        private float _volume = 1.0f;  //볼륨조절

        private AudioManager() { }  //생성자 비공개


        //호출 AudioManager.Instance.Play("Sounds/bgm.mp3");
        ///단발 재생 (MP3/WAV 모두 지원)
        public void PlayOneShot(string path)
        {
            if (!File.Exists(path))
            {
                System.Console.WriteLine($"파일 없음: {path}");
                return;
            }

            try
            {
                var waveOut = new WaveOutEvent();  //소리 출력 장치
                var reader = new AudioFileReader(path);
                reader.Volume = _volume;
                waveOut.Init(reader);  //읽는것과 출력 연결
                waveOut.Play();

                //재생 끝 메모리 해제
                //+=핸들러추가, s이벤트발생객체, e이벤트관련정보
                waveOut.PlaybackStopped += (s, e) => 
                {
                    waveOut.Dispose();
                    reader.Dispose();
                };
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[오디오 재생 오류 - OneShot] {ex.Message}");
            }
        }

        //루프 재생
        public void Play(string path)
        {
            Stop(); //먼저 정지

            if (!File.Exists(path))
            {
                System.Console.WriteLine($"파일 없음: {path}");
                return;
            }

            try
            {
                _outputDevice = new WaveOutEvent();
                _audioFile = new AudioFileReader(path);
                _audioFile.Volume = _volume;

                var loop = new LoopStream(_audioFile); //루프 지원 클래스
                _outputDevice.Init(loop);
                _outputDevice.Play();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[오디오 재생 오류 - Loop] {ex.Message}");
            }
        }

        ///재생 정지
        public void Stop()
        {
            _outputDevice?.Stop();
            _outputDevice?.Dispose();
            _audioFile?.Dispose();
            _outputDevice = null;
            _audioFile = null;
        }
        public void SetVolume(float volume)  //볼륨조절
        {
            _volume = Math.Clamp(volume, 0f, 1f);

            if (_audioFile != null)
                _audioFile.Volume = _volume;
        }
        //Fade in/out 기능
        //await AudioManager.Instance.FadeIn(1.0f, 2000); 2초 동안 볼륨 올리기
        //await AudioManager.Instance.FadeOut(1500);  1.5초 동안 소리 줄이고 정지
        public async Task FadeIn(float targetVolume = 1.0f, int durationMs = 1000)
        {
            float step = 0.05f;
            int delay = durationMs / (int)(targetVolume / step);

            for (float v = 0; v <= targetVolume; v += step)
            {
                SetVolume(v);
                await Task.Delay(delay);
            }

            SetVolume(targetVolume); //정확히 도달
        }

        public async Task FadeOut(int durationMs = 1000)
        {
            float startVolume = _volume;
            float step = 0.05f;
            int delay = durationMs / (int)(startVolume / step);

            for (float v = startVolume; v >= 0f; v -= step)
            {
                SetVolume(v);
                await Task.Delay(delay);
            }

            SetVolume(0f);
            Stop(); //페이드 아웃 끝나면 정지
        }
    }

    ///NAudio에서 루프 재생을 지원하게 만드는 보조 클래스
    internal class LoopStream : WaveStream
    {
        private readonly WaveStream _sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            _sourceStream = sourceStream;
        }

        public override WaveFormat WaveFormat => _sourceStream.WaveFormat;
        public override long Length => long.MaxValue; //무한 루프처럼 보이게

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
