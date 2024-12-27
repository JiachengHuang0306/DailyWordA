using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyWordA.Library.Models;
using NAudio.Wave;


namespace DailyWordA.Library.Services;

public class MusicPlayer : IMusicPlayer
{
    private List<MusicObject> _playlist = new(); // 当前播放列表
    private MusicObject _currentTrack; // 当前播放的音乐
    private WaveOutEvent _waveOut; // 音频播放控制
    private AudioFileReader _audioFileReader; // 音频文件读取
    private bool _isShuffle; // 是否随机播放
    private bool _isRepeat; // 是否循环播放

    public MusicPlayer() {
        _waveOut = new WaveOutEvent();
        _waveOut.PlaybackStopped += OnPlaybackStopped;
    }

    public void SetPlaylist(List<MusicObject> playlist) {
        _playlist = playlist ?? throw new ArgumentNullException(nameof(playlist));
    }

    public List<MusicObject> GetPlaylist() {
        return _playlist;
    }

    public void Play() {
        if (_currentTrack == null && _playlist.Any())
        {
            // 如果当前没有选择的音乐，则播放第一首音乐
            SetCurrentTrack(_playlist.First());
        }

        if (_audioFileReader == null)
        {
            LoadCurrentTrack();
        }

        _waveOut.Play();
    }

    public void Pause() {
        _waveOut.Pause();
    }

    public void Stop() {
        _waveOut.Stop();
        _audioFileReader?.Dispose();
        _audioFileReader = null;
    }

    public void Next() {
        if (!_playlist.Any()) return;

        // 获取当前音乐在播放列表中的索引
        int currentIndex = _playlist.IndexOf(_currentTrack);

        if (_isShuffle)
        {
            // 随机播放模式下，选择一个随机索引
            var random = new Random();
            currentIndex = random.Next(_playlist.Count);
        }
        else
        {
            // 顺序播放模式下，选择下一首
            currentIndex = (currentIndex + 1) % _playlist.Count;
        }

        SetCurrentTrack(_playlist[currentIndex]);
        Play();
    }

    public void Previous() {
        if (!_playlist.Any()) return;

        // 获取当前音乐在播放列表中的索引
        int currentIndex = _playlist.IndexOf(_currentTrack);

        if (_isShuffle)
        {
            // 随机播放模式下，选择一个随机索引
            var random = new Random();
            currentIndex = random.Next(_playlist.Count);
        }
        else
        {
            // 顺序播放模式下，选择上一首
            currentIndex = (currentIndex - 1 + _playlist.Count) % _playlist.Count;
        }

        SetCurrentTrack(_playlist[currentIndex]);
        Play();
    }

    public void SetCurrentTrack(MusicObject track) {
        if (track == null) throw new ArgumentNullException(nameof(track));
        _currentTrack = track;

        // 停止当前音乐并加载新音乐
        Stop();
        LoadCurrentTrack();
    }

    public MusicObject GetCurrentTrack() {
        return _currentTrack;
    }

    public void Seek(TimeSpan position) {
        if (_audioFileReader != null)
        {
            _audioFileReader.CurrentTime = position;
        }
    }

    public TimeSpan GetProgress() {
        return _audioFileReader?.CurrentTime ?? TimeSpan.Zero;
    }

    public TimeSpan GetDuration() {
        return _audioFileReader?.TotalTime ?? TimeSpan.Zero;
    }

    public void ToggleShuffle() {
        _isShuffle = !_isShuffle;
    }

    public void ToggleRepeat() {
        _isRepeat = !_isRepeat;
    }

    private void LoadCurrentTrack() {
        if (_audioFileReader != null) {
            _audioFileReader.Dispose();
            _audioFileReader = null;
        }

        if (_currentTrack == null) return;

        try {
            _audioFileReader = new AudioFileReader(_currentTrack.FilePath);
            _waveOut.Init(_audioFileReader);
        }
        catch (Exception ex) {
            Console.WriteLine($"加载音乐失败：{ex.Message}");
        }
    }

    private void OnPlaybackStopped(object sender, StoppedEventArgs e) {
        if (_isRepeat) {
            // 单曲循环
            Play();
        }
        else {
            // 顺序播放或随机播放
            Next();
        }
    }
}