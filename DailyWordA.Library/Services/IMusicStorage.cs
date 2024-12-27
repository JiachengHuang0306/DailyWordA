using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DailyWordA.Library.Models;

namespace DailyWordA.Library.Services;

public interface IMusicStorage {
    // 获取全部音乐列表
    Task<IList<MusicObject>> GetAllMusicAsync();
    
    
    Task InitializeAsync();
    
    // 根据音乐 ID 获取单个音乐的详细信息
    Task<MusicObject> GetMusicByIdAsync(int id);

    
    // 添加新的音乐到数据库
    Task AddMusicAsync(MusicObject music);
    
    // 从数据库中删除音乐
    Task DeleteMusicAsync(int id);

    
    // 更新音乐信息
    Task UpdateMusicAsync(MusicObject music);
    
    Task ImportMusicFromPathFileAsync(string pathFile);

    bool IsInitialized { get; }
    
    Task ClearAllMusicAsync();
}