using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using DailyWordA.Library.Models;

namespace DailyWordA.Library.Services;

public interface ICourseStorage
{
    bool IsInitialized { get; }

    Task InitializeAsync(); // 初始化数据库
    Task InitializeAsyncForFirstTime(IEnumerable<CourseObject> initialData); // 首次初始化时导入数据

    Task<CourseObject> GetCourseAsync(int id); // 根据 ID 获取课程
    Task<IList<CourseObject>> GetCoursesAsync(Expression<Func<CourseObject, bool>> where, int skip, int take); // 条件查询课程
    Task SaveCourseAsync(CourseObject courseObject); // 保存或更新课程
    Task DeleteCourseAsync(int id); // 根据 ID 删除课程

    Task<IList<CourseObject>> GetCoursesByDateAsync(DateTime date); // 获取指定日期的课程
    Task<CourseObject> GetRandomCourseAsync(); // 随机获取课程
    Task CloseAsync(); // 关闭数据库连接
}