using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyWordA.Library.Services;
using DailyWordA.Library.Services;

namespace DailyWordA.Library.ViewModels;

public class InitializationViewModel : ViewModelBase {
    private readonly IWordStorage _wordStorage;
    private readonly ICourseStorage _courseStorage;
    private readonly IMemoStorage _memoStorage;
    private readonly IRootNavigationService _rootNavigationService;
    private readonly IWordFavoriteStorage _wordFavoriteStorage;

    public InitializationViewModel(IWordStorage wordStorage, 
        ICourseStorage courseStorage,
        IMemoStorage memoStorage,
        IRootNavigationService rootNavigationService,
        IWordFavoriteStorage wordFavoriteStorage) {
        _wordStorage = wordStorage;
        _courseStorage = courseStorage;
        _memoStorage = memoStorage;
        _rootNavigationService = rootNavigationService;
        _wordFavoriteStorage = wordFavoriteStorage;
        
        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
    }
    
    public ICommand OnInitializedCommand { get; }

    private async Task OnInitializedAsync() {
        if (!_wordStorage.IsInitialized) {
            await _wordStorage.InitializeAsync();
        }
        
        if (!_wordFavoriteStorage.IsInitialized) {
            await _wordFavoriteStorage.InitializeAsync();
        }
        
        if (!_courseStorage.IsInitialized) {
            await _courseStorage.InitializeAsync();
        }
        
        if (!_memoStorage.IsInitialized) {
            await _memoStorage.InitializeAsync();
        }

        await Task.Delay(3000);

        _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
    }

}