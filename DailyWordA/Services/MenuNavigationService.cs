using System;
using DailyWordA.Library.Services;
using DailyWordA.Library.ViewModels;

namespace DailyWordA.Services;

public class MenuNavigationService : IMenuNavigationService {
    public void NavigateTo(string view, object parameter = null) {
        ViewModelBase viewModel = view switch {
            MenuNavigationConstant.TodayWordView => ServiceLocator.Current
                .TodayWordViewModel,
            MenuNavigationConstant.TodayMottoView => ServiceLocator.Current
                .TodayMottoViewModel,
            MenuNavigationConstant.TranslateView => ServiceLocator.Current.
                TranslateViewModel,
            MenuNavigationConstant.WordQueryView => ServiceLocator.Current.
                WordQueryViewModel,
            MenuNavigationConstant.WordFavoriteView => ServiceLocator.Current.
                WordFavoriteViewModel,
            MenuNavigationConstant.WordQuizView => ServiceLocator.Current.
                WordQuizViewModel,
            MenuNavigationConstant.MemoView => ServiceLocator.Current.
                MemoViewModel,
            _ => throw new Exception("未知的视图。")
        };

        if (parameter is not null) {
            viewModel.SetParameter(parameter);
        }

        ServiceLocator.Current.MainViewModel.SetMenuAndContent(view, viewModel);
    }
}