using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DailyWordA.Library.Helpers;
using DailyWordA.Library.Models;
using SQLite;

namespace DailyWordA.Library.Services;

public class MusicStorage : IMusicStorage
{
    private const string DbName = "musicdb.sqlite3";

    private static readonly string MusicDbPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        DbName);

    private SQLiteAsyncConnection _connection;

    private SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(MusicDbPath);

    public bool IsInitialized { get; set; }

    // 初始化数据库
    public async Task InitializeAsync()
    {
        if (IsInitialized) return;

        Console.WriteLine("初始化音乐数据库...");
        await Connection.CreateTableAsync<MusicObject>();
        Console.WriteLine("音乐数据库已初始化完成。");
        Console.WriteLine($"Database Path: {MusicDbPath}");

        IsInitialized = true;
    }

    // 获取所有音乐列表
    public async Task<IList<MusicObject>> GetAllMusicAsync()
    {
        return await Connection.Table<MusicObject>().ToListAsync();
    }

    // 根据音乐 ID 获取音乐信息
    public async Task<MusicObject> GetMusicByIdAsync(int id)
    {
        return await Connection.FindAsync<MusicObject>(id);
    }

    
    // 清空所有音乐数据
    public async Task ClearAllMusicAsync()
    {
        try
        {
            await Connection.DeleteAllAsync<MusicObject>();
            Console.WriteLine("所有音乐数据已清空！");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"清空音乐数据时出错: {ex.Message}");
        }
    }
    
    // 添加新的音乐
    public async Task AddMusicAsync(MusicObject music)
    {
        if (music == null) throw new ArgumentNullException(nameof(music));
        await Connection.InsertAsync(music);
    }

    // 删除指定 ID 的音乐
    public async Task DeleteMusicAsync(int id)
    {
        var music = await GetMusicByIdAsync(id);
        if (music != null)
        {
            await Connection.DeleteAsync(music);
        }
    }

    // 更新音乐信息
    public async Task UpdateMusicAsync(MusicObject music)
    {
        if (music == null) throw new ArgumentNullException(nameof(music));
        await Connection.UpdateAsync(music);
    }

    // 导入多个音乐文件
    public async Task ImportMusicFromPathFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"路径文件不存在: {filePath}");
            return;
        }

        var importer = new MusicImporter(this);
        var lines = await File.ReadAllLinesAsync(filePath); // 逐行读取文件内容

        foreach (var line in lines)
        {
            var trimmedPath = line.Trim(); // 去掉路径前后的空格
            if (!string.IsNullOrEmpty(trimmedPath))
            {
                Console.WriteLine($"正在导入文件: {trimmedPath}");
                await importer.ImportMusicAsync(trimmedPath);
            }
        }

        Console.WriteLine("所有音乐文件导入完成！");
    }
}