using System;
using System.IO;
using System.Media;

namespace DoAnOOP.Services
{
    // 15. Lớp SoundService phát âm thanh
    public sealed class SoundService : IDisposable
    {
        private static readonly Lazy<SoundService> _instance = new Lazy<SoundService>(() => new SoundService());
        private SoundPlayer _player;
        private readonly string _soundFilePath;

        public static SoundService Instance => _instance.Value;

        private SoundService()
        {
            _soundFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pick.wav");
            _player = new SoundPlayer();
            LoadSound();
        }

        private void LoadSound()
        {
            try
            {
                if (File.Exists(_soundFilePath))
                {
                    _player.SoundLocation = _soundFilePath;
                    _player.LoadAsync();
                }
                else
                {
                    Console.WriteLine($"Không tìm thấy file âm thanh: {_soundFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải âm thanh: {ex.Message}");
            }
        }

        public void PlayEnterSound()
        {
            if (_player.IsLoadCompleted)
            {
                try
                {
                    _player.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi phát âm thanh: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            _player?.Dispose();
        }
    }
}