using DailyWordA.Library.Models;

namespace DailyWordA.Library.Services;

public interface IMusicPlayer {
    // 设置播放列表
    void SetPlaylist(List<MusicObject> playlist);
    
    // 播放当前音乐
    void Play();
    
    // 暂停当前音乐
    void Pause();
    
    // 停止当前音乐
    void Stop();
    
    // 播放下一首音乐
    void Next();
    
    // 播放上一首音乐
    void Previous();
    
    // 设置当前播放的音乐
    void SetCurrentTrack(MusicObject track);
    
    // 获取当前播放的音乐
    MusicObject GetCurrentTrack();
    
    // 跳转到指定的播放位置
    void Seek(TimeSpan position);
    
    // 获取当前播放进度
    TimeSpan GetProgress();
    
    // 获取当前音乐总时长
    TimeSpan GetDuration();

    
    // 切换随机播放模式
    void ToggleShuffle();
    
    // 切换重复播放模式
    void ToggleRepeat();
    
    // 获取播放列表
    List<MusicObject> GetPlaylist();
}