using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyWordA.Library.Services;

namespace DailyWordA.Library.ViewModels;

public class MainWindowViewModel : ViewModelBase {
    private readonly IWordStorage _wordStorage;
    //private readonly ICourseStorage _courseStorage;
    private readonly IMemoStorage _memoStorage;
    private readonly IRootNavigationService _rootNavigationService;
    private readonly IWordFavoriteStorage _wordFavoriteStorage;

    public MainWindowViewModel(IWordStorage wordStorage, 
        //ICourseStorage courseStorage,
        IMemoStorage memoStorage,
        IRootNavigationService rootNavigationService,
        IWordFavoriteStorage wordFavoriteStorage) {
        _wordStorage = wordStorage;
        //_courseStorage = courseStorage;
        _memoStorage = memoStorage;
        _rootNavigationService = rootNavigationService;
        _wordFavoriteStorage = wordFavoriteStorage;
        
        OnInitializedCommand = new RelayCommand(OnInitialized);
    }

    private ViewModelBase _content;
    
    // 内部提供一个ViewModel
    public ViewModelBase Content {
        get => _content;
        set => SetProperty(ref _content, value);
    }
    
    public ICommand OnInitializedCommand { get; }

    public void OnInitialized() {
        if (!_wordStorage.IsInitialized || !_wordFavoriteStorage.IsInitialized || !_memoStorage.IsInitialized) {  //|| !_courseStorage.IsInitialized
            _rootNavigationService.NavigateTo(RootNavigationConstant.InitializationView);
        }
        else {
            _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
        }
        
    }
}