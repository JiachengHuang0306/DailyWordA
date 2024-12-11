using SQLite;

namespace DailyWordA.Library.Models;

[Table("Memos")]
public class MemoObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } // 自动生成的唯一主键

    public string Date { get; set; } // 日期字段，允许重复

    public string Content { get; set; } // 备忘录内容
}