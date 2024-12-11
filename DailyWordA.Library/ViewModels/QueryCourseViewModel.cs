using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyWordA.Library.Models;
using DailyWordA.Library.Services;
using DailyWordA.Library.ViewModels;


namespace DailyWordA.Library.ViewModels;

public class QueryCourseViewModel : ViewModelBase
    {
        private readonly ICourseStorage _courseStorage;
        private string _queryText;

        public QueryCourseViewModel(ICourseStorage courseStorage)
        {
            _courseStorage = courseStorage;

            // 初始化查询结果列表
            QueryResults = new ObservableCollection<CourseObject>();

            // 初始化搜索命令
            SearchCommand = new AsyncRelayCommand(SearchCoursesAsync);
        }

        // 查询输入框绑定的文本
        public string QueryText
        {
            get => _queryText;
            set
            {
                if (_queryText != value)
                {
                    _queryText = value;
                    OnPropertyChanged();
                }
            }
        }

        // 查询结果
        public ObservableCollection<CourseObject> QueryResults { get; }

        // 搜索命令
        public ICommand SearchCommand { get; }

        // 查询逻辑
        private async Task SearchCoursesAsync()
        {
            // 如果查询关键字为空，清空结果列表
            if (string.IsNullOrWhiteSpace(QueryText))
            {
                QueryResults.Clear();
                return;
            }

            try
            {
                // 使用 ICourseStorage 进行模糊查询
                var results = await _courseStorage.GetCoursesAsync(
                    c => c.Name.Contains(QueryText, StringComparison.OrdinalIgnoreCase), 
                    skip: 0, 
                    take: 50); // 假设最多返回 50 条结果

                // 清空现有查询结果
                QueryResults.Clear();

                // 添加新结果
                foreach (var course in results)
                {
                    QueryResults.Add(course);
                }

                // 调试输出
                Console.WriteLine($"查询关键字: {QueryText}, 查询结果: {QueryResults.Count} 条");
            }
            catch (Exception ex)
            {
                // 错误处理（可以扩展为显示到 UI）
                Console.WriteLine($"查询课程时出错: {ex.Message}");
            }
        }
    }