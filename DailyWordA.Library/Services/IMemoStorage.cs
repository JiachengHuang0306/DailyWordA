using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DailyWordA.Library.Models;

namespace DailyWordA.Library.Services;

public interface IMemoStorage {
    // 数据库是否初始化
    bool IsInitialized { get; }

    // 初始化数据库（用于首次加载）
    Task InitializeAsync();

    // 首次运行时初始化数据
    Task InitializeAsyncForFirstTime(IEnumerable<MemoObject> initialData);

    // 根据 ID 获取备忘录
    Task<MemoObject> GetMemoAsync(int id);

    // 获取随机备忘录
    Task<MemoObject> GetRandomMemoAsync();

    // 根据条件查询备忘录
    Task<IList<MemoObject>> GetMemosAsync(Expression<Func<MemoObject, bool>> where, int skip, int take);

    // 获取指定日期的备忘录
    Task<IList<MemoObject>> GetMemosByDateAsync(DateTime date);

    // 保存或更新备忘录
    Task SaveMemoAsync(MemoObject memoObject);

    // 删除备忘录
    Task DeleteMemoAsync(int id);

    // 关闭数据库连接
    Task CloseAsync();
}