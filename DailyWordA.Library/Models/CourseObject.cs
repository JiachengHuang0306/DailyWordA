using SQLite;

namespace DailyWordA.Library.Models;

[Table("courses_table")]
public class CourseObject { 
    [PrimaryKey, AutoIncrement] 
    public int Id { get; set; } // 唯一 ID
    
    [Column("name")] 
    public string Name { get; set; } // 课程名称
    
    [Column("date")] 
    public DateTime Date { get; set; } // 上课日期
    
    [Column("time")] 
    public string Time { get; set; } // 上课时间（如 "08:00-09:30"）
    
    [Column("location")] 
    public string Location { get; set; } // 上课地点
    
    [Column("teacher")] 
    public string Teacher { get; set; } // 授课教师
    
    [Column("type")] 
    public string Type { get; set; } // 课程类型（必修/选修）
    
    [Column("credits")] 
    public double? Credits { get; set; } // 学分
    
}