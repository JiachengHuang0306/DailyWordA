using SQLite;

namespace DailyWordA.Library.Models;


[Table("MusicObject")]
public class MusicObject {
    
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }            // 音乐的唯一标识
    public string Title { get; set; }      // 音乐标题
    public string Artist { get; set; }     // 艺术家
    public string FilePath { get; set; }   // 音乐文件的本地路径
    
    public TimeSpan Duration { get; set; } // 音乐时长
    
    public string DisplayName => $"{Title} - {Artist}";
}