using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DailyWordA.Library.Models;
using DailyWordA.Library.Services;
using DailyWordA.Library.ViewModels;

namespace DailyWordA.Library.ViewModels;

public class TodayCourseViewModel : ViewModelBase
    {
        private readonly ICourseStorage _courseStorage;

        public TodayCourseViewModel(ICourseStorage courseStorage)
        {
            _courseStorage = courseStorage;

            // 初始化课程列表
            Courses = new ObservableCollection<CourseObject>();

            // 加载今日课程
            Task.Run(async () =>
            {
                // 如果数据库未初始化，则同步数据
                if (!_courseStorage.IsInitialized)
                {
                    await _courseStorage.InitializeAsync(); // 初始化数据库
                    await _courseStorage.InitializeAsyncForFirstTime(await GetMockCourses()); // 模拟数据同步
                }

                await LoadTodayCoursesAsync();
            });
        }

        // 今日课程列表
        public ObservableCollection<CourseObject> Courses { get; }

        // 加载今日课程
        private async Task LoadTodayCoursesAsync()
        {
            var todayCourses = await _courseStorage.GetCoursesByDateAsync(DateTime.Now); // 获取今日课程
            Courses.Clear();

            foreach (var course in todayCourses)
            {
                Courses.Add(course);
            }
        }

        // 模拟同步数据的方法（替换为真实的 API 同步逻辑）
        private async Task<IEnumerable<CourseObject>> GetMockCourses()
        {
            await Task.Delay(100); // 模拟网络延迟
            return new List<CourseObject>
            {
                new CourseObject
                {
                    Name = "线性代数",
                    Date = DateTime.Now,
                    Time = "08:00-09:30",
                    Location = "教学楼 A202",
                    Teacher = "张三",
                    Type = "必修",
                    Credits = 3.0
                },
                new CourseObject
                {
                    Name = "数据结构",
                    Date = DateTime.Now,
                    Time = "10:00-11:30",
                    Location = "教学楼 B105",
                    Teacher = "李四",
                    Type = "必修",
                    Credits = 3.5
                },
                new CourseObject
                {
                    Name = "高等数学",
                    Date = DateTime.Now,
                    Time = "14:00-15:30",
                    Location = "教学楼 C301",
                    Teacher = "王五",
                    Type = "选修",
                    Credits = 2.0
                }
            };
        }
    }