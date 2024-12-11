using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using DailyWordA.Library.Helpers;
using DailyWordA.Library.Models;
using DailyWordA.Library.Services;

namespace DailyWordA.Library.ViewModels;

public class MemoViewModel : ViewModelBase
{
    private readonly IMemoStorage _memoStorage;
    private DateTime _selectedDate;
    private string _newMemoContent;
    private MemoObject _selectedMemo;

    public MemoViewModel(IMemoStorage memoStorage)
    {
        _memoStorage = memoStorage;
        MemoList = new ObservableCollection<MemoObject>();

        AddMemoCommand = new AsyncRelayCommand(AddMemoAsync);
        LoadMemosCommand = new AsyncRelayCommand(LoadMemosAsync);
        DeleteMemoCommand = new AsyncRelayCommand<MemoObject>(DeleteMemoAsync);
        EditMemoCommand = new AsyncRelayCommand<MemoObject>(EditMemoAsync);
        
        _selectedDate = DateTime.Now;
    }

    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                OnPropertyChanged();

                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    LoadMemosCommand.Execute(null);
                });
            }
        }
    }

    public string NewMemoContent
    {
        get => _newMemoContent;
        set
        {
            if (_newMemoContent != value)
            {
                _newMemoContent = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<MemoObject> MemoList { get; }

    public AsyncRelayCommand AddMemoCommand { get; }
    public AsyncRelayCommand LoadMemosCommand { get; }
    public AsyncRelayCommand<MemoObject> DeleteMemoCommand { get; }
    public AsyncRelayCommand<MemoObject> EditMemoCommand { get; }

    private async Task AddMemoAsync()
    {
        if (string.IsNullOrWhiteSpace(NewMemoContent))
            return;

        var newMemo = new MemoObject
        {
            Date = DateHelper.ToDateString(SelectedDate),
            Content = NewMemoContent
        };

        try
        {
            await _memoStorage.SaveMemoAsync(newMemo);
            NewMemoContent = string.Empty;

            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await LoadMemosAsync();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"添加备忘事项时出错: {ex.Message}");
        }
    }

    // private async Task LoadMemosAsync() {
    //     try {
    //         var memos = await _memoStorage.GetMemosByDateAsync(SelectedDate);
    //
    //         await Dispatcher.UIThread.InvokeAsync(() => {
    //             MemoList.Clear();
    //             foreach (var memo in memos) {
    //                 MemoList.Add(memo);
    //             }
    //         });
    //     }
    //     catch (Exception ex) {
    //         Console.WriteLine($"加载备忘事项时出错: {ex.Message}");
    //     }
    // }
    private async Task LoadMemosAsync() {
        var memos = await _memoStorage.GetMemosByDateAsync(SelectedDate);

        await Dispatcher.UIThread.InvokeAsync(() => {
            MemoList.Clear();
            foreach (var memo in memos) {
                MemoList.Add(memo);
            }
        });
    }

    private async Task DeleteMemoAsync(MemoObject memo)
    {
        if (memo == null)
            return;

        try
        {
            await _memoStorage.DeleteMemoAsync(memo.Id);

            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await LoadMemosAsync();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"删除备忘事项时出错: {ex.Message}");
        }
    }

    private async Task EditMemoAsync(MemoObject memoObject)
    {
        if (memoObject == null)
        {
            Console.WriteLine("未选择备忘录进行编辑！");
            return;
        }

        if (string.IsNullOrWhiteSpace(NewMemoContent))
        {
            Console.WriteLine("编辑内容为空！");
            return;
        }

        try
        {
            // 更新备忘录内容
            memoObject.Content = NewMemoContent;
            await _memoStorage.SaveMemoAsync(memoObject);

            // 清空输入框
            NewMemoContent = string.Empty;

            // 重新加载备忘录列表
            await LoadMemosAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"编辑备忘录时出错: {ex.Message}");
        }
    }
    
    public MemoObject SelectedMemo
    {
        get => _selectedMemo;
        set
        {
            if (_selectedMemo != value)
            {
                _selectedMemo = value;
                OnPropertyChanged();
                if (_selectedMemo != null)
                {
                    // 将选中的备忘录内容加载到输入框
                    NewMemoContent = _selectedMemo.Content;
                }
            }
        }
    }
}