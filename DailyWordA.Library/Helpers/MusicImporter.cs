using System;
using System.IO;
using System.Threading.Tasks;
using DailyWordA.Library.Models;
using DailyWordA.Library.Services;
using TagLib;
using File = System.IO.File;

namespace DailyWordA.Library.Helpers;

public class MusicImporter
{
    private readonly IMusicStorage _musicStorage;

    public MusicImporter(IMusicStorage musicStorage)
    {
        _musicStorage = musicStorage;
    }

    public async Task ImportMusicAsync(string mp3FilePath) {
        // 打印文件路径日志，检查是否正确
        Console.WriteLine($"正在导入文件路径: {mp3FilePath}");

        if (!File.Exists(mp3FilePath)) {
            Console.WriteLine($"文件不存在: {mp3FilePath}");
            return;
        }

        try {
            // 使用 TagLib 读取 MP3 元数据
            var tagFile = TagLib.File.Create(mp3FilePath);
            var title = tagFile.Tag.Title ?? Path.GetFileNameWithoutExtension(mp3FilePath);
            var artist = tagFile.Tag.FirstAlbumArtist ?? "Unknown Artist";
            var duration = tagFile.Properties.Duration;

            // 创建 MusicObject
            var music = new MusicObject {
                Title = title,
                Artist = artist,
                FilePath = mp3FilePath,
                Duration = duration
            };

            Console.WriteLine($"导入音乐: {title} - {artist} ({duration})");

            // 保存到数据库
            await _musicStorage.AddMusicAsync(music);
            Console.WriteLine($"音乐 '{music.Title}' 成功导入到数据库");
        }
        catch (Exception ex) {
            Console.WriteLine($"导入音乐失败: {ex.Message}");
        }
    }
}