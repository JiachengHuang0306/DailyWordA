using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using DailyWordA.Library.Models;
using DailyWordA.Library.Services;
using MvvmHelpers;

namespace DailyWordA.Library.ViewModels;

public class MusicPlayerViewModel : ViewModelBase {
    private readonly IMusicPlayer _musicPlayer;
    private readonly IMusicStorage _musicStorage;

    public MusicPlayerViewModel(IMusicPlayer musicPlayer, IMusicStorage musicStorage) {
        _musicPlayer = musicPlayer;
        _musicStorage = musicStorage;

        Playlist = new ObservableCollection<MusicObject>();
        LoadPlaylistCommand = new AsyncRelayCommand(LoadPlaylistAsync);
        PlayCommand = new RelayCommand(Play);
        PauseCommand = new RelayCommand(Pause);
        StopCommand = new RelayCommand(Stop);
        NextCommand = new RelayCommand(Next);
        PreviousCommand = new RelayCommand(Previous);
        ShuffleCommand = new RelayCommand(ToggleShuffle);
        RepeatCommand = new RelayCommand(ToggleRepeat);
        SeekCommand = new RelayCommand<TimeSpan>(Seek);

        // 初始化播放列表
        InitializeAsync();
    }
    
    private async void InitializeAsync() {
        //var pathFile = "/Users/jiachenghuang/Desktop/DWANEU/DailyWordA.Library/MusicPaths.txt";
        var pathFile = @"\\Mac\Home\Desktop\DWANEU\DailyWordA.Library\MusicPaths.txt";
        
        
        if (File.Exists(pathFile))
        {
            await _musicStorage.ClearAllMusicAsync();
            
            Console.WriteLine($"开始从 {pathFile} 导入音乐文件...");
            await _musicStorage.ImportMusicFromPathFileAsync(pathFile);
            Console.WriteLine("批量音乐文件导入完成！");

            // 导入完成后重新加载播放列表
            await LoadPlaylistAsync();
        }
        else
        {
            Console.WriteLine($"路径文件不存在: {pathFile}");
        }
    }

    // 播放列表
    public ObservableCollection<MusicObject> Playlist { get; }

    // 当前播放的音乐
    private MusicObject _currentTrack;

    public MusicObject CurrentTrack {
        get => _currentTrack;
        set
        {
            if (_currentTrack != value)
            {
                _currentTrack = value;
                OnPropertyChanged();
            }
        }
    }

    // 播放进度
    private TimeSpan _progress;

    public TimeSpan Progress {
        get => _progress;
        set
        {
            if (_progress != value)
            {
                _progress = value;
                OnPropertyChanged();
            }
        }
    }

    // 播放总时长
    private TimeSpan _duration;

    public TimeSpan Duration {
        get => _duration;
        set
        {
            if (_duration != value)
            {
                _duration = value;
                OnPropertyChanged();
            }
        }
    }

    // 是否正在播放
    private bool _isPlaying;

    public bool IsPlaying
    {
        get => _isPlaying;
        set
        {
            if (_isPlaying != value)
            {
                _isPlaying = value;
                OnPropertyChanged();
            }
        }
    }

    // 是否随机播放
    private bool _isShuffle;

    public bool IsShuffle {
        get => _isShuffle;
        set
        {
            if (_isShuffle != value)
            {
                _isShuffle = value;
                OnPropertyChanged();
            }
        }
    }

    // 是否循环播放
    private bool _isRepeat;

    public bool IsRepeat {
        get => _isRepeat;
        set
        {
            if (_isRepeat != value)
            {
                _isRepeat = value;
                OnPropertyChanged();
            }
        }
    }

    // 加载播放列表
    public ICommand LoadPlaylistCommand { get; }

    private async Task LoadPlaylistAsync()
    {
        try
        {
            var musicList = await _musicStorage.GetAllMusicAsync();
            Console.WriteLine($"从数据库加载了 {musicList.Count} 首音乐");

            Playlist.Clear();
            foreach (var music in musicList)
            {
                Playlist.Add(music);
            }

            _musicPlayer.SetPlaylist(new List<MusicObject>(Playlist));
            CurrentTrack = Playlist.Count > 0 ? Playlist[0] : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"加载播放列表失败: {ex.Message}");
        }
    }
    
    // 播放音乐
    public ICommand PlayCommand { get; }

    private void Play() {
        if (Playlist.Count == 0) {
            Console.WriteLine("播放列表为空，无法播放音乐。");
            return;
        }

        if (CurrentTrack == null) {
            CurrentTrack = Playlist[0]; // 默认播放第一首
        }

        _musicPlayer.SetCurrentTrack(CurrentTrack);
        _musicPlayer.Play();
        IsPlaying = true;

        Task.Run(UpdateProgressAsync); // 启动更新播放进度的任务
    }

    // 暂停音乐
    public ICommand PauseCommand { get; }

    private void Pause() {
        _musicPlayer.Pause();
        IsPlaying = false;
    }

    // 停止音乐
    public ICommand StopCommand { get; }

    private void Stop() {
        _musicPlayer.Stop();
        IsPlaying = false;
        Progress = TimeSpan.Zero;
    }

    // 下一首
    public ICommand NextCommand { get; }

    private void Next() {
        _musicPlayer.Next();
        CurrentTrack = _musicPlayer.GetCurrentTrack();
    }

    // 上一首
    public ICommand PreviousCommand { get; }

    private void Previous() {
        _musicPlayer.Previous();
        CurrentTrack = _musicPlayer.GetCurrentTrack();
    }

    // 切换随机播放模式
    public ICommand ShuffleCommand { get; }

    private void ToggleShuffle() {
        _musicPlayer.ToggleShuffle();
        IsShuffle = !IsShuffle;
    }

    // 切换循环播放模式
    public ICommand RepeatCommand { get; }

    private void ToggleRepeat() {
        _musicPlayer.ToggleRepeat();
        IsRepeat = !IsRepeat;
    }

    // 跳转播放进度
    public ICommand SeekCommand { get; }

    private void Seek(TimeSpan position) {
        _musicPlayer.Seek(position);
        Progress = position;
    }

    // 更新播放进度
    private async Task UpdateProgressAsync() {
        while (IsPlaying) {
            await Task.Delay(500); // 每 500ms 更新一次进度
            Dispatcher.UIThread.Post(() =>
            {
                Progress = _musicPlayer.GetProgress();
                Duration = _musicPlayer.GetDuration();
            });
        }
    }
}